namespace MinecraftServerHelper
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        // Global controls
        private System.Windows.Forms.ImageList imageListCategories;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.ProgressBar progressBarSftp;
        private System.Windows.Forms.RichTextBox rtbLog;
        private System.Windows.Forms.Label lblServerStatus;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.TabControl tabControlCategories;
        private System.Windows.Forms.TabPage tabPageMods;
        private System.Windows.Forms.TabPage tabPageShaderpacks;
        private System.Windows.Forms.TabPage tabPageLaunchers;

        // Controls for Mods tab
        private System.Windows.Forms.ListView listViewServerMods;
        private System.Windows.Forms.ListView listViewLocalMods;
        private System.Windows.Forms.Label lblServerMods;
        private System.Windows.Forms.Label lblLocalMods;
        private System.Windows.Forms.Button btnDownloadMods;
        private System.Windows.Forms.Button btnOpenLocalMods;
        private System.Windows.Forms.Button btnDeleteLocalMods;
        private System.Windows.Forms.Button btnRefreshLocalMods;
        private System.Windows.Forms.Button btnDeleteAllLocalMods;

        // Controls for Shaderpacks tab
        private System.Windows.Forms.ListView listViewServerShaderpacks;
        private System.Windows.Forms.ListView listViewLocalShaderpacks;
        private System.Windows.Forms.Label lblServerShaderpacks;
        private System.Windows.Forms.Label lblLocalShaderpacks;
        private System.Windows.Forms.Button btnDownloadShaderpacks;
        private System.Windows.Forms.Button btnOpenLocalShaderpacks;
        private System.Windows.Forms.Button btnDeleteLocalShaderpacks;
        private System.Windows.Forms.Button btnRefreshLocalShaderpacks;
        private System.Windows.Forms.Button btnDeleteAllLocalShaderpacks;

        // Controls for Launchers tab
        private System.Windows.Forms.ListView listViewServerLaunchers;
        private System.Windows.Forms.ListView listViewLocalLaunchers;
        private System.Windows.Forms.Label lblServerLaunchers;
        private System.Windows.Forms.Label lblLocalLaunchers;
        private System.Windows.Forms.Button btnDownloadLaunchers;
        private System.Windows.Forms.Button btnOpenLocalLaunchers;
        private System.Windows.Forms.Button btnDeleteLocalLaunchers;
        private System.Windows.Forms.Button btnRefreshLocalLaunchers;
        private System.Windows.Forms.Button btnDeleteAllLocalLaunchers;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            imageListCategories = new ImageList(components);
            btnConnect = new Button();
            btnRefresh = new Button();
            progressBarSftp = new ProgressBar();
            rtbLog = new RichTextBox();
            lblServerStatus = new Label();
            lblProgress = new Label();
            tabControlCategories = new TabControl();
            tabPageMods = new TabPage();
            listViewServerMods = new ListView();
            listViewLocalMods = new ListView();
            lblServerMods = new Label();
            lblLocalMods = new Label();
            btnDownloadMods = new Button();
            btnOpenLocalMods = new Button();
            btnDeleteLocalMods = new Button();
            btnRefreshLocalMods = new Button();
            btnDeleteAllLocalMods = new Button();
            tabPageShaderpacks = new TabPage();
            listViewServerShaderpacks = new ListView();
            listViewLocalShaderpacks = new ListView();
            lblServerShaderpacks = new Label();
            lblLocalShaderpacks = new Label();
            btnDownloadShaderpacks = new Button();
            btnOpenLocalShaderpacks = new Button();
            btnDeleteLocalShaderpacks = new Button();
            btnRefreshLocalShaderpacks = new Button();
            btnDeleteAllLocalShaderpacks = new Button();
            tabPageLaunchers = new TabPage();
            listViewServerLaunchers = new ListView();
            listViewLocalLaunchers = new ListView();
            lblServerLaunchers = new Label();
            lblLocalLaunchers = new Label();
            btnDownloadLaunchers = new Button();
            btnOpenLocalLaunchers = new Button();
            btnDeleteLocalLaunchers = new Button();
            btnRefreshLocalLaunchers = new Button();
            btnDeleteAllLocalLaunchers = new Button();
            tabControlCategories.SuspendLayout();
            tabPageMods.SuspendLayout();
            tabPageShaderpacks.SuspendLayout();
            tabPageLaunchers.SuspendLayout();
            SuspendLayout();
            // 
            // imageListCategories
            // 
            imageListCategories.ColorDepth = ColorDepth.Depth32Bit;
            imageListCategories.ImageSize = new Size(16, 16);
            imageListCategories.TransparentColor = Color.Transparent;
            // 
            // btnConnect
            // 
            btnConnect.Location = new Point(12, 500);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new Size(140, 30);
            btnConnect.TabIndex = 1;
            btnConnect.Text = "Connect (SFTP)";
            btnConnect.UseVisualStyleBackColor = true;
            btnConnect.Click += btnConnect_Click;
            // 
            // btnRefresh
            // 
            btnRefresh.Location = new Point(12, 535);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(140, 30);
            btnRefresh.TabIndex = 2;
            btnRefresh.Text = "Refresh All Tabs";
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // progressBarSftp
            // 
            progressBarSftp.Location = new Point(12, 41);
            progressBarSftp.Name = "progressBarSftp";
            progressBarSftp.Size = new Size(760, 23);
            progressBarSftp.TabIndex = 3;
            progressBarSftp.Visible = false;
            // 
            // rtbLog
            // 
            rtbLog.Location = new Point(170, 500);
            rtbLog.Name = "rtbLog";
            rtbLog.Size = new Size(400, 100);
            rtbLog.TabIndex = 4;
            rtbLog.Text = "";
            // 
            // lblServerStatus
            // 
            lblServerStatus.AutoSize = true;
            lblServerStatus.Location = new Point(576, 500);
            lblServerStatus.Name = "lblServerStatus";
            lblServerStatus.Size = new Size(131, 15);
            lblServerStatus.TabIndex = 5;
            lblServerStatus.Text = "Minecraft Server Status:";
            // 
            // lblProgress
            // 
            lblProgress.AutoSize = true;
            lblProgress.Location = new Point(12, 23);
            lblProgress.Name = "lblProgress";
            lblProgress.Size = new Size(52, 15);
            lblProgress.TabIndex = 6;
            lblProgress.Text = "Progress";
            // 
            // tabControlCategories
            // 
            tabControlCategories.Controls.Add(tabPageMods);
            tabControlCategories.Controls.Add(tabPageShaderpacks);
            tabControlCategories.Controls.Add(tabPageLaunchers);
            tabControlCategories.Location = new Point(12, 70);
            tabControlCategories.Name = "tabControlCategories";
            tabControlCategories.SelectedIndex = 0;
            tabControlCategories.Size = new Size(760, 420);
            tabControlCategories.TabIndex = 0;
            // 
            // tabPageMods
            // 
            tabPageMods.Controls.Add(listViewServerMods);
            tabPageMods.Controls.Add(listViewLocalMods);
            tabPageMods.Controls.Add(lblServerMods);
            tabPageMods.Controls.Add(lblLocalMods);
            tabPageMods.Controls.Add(btnDownloadMods);
            tabPageMods.Controls.Add(btnOpenLocalMods);
            tabPageMods.Controls.Add(btnDeleteLocalMods);
            tabPageMods.Controls.Add(btnRefreshLocalMods);
            tabPageMods.Controls.Add(btnDeleteAllLocalMods);
            tabPageMods.Location = new Point(4, 24);
            tabPageMods.Name = "tabPageMods";
            tabPageMods.Size = new Size(752, 392);
            tabPageMods.TabIndex = 0;
            tabPageMods.Text = "Mods";
            // 
            // listViewServerMods
            // 
            listViewServerMods.LargeImageList = imageListCategories;
            listViewServerMods.Location = new Point(6, 30);
            listViewServerMods.Name = "listViewServerMods";
            listViewServerMods.Size = new Size(360, 300);
            listViewServerMods.TabIndex = 0;
            listViewServerMods.UseCompatibleStateImageBehavior = false;
            listViewServerMods.View = View.Tile;
            listViewServerMods.ItemDrag += listViewServerMods_ItemDrag;
            // 
            // listViewLocalMods
            // 
            listViewLocalMods.AllowDrop = true;
            listViewLocalMods.LargeImageList = imageListCategories;
            listViewLocalMods.Location = new Point(372, 30);
            listViewLocalMods.Name = "listViewLocalMods";
            listViewLocalMods.Size = new Size(360, 300);
            listViewLocalMods.TabIndex = 1;
            listViewLocalMods.UseCompatibleStateImageBehavior = false;
            listViewLocalMods.View = View.Tile;
            listViewLocalMods.DragDrop += listViewLocalMods_DragDrop;
            listViewLocalMods.DragEnter += listViewLocalMods_DragEnter;
            // 
            // lblServerMods
            // 
            lblServerMods.AutoSize = true;
            lblServerMods.Location = new Point(6, 10);
            lblServerMods.Name = "lblServerMods";
            lblServerMods.Size = new Size(75, 15);
            lblServerMods.TabIndex = 2;
            lblServerMods.Text = "Server Mods:";
            // 
            // lblLocalMods
            // 
            lblLocalMods.AutoSize = true;
            lblLocalMods.Location = new Point(372, 10);
            lblLocalMods.Name = "lblLocalMods";
            lblLocalMods.Size = new Size(71, 15);
            lblLocalMods.TabIndex = 3;
            lblLocalMods.Text = "Local Mods:";
            // 
            // btnDownloadMods
            // 
            btnDownloadMods.Location = new Point(6, 340);
            btnDownloadMods.Name = "btnDownloadMods";
            btnDownloadMods.Size = new Size(140, 30);
            btnDownloadMods.TabIndex = 4;
            btnDownloadMods.Text = "Download Mods";
            btnDownloadMods.UseVisualStyleBackColor = true;
            btnDownloadMods.Click += btnDownloadMods_Click;
            // 
            // btnOpenLocalMods
            // 
            btnOpenLocalMods.Location = new Point(158, 340);
            btnOpenLocalMods.Name = "btnOpenLocalMods";
            btnOpenLocalMods.Size = new Size(140, 30);
            btnOpenLocalMods.TabIndex = 5;
            btnOpenLocalMods.Text = "Open Folder";
            btnOpenLocalMods.UseVisualStyleBackColor = true;
            btnOpenLocalMods.Click += btnOpenLocalMods_Click;
            // 
            // btnDeleteLocalMods
            // 
            btnDeleteLocalMods.Location = new Point(310, 340);
            btnDeleteLocalMods.Name = "btnDeleteLocalMods";
            btnDeleteLocalMods.Size = new Size(140, 30);
            btnDeleteLocalMods.TabIndex = 6;
            btnDeleteLocalMods.Text = "Delete Selected";
            btnDeleteLocalMods.UseVisualStyleBackColor = true;
            btnDeleteLocalMods.Click += btnDeleteLocalMods_Click;
            // 
            // btnRefreshLocalMods
            // 
            btnRefreshLocalMods.Location = new Point(462, 340);
            btnRefreshLocalMods.Name = "btnRefreshLocalMods";
            btnRefreshLocalMods.Size = new Size(140, 30);
            btnRefreshLocalMods.TabIndex = 7;
            btnRefreshLocalMods.Text = "Refresh Local";
            btnRefreshLocalMods.UseVisualStyleBackColor = true;
            btnRefreshLocalMods.Click += btnRefreshLocalMods_Click;
            // 
            // btnDeleteAllLocalMods
            // 
            btnDeleteAllLocalMods.Location = new Point(614, 340);
            btnDeleteAllLocalMods.Name = "btnDeleteAllLocalMods";
            btnDeleteAllLocalMods.Size = new Size(118, 30);
            btnDeleteAllLocalMods.TabIndex = 8;
            btnDeleteAllLocalMods.Text = "Delete All";
            btnDeleteAllLocalMods.UseVisualStyleBackColor = true;
            btnDeleteAllLocalMods.Click += btnDeleteAllLocalMods_Click;
            // 
            // tabPageShaderpacks
            // 
            tabPageShaderpacks.Controls.Add(listViewServerShaderpacks);
            tabPageShaderpacks.Controls.Add(listViewLocalShaderpacks);
            tabPageShaderpacks.Controls.Add(lblServerShaderpacks);
            tabPageShaderpacks.Controls.Add(lblLocalShaderpacks);
            tabPageShaderpacks.Controls.Add(btnDownloadShaderpacks);
            tabPageShaderpacks.Controls.Add(btnOpenLocalShaderpacks);
            tabPageShaderpacks.Controls.Add(btnDeleteLocalShaderpacks);
            tabPageShaderpacks.Controls.Add(btnRefreshLocalShaderpacks);
            tabPageShaderpacks.Controls.Add(btnDeleteAllLocalShaderpacks);
            tabPageShaderpacks.Location = new Point(4, 24);
            tabPageShaderpacks.Name = "tabPageShaderpacks";
            tabPageShaderpacks.Size = new Size(752, 392);
            tabPageShaderpacks.TabIndex = 1;
            tabPageShaderpacks.Text = "Shaderpacks";
            // 
            // listViewServerShaderpacks
            // 
            listViewServerShaderpacks.LargeImageList = imageListCategories;
            listViewServerShaderpacks.Location = new Point(6, 30);
            listViewServerShaderpacks.Name = "listViewServerShaderpacks";
            listViewServerShaderpacks.Size = new Size(360, 300);
            listViewServerShaderpacks.TabIndex = 0;
            listViewServerShaderpacks.UseCompatibleStateImageBehavior = false;
            listViewServerShaderpacks.View = View.Tile;
            listViewServerShaderpacks.ItemDrag += listViewServerShaderpacks_ItemDrag;
            // 
            // listViewLocalShaderpacks
            // 
            listViewLocalShaderpacks.AllowDrop = true;
            listViewLocalShaderpacks.LargeImageList = imageListCategories;
            listViewLocalShaderpacks.Location = new Point(372, 30);
            listViewLocalShaderpacks.Name = "listViewLocalShaderpacks";
            listViewLocalShaderpacks.Size = new Size(360, 300);
            listViewLocalShaderpacks.TabIndex = 1;
            listViewLocalShaderpacks.UseCompatibleStateImageBehavior = false;
            listViewLocalShaderpacks.View = View.Tile;
            listViewLocalShaderpacks.DragDrop += listViewLocalShaderpacks_DragDrop;
            listViewLocalShaderpacks.DragEnter += listViewLocalShaderpacks_DragEnter;
            // 
            // lblServerShaderpacks
            // 
            lblServerShaderpacks.AutoSize = true;
            lblServerShaderpacks.Location = new Point(6, 10);
            lblServerShaderpacks.Name = "lblServerShaderpacks";
            lblServerShaderpacks.Size = new Size(111, 15);
            lblServerShaderpacks.TabIndex = 2;
            lblServerShaderpacks.Text = "Server Shaderpacks:";
            // 
            // lblLocalShaderpacks
            // 
            lblLocalShaderpacks.AutoSize = true;
            lblLocalShaderpacks.Location = new Point(372, 10);
            lblLocalShaderpacks.Name = "lblLocalShaderpacks";
            lblLocalShaderpacks.Size = new Size(107, 15);
            lblLocalShaderpacks.TabIndex = 3;
            lblLocalShaderpacks.Text = "Local Shaderpacks:";
            // 
            // btnDownloadShaderpacks
            // 
            btnDownloadShaderpacks.Location = new Point(6, 340);
            btnDownloadShaderpacks.Name = "btnDownloadShaderpacks";
            btnDownloadShaderpacks.Size = new Size(140, 30);
            btnDownloadShaderpacks.TabIndex = 4;
            btnDownloadShaderpacks.Text = "Download";
            btnDownloadShaderpacks.UseVisualStyleBackColor = true;
            btnDownloadShaderpacks.Click += btnDownloadShaderpacks_Click;
            // 
            // btnOpenLocalShaderpacks
            // 
            btnOpenLocalShaderpacks.Location = new Point(158, 340);
            btnOpenLocalShaderpacks.Name = "btnOpenLocalShaderpacks";
            btnOpenLocalShaderpacks.Size = new Size(140, 30);
            btnOpenLocalShaderpacks.TabIndex = 5;
            btnOpenLocalShaderpacks.Text = "Open Folder";
            btnOpenLocalShaderpacks.UseVisualStyleBackColor = true;
            btnOpenLocalShaderpacks.Click += btnOpenLocalShaderpacks_Click;
            // 
            // btnDeleteLocalShaderpacks
            // 
            btnDeleteLocalShaderpacks.Location = new Point(310, 340);
            btnDeleteLocalShaderpacks.Name = "btnDeleteLocalShaderpacks";
            btnDeleteLocalShaderpacks.Size = new Size(140, 30);
            btnDeleteLocalShaderpacks.TabIndex = 6;
            btnDeleteLocalShaderpacks.Text = "Delete Selected";
            btnDeleteLocalShaderpacks.UseVisualStyleBackColor = true;
            btnDeleteLocalShaderpacks.Click += btnDeleteLocalShaderpacks_Click;
            // 
            // btnRefreshLocalShaderpacks
            // 
            btnRefreshLocalShaderpacks.Location = new Point(462, 340);
            btnRefreshLocalShaderpacks.Name = "btnRefreshLocalShaderpacks";
            btnRefreshLocalShaderpacks.Size = new Size(140, 30);
            btnRefreshLocalShaderpacks.TabIndex = 7;
            btnRefreshLocalShaderpacks.Text = "Refresh Local";
            btnRefreshLocalShaderpacks.UseVisualStyleBackColor = true;
            btnRefreshLocalShaderpacks.Click += btnRefreshLocalShaderpacks_Click;
            // 
            // btnDeleteAllLocalShaderpacks
            // 
            btnDeleteAllLocalShaderpacks.Location = new Point(614, 340);
            btnDeleteAllLocalShaderpacks.Name = "btnDeleteAllLocalShaderpacks";
            btnDeleteAllLocalShaderpacks.Size = new Size(118, 30);
            btnDeleteAllLocalShaderpacks.TabIndex = 8;
            btnDeleteAllLocalShaderpacks.Text = "Delete All";
            btnDeleteAllLocalShaderpacks.UseVisualStyleBackColor = true;
            btnDeleteAllLocalShaderpacks.Click += btnDeleteAllLocalShaderpacks_Click;
            // 
            // tabPageLaunchers
            // 
            tabPageLaunchers.Controls.Add(listViewServerLaunchers);
            tabPageLaunchers.Controls.Add(listViewLocalLaunchers);
            tabPageLaunchers.Controls.Add(lblServerLaunchers);
            tabPageLaunchers.Controls.Add(lblLocalLaunchers);
            tabPageLaunchers.Controls.Add(btnDownloadLaunchers);
            tabPageLaunchers.Controls.Add(btnOpenLocalLaunchers);
            tabPageLaunchers.Controls.Add(btnDeleteLocalLaunchers);
            tabPageLaunchers.Controls.Add(btnRefreshLocalLaunchers);
            tabPageLaunchers.Controls.Add(btnDeleteAllLocalLaunchers);
            tabPageLaunchers.Location = new Point(4, 24);
            tabPageLaunchers.Name = "tabPageLaunchers";
            tabPageLaunchers.Size = new Size(752, 392);
            tabPageLaunchers.TabIndex = 2;
            tabPageLaunchers.Text = "Launchers";
            // 
            // listViewServerLaunchers
            // 
            listViewServerLaunchers.LargeImageList = imageListCategories;
            listViewServerLaunchers.Location = new Point(6, 30);
            listViewServerLaunchers.Name = "listViewServerLaunchers";
            listViewServerLaunchers.Size = new Size(360, 300);
            listViewServerLaunchers.TabIndex = 0;
            listViewServerLaunchers.UseCompatibleStateImageBehavior = false;
            listViewServerLaunchers.View = View.Tile;
            listViewServerLaunchers.ItemDrag += listViewServerLaunchers_ItemDrag;
            // 
            // listViewLocalLaunchers
            // 
            listViewLocalLaunchers.AllowDrop = true;
            listViewLocalLaunchers.LargeImageList = imageListCategories;
            listViewLocalLaunchers.Location = new Point(372, 30);
            listViewLocalLaunchers.Name = "listViewLocalLaunchers";
            listViewLocalLaunchers.Size = new Size(360, 300);
            listViewLocalLaunchers.TabIndex = 1;
            listViewLocalLaunchers.UseCompatibleStateImageBehavior = false;
            listViewLocalLaunchers.View = View.Tile;
            listViewLocalLaunchers.DragDrop += listViewLocalLaunchers_DragDrop;
            listViewLocalLaunchers.DragEnter += listViewLocalLaunchers_DragEnter;
            // 
            // lblServerLaunchers
            // 
            lblServerLaunchers.AutoSize = true;
            lblServerLaunchers.Location = new Point(6, 10);
            lblServerLaunchers.Name = "lblServerLaunchers";
            lblServerLaunchers.Size = new Size(99, 15);
            lblServerLaunchers.TabIndex = 2;
            lblServerLaunchers.Text = "Server Launchers:";
            // 
            // lblLocalLaunchers
            // 
            lblLocalLaunchers.AutoSize = true;
            lblLocalLaunchers.Location = new Point(372, 10);
            lblLocalLaunchers.Name = "lblLocalLaunchers";
            lblLocalLaunchers.Size = new Size(95, 15);
            lblLocalLaunchers.TabIndex = 3;
            lblLocalLaunchers.Text = "Local Launchers:";
            // 
            // btnDownloadLaunchers
            // 
            btnDownloadLaunchers.Location = new Point(6, 340);
            btnDownloadLaunchers.Name = "btnDownloadLaunchers";
            btnDownloadLaunchers.Size = new Size(140, 30);
            btnDownloadLaunchers.TabIndex = 4;
            btnDownloadLaunchers.Text = "Download";
            btnDownloadLaunchers.UseVisualStyleBackColor = true;
            btnDownloadLaunchers.Click += btnDownloadLaunchers_Click;
            // 
            // btnOpenLocalLaunchers
            // 
            btnOpenLocalLaunchers.Location = new Point(158, 340);
            btnOpenLocalLaunchers.Name = "btnOpenLocalLaunchers";
            btnOpenLocalLaunchers.Size = new Size(140, 30);
            btnOpenLocalLaunchers.TabIndex = 5;
            btnOpenLocalLaunchers.Text = "Open Folder";
            btnOpenLocalLaunchers.UseVisualStyleBackColor = true;
            btnOpenLocalLaunchers.Click += btnOpenLocalLaunchers_Click;
            // 
            // btnDeleteLocalLaunchers
            // 
            btnDeleteLocalLaunchers.Location = new Point(310, 340);
            btnDeleteLocalLaunchers.Name = "btnDeleteLocalLaunchers";
            btnDeleteLocalLaunchers.Size = new Size(140, 30);
            btnDeleteLocalLaunchers.TabIndex = 6;
            btnDeleteLocalLaunchers.Text = "Delete Selected";
            btnDeleteLocalLaunchers.UseVisualStyleBackColor = true;
            btnDeleteLocalLaunchers.Click += btnDeleteLocalLaunchers_Click;
            // 
            // btnRefreshLocalLaunchers
            // 
            btnRefreshLocalLaunchers.Location = new Point(462, 340);
            btnRefreshLocalLaunchers.Name = "btnRefreshLocalLaunchers";
            btnRefreshLocalLaunchers.Size = new Size(140, 30);
            btnRefreshLocalLaunchers.TabIndex = 7;
            btnRefreshLocalLaunchers.Text = "Refresh Local";
            btnRefreshLocalLaunchers.UseVisualStyleBackColor = true;
            btnRefreshLocalLaunchers.Click += btnRefreshLocalLaunchers_Click;
            // 
            // btnDeleteAllLocalLaunchers
            // 
            btnDeleteAllLocalLaunchers.Location = new Point(614, 340);
            btnDeleteAllLocalLaunchers.Name = "btnDeleteAllLocalLaunchers";
            btnDeleteAllLocalLaunchers.Size = new Size(118, 30);
            btnDeleteAllLocalLaunchers.TabIndex = 8;
            btnDeleteAllLocalLaunchers.Text = "Delete All";
            btnDeleteAllLocalLaunchers.UseVisualStyleBackColor = true;
            btnDeleteAllLocalLaunchers.Click += btnDeleteAllLocalLaunchers_Click;
            // 
            // Form1
            // 
            ClientSize = new Size(784, 607);
            Controls.Add(tabControlCategories);
            Controls.Add(btnConnect);
            Controls.Add(btnRefresh);
            Controls.Add(progressBarSftp);
            Controls.Add(rtbLog);
            Controls.Add(lblServerStatus);
            Controls.Add(lblProgress);
            Name = "Form1";
            Text = "Minecraft Server Helper";
            Load += Form1_Load;
            tabControlCategories.ResumeLayout(false);
            tabPageMods.ResumeLayout(false);
            tabPageMods.PerformLayout();
            tabPageShaderpacks.ResumeLayout(false);
            tabPageShaderpacks.PerformLayout();
            tabPageLaunchers.ResumeLayout(false);
            tabPageLaunchers.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
    }
}
