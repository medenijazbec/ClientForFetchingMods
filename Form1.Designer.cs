namespace MinecraftServerHelper
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.ListView listViewServerMods;
        private System.Windows.Forms.ListView listViewLocalMods;
        private System.Windows.Forms.ImageList imageListMods;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.Button btnOpenLocalMods;
        private System.Windows.Forms.Label lblServerStatus;
        private System.Windows.Forms.Label lblServerMods;
        private System.Windows.Forms.Label lblLocalMods;
        private System.Windows.Forms.Button btnDeleteLocalMods;
        private System.Windows.Forms.Button btnRefreshLocalOnly;
        private System.Windows.Forms.Button btnDeleteAllLocalMods;
        private System.Windows.Forms.ProgressBar progressBarSftp;
        private System.Windows.Forms.RichTextBox rtbLog;
        private System.Windows.Forms.Label lblProgress;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            listViewServerMods = new ListView();
            imageListMods = new ImageList(components);
            listViewLocalMods = new ListView();
            btnConnect = new Button();
            btnRefresh = new Button();
            btnDownload = new Button();
            btnOpenLocalMods = new Button();
            lblServerStatus = new Label();
            lblServerMods = new Label();
            lblLocalMods = new Label();
            btnDeleteLocalMods = new Button();
            btnRefreshLocalOnly = new Button();
            btnDeleteAllLocalMods = new Button();
            progressBarSftp = new ProgressBar();
            rtbLog = new RichTextBox();
            lblProgress = new Label();
            richTextBox1 = new RichTextBox();
            SuspendLayout();
            // 
            // listViewServerMods
            // 
            listViewServerMods.LargeImageList = imageListMods;
            listViewServerMods.Location = new Point(12, 68);
            listViewServerMods.Name = "listViewServerMods";
            listViewServerMods.Size = new Size(360, 400);
            listViewServerMods.TabIndex = 1;
            listViewServerMods.UseCompatibleStateImageBehavior = false;
            listViewServerMods.View = View.Tile;
            listViewServerMods.ItemDrag += listViewServerMods_ItemDrag;
            // 
            // imageListMods
            // 
            imageListMods.ColorDepth = ColorDepth.Depth32Bit;
            imageListMods.ImageSize = new Size(32, 32);
            imageListMods.TransparentColor = Color.Transparent;
            // 
            // listViewLocalMods
            // 
            listViewLocalMods.AllowDrop = true;
            listViewLocalMods.LargeImageList = imageListMods;
            listViewLocalMods.Location = new Point(390, 68);
            listViewLocalMods.Name = "listViewLocalMods";
            listViewLocalMods.Size = new Size(360, 400);
            listViewLocalMods.TabIndex = 3;
            listViewLocalMods.UseCompatibleStateImageBehavior = false;
            listViewLocalMods.View = View.Tile;
            listViewLocalMods.DragDrop += listViewLocalMods_DragDrop;
            listViewLocalMods.DragEnter += listViewLocalMods_DragEnter;
            // 
            // btnConnect
            // 
            btnConnect.Location = new Point(12, 474);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new Size(140, 30);
            btnConnect.TabIndex = 4;
            btnConnect.Text = "Connect (SFTP)";
            btnConnect.UseVisualStyleBackColor = true;
            btnConnect.Click += btnConnect_Click;
            // 
            // btnRefresh
            // 
            btnRefresh.Location = new Point(12, 510);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(140, 30);
            btnRefresh.TabIndex = 5;
            btnRefresh.Text = "Refresh Both Lists";
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // btnDownload
            // 
            btnDownload.Location = new Point(12, 546);
            btnDownload.Name = "btnDownload";
            btnDownload.Size = new Size(140, 42);
            btnDownload.TabIndex = 6;
            btnDownload.Text = "Download and Transfer Mods";
            btnDownload.UseVisualStyleBackColor = true;
            btnDownload.Click += btnDownload_Click;
            // 
            // btnOpenLocalMods
            // 
            btnOpenLocalMods.Location = new Point(610, 598);
            btnOpenLocalMods.Name = "btnOpenLocalMods";
            btnOpenLocalMods.Size = new Size(140, 30);
            btnOpenLocalMods.TabIndex = 10;
            btnOpenLocalMods.Text = "Open Folder";
            btnOpenLocalMods.UseVisualStyleBackColor = true;
            btnOpenLocalMods.Click += btnOpenLocalMods_Click;
            // 
            // lblServerStatus
            // 
            lblServerStatus.AutoSize = true;
            lblServerStatus.Location = new Point(316, 8);
            lblServerStatus.Name = "lblServerStatus";
            lblServerStatus.Size = new Size(134, 15);
            lblServerStatus.TabIndex = 11;
            lblServerStatus.Text = "Minecraft Server Status: ";
            // 
            // lblServerMods
            // 
            lblServerMods.AutoSize = true;
            lblServerMods.Location = new Point(12, 50);
            lblServerMods.Name = "lblServerMods";
            lblServerMods.Size = new Size(127, 15);
            lblServerMods.TabIndex = 0;
            lblServerMods.Text = "Current server modlist:";
            // 
            // lblLocalMods
            // 
            lblLocalMods.AutoSize = true;
            lblLocalMods.Location = new Point(390, 50);
            lblLocalMods.Name = "lblLocalMods";
            lblLocalMods.Size = new Size(95, 15);
            lblLocalMods.TabIndex = 2;
            lblLocalMods.Text = "Your local mods:";
            // 
            // btnDeleteLocalMods
            // 
            btnDeleteLocalMods.Location = new Point(610, 475);
            btnDeleteLocalMods.Name = "btnDeleteLocalMods";
            btnDeleteLocalMods.Size = new Size(140, 43);
            btnDeleteLocalMods.TabIndex = 7;
            btnDeleteLocalMods.Text = "Delete Selected Local Mods";
            btnDeleteLocalMods.UseVisualStyleBackColor = true;
            btnDeleteLocalMods.Click += btnDeleteLocalMods_Click;
            // 
            // btnRefreshLocalOnly
            // 
            btnRefreshLocalOnly.Location = new Point(610, 524);
            btnRefreshLocalOnly.Name = "btnRefreshLocalOnly";
            btnRefreshLocalOnly.Size = new Size(140, 30);
            btnRefreshLocalOnly.TabIndex = 8;
            btnRefreshLocalOnly.Text = "Refresh Local Mods";
            btnRefreshLocalOnly.UseVisualStyleBackColor = true;
            btnRefreshLocalOnly.Click += btnRefreshLocalOnly_Click;
            // 
            // btnDeleteAllLocalMods
            // 
            btnDeleteAllLocalMods.Location = new Point(610, 562);
            btnDeleteAllLocalMods.Name = "btnDeleteAllLocalMods";
            btnDeleteAllLocalMods.Size = new Size(140, 30);
            btnDeleteAllLocalMods.TabIndex = 9;
            btnDeleteAllLocalMods.Text = "Delete All Local Mods";
            btnDeleteAllLocalMods.UseVisualStyleBackColor = true;
            btnDeleteAllLocalMods.Click += btnDeleteAllLocalMods_Click;
            // 
            // progressBarSftp
            // 
            progressBarSftp.Location = new Point(12, 26);
            progressBarSftp.Name = "progressBarSftp";
            progressBarSftp.Size = new Size(738, 23);
            progressBarSftp.TabIndex = 12;
            progressBarSftp.Visible = false;
            // 
            // rtbLog
            // 
            rtbLog.BorderStyle = BorderStyle.None;
            rtbLog.Location = new Point(158, 478);
            rtbLog.Name = "rtbLog";
            rtbLog.Size = new Size(446, 176);
            rtbLog.TabIndex = 13;
            rtbLog.Text = "";
            // 
            // lblProgress
            // 
            lblProgress.AutoSize = true;
            lblProgress.Location = new Point(12, 8);
            lblProgress.Name = "lblProgress";
            lblProgress.Size = new Size(52, 15);
            lblProgress.TabIndex = 14;
            lblProgress.Text = "Progress";
            // 
            // richTextBox1
            // 
            richTextBox1.BackColor = SystemColors.InactiveBorder;
            richTextBox1.BorderStyle = BorderStyle.None;
            richTextBox1.Location = new Point(12, 594);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(151, 102);
            richTextBox1.TabIndex = 16;
            richTextBox1.Text = "Downloads mods and moves them into .minecraft/mods folder";
            // 
            // Form1
            // 
            ClientSize = new Size(764, 654);
            Controls.Add(richTextBox1);
            Controls.Add(lblProgress);
            Controls.Add(rtbLog);
            Controls.Add(progressBarSftp);
            Controls.Add(btnOpenLocalMods);
            Controls.Add(btnDeleteAllLocalMods);
            Controls.Add(btnRefreshLocalOnly);
            Controls.Add(btnDeleteLocalMods);
            Controls.Add(lblServerStatus);
            Controls.Add(btnDownload);
            Controls.Add(btnRefresh);
            Controls.Add(btnConnect);
            Controls.Add(listViewLocalMods);
            Controls.Add(lblLocalMods);
            Controls.Add(listViewServerMods);
            Controls.Add(lblServerMods);
            Name = "Form1";
            Text = "Minecraft Mod Transfer";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private RichTextBox richTextBox1;
    }
}
