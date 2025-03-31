using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks; // For Task.Run and async/await
using System.Windows.Forms;
using Renci.SshNet;
using Renci.SshNet.Sftp;

namespace MinecraftServerHelper
{
    public partial class Form1 : Form
    {
        // ==================================================
        // ========== SFTP CONNECTION INFORMATION ===========
        // ==================================================
        private readonly string sftpHost = "casfire.duckdns.org";
        private readonly int sftpPort = 22;
        // Credentials will be supplied by the user via the popup.
        private string sftpUser = "";     // default
        private string sftpPass = "";             // default (empty means not connected)
        
        private readonly string sftpRemotePath = "/mnt/Files/currentMods";

        // ==================================================
        // ========= LOCAL FILESYSTEM CONFIGURATION =========
        // ==================================================
        private readonly string downloadFolder = Path.Combine(Application.StartupPath, "DownloadedMods");
        private readonly string localMinecraftModsPath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".minecraft", "mods");

        // ==================================================
        // ======= MINECRAFT SERVER STATUS INFORMATION ======
        // ==================================================
        private readonly string serverAddress = "casfire.duckdns.org";
        private readonly int serverPort = 25565;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Load jar icon from resources, if available.
            if (Properties.Resources.jarIcon != null)
            {
                using (var ms = new MemoryStream(Properties.Resources.jarIcon))
                {
                    using (var icon = new Icon(ms))
                    {
                        imageListMods.Images.Add("jarIcon", icon.ToBitmap());
                    }
                }
            }
            // Refresh both lists and check server status.
            RefreshModsList();      // Server mods list
            RefreshLocalModsList(); // Local mods list
            CheckServerStatus();
            Log("Application started.");
        }

        /// <summary>
        /// Thread-safe log method.
        /// </summary>
        private void Log(string message)
        {
            string logMessage = $"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}";
            if (rtbLog.InvokeRequired)
            {
                rtbLog.Invoke(new Action(() =>
                {
                    rtbLog.AppendText(logMessage);
                    rtbLog.SelectionStart = rtbLog.Text.Length;
                    rtbLog.ScrollToCaret();
                }));
            }
            else
            {
                rtbLog.AppendText(logMessage);
                rtbLog.SelectionStart = rtbLog.Text.Length;
                rtbLog.ScrollToCaret();
            }
        }


        /// <summary>
        /// Opens a credentials popup so the user can update the SFTP username and password.
        /// </summary>
        private void btnConnect_Click(object sender, EventArgs e)
        {
            using (var credForm = new CredentialsForm(sftpUser, sftpPass))
            {
                if (credForm.ShowDialog() == DialogResult.OK)
                {
                    sftpUser = credForm.Username;
                    sftpPass = credForm.Password;
                    Log("Credentials updated. Attempting SFTP connection...");
                    RefreshModsList();
                }
            }
        }

        /// <summary>
        /// Refreshes the server ListView by connecting via SFTP and listing only .jar files.
        /// Updates the label with the mod count.
        /// </summary>
        private void RefreshModsList()
        {
            listViewServerMods.Items.Clear();
            int count = 0;
            try
            {
                using (var sftp = new SftpClient(sftpHost, sftpPort, sftpUser, sftpPass))
                {
                    sftp.Connect();
                    var files = sftp.ListDirectory(sftpRemotePath);
                    foreach (var file in files)
                    {
                        // Skip directories, symlinks, and non-.jar files
                        if (!file.IsDirectory && !file.IsSymbolicLink &&
                            file.Name.EndsWith(".jar", StringComparison.OrdinalIgnoreCase))
                        {
                            var item = new ListViewItem(file.Name);
                            if (imageListMods.Images.ContainsKey("jarIcon"))
                                item.ImageKey = "jarIcon";
                            listViewServerMods.Items.Add(item);
                            count++;
                        }
                    }
                    sftp.Disconnect();
                }
                lblServerMods.Text = $"Current server modlist: {count} mods";
                Log($"Server mod list refreshed. {count} .jar mod(s) found.");
            }
            catch (Exception ex)
            {
                listViewServerMods.Items.Add("Could not access SFTP folder: " + ex.Message);
                Log("Error refreshing server mod list: " + ex.Message);
            }
        }

        /// <summary>
        /// Refreshes the local mods ListView by reading the local .minecraft\mods folder.
        /// Updates the label with the mod count.
        /// </summary>
        private void RefreshLocalModsList()
        {
            listViewLocalMods.Items.Clear();
            int count = 0;
            try
            {
                if (!Directory.Exists(localMinecraftModsPath))
                {
                    Directory.CreateDirectory(localMinecraftModsPath);
                }
                string[] files = Directory.GetFiles(localMinecraftModsPath, "*.jar");
                foreach (string file in files)
                {
                    var item = new ListViewItem(Path.GetFileName(file));
                    if (imageListMods.Images.ContainsKey("jarIcon"))
                        item.ImageKey = "jarIcon";
                    listViewLocalMods.Items.Add(item);
                    count++;
                }
                if (count == 0)
                {
                    listViewLocalMods.Items.Add("No local .jar mods found.");
                }
                lblLocalMods.Text = $"Your local mods: {count} mods";
                Log("Local mod list refreshed. " + count + " .jar mod(s) found.");
            }
            catch (Exception ex)
            {
                listViewLocalMods.Items.Add("Error reading local mods: " + ex.Message);
                Log("Error refreshing local mod list: " + ex.Message);
            }
        }

        /// <summary>
        /// Checks if the Minecraft server is online.
        /// </summary>
        private void CheckServerStatus()
        {
            try
            {
                using (var client = new TcpClient())
                {
                    var result = client.BeginConnect(serverAddress, serverPort, null, null);
                    bool success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(3));
                    lblServerStatus.Text = success && client.Connected
                        ? "Minecraft Server Status: Online"
                        : "Minecraft Server Status: Offline";
                    Log("Server status checked: " + lblServerStatus.Text);
                }
            }
            catch (Exception ex)
            {
                lblServerStatus.Text = "Minecraft Server Status: Offline";
                Log("Error checking server status: " + ex.Message);
            }
        }

        /// <summary>
        /// Combined async method that downloads only .jar files from the SFTP server to the local DownloadedMods folder
        /// and then transfers them to the local .minecraft\mods folder.
        /// Updates a progress bar and logs actions. Disables the button during operation.
        /// </summary>
        private async void btnDownload_Click(object sender, EventArgs e)
        {
            // Only enable the button if credentials are set.
            if (string.IsNullOrWhiteSpace(sftpUser) || string.IsNullOrWhiteSpace(sftpPass))
            {
                Log("SFTP credentials not set. Please connect first.");
                return;
            }

            btnDownload.Enabled = false;
            try
            {
                await Task.Run(() =>
                {
                    // --- Download Phase ---
                    this.Invoke(new Action(() =>
                    {
                        if (!Directory.Exists(downloadFolder))
                        {
                            Directory.CreateDirectory(downloadFolder);
                            Log($"Created download folder: {downloadFolder}");
                        }
                        else
                        {
                            foreach (FileInfo file in new DirectoryInfo(downloadFolder).GetFiles())
                            {
                                file.Delete();
                            }
                            Log("Cleared old files from download folder.");
                        }
                        Log("Starting download phase...");
                    }));

                    using (var sftp = new SftpClient(sftpHost, sftpPort, sftpUser, sftpPass))
                    {
                        sftp.Connect();
                        var files = sftp.ListDirectory(sftpRemotePath);
                        var modFiles = new System.Collections.Generic.List<ISftpFile>();
                        foreach (var f in files)
                        {
                            if (!f.IsDirectory && !f.IsSymbolicLink &&
                                f.Name.EndsWith(".jar", StringComparison.OrdinalIgnoreCase))
                            {
                                modFiles.Add(f);
                            }
                        }
                        if (modFiles.Count == 0)
                        {
                            this.Invoke(new Action(() =>
                            {
                                Log("No .jar mods found on the server to download.");
                            }));
                            sftp.Disconnect();
                            return;
                        }
                        this.Invoke(new Action(() =>
                        {
                            progressBarSftp.Minimum = 0;
                            progressBarSftp.Maximum = modFiles.Count;
                            progressBarSftp.Value = 0;
                            progressBarSftp.Visible = true;
                        }));
                        int downloadedCount = 0;
                        foreach (var file in modFiles)
                        {
                            string localFilePath = Path.Combine(downloadFolder, file.Name);
                            using (var fs = new FileStream(localFilePath, FileMode.Create))
                            {
                                sftp.DownloadFile(file.FullName, fs);
                            }
                            downloadedCount++;
                            this.Invoke(new Action(() =>
                            {
                                progressBarSftp.Value = downloadedCount;
                                Log($"Downloaded: {file.Name}");
                            }));
                        }
                        sftp.Disconnect();
                        this.Invoke(new Action(() =>
                        {
                            Log($"Downloaded {downloadedCount} .jar mod(s) to {downloadFolder}");
                        }));
                    }

                    // --- Transfer Phase ---
                    this.Invoke(new Action(() =>
                    {
                        Log("Starting transfer phase...");
                        if (!Directory.Exists(localMinecraftModsPath))
                        {
                            Directory.CreateDirectory(localMinecraftModsPath);
                            Log($"Created local mods folder: {localMinecraftModsPath}");
                        }
                        else
                        {
                            // Backup existing .jar files
                            string[] existingFiles = Directory.GetFiles(localMinecraftModsPath, "*.jar");
                            if (existingFiles.Length > 0)
                            {
                                string modsOldFolder = Path.Combine(localMinecraftModsPath, "mods_old");
                                if (!Directory.Exists(modsOldFolder))
                                {
                                    Directory.CreateDirectory(modsOldFolder);
                                    Log($"Created backup folder: {modsOldFolder}");
                                }
                                foreach (var file in existingFiles)
                                {
                                    string destFile = Path.Combine(modsOldFolder, Path.GetFileName(file));
                                    if (File.Exists(destFile))
                                        File.Delete(destFile);
                                    File.Move(file, destFile);
                                    Log($"Backed up local mod: {Path.GetFileName(file)}");
                                }
                            }
                        }
                    }));

                    if (Directory.Exists(downloadFolder))
                    {
                        var downloadedFiles = Directory.GetFiles(downloadFolder, "*.jar");
                        this.Invoke(new Action(() =>
                        {
                            progressBarSftp.Minimum = 0;
                            progressBarSftp.Maximum = downloadedFiles.Length;
                            progressBarSftp.Value = 0;
                            progressBarSftp.Visible = true;
                        }));
                        int transferredCount = 0;
                        foreach (string file in downloadedFiles)
                        {
                            string destFile = Path.Combine(localMinecraftModsPath, Path.GetFileName(file));
                            File.Copy(file, destFile, true);
                            transferredCount++;
                            this.Invoke(new Action(() =>
                            {
                                progressBarSftp.Value = transferredCount;
                                Log($"Transferred: {Path.GetFileName(file)}");
                            }));
                        }
                        this.Invoke(new Action(() =>
                        {
                            Log("Mods transferred successfully!");
                            RefreshLocalModsList();
                        }));
                    }
                    else
                    {
                        this.Invoke(new Action(() =>
                        {
                            Log("No downloaded mods found. Please download first.");
                        }));
                    }
                });
            }
            catch (Exception ex)
            {
                Log("Error downloading or transferring mods: " + ex.Message);
            }
            finally
            {
                progressBarSftp.Visible = false;
                // Re-enable button if credentials are provided.
                btnDownload.Enabled = !string.IsNullOrWhiteSpace(sftpUser) && !string.IsNullOrWhiteSpace(sftpPass);
            }
        }

        /// <summary>
        /// Opens the local mods folder in Explorer.
        /// </summary>
        private void btnOpenLocalMods_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Directory.Exists(localMinecraftModsPath))
                {
                    Directory.CreateDirectory(localMinecraftModsPath);
                    Log($"Created local mods folder: {localMinecraftModsPath}");
                }
                Process.Start("explorer.exe", localMinecraftModsPath);
                Log("Opened local mods folder in Explorer.");
            }
            catch (Exception ex)
            {
                Log("Error opening local mods folder: " + ex.Message);
            }
        }

        /// <summary>
        /// Initiates a drag from the server mod list (only .jar items).
        /// </summary>
        private void listViewServerMods_ItemDrag(object sender, ItemDragEventArgs e)
        {
            var items = new System.Collections.Generic.List<string>();
            foreach (ListViewItem selectedItem in listViewServerMods.SelectedItems)
            {
                if (selectedItem.Text.EndsWith(".jar", StringComparison.OrdinalIgnoreCase))
                {
                    items.Add(selectedItem.Text);
                }
            }
            if (items.Count > 0)
            {
                DoDragDrop(items.ToArray(), DragDropEffects.Copy);
            }
        }

        /// <summary>
        /// When dragging over the local mods list, check if data is acceptable.
        /// </summary>
        private void listViewLocalMods_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(string[])))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        /// <summary>
        /// When items are dropped onto the local mods list, download each .jar mod individually.
        /// </summary>
        private async void listViewLocalMods_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                var fileNames = (string[])e.Data.GetData(typeof(string[]));
                if (fileNames == null || fileNames.Length == 0) return;

                var jarFiles = new System.Collections.Generic.List<string>();
                foreach (var f in fileNames)
                {
                    if (f.EndsWith(".jar", StringComparison.OrdinalIgnoreCase))
                    {
                        jarFiles.Add(f);
                    }
                }
                if (jarFiles.Count == 0) return;

                progressBarSftp.Minimum = 0;
                progressBarSftp.Maximum = jarFiles.Count;
                progressBarSftp.Value = 0;
                progressBarSftp.Visible = true;

                await Task.Run(() =>
                {
                    int transferredCount = 0;
                    using (var sftp = new SftpClient(sftpHost, sftpPort, sftpUser, sftpPass))
                    {
                        sftp.Connect();
                        foreach (string fileName in jarFiles)
                        {
                            string remoteFile = sftpRemotePath.TrimEnd('/') + "/" + fileName;
                            string localFile = Path.Combine(localMinecraftModsPath, fileName);
                            if (File.Exists(localFile))
                            {
                                string modsOldFolder = Path.Combine(localMinecraftModsPath, "mods_old");
                                if (!Directory.Exists(modsOldFolder))
                                    Directory.CreateDirectory(modsOldFolder);
                                string backupFile = Path.Combine(modsOldFolder, fileName);
                                if (File.Exists(backupFile))
                                    File.Delete(backupFile);
                                File.Move(localFile, backupFile);
                                this.Invoke(new Action(() => Log($"Backed up local mod: {fileName}")));
                            }
                            using (var fs = new FileStream(localFile, FileMode.Create))
                            {
                                sftp.DownloadFile(remoteFile, fs);
                            }
                            transferredCount++;
                            this.Invoke(new Action(() =>
                            {
                                progressBarSftp.Value = transferredCount;
                                Log($"Transferred (drag-drop): {fileName}");
                            }));
                        }
                        sftp.Disconnect();
                    }
                    this.Invoke(new Action(() =>
                    {
                        Log($"Transferred {transferredCount} .jar mod(s) via drag-drop to local mods folder.");
                        RefreshLocalModsList();
                    }));
                });
            }
            catch (Exception ex)
            {
                Log("Error transferring dragged mods: " + ex.Message);
            }
            finally
            {
                progressBarSftp.Visible = false;
            }
        }

        /// <summary>
        /// Deletes the selected mods (only .jar) from the local mods folder.
        /// </summary>
        private void btnDeleteLocalMods_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Directory.Exists(localMinecraftModsPath))
                {
                    Log("No local mods folder found.");
                    return;
                }
                if (listViewLocalMods.SelectedItems.Count == 0)
                {
                    Log("No local mods selected to delete.");
                    return;
                }
                foreach (ListViewItem item in listViewLocalMods.SelectedItems)
                {
                    if (!item.Text.EndsWith(".jar", StringComparison.OrdinalIgnoreCase))
                        continue;
                    string filePath = Path.Combine(localMinecraftModsPath, item.Text);
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                        Log($"Deleted local mod: {item.Text}");
                    }
                }
                RefreshLocalModsList();
            }
            catch (Exception ex)
            {
                Log("Error deleting selected local mods: " + ex.Message);
            }
        }

        /// <summary>
        /// Deletes all .jar mods from the local mods folder.
        /// </summary>
        private void btnDeleteAllLocalMods_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Directory.Exists(localMinecraftModsPath))
                {
                    Log("No local mods folder found.");
                    return;
                }
                var jarFiles = Directory.GetFiles(localMinecraftModsPath, "*.jar");
                int deleteCount = 0;
                foreach (var file in jarFiles)
                {
                    File.Delete(file);
                    deleteCount++;
                    Log($"Deleted local mod: {Path.GetFileName(file)}");
                }
                Log($"Deleted all local mods. ({deleteCount} file(s))");
                RefreshLocalModsList();
            }
            catch (Exception ex)
            {
                Log("Error deleting all local mods: " + ex.Message);
            }
        }

        /// <summary>
        /// Refreshes only the local mods ListView.
        /// </summary>
        private void btnRefreshLocalOnly_Click(object sender, EventArgs e)
        {
            RefreshLocalModsList();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshModsList();
            CheckServerStatus();
            RefreshLocalModsList();
        }
    }
}
