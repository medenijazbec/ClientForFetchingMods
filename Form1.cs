using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
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
        private string sftpUser = "";
        private string sftpPass = "";

        // ==================================================
        // =========== REMOTE SFTP PATHS (CURRENTMODS) ========
        // ==================================================
        private readonly string sftpRemoteModsPath = "/mnt/Files/currentMods/mods";
        private readonly string sftpRemoteShaderpacksPath = "/mnt/Files/currentMods/shaderpacks";
        private readonly string sftpRemoteLaunchersPath = "/mnt/Files/currentMods/launchers";

        // ==================================================
        // ========= LOCAL FILESYSTEM CONFIGURATION =========
        // ==================================================
        // Download folders (temporary staging)
        private readonly string downloadFolderMods = Path.Combine(Application.StartupPath, "DownloadedMods");
        private readonly string downloadFolderShaderpacks = Path.Combine(Application.StartupPath, "DownloadedShaderpacks");
        private readonly string downloadFolderLaunchers = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "DownloadedLaunchers"); // Changed to Desktop folder

        // Local destination folders for transfer
        private readonly string localMinecraftModsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".minecraft", "mods");
        private readonly string localMinecraftShaderpacksPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".minecraft", "shaderpacks");
        private readonly string localMinecraftLaunchersPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".minecraft", "launchers");

        // ==================================================
        // =========== FILE EXTENSION FILTERS ===============
        // ==================================================
        private readonly string modsFileExtension = ".jar";
        private readonly string shaderpacksFileExtension = ".zip";
        private readonly string launchersFileExtension = ".zip";

        // ==================================================
        // ========= MINECRAFT SERVER STATUS INFORMATION =====
        // ==================================================
        private readonly string serverAddress = "casfire.duckdns.org";
        private readonly int serverPort = 25565;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Load jar icon from resources if available
            if (Properties.Resources.jarIcon != null)
            {
                using (var ms = new MemoryStream(Properties.Resources.jarIcon))
                {
                    using (var icon = new Icon(ms))
                    {
                        imageListCategories.Images.Add("jarIcon", icon.ToBitmap());
                    }
                }
            }

            // Refresh all tabs and check server status
            RefreshModsTab();
            RefreshShaderpacksTab();
            RefreshLaunchersTab();
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
                    RefreshModsTab();
                    RefreshShaderpacksTab();
                    RefreshLaunchersTab();
                }
            }
        }

        /// <summary>
        /// Refresh global items: refresh all three tabs and check server status.
        /// </summary>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshModsTab();
            RefreshShaderpacksTab();
            RefreshLaunchersTab();
            CheckServerStatus();
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
                    lblServerStatus.Text = (success && client.Connected)
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

        #region MODS TAB METHODS

        private void RefreshModsTab()
        {
            RefreshServerModsList();
            RefreshLocalModsList();
        }

        private void RefreshServerModsList()
        {
            listViewServerMods.Items.Clear();
            int count = 0;
            try
            {
                using (var sftp = new SftpClient(sftpHost, sftpPort, sftpUser, sftpPass))
                {
                    sftp.Connect();
                    var files = sftp.ListDirectory(sftpRemoteModsPath);
                    foreach (var file in files)
                    {
                        if (!file.IsDirectory && !file.IsSymbolicLink &&
                            file.Name.EndsWith(modsFileExtension, StringComparison.OrdinalIgnoreCase))
                        {
                            var item = new ListViewItem(file.Name);
                            if (imageListCategories.Images.ContainsKey("jarIcon"))
                                item.ImageKey = "jarIcon";
                            listViewServerMods.Items.Add(item);
                            count++;
                        }
                    }
                    sftp.Disconnect();
                }
                lblServerMods.Text = $"Server Mods: {count} mod(s)";
                Log($"Mods list refreshed. {count} mod(s) found.");
            }
            catch (Exception ex)
            {
                listViewServerMods.Items.Add("Could not access SFTP folder: " + ex.Message);
                Log("Error refreshing mods list: " + ex.Message);
            }
        }
        private void btnRefreshLocalMods_Click(object sender, EventArgs e)
        {
            RefreshLocalModsList();
        }
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
                string[] files = Directory.GetFiles(localMinecraftModsPath, "*" + modsFileExtension);
                foreach (string file in files)
                {
                    var item = new ListViewItem(Path.GetFileName(file));
                    if (imageListCategories.Images.ContainsKey("jarIcon"))
                        item.ImageKey = "jarIcon";
                    listViewLocalMods.Items.Add(item);
                    count++;
                }
                if (count == 0)
                {
                    listViewLocalMods.Items.Add("No local mods found.");
                }
                lblLocalMods.Text = $"Local Mods: {count} mod(s)";
                Log("Local mods list refreshed. " + count + " mod(s) found.");
            }
            catch (Exception ex)
            {
                listViewLocalMods.Items.Add("Error reading local mods: " + ex.Message);
                Log("Error refreshing local mods list: " + ex.Message);
            }
        }

        private async void btnDownloadMods_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(sftpUser) || string.IsNullOrWhiteSpace(sftpPass))
            {
                Log("SFTP credentials not set. Please connect first.");
                return;
            }

            btnDownloadMods.Enabled = false;
            try
            {
                await Task.Run(() =>
                {
                    // --- Download Phase for Mods ---
                    this.Invoke(new Action(() =>
                    {
                        if (!Directory.Exists(downloadFolderMods))
                        {
                            Directory.CreateDirectory(downloadFolderMods);
                            Log($"Created download folder: {downloadFolderMods}");
                        }
                        else
                        {
                            foreach (FileInfo file in new DirectoryInfo(downloadFolderMods).GetFiles())
                            {
                                file.Delete();
                            }
                            Log("Cleared old files from download folder.");
                        }
                        Log("Starting download phase for mods...");
                    }));

                    using (var sftp = new SftpClient(sftpHost, sftpPort, sftpUser, sftpPass))
                    {
                        sftp.Connect();
                        var files = sftp.ListDirectory(sftpRemoteModsPath);
                        var modFiles = new List<ISftpFile>();
                        foreach (var f in files)
                        {
                            if (!f.IsDirectory && !f.IsSymbolicLink &&
                                f.Name.EndsWith(modsFileExtension, StringComparison.OrdinalIgnoreCase))
                            {
                                modFiles.Add(f);
                            }
                        }
                        if (modFiles.Count == 0)
                        {
                            this.Invoke(new Action(() =>
                            {
                                Log("No mod files found on the server to download.");
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
                            string localFilePath = Path.Combine(downloadFolderMods, file.Name);
                            using (var fs = new FileStream(localFilePath, FileMode.Create))
                            {
                                sftp.DownloadFile(file.FullName, fs);
                            }
                            downloadedCount++;
                            this.Invoke(new Action(() =>
                            {
                                progressBarSftp.Value = downloadedCount;
                                Log($"Downloaded mod: {file.Name}");
                            }));
                        }
                        sftp.Disconnect();
                        this.Invoke(new Action(() =>
                        {
                            Log($"Downloaded {downloadedCount} mod file(s) to {downloadFolderMods}");
                        }));
                    }

                    // --- Transfer Phase for Mods ---
                    this.Invoke(new Action(() =>
                    {
                        Log("Starting transfer phase for mods...");
                        if (!Directory.Exists(localMinecraftModsPath))
                        {
                            Directory.CreateDirectory(localMinecraftModsPath);
                            Log($"Created local mods folder: {localMinecraftModsPath}");
                        }
                        else
                        {
                            // Backup existing mod files
                            string[] existingFiles = Directory.GetFiles(localMinecraftModsPath, "*" + modsFileExtension);
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

                    if (Directory.Exists(downloadFolderMods))
                    {
                        var downloadedFiles = Directory.GetFiles(downloadFolderMods, "*" + modsFileExtension);
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
                                Log($"Transferred mod: {Path.GetFileName(file)}");
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
                btnDownloadMods.Enabled = !string.IsNullOrWhiteSpace(sftpUser) && !string.IsNullOrWhiteSpace(sftpPass);
            }
        }

        private void listViewServerMods_ItemDrag(object sender, ItemDragEventArgs e)
        {
            var items = new List<string>();
            foreach (ListViewItem selectedItem in listViewServerMods.SelectedItems)
            {
                if (selectedItem.Text.EndsWith(modsFileExtension, StringComparison.OrdinalIgnoreCase))
                {
                    items.Add(selectedItem.Text);
                }
            }
            if (items.Count > 0)
            {
                DoDragDrop(items.ToArray(), DragDropEffects.Copy);
            }
        }

        private void listViewLocalMods_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(string[])))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private async void listViewLocalMods_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                var fileNames = (string[])e.Data.GetData(typeof(string[]));
                if (fileNames == null || fileNames.Length == 0) return;

                var jarFiles = new List<string>();
                foreach (var f in fileNames)
                {
                    if (f.EndsWith(modsFileExtension, StringComparison.OrdinalIgnoreCase))
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
                            string remoteFile = sftpRemoteModsPath.TrimEnd('/') + "/" + fileName;
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
                                Log($"Transferred (drag-drop) mod: {fileName}");
                            }));
                        }
                        sftp.Disconnect();
                    }
                    this.Invoke(new Action(() =>
                    {
                        Log($"Transferred {transferredCount} mod(s) via drag-drop to local mods folder.");
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
                    if (!item.Text.EndsWith(modsFileExtension, StringComparison.OrdinalIgnoreCase))
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

        private void btnDeleteAllLocalMods_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Directory.Exists(localMinecraftModsPath))
                {
                    Log("No local mods folder found.");
                    return;
                }
                var files = Directory.GetFiles(localMinecraftModsPath, "*" + modsFileExtension);
                int deleteCount = 0;
                foreach (var file in files)
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

        private void btnRefreshLocalOnly_Click(object sender, EventArgs e)
        {
            RefreshLocalModsList();
        }

        #endregion

        #region SHADERPACKS TAB METHODS

        private void RefreshShaderpacksTab()
        {
            RefreshServerShaderpacksList();
            RefreshLocalShaderpacksList();
        }

        private void RefreshServerShaderpacksList()
        {
            listViewServerShaderpacks.Items.Clear();
            int count = 0;
            try
            {
                using (var sftp = new SftpClient(sftpHost, sftpPort, sftpUser, sftpPass))
                {
                    sftp.Connect();
                    var files = sftp.ListDirectory(sftpRemoteShaderpacksPath);
                    foreach (var file in files)
                    {
                        if (!file.IsDirectory && !file.IsSymbolicLink &&
                            file.Name.EndsWith(shaderpacksFileExtension, StringComparison.OrdinalIgnoreCase))
                        {
                            var item = new ListViewItem(file.Name);
                            if (imageListCategories.Images.ContainsKey("jarIcon"))
                                item.ImageKey = "jarIcon";
                            listViewServerShaderpacks.Items.Add(item);
                            count++;
                        }
                    }
                    sftp.Disconnect();
                }
                lblServerShaderpacks.Text = $"Server Shaderpacks: {count} file(s)";
                Log($"Shaderpacks list refreshed. {count} file(s) found.");
            }
            catch (Exception ex)
            {
                listViewServerShaderpacks.Items.Add("Error: " + ex.Message);
                Log("Error refreshing shaderpacks list: " + ex.Message);
            }
        }

        private void RefreshLocalShaderpacksList()
        {
            listViewLocalShaderpacks.Items.Clear();
            int count = 0;
            try
            {
                if (!Directory.Exists(localMinecraftShaderpacksPath))
                {
                    Directory.CreateDirectory(localMinecraftShaderpacksPath);
                }
                string[] files = Directory.GetFiles(localMinecraftShaderpacksPath, "*" + shaderpacksFileExtension);
                foreach (string file in files)
                {
                    var item = new ListViewItem(Path.GetFileName(file));
                    if (imageListCategories.Images.ContainsKey("jarIcon"))
                        item.ImageKey = "jarIcon";
                    listViewLocalShaderpacks.Items.Add(item);
                    count++;
                }
                if (count == 0)
                {
                    listViewLocalShaderpacks.Items.Add("No local shaderpacks found.");
                }
                lblLocalShaderpacks.Text = $"Local Shaderpacks: {count} file(s)";
                Log("Local shaderpacks list refreshed. " + count + " file(s) found.");
            }
            catch (Exception ex)
            {
                listViewLocalShaderpacks.Items.Add("Error: " + ex.Message);
                Log("Error refreshing local shaderpacks list: " + ex.Message);
            }
        }

        private async void btnDownloadShaderpacks_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(sftpUser) || string.IsNullOrWhiteSpace(sftpPass))
            {
                Log("SFTP credentials not set. Please connect first.");
                return;
            }

            btnDownloadShaderpacks.Enabled = false;
            try
            {
                await Task.Run(() =>
                {
                    // --- Download Phase for Shaderpacks ---
                    this.Invoke(new Action(() =>
                    {
                        if (!Directory.Exists(downloadFolderShaderpacks))
                        {
                            Directory.CreateDirectory(downloadFolderShaderpacks);
                            Log($"Created download folder: {downloadFolderShaderpacks}");
                        }
                        else
                        {
                            foreach (FileInfo file in new DirectoryInfo(downloadFolderShaderpacks).GetFiles())
                            {
                                file.Delete();
                            }
                            Log("Cleared old files from shaderpacks download folder.");
                        }
                        Log("Starting download phase for shaderpacks...");
                    }));

                    using (var sftp = new SftpClient(sftpHost, sftpPort, sftpUser, sftpPass))
                    {
                        sftp.Connect();
                        var files = sftp.ListDirectory(sftpRemoteShaderpacksPath);
                        var packFiles = new List<ISftpFile>();
                        foreach (var f in files)
                        {
                            if (!f.IsDirectory && !f.IsSymbolicLink &&
                                f.Name.EndsWith(shaderpacksFileExtension, StringComparison.OrdinalIgnoreCase))
                            {
                                packFiles.Add(f);
                            }
                        }
                        if (packFiles.Count == 0)
                        {
                            this.Invoke(new Action(() =>
                            {
                                Log("No shaderpack files found on the server to download.");
                            }));
                            sftp.Disconnect();
                            return;
                        }
                        this.Invoke(new Action(() =>
                        {
                            progressBarSftp.Minimum = 0;
                            progressBarSftp.Maximum = packFiles.Count;
                            progressBarSftp.Value = 0;
                            progressBarSftp.Visible = true;
                        }));
                        int downloadedCount = 0;
                        foreach (var file in packFiles)
                        {
                            string localFilePath = Path.Combine(downloadFolderShaderpacks, file.Name);
                            using (var fs = new FileStream(localFilePath, FileMode.Create))
                            {
                                sftp.DownloadFile(file.FullName, fs);
                            }
                            downloadedCount++;
                            this.Invoke(new Action(() =>
                            {
                                progressBarSftp.Value = downloadedCount;
                                Log($"Downloaded shaderpack: {file.Name}");
                            }));
                        }
                        sftp.Disconnect();
                        this.Invoke(new Action(() =>
                        {
                            Log($"Downloaded {downloadedCount} shaderpack file(s) to {downloadFolderShaderpacks}");
                        }));
                    }

                    // --- Transfer Phase for Shaderpacks ---
                    this.Invoke(new Action(() =>
                    {
                        Log("Starting transfer phase for shaderpacks...");
                        if (!Directory.Exists(localMinecraftShaderpacksPath))
                        {
                            Directory.CreateDirectory(localMinecraftShaderpacksPath);
                            Log($"Created local shaderpacks folder: {localMinecraftShaderpacksPath}");
                        }
                        else
                        {
                            string[] existingFiles = Directory.GetFiles(localMinecraftShaderpacksPath, "*" + shaderpacksFileExtension);
                            if (existingFiles.Length > 0)
                            {
                                string packOldFolder = Path.Combine(localMinecraftShaderpacksPath, "shaderpacks_old");
                                if (!Directory.Exists(packOldFolder))
                                {
                                    Directory.CreateDirectory(packOldFolder);
                                    Log($"Created backup folder: {packOldFolder}");
                                }
                                foreach (var file in existingFiles)
                                {
                                    string destFile = Path.Combine(packOldFolder, Path.GetFileName(file));
                                    if (File.Exists(destFile))
                                        File.Delete(destFile);
                                    File.Move(file, destFile);
                                    Log($"Backed up local shaderpack: {Path.GetFileName(file)}");
                                }
                            }
                        }
                    }));

                    if (Directory.Exists(downloadFolderShaderpacks))
                    {
                        var downloadedFiles = Directory.GetFiles(downloadFolderShaderpacks, "*" + shaderpacksFileExtension);
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
                            string destFile = Path.Combine(localMinecraftShaderpacksPath, Path.GetFileName(file));
                            File.Copy(file, destFile, true);
                            transferredCount++;
                            this.Invoke(new Action(() =>
                            {
                                progressBarSftp.Value = transferredCount;
                                Log($"Transferred shaderpack: {Path.GetFileName(file)}");
                            }));
                        }
                        this.Invoke(new Action(() =>
                        {
                            Log("Shaderpacks transferred successfully!");
                            RefreshLocalShaderpacksList();
                        }));
                    }
                    else
                    {
                        this.Invoke(new Action(() =>
                        {
                            Log("No downloaded shaderpack files found. Please download first.");
                        }));
                    }
                });
            }
            catch (Exception ex)
            {
                Log("Error downloading or transferring shaderpacks: " + ex.Message);
            }
            finally
            {
                progressBarSftp.Visible = false;
                btnDownloadShaderpacks.Enabled = !string.IsNullOrWhiteSpace(sftpUser) && !string.IsNullOrWhiteSpace(sftpPass);
            }
        }

        private void listViewServerShaderpacks_ItemDrag(object sender, ItemDragEventArgs e)
        {
            var items = new List<string>();
            foreach (ListViewItem selectedItem in listViewServerShaderpacks.SelectedItems)
            {
                if (selectedItem.Text.EndsWith(shaderpacksFileExtension, StringComparison.OrdinalIgnoreCase))
                {
                    items.Add(selectedItem.Text);
                }
            }
            if (items.Count > 0)
            {
                DoDragDrop(items.ToArray(), DragDropEffects.Copy);
            }
        }

        private void listViewLocalShaderpacks_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(string[])))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private async void listViewLocalShaderpacks_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                var fileNames = (string[])e.Data.GetData(typeof(string[]));
                if (fileNames == null || fileNames.Length == 0) return;

                var packFiles = new List<string>();
                foreach (var f in fileNames)
                {
                    if (f.EndsWith(shaderpacksFileExtension, StringComparison.OrdinalIgnoreCase))
                    {
                        packFiles.Add(f);
                    }
                }
                if (packFiles.Count == 0) return;

                progressBarSftp.Minimum = 0;
                progressBarSftp.Maximum = packFiles.Count;
                progressBarSftp.Value = 0;
                progressBarSftp.Visible = true;

                await Task.Run(() =>
                {
                    int transferredCount = 0;
                    using (var sftp = new SftpClient(sftpHost, sftpPort, sftpUser, sftpPass))
                    {
                        sftp.Connect();
                        foreach (string fileName in packFiles)
                        {
                            string remoteFile = sftpRemoteShaderpacksPath.TrimEnd('/') + "/" + fileName;
                            string localFile = Path.Combine(localMinecraftShaderpacksPath, fileName);
                            if (File.Exists(localFile))
                            {
                                string packOldFolder = Path.Combine(localMinecraftShaderpacksPath, "shaderpacks_old");
                                if (!Directory.Exists(packOldFolder))
                                    Directory.CreateDirectory(packOldFolder);
                                string backupFile = Path.Combine(packOldFolder, fileName);
                                if (File.Exists(backupFile))
                                    File.Delete(backupFile);
                                File.Move(localFile, backupFile);
                                this.Invoke(new Action(() => Log($"Backed up local shaderpack: {fileName}")));
                            }
                            using (var fs = new FileStream(localFile, FileMode.Create))
                            {
                                sftp.DownloadFile(remoteFile, fs);
                            }
                            transferredCount++;
                            this.Invoke(new Action(() =>
                            {
                                progressBarSftp.Value = transferredCount;
                                Log($"Transferred (drag-drop) shaderpack: {fileName}");
                            }));
                        }
                        sftp.Disconnect();
                    }
                    this.Invoke(new Action(() =>
                    {
                        Log($"Transferred {transferredCount} shaderpack(s) via drag-drop.");
                        RefreshLocalShaderpacksList();
                    }));
                });
            }
            catch (Exception ex)
            {
                Log("Error transferring dragged shaderpacks: " + ex.Message);
            }
            finally
            {
                progressBarSftp.Visible = false;
            }
        }

        private void btnOpenLocalShaderpacks_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Directory.Exists(localMinecraftShaderpacksPath))
                {
                    Directory.CreateDirectory(localMinecraftShaderpacksPath);
                    Log($"Created local shaderpacks folder: {localMinecraftShaderpacksPath}");
                }
                Process.Start("explorer.exe", localMinecraftShaderpacksPath);
                Log("Opened local shaderpacks folder in Explorer.");
            }
            catch (Exception ex)
            {
                Log("Error opening local shaderpacks folder: " + ex.Message);
            }
        }

        private void btnDeleteLocalShaderpacks_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Directory.Exists(localMinecraftShaderpacksPath))
                {
                    Log("No local shaderpacks folder found.");
                    return;
                }
                if (listViewLocalShaderpacks.SelectedItems.Count == 0)
                {
                    Log("No local shaderpacks selected to delete.");
                    return;
                }
                foreach (ListViewItem item in listViewLocalShaderpacks.SelectedItems)
                {
                    if (!item.Text.EndsWith(shaderpacksFileExtension, StringComparison.OrdinalIgnoreCase))
                        continue;
                    string filePath = Path.Combine(localMinecraftShaderpacksPath, item.Text);
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                        Log($"Deleted local shaderpack: {item.Text}");
                    }
                }
                RefreshLocalShaderpacksList();
            }
            catch (Exception ex)
            {
                Log("Error deleting selected local shaderpacks: " + ex.Message);
            }
        }

        private void btnDeleteAllLocalShaderpacks_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Directory.Exists(localMinecraftShaderpacksPath))
                {
                    Log("No local shaderpacks folder found.");
                    return;
                }
                var files = Directory.GetFiles(localMinecraftShaderpacksPath, "*" + shaderpacksFileExtension);
                int deleteCount = 0;
                foreach (var file in files)
                {
                    File.Delete(file);
                    deleteCount++;
                    Log($"Deleted local shaderpack: {Path.GetFileName(file)}");
                }
                Log($"Deleted all local shaderpacks. ({deleteCount} file(s))");
                RefreshLocalShaderpacksList();
            }
            catch (Exception ex)
            {
                Log("Error deleting all local shaderpacks: " + ex.Message);
            }
        }

        private void btnRefreshLocalShaderpacks_Click(object sender, EventArgs e)
        {
            RefreshLocalShaderpacksList();
        }

        #endregion

        #region LAUNCHERS TAB METHODS

        private void RefreshLaunchersTab()
        {
            RefreshServerLaunchersList();
            RefreshLocalLaunchersList();
        }

        private void RefreshServerLaunchersList()
        {
            listViewServerLaunchers.Items.Clear();
            int count = 0;
            try
            {
                using (var sftp = new SftpClient(sftpHost, sftpPort, sftpUser, sftpPass))
                {
                    sftp.Connect();
                    var files = sftp.ListDirectory(sftpRemoteLaunchersPath);
                    foreach (var file in files)
                    {
                        if (!file.IsDirectory && !file.IsSymbolicLink &&
                            file.Name.EndsWith(launchersFileExtension, StringComparison.OrdinalIgnoreCase))
                        {
                            var item = new ListViewItem(file.Name);
                            if (imageListCategories.Images.ContainsKey("jarIcon"))
                                item.ImageKey = "jarIcon";
                            listViewServerLaunchers.Items.Add(item);
                            count++;
                        }
                    }
                    sftp.Disconnect();
                }
                lblServerLaunchers.Text = $"Server Launchers: {count} file(s)";
                Log($"Launchers list refreshed. {count} file(s) found.");
            }
            catch (Exception ex)
            {
                listViewServerLaunchers.Items.Add("Error: " + ex.Message);
                Log("Error refreshing launchers list: " + ex.Message);
            }
        }

        private void RefreshLocalLaunchersList()
        {
            listViewLocalLaunchers.Items.Clear();
            int count = 0;
            try
            {
                if (!Directory.Exists(localMinecraftLaunchersPath))
                {
                    Directory.CreateDirectory(localMinecraftLaunchersPath);
                }
                string[] files = Directory.GetFiles(localMinecraftLaunchersPath, "*" + launchersFileExtension);
                foreach (string file in files)
                {
                    var item = new ListViewItem(Path.GetFileName(file));
                    if (imageListCategories.Images.ContainsKey("jarIcon"))
                        item.ImageKey = "jarIcon";
                    listViewLocalLaunchers.Items.Add(item);
                    count++;
                }
                if (count == 0)
                {
                    listViewLocalLaunchers.Items.Add("No local launchers found.");
                }
                lblLocalLaunchers.Text = $"Local Launchers: {count} file(s)";
                Log("Local launchers list refreshed. " + count + " file(s) found.");
            }
            catch (Exception ex)
            {
                listViewLocalLaunchers.Items.Add("Error: " + ex.Message);
                Log("Error refreshing local launchers list: " + ex.Message);
            }
        }

        private async void btnDownloadLaunchers_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(sftpUser) || string.IsNullOrWhiteSpace(sftpPass))
            {
                Log("SFTP credentials not set. Please connect first.");
                return;
            }

            btnDownloadLaunchers.Enabled = false;
            try
            {
                await Task.Run(() =>
                {
                    // --- Download Phase for Launchers ---
                    this.Invoke(new Action(() =>
                    {
                        if (!Directory.Exists(downloadFolderLaunchers))
                        {
                            Directory.CreateDirectory(downloadFolderLaunchers);
                            Log($"Created download folder: {downloadFolderLaunchers}");
                        }
                        else
                        {
                            foreach (FileInfo file in new DirectoryInfo(downloadFolderLaunchers).GetFiles())
                            {
                                file.Delete();
                            }
                            Log("Cleared old files from launchers download folder.");
                        }
                        Log("Starting download phase for launchers...");
                    }));

                    using (var sftp = new SftpClient(sftpHost, sftpPort, sftpUser, sftpPass))
                    {
                        sftp.Connect();
                        var files = sftp.ListDirectory(sftpRemoteLaunchersPath);
                        var launcherFiles = new List<ISftpFile>();
                        foreach (var f in files)
                        {
                            if (!f.IsDirectory && !f.IsSymbolicLink &&
                                f.Name.EndsWith(launchersFileExtension, StringComparison.OrdinalIgnoreCase))
                            {
                                launcherFiles.Add(f);
                            }
                        }
                        if (launcherFiles.Count == 0)
                        {
                            this.Invoke(new Action(() =>
                            {
                                Log("No launcher files found on the server to download.");
                            }));
                            sftp.Disconnect();
                            return;
                        }
                        int downloadedCount = 0;
                        foreach (var file in launcherFiles)
                        {
                            string localFilePath = Path.Combine(downloadFolderLaunchers, file.Name);
                            using (var fs = new FileStream(localFilePath, FileMode.Create))
                            {
                                // Cast file.Attributes.Size to ulong to match the type of 'downloaded'.
                                ulong totalBytes = (ulong)file.Attributes.Size;
                                // Set progress bar for the current file download in percentage.
                                this.Invoke(new Action(() =>
                                {
                                    progressBarSftp.Minimum = 0;
                                    progressBarSftp.Maximum = 100;
                                    progressBarSftp.Value = 0;
                                    progressBarSftp.Visible = true;
                                }));

                                // Download file with progress callback.
                                sftp.DownloadFile(file.FullName, fs, downloaded =>
                                {
                                    int percent = (int)((downloaded * 100) / totalBytes);
                                    this.Invoke(new Action(() =>
                                    {
                                        progressBarSftp.Value = percent;
                                    }));
                                });
                            }
                            downloadedCount++;
                            this.Invoke(new Action(() =>
                            {
                                Log($"Downloaded launcher: {file.Name} ({downloadedCount}/{launcherFiles.Count})");
                            }));
                        }
                        sftp.Disconnect();
                        this.Invoke(new Action(() =>
                        {
                            Log($"Downloaded {downloadedCount} launcher file(s) to {downloadFolderLaunchers}");
                        }));
                    }

                    // --- Transfer Phase for Launchers ---
                    this.Invoke(new Action(() =>
                    {
                        Log("Starting transfer phase for launchers...");
                        if (!Directory.Exists(localMinecraftLaunchersPath))
                        {
                            Directory.CreateDirectory(localMinecraftLaunchersPath);
                            Log($"Created local launchers folder: {localMinecraftLaunchersPath}");
                        }
                        else
                        {
                            string[] existingFiles = Directory.GetFiles(localMinecraftLaunchersPath, "*" + launchersFileExtension);
                            if (existingFiles.Length > 0)
                            {
                                string launchersOldFolder = Path.Combine(localMinecraftLaunchersPath, "launchers_old");
                                if (!Directory.Exists(launchersOldFolder))
                                {
                                    Directory.CreateDirectory(launchersOldFolder);
                                    Log($"Created backup folder: {launchersOldFolder}");
                                }
                                foreach (var file in existingFiles)
                                {
                                    string destFile = Path.Combine(launchersOldFolder, Path.GetFileName(file));
                                    if (File.Exists(destFile))
                                        File.Delete(destFile);
                                    File.Move(file, destFile);
                                    Log($"Backed up local launcher: {Path.GetFileName(file)}");
                                }
                            }
                        }
                    }));

                    if (Directory.Exists(downloadFolderLaunchers))
                    {
                        var downloadedFiles = Directory.GetFiles(downloadFolderLaunchers, "*" + launchersFileExtension);
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
                            string destFile = Path.Combine(localMinecraftLaunchersPath, Path.GetFileName(file));
                            File.Copy(file, destFile, true);
                            transferredCount++;
                            this.Invoke(new Action(() =>
                            {
                                progressBarSftp.Value = transferredCount;
                                Log($"Transferred launcher: {Path.GetFileName(file)}");
                            }));
                        }
                        this.Invoke(new Action(() =>
                        {
                            Log("Launchers transferred successfully!");
                            RefreshLocalLaunchersList();
                        }));
                    }
                    else
                    {
                        this.Invoke(new Action(() =>
                        {
                            Log("No downloaded launcher files found. Please download first.");
                        }));
                    }
                });
            }
            catch (Exception ex)
            {
                Log("Error downloading or transferring launchers: " + ex.Message);
            }
            finally
            {
                progressBarSftp.Visible = false;
                btnDownloadLaunchers.Enabled = !string.IsNullOrWhiteSpace(sftpUser) && !string.IsNullOrWhiteSpace(sftpPass);
            }
        }



        private void listViewServerLaunchers_ItemDrag(object sender, ItemDragEventArgs e)
        {
            var items = new List<string>();
            foreach (ListViewItem selectedItem in listViewServerLaunchers.SelectedItems)
            {
                if (selectedItem.Text.EndsWith(launchersFileExtension, StringComparison.OrdinalIgnoreCase))
                {
                    items.Add(selectedItem.Text);
                }
            }
            if (items.Count > 0)
            {
                DoDragDrop(items.ToArray(), DragDropEffects.Copy);
            }
        }

        private void listViewLocalLaunchers_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(string[])))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private async void listViewLocalLaunchers_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                var fileNames = (string[])e.Data.GetData(typeof(string[]));
                if (fileNames == null || fileNames.Length == 0) return;

                var jarFiles = new List<string>();
                foreach (var f in fileNames)
                {
                    if (f.EndsWith(launchersFileExtension, StringComparison.OrdinalIgnoreCase))
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
                            string remoteFile = sftpRemoteLaunchersPath.TrimEnd('/') + "/" + fileName;
                            string localFile = Path.Combine(localMinecraftLaunchersPath, fileName);
                            if (File.Exists(localFile))
                            {
                                string launchersOldFolder = Path.Combine(localMinecraftLaunchersPath, "launchers_old");
                                if (!Directory.Exists(launchersOldFolder))
                                    Directory.CreateDirectory(launchersOldFolder);
                                string backupFile = Path.Combine(launchersOldFolder, fileName);
                                if (File.Exists(backupFile))
                                    File.Delete(backupFile);
                                File.Move(localFile, backupFile);
                                this.Invoke(new Action(() => Log($"Backed up local launcher: {fileName}")));
                            }
                            using (var fs = new FileStream(localFile, FileMode.Create))
                            {
                                sftp.DownloadFile(remoteFile, fs);
                            }
                            transferredCount++;
                            this.Invoke(new Action(() =>
                            {
                                progressBarSftp.Value = transferredCount;
                                Log($"Transferred (drag-drop) launcher: {fileName}");
                            }));
                        }
                        sftp.Disconnect();
                    }
                    this.Invoke(new Action(() =>
                    {
                        Log($"Transferred {transferredCount} launcher(s) via drag-drop.");
                        RefreshLocalLaunchersList();
                    }));
                });
            }
            catch (Exception ex)
            {
                Log("Error transferring dragged launchers: " + ex.Message);
            }
            finally
            {
                progressBarSftp.Visible = false;
            }
        }

        private void btnOpenLocalLaunchers_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Directory.Exists(localMinecraftLaunchersPath))
                {
                    Directory.CreateDirectory(localMinecraftLaunchersPath);
                    Log($"Created local launchers folder: {localMinecraftLaunchersPath}");
                }
                Process.Start("explorer.exe", localMinecraftLaunchersPath);
                Log("Opened local launchers folder in Explorer.");
            }
            catch (Exception ex)
            {
                Log("Error opening local launchers folder: " + ex.Message);
            }
        }

        private void btnDeleteLocalLaunchers_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Directory.Exists(localMinecraftLaunchersPath))
                {
                    Log("No local launchers folder found.");
                    return;
                }
                if (listViewLocalLaunchers.SelectedItems.Count == 0)
                {
                    Log("No local launchers selected to delete.");
                    return;
                }
                foreach (ListViewItem item in listViewLocalLaunchers.SelectedItems)
                {
                    if (!item.Text.EndsWith(launchersFileExtension, StringComparison.OrdinalIgnoreCase))
                        continue;
                    string filePath = Path.Combine(localMinecraftLaunchersPath, item.Text);
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                        Log($"Deleted local launcher: {item.Text}");
                    }
                }
                RefreshLocalLaunchersList();
            }
            catch (Exception ex)
            {
                Log("Error deleting selected local launchers: " + ex.Message);
            }
        }

        private void btnDeleteAllLocalLaunchers_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Directory.Exists(localMinecraftLaunchersPath))
                {
                    Log("No local launchers folder found.");
                    return;
                }
                var files = Directory.GetFiles(localMinecraftLaunchersPath, "*" + launchersFileExtension);
                int deleteCount = 0;
                foreach (var file in files)
                {
                    File.Delete(file);
                    deleteCount++;
                    Log($"Deleted local launcher: {Path.GetFileName(file)}");
                }
                Log($"Deleted all local launchers. ({deleteCount} file(s))");
                RefreshLocalLaunchersList();
            }
            catch (Exception ex)
            {
                Log("Error deleting all local launchers: " + ex.Message);
            }
        }

        private void btnRefreshLocalLaunchers_Click(object sender, EventArgs e)
        {
            RefreshLocalLaunchersList();
        }

        #endregion

        private void tabPageMods_Click(object sender, EventArgs e)
        {

        }
    }
}
