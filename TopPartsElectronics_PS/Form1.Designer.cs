namespace TopPartsElectronics_PS
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mainMenuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.productionStatusToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.productionInputToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.shippingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lotinfostatusStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.masterSetupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clientToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.makerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.productToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.materialToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.processToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.partsCompositionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.userToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.endProgramToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Font = new System.Drawing.Font("Bahnschrift Condensed", 12F);
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mainMenuToolStripMenuItem,
            this.masterSetupToolStripMenuItem,
            this.endProgramToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(6, 3, 0, 3);
            this.menuStrip1.Size = new System.Drawing.Size(1011, 34);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // mainMenuToolStripMenuItem
            // 
            this.mainMenuToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.productionStatusToolStripMenuItem,
            this.productionInputToolStripMenuItem,
            this.shippingToolStripMenuItem,
            this.lotinfostatusStripMenuItem});
            this.mainMenuToolStripMenuItem.Name = "mainMenuToolStripMenuItem";
            this.mainMenuToolStripMenuItem.Size = new System.Drawing.Size(92, 28);
            this.mainMenuToolStripMenuItem.Text = "Main Menu";
            // 
            // productionStatusToolStripMenuItem
            // 
            this.productionStatusToolStripMenuItem.Name = "productionStatusToolStripMenuItem";
            this.productionStatusToolStripMenuItem.Size = new System.Drawing.Size(233, 28);
            this.productionStatusToolStripMenuItem.Text = "Production Status";
            this.productionStatusToolStripMenuItem.Click += new System.EventHandler(this.productionStatusToolStripMenuItem_Click);
            // 
            // productionInputToolStripMenuItem
            // 
            this.productionInputToolStripMenuItem.Name = "productionInputToolStripMenuItem";
            this.productionInputToolStripMenuItem.Size = new System.Drawing.Size(233, 28);
            this.productionInputToolStripMenuItem.Text = "Production Input";
            this.productionInputToolStripMenuItem.Click += new System.EventHandler(this.productionInputToolStripMenuItem_Click);
            // 
            // shippingToolStripMenuItem
            // 
            this.shippingToolStripMenuItem.Name = "shippingToolStripMenuItem";
            this.shippingToolStripMenuItem.Size = new System.Drawing.Size(233, 28);
            this.shippingToolStripMenuItem.Text = "Shipping";
            this.shippingToolStripMenuItem.Click += new System.EventHandler(this.shippingToolStripMenuItem_Click);
            // 
            // lotinfostatusStripMenuItem
            // 
            this.lotinfostatusStripMenuItem.Name = "lotinfostatusStripMenuItem";
            this.lotinfostatusStripMenuItem.Size = new System.Drawing.Size(233, 28);
            this.lotinfostatusStripMenuItem.Text = "Lot Information Status";
            this.lotinfostatusStripMenuItem.Click += new System.EventHandler(this.lotinfostatusStripMenuItem_Click);
            // 
            // masterSetupToolStripMenuItem
            // 
            this.masterSetupToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clientToolStripMenuItem,
            this.makerToolStripMenuItem,
            this.productToolStripMenuItem,
            this.materialToolStripMenuItem,
            this.processToolStripMenuItem,
            this.partsCompositionToolStripMenuItem,
            this.userToolStripMenuItem});
            this.masterSetupToolStripMenuItem.Name = "masterSetupToolStripMenuItem";
            this.masterSetupToolStripMenuItem.Size = new System.Drawing.Size(109, 28);
            this.masterSetupToolStripMenuItem.Text = "Master Setup";
            // 
            // clientToolStripMenuItem
            // 
            this.clientToolStripMenuItem.Name = "clientToolStripMenuItem";
            this.clientToolStripMenuItem.Size = new System.Drawing.Size(141, 28);
            this.clientToolStripMenuItem.Text = "Client";
            this.clientToolStripMenuItem.Click += new System.EventHandler(this.clientToolStripMenuItem_Click);
            // 
            // makerToolStripMenuItem
            // 
            this.makerToolStripMenuItem.Name = "makerToolStripMenuItem";
            this.makerToolStripMenuItem.Size = new System.Drawing.Size(141, 28);
            this.makerToolStripMenuItem.Text = "Maker";
            this.makerToolStripMenuItem.Click += new System.EventHandler(this.makerToolStripMenuItem_Click);
            // 
            // productToolStripMenuItem
            // 
            this.productToolStripMenuItem.Name = "productToolStripMenuItem";
            this.productToolStripMenuItem.Size = new System.Drawing.Size(141, 28);
            this.productToolStripMenuItem.Text = "Product";
            this.productToolStripMenuItem.Click += new System.EventHandler(this.productToolStripMenuItem_Click);
            // 
            // materialToolStripMenuItem
            // 
            this.materialToolStripMenuItem.Name = "materialToolStripMenuItem";
            this.materialToolStripMenuItem.Size = new System.Drawing.Size(141, 28);
            this.materialToolStripMenuItem.Text = "Material";
            this.materialToolStripMenuItem.Click += new System.EventHandler(this.materialToolStripMenuItem_Click);
            // 
            // processToolStripMenuItem
            // 
            this.processToolStripMenuItem.Name = "processToolStripMenuItem";
            this.processToolStripMenuItem.Size = new System.Drawing.Size(141, 28);
            this.processToolStripMenuItem.Text = "Process";
            this.processToolStripMenuItem.Click += new System.EventHandler(this.processToolStripMenuItem_Click);
            // 
            // partsCompositionToolStripMenuItem
            // 
            this.partsCompositionToolStripMenuItem.Name = "partsCompositionToolStripMenuItem";
            this.partsCompositionToolStripMenuItem.Size = new System.Drawing.Size(141, 28);
            this.partsCompositionToolStripMenuItem.Text = "BOM";
            this.partsCompositionToolStripMenuItem.Click += new System.EventHandler(this.partsCompositionToolStripMenuItem_Click);
            // 
            // userToolStripMenuItem
            // 
            this.userToolStripMenuItem.Name = "userToolStripMenuItem";
            this.userToolStripMenuItem.Size = new System.Drawing.Size(141, 28);
            this.userToolStripMenuItem.Text = "User";
            this.userToolStripMenuItem.Click += new System.EventHandler(this.userToolStripMenuItem_Click);
            // 
            // endProgramToolStripMenuItem
            // 
            this.endProgramToolStripMenuItem.Name = "endProgramToolStripMenuItem";
            this.endProgramToolStripMenuItem.Size = new System.Drawing.Size(47, 28);
            this.endProgramToolStripMenuItem.Text = "Exit";
            this.endProgramToolStripMenuItem.Click += new System.EventHandler(this.endProgramToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImage = global::TopPartsElectronics_PS.Properties.Resources.top_logo_2;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(1011, 647);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Bahnschrift Condensed", 12F);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Name = "Form1";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TOP PARTS MALAYSIA PRODUCTION SYSTEM - Last Updated Date : 07-09-2023 ";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mainMenuToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem masterSetupToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem endProgramToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem userToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem clientToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem productToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem materialToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem processToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem partsCompositionToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem makerToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem productionStatusToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem productionInputToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem shippingToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem lotinfostatusStripMenuItem;
    }
}

