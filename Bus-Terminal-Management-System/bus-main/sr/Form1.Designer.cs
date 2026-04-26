namespace sr
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Timer timer1;
        private Bunifu.UI.WinForms.BunifuGradientPanel bunifuGradientPanel1;
        private System.Windows.Forms.Panel panelContent;
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.Panel panelRight;
        private System.Windows.Forms.PictureBox pictureBoxBus;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblLoading;
        private CircularProgressBar.CircularProgressBar progressbar;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.bunifuGradientPanel1 = new Bunifu.UI.WinForms.BunifuGradientPanel();
            this.panelContent = new System.Windows.Forms.Panel();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.pictureBoxBus = new System.Windows.Forms.PictureBox();
            this.panelRight = new System.Windows.Forms.Panel();
            this.lblLoading = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.progressbar = new CircularProgressBar.CircularProgressBar();

            this.bunifuGradientPanel1.SuspendLayout();
            this.panelContent.SuspendLayout();
            this.panelLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBus)).BeginInit();
            this.panelRight.SuspendLayout();
            this.SuspendLayout();

            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 35;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);

            // 
            // bunifuGradientPanel1
            // 
            this.bunifuGradientPanel1.BackColor = System.Drawing.Color.Transparent;
            this.bunifuGradientPanel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bunifuGradientPanel1.BorderRadius = 1;
            this.bunifuGradientPanel1.Controls.Add(this.panelContent);
            this.bunifuGradientPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bunifuGradientPanel1.GradientBottomLeft = System.Drawing.Color.FromArgb(11, 24, 38);
            this.bunifuGradientPanel1.GradientBottomRight = System.Drawing.Color.FromArgb(19, 58, 84);
            this.bunifuGradientPanel1.GradientTopLeft = System.Drawing.Color.FromArgb(22, 34, 50);
            this.bunifuGradientPanel1.GradientTopRight = System.Drawing.Color.FromArgb(52, 152, 219);
            this.bunifuGradientPanel1.Location = new System.Drawing.Point(0, 0);
            this.bunifuGradientPanel1.Name = "bunifuGradientPanel1";
            this.bunifuGradientPanel1.Quality = 10;
            this.bunifuGradientPanel1.Size = new System.Drawing.Size(1000, 500);
            this.bunifuGradientPanel1.TabIndex = 0;
            this.bunifuGradientPanel1.Click += new System.EventHandler(this.bunifuGradientPanel1_Click);

            // 
            // panelContent
            // 
            this.panelContent.BackColor = System.Drawing.Color.Transparent;
            this.panelContent.Controls.Add(this.panelLeft);
            this.panelContent.Controls.Add(this.panelRight);
            this.panelContent.Location = new System.Drawing.Point(35, 35);
            this.panelContent.Name = "panelContent";
            this.panelContent.Size = new System.Drawing.Size(930, 430);
            this.panelContent.TabIndex = 0;

            // 
            // panelLeft
            // 
            this.panelLeft.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            this.panelLeft.Controls.Add(this.pictureBoxBus);
            this.panelLeft.Location = new System.Drawing.Point(0, 0);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(535, 430);
            this.panelLeft.TabIndex = 0;

            // 
            // pictureBoxBus
            // 
            this.pictureBoxBus.BackColor = System.Drawing.Color.White;
            this.pictureBoxBus.Location = new System.Drawing.Point(25, 35);
            this.pictureBoxBus.Name = "pictureBoxBus";
            this.pictureBoxBus.Size = new System.Drawing.Size(485, 360);
            this.pictureBoxBus.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxBus.TabIndex = 0;
            this.pictureBoxBus.TabStop = false;
            this.pictureBoxBus.Click += new System.EventHandler(this.pictureBoxBus_Click);

            // 
            // panelRight
            // 
            this.panelRight.BackColor = System.Drawing.Color.FromArgb(22, 34, 50);
            this.panelRight.Controls.Add(this.lblLoading);
            this.panelRight.Controls.Add(this.label3);
            this.panelRight.Controls.Add(this.label2);
            this.panelRight.Controls.Add(this.label1);
            this.panelRight.Controls.Add(this.progressbar);
            this.panelRight.Location = new System.Drawing.Point(535, 0);
            this.panelRight.Name = "panelRight";
            this.panelRight.Size = new System.Drawing.Size(395, 430);
            this.panelRight.TabIndex = 1;

            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Segoe UI", 25F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(30, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(335, 55);
            this.label1.TabIndex = 0;
            this.label1.Text = "Welcome to BTMS";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.FromArgb(52, 152, 219);
            this.label2.Location = new System.Drawing.Point(25, 108);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(345, 35);
            this.label2.TabIndex = 1;
            this.label2.Text = "Bus Terminal Management System";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.label3.ForeColor = System.Drawing.Color.Gainsboro;
            this.label3.Location = new System.Drawing.Point(45, 150);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(305, 55);
            this.label3.TabIndex = 2;
            this.label3.Text = "Loading system modules, schedules, reservations, payments, and reports...";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // 
            // progressbar
            // 
            this.progressbar.AnimationFunction = WinFormAnimation.KnownAnimationFunctions.Liner;
            this.progressbar.AnimationSpeed = 500;
            this.progressbar.BackColor = System.Drawing.Color.Transparent;
            this.progressbar.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold);
            this.progressbar.ForeColor = System.Drawing.Color.White;
            this.progressbar.InnerColor = System.Drawing.Color.FromArgb(22, 34, 50);
            this.progressbar.InnerMargin = 2;
            this.progressbar.InnerWidth = -1;
            this.progressbar.Location = new System.Drawing.Point(125, 225);
            this.progressbar.MarqueeAnimationSpeed = 2000;
            this.progressbar.Name = "progressbar";
            this.progressbar.OuterColor = System.Drawing.Color.FromArgb(12, 24, 35);
            this.progressbar.OuterMargin = -25;
            this.progressbar.OuterWidth = 26;
            this.progressbar.ProgressColor = System.Drawing.Color.FromArgb(52, 152, 219);
            this.progressbar.ProgressWidth = 10;
            this.progressbar.SecondaryFont = new System.Drawing.Font("Segoe UI", 36F);
            this.progressbar.Size = new System.Drawing.Size(145, 145);
            this.progressbar.StartAngle = 270;
            this.progressbar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressbar.SubscriptColor = System.Drawing.Color.Gainsboro;
            this.progressbar.SubscriptMargin = new System.Windows.Forms.Padding(10, -35, 0, 0);
            this.progressbar.SubscriptText = "";
            this.progressbar.SuperscriptColor = System.Drawing.Color.Gainsboro;
            this.progressbar.SuperscriptMargin = new System.Windows.Forms.Padding(10, 35, 0, 0);
            this.progressbar.SuperscriptText = "";
            this.progressbar.TabIndex = 3;
            this.progressbar.Text = "0%";
            this.progressbar.TextMargin = new System.Windows.Forms.Padding(8, 8, 0, 0);
            this.progressbar.Value = 0;

            // 
            // lblLoading
            // 
            this.lblLoading.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Italic);
            this.lblLoading.ForeColor = System.Drawing.Color.FromArgb(190, 205, 220);
            this.lblLoading.Location = new System.Drawing.Point(45, 378);
            this.lblLoading.Name = "lblLoading";
            this.lblLoading.Size = new System.Drawing.Size(305, 25);
            this.lblLoading.TabIndex = 4;
            this.lblLoading.Text = "Please wait...";
            this.lblLoading.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(22, 34, 50);
            this.ClientSize = new System.Drawing.Size(1000, 500);
            this.Controls.Add(this.bunifuGradientPanel1);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BTMS Loading";
            this.Load += new System.EventHandler(this.Form1_Load);

            this.panelRight.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBus)).EndInit();
            this.panelLeft.ResumeLayout(false);
            this.panelContent.ResumeLayout(false);
            this.bunifuGradientPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
        }


    }
}