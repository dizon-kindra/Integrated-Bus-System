namespace sr
{
    partial class login
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.Panel panelRight;
        private System.Windows.Forms.Panel panelLoginCard;
        private System.Windows.Forms.Panel panelTopBar;

        private System.Windows.Forms.Label lblSystemTitle;
        private System.Windows.Forms.Label lblSystemSubtitle;
        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.Label lblAdminPortal;
        private System.Windows.Forms.Label lblFooterText;

        private System.Windows.Forms.Panel panelIconCircle;
        private System.Windows.Forms.Label lblBusIcon;

        private System.Windows.Forms.Panel panelUsernameLine;
        private System.Windows.Forms.Panel panelPasswordLine;

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label pictureBox1;
        private System.Windows.Forms.Label label1;
        private Bunifu.UI.WinForms.BunifuButton.BunifuButton bunifuButton1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private Bunifu.UI.WinForms.BunifuCheckBox bunifuCheckBox1;
        private System.Windows.Forms.Label label5;

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
            Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges borderEdges1 = new Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges();

            this.panelMain = new System.Windows.Forms.Panel();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.panelRight = new System.Windows.Forms.Panel();
            this.panelLoginCard = new System.Windows.Forms.Panel();
            this.panelTopBar = new System.Windows.Forms.Panel();

            this.lblSystemTitle = new System.Windows.Forms.Label();
            this.lblSystemSubtitle = new System.Windows.Forms.Label();
            this.lblWelcome = new System.Windows.Forms.Label();
            this.lblAdminPortal = new System.Windows.Forms.Label();
            this.lblFooterText = new System.Windows.Forms.Label();

            this.panelIconCircle = new System.Windows.Forms.Panel();
            this.lblBusIcon = new System.Windows.Forms.Label();

            this.panelUsernameLine = new System.Windows.Forms.Panel();
            this.panelPasswordLine = new System.Windows.Forms.Panel();

            this.panel1 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.bunifuCheckBox1 = new Bunifu.UI.WinForms.BunifuCheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.bunifuButton1 = new Bunifu.UI.WinForms.BunifuButton.BunifuButton();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();

            this.panelMain.SuspendLayout();
            this.panelLeft.SuspendLayout();
            this.panelRight.SuspendLayout();
            this.panelLoginCard.SuspendLayout();
            this.panelIconCircle.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();

            // 
            // login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(238, 242, 247);
            this.ClientSize = new System.Drawing.Size(1050, 620);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "login";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Admin Login";
            this.Load += new System.EventHandler(this.login_Load);

            // 
            // panelMain
            // 
            this.panelMain.BackColor = System.Drawing.Color.FromArgb(238, 242, 247);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Controls.Add(this.panelLeft);
            this.panelMain.Controls.Add(this.panelRight);
            this.Controls.Add(this.panelMain);

            // 
            // panelLeft
            // 
            this.panelLeft.BackColor = System.Drawing.Color.FromArgb(22, 34, 50);
            this.panelLeft.Location = new System.Drawing.Point(0, 0);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(430, 620);
            this.panelLeft.TabIndex = 0;

            // 
            // panelTopBar
            // 
            this.panelTopBar.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            this.panelTopBar.Location = new System.Drawing.Point(0, 0);
            this.panelTopBar.Name = "panelTopBar";
            this.panelTopBar.Size = new System.Drawing.Size(430, 8);
            this.panelTopBar.TabIndex = 0;
            this.panelLeft.Controls.Add(this.panelTopBar);

            // 
            // panelIconCircle
            // 
            this.panelIconCircle.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            this.panelIconCircle.Location = new System.Drawing.Point(65, 95);
            this.panelIconCircle.Name = "panelIconCircle";
            this.panelIconCircle.Size = new System.Drawing.Size(90, 90);
            this.panelIconCircle.TabIndex = 1;
            this.panelLeft.Controls.Add(this.panelIconCircle);

            // 
            // lblBusIcon
            // 
            this.lblBusIcon.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBusIcon.Font = new System.Drawing.Font("Segoe UI", 32F, System.Drawing.FontStyle.Bold);
            this.lblBusIcon.ForeColor = System.Drawing.Color.White;
            this.lblBusIcon.Location = new System.Drawing.Point(0, 0);
            this.lblBusIcon.Name = "lblBusIcon";
            this.lblBusIcon.Size = new System.Drawing.Size(90, 90);
            this.lblBusIcon.TabIndex = 0;
            this.lblBusIcon.Text = "B";
            this.lblBusIcon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.panelIconCircle.Controls.Add(this.lblBusIcon);

            // 
            // lblSystemTitle
            // 
            this.lblSystemTitle.Font = new System.Drawing.Font("Segoe UI", 23F, System.Drawing.FontStyle.Bold);
            this.lblSystemTitle.ForeColor = System.Drawing.Color.White;
            this.lblSystemTitle.Location = new System.Drawing.Point(65, 218);
            this.lblSystemTitle.Name = "lblSystemTitle";
            this.lblSystemTitle.Size = new System.Drawing.Size(320, 105);
            this.lblSystemTitle.TabIndex = 2;
            this.lblSystemTitle.Text = "Bus Terminal\r\nManagement";
            this.panelLeft.Controls.Add(this.lblSystemTitle);

            // 
            // lblSystemSubtitle
            // 
            this.lblSystemSubtitle.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblSystemSubtitle.ForeColor = System.Drawing.Color.FromArgb(190, 205, 220);
            this.lblSystemSubtitle.Location = new System.Drawing.Point(68, 345);
            this.lblSystemSubtitle.Name = "lblSystemSubtitle";
            this.lblSystemSubtitle.Size = new System.Drawing.Size(315, 85);
            this.lblSystemSubtitle.TabIndex = 3;
            this.lblSystemSubtitle.Text = "Secure admin access for schedules,\r\nreservations, payments, boarding,\r\nand terminal reports.";
            this.panelLeft.Controls.Add(this.lblSystemSubtitle);

            // 
            // lblFooterText
            // 
            this.lblFooterText.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblFooterText.ForeColor = System.Drawing.Color.FromArgb(140, 160, 180);
            this.lblFooterText.Location = new System.Drawing.Point(68, 552);
            this.lblFooterText.Name = "lblFooterText";
            this.lblFooterText.Size = new System.Drawing.Size(320, 30);
            this.lblFooterText.TabIndex = 4;
            this.lblFooterText.Text = "Admin / Staff Authorized Access Only";
            this.panelLeft.Controls.Add(this.lblFooterText);

            // 
            // panelRight
            // 
            this.panelRight.BackColor = System.Drawing.Color.FromArgb(238, 242, 247);
            this.panelRight.Location = new System.Drawing.Point(430, 0);
            this.panelRight.Name = "panelRight";
            this.panelRight.Size = new System.Drawing.Size(620, 620);
            this.panelRight.TabIndex = 1;
            this.panelMain.Controls.Add(this.panelRight);

            // 
            // lblWelcome
            // 
            this.lblWelcome.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblWelcome.ForeColor = System.Drawing.Color.FromArgb(32, 45, 64);
            this.lblWelcome.Location = new System.Drawing.Point(85, 60);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(460, 45);
            this.lblWelcome.TabIndex = 0;
            this.lblWelcome.Text = "Welcome Back";
            this.lblWelcome.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.panelRight.Controls.Add(this.lblWelcome);

            // 
            // lblAdminPortal
            // 
            this.lblAdminPortal.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.lblAdminPortal.ForeColor = System.Drawing.Color.Gray;
            this.lblAdminPortal.Location = new System.Drawing.Point(85, 103);
            this.lblAdminPortal.Name = "lblAdminPortal";
            this.lblAdminPortal.Size = new System.Drawing.Size(460, 28);
            this.lblAdminPortal.TabIndex = 1;
            this.lblAdminPortal.Text = "Sign in to continue to the admin dashboard";
            this.lblAdminPortal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.panelRight.Controls.Add(this.lblAdminPortal);

            // 
            // panelLoginCard
            // 
            this.panelLoginCard.BackColor = System.Drawing.Color.White;
            this.panelLoginCard.Location = new System.Drawing.Point(115, 145);
            this.panelLoginCard.Name = "panelLoginCard";
            this.panelLoginCard.Size = new System.Drawing.Size(390, 430);
            this.panelLoginCard.TabIndex = 2;
            this.panelRight.Controls.Add(this.panelLoginCard);

            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.bunifuCheckBox1);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.bunifuButton1);
            this.panel1.Controls.Add(this.panelPasswordLine);
            this.panel1.Controls.Add(this.panelUsernameLine);
            this.panel1.Controls.Add(this.textBox2);
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.label1);
            this.panelLoginCard.Controls.Add(this.panel1);

            // 
            // pictureBox1 - User Icon
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            this.pictureBox1.Font = new System.Drawing.Font("Segoe MDL2 Assets", 30F, System.Drawing.FontStyle.Regular);
            this.pictureBox1.ForeColor = System.Drawing.Color.White;
            this.pictureBox1.Location = new System.Drawing.Point(155, 25);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(80, 65);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.Text = "\uE77B";
            this.pictureBox1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(32, 45, 64);
            this.label1.Location = new System.Drawing.Point(0, 98);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(390, 40);
            this.label1.TabIndex = 0;
            this.label1.Text = "Admin Login";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // 
            // label2 username
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.FromArgb(32, 45, 64);
            this.label2.Location = new System.Drawing.Point(45, 153);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 19);
            this.label2.TabIndex = 2;
            this.label2.Text = "Username";
            this.label2.Click += new System.EventHandler(this.label2_Click);

            // 
            // textBox1 username
            // 
            this.textBox1.BackColor = System.Drawing.Color.White;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.textBox1.ForeColor = System.Drawing.Color.FromArgb(32, 45, 64);
            this.textBox1.Location = new System.Drawing.Point(45, 178);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(300, 22);
            this.textBox1.TabIndex = 1;

            // 
            // panelUsernameLine
            // 
            this.panelUsernameLine.BackColor = System.Drawing.Color.FromArgb(210, 218, 226);
            this.panelUsernameLine.Location = new System.Drawing.Point(45, 207);
            this.panelUsernameLine.Name = "panelUsernameLine";
            this.panelUsernameLine.Size = new System.Drawing.Size(300, 2);
            this.panelUsernameLine.TabIndex = 10;

            // 
            // label3 password
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label3.ForeColor = System.Drawing.Color.FromArgb(32, 45, 64);
            this.label3.Location = new System.Drawing.Point(45, 230);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 19);
            this.label3.TabIndex = 3;
            this.label3.Text = "Password";

            // 
            // textBox2 password
            // 
            this.textBox2.BackColor = System.Drawing.Color.White;
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox2.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.textBox2.ForeColor = System.Drawing.Color.FromArgb(32, 45, 64);
            this.textBox2.Location = new System.Drawing.Point(45, 255);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(300, 22);
            this.textBox2.TabIndex = 2;
            this.textBox2.UseSystemPasswordChar = true;

            // 
            // panelPasswordLine
            // 
            this.panelPasswordLine.BackColor = System.Drawing.Color.FromArgb(210, 218, 226);
            this.panelPasswordLine.Location = new System.Drawing.Point(45, 284);
            this.panelPasswordLine.Name = "panelPasswordLine";
            this.panelPasswordLine.Size = new System.Drawing.Size(300, 2);
            this.panelPasswordLine.TabIndex = 11;

            // 
            // bunifuCheckBox1
            // 
            this.bunifuCheckBox1.AllowBindingControlAnimation = true;
            this.bunifuCheckBox1.AllowBindingControlColorChanges = false;
            this.bunifuCheckBox1.AllowBindingControlLocation = true;
            this.bunifuCheckBox1.AllowCheckBoxAnimation = false;
            this.bunifuCheckBox1.AllowCheckmarkAnimation = true;
            this.bunifuCheckBox1.AllowOnHoverStates = true;
            this.bunifuCheckBox1.AutoCheck = true;
            this.bunifuCheckBox1.BackColor = System.Drawing.Color.Transparent;
            this.bunifuCheckBox1.BindingControlPosition = Bunifu.UI.WinForms.BunifuCheckBox.BindingControlPositions.Right;
            this.bunifuCheckBox1.BorderRadius = 12;
            this.bunifuCheckBox1.Checked = false;
            this.bunifuCheckBox1.CheckState = Bunifu.UI.WinForms.BunifuCheckBox.CheckStates.Unchecked;
            this.bunifuCheckBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bunifuCheckBox1.CustomCheckmarkImage = null;
            this.bunifuCheckBox1.Location = new System.Drawing.Point(45, 305);
            this.bunifuCheckBox1.MinimumSize = new System.Drawing.Size(17, 17);
            this.bunifuCheckBox1.Name = "bunifuCheckBox1";

            this.bunifuCheckBox1.OnCheck.BorderColor = System.Drawing.Color.FromArgb(52, 152, 219);
            this.bunifuCheckBox1.OnCheck.BorderRadius = 12;
            this.bunifuCheckBox1.OnCheck.BorderThickness = 2;
            this.bunifuCheckBox1.OnCheck.CheckBoxColor = System.Drawing.Color.FromArgb(52, 152, 219);
            this.bunifuCheckBox1.OnCheck.CheckmarkColor = System.Drawing.Color.White;
            this.bunifuCheckBox1.OnCheck.CheckmarkThickness = 2;

            this.bunifuCheckBox1.OnDisable.BorderColor = System.Drawing.Color.LightGray;
            this.bunifuCheckBox1.OnDisable.BorderRadius = 12;
            this.bunifuCheckBox1.OnDisable.BorderThickness = 2;
            this.bunifuCheckBox1.OnDisable.CheckBoxColor = System.Drawing.Color.Transparent;
            this.bunifuCheckBox1.OnDisable.CheckmarkColor = System.Drawing.Color.LightGray;
            this.bunifuCheckBox1.OnDisable.CheckmarkThickness = 2;

            this.bunifuCheckBox1.OnHoverChecked.BorderColor = System.Drawing.Color.FromArgb(52, 152, 219);
            this.bunifuCheckBox1.OnHoverChecked.BorderRadius = 12;
            this.bunifuCheckBox1.OnHoverChecked.BorderThickness = 2;
            this.bunifuCheckBox1.OnHoverChecked.CheckBoxColor = System.Drawing.Color.FromArgb(52, 152, 219);
            this.bunifuCheckBox1.OnHoverChecked.CheckmarkColor = System.Drawing.Color.White;
            this.bunifuCheckBox1.OnHoverChecked.CheckmarkThickness = 2;

            this.bunifuCheckBox1.OnHoverUnchecked.BorderColor = System.Drawing.Color.FromArgb(52, 152, 219);
            this.bunifuCheckBox1.OnHoverUnchecked.BorderRadius = 12;
            this.bunifuCheckBox1.OnHoverUnchecked.BorderThickness = 1;
            this.bunifuCheckBox1.OnHoverUnchecked.CheckBoxColor = System.Drawing.Color.Transparent;

            this.bunifuCheckBox1.OnUncheck.BorderColor = System.Drawing.Color.DarkGray;
            this.bunifuCheckBox1.OnUncheck.BorderRadius = 12;
            this.bunifuCheckBox1.OnUncheck.BorderThickness = 1;
            this.bunifuCheckBox1.OnUncheck.CheckBoxColor = System.Drawing.Color.Transparent;

            this.bunifuCheckBox1.Size = new System.Drawing.Size(21, 21);
            this.bunifuCheckBox1.Style = Bunifu.UI.WinForms.BunifuCheckBox.CheckBoxStyles.Flat;
            this.bunifuCheckBox1.TabIndex = 3;
            this.bunifuCheckBox1.ThreeState = false;
            this.bunifuCheckBox1.ToolTipText = null;
            this.bunifuCheckBox1.CheckedChanged += new System.EventHandler<Bunifu.UI.WinForms.BunifuCheckBox.CheckedChangedEventArgs>(this.bunifuCheckBox1_CheckedChanged);

            // 
            // label5 show password
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.label5.ForeColor = System.Drawing.Color.Gray;
            this.label5.Location = new System.Drawing.Point(72, 306);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(98, 17);
            this.label5.TabIndex = 9;
            this.label5.Text = "Show password";

            // 
            // label4 message
            // 
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.label4.ForeColor = System.Drawing.Color.Firebrick;
            this.label4.Location = new System.Drawing.Point(45, 332);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(300, 20);
            this.label4.TabIndex = 7;
            this.label4.Text = "";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // 
            // bunifuButton1 login
            // 
            this.bunifuButton1.AllowAnimations = true;
            this.bunifuButton1.AllowMouseEffects = true;
            this.bunifuButton1.AllowToggling = false;
            this.bunifuButton1.AnimationSpeed = 200;
            this.bunifuButton1.AutoGenerateColors = false;
            this.bunifuButton1.AutoRoundBorders = false;
            this.bunifuButton1.AutoSizeLeftIcon = true;
            this.bunifuButton1.AutoSizeRightIcon = true;
            this.bunifuButton1.BackColor = System.Drawing.Color.Transparent;
            this.bunifuButton1.BackColor1 = System.Drawing.Color.FromArgb(52, 152, 219);
            this.bunifuButton1.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.bunifuButton1.ButtonText = "SIGN IN";
            this.bunifuButton1.ButtonTextMarginLeft = 0;
            this.bunifuButton1.ColorContrastOnClick = 45;
            this.bunifuButton1.ColorContrastOnHover = 45;
            this.bunifuButton1.Cursor = System.Windows.Forms.Cursors.Hand;

            borderEdges1.BottomLeft = true;
            borderEdges1.BottomRight = true;
            borderEdges1.TopLeft = true;
            borderEdges1.TopRight = true;
            this.bunifuButton1.CustomizableEdges = borderEdges1;

            this.bunifuButton1.DialogResult = System.Windows.Forms.DialogResult.None;
            this.bunifuButton1.DisabledBorderColor = System.Drawing.Color.FromArgb(191, 191, 191);
            this.bunifuButton1.DisabledFillColor = System.Drawing.Color.FromArgb(204, 204, 204);
            this.bunifuButton1.DisabledForecolor = System.Drawing.Color.FromArgb(168, 160, 168);
            this.bunifuButton1.FocusState = Bunifu.UI.WinForms.BunifuButton.BunifuButton.ButtonStates.Pressed;
            this.bunifuButton1.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.bunifuButton1.ForeColor = System.Drawing.Color.White;

            this.bunifuButton1.IconLeftAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bunifuButton1.IconLeftCursor = System.Windows.Forms.Cursors.Hand;
            this.bunifuButton1.IconLeftPadding = new System.Windows.Forms.Padding(11, 3, 3, 3);
            this.bunifuButton1.IconMarginLeft = 11;
            this.bunifuButton1.IconPadding = 10;
            this.bunifuButton1.IconRightAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.bunifuButton1.IconRightCursor = System.Windows.Forms.Cursors.Hand;
            this.bunifuButton1.IconRightPadding = new System.Windows.Forms.Padding(3, 3, 7, 3);
            this.bunifuButton1.IconSize = 25;

            this.bunifuButton1.IdleBorderColor = System.Drawing.Color.FromArgb(52, 152, 219);
            this.bunifuButton1.IdleBorderRadius = 10;
            this.bunifuButton1.IdleBorderThickness = 1;
            this.bunifuButton1.IdleFillColor = System.Drawing.Color.FromArgb(52, 152, 219);
            this.bunifuButton1.IdleIconLeftImage = null;
            this.bunifuButton1.IdleIconRightImage = null;
            this.bunifuButton1.IndicateFocus = false;

            this.bunifuButton1.Location = new System.Drawing.Point(45, 362);
            this.bunifuButton1.Name = "bunifuButton1";

            this.bunifuButton1.OnDisabledState.BorderColor = System.Drawing.Color.FromArgb(191, 191, 191);
            this.bunifuButton1.OnDisabledState.BorderRadius = 10;
            this.bunifuButton1.OnDisabledState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.bunifuButton1.OnDisabledState.BorderThickness = 1;
            this.bunifuButton1.OnDisabledState.FillColor = System.Drawing.Color.FromArgb(204, 204, 204);
            this.bunifuButton1.OnDisabledState.ForeColor = System.Drawing.Color.FromArgb(168, 160, 168);
            this.bunifuButton1.OnDisabledState.IconLeftImage = null;
            this.bunifuButton1.OnDisabledState.IconRightImage = null;

            this.bunifuButton1.onHoverState.BorderColor = System.Drawing.Color.FromArgb(41, 128, 185);
            this.bunifuButton1.onHoverState.BorderRadius = 10;
            this.bunifuButton1.onHoverState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.bunifuButton1.onHoverState.BorderThickness = 1;
            this.bunifuButton1.onHoverState.FillColor = System.Drawing.Color.FromArgb(41, 128, 185);
            this.bunifuButton1.onHoverState.ForeColor = System.Drawing.Color.White;
            this.bunifuButton1.onHoverState.IconLeftImage = null;
            this.bunifuButton1.onHoverState.IconRightImage = null;

            this.bunifuButton1.OnIdleState.BorderColor = System.Drawing.Color.FromArgb(52, 152, 219);
            this.bunifuButton1.OnIdleState.BorderRadius = 10;
            this.bunifuButton1.OnIdleState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.bunifuButton1.OnIdleState.BorderThickness = 1;
            this.bunifuButton1.OnIdleState.FillColor = System.Drawing.Color.FromArgb(52, 152, 219);
            this.bunifuButton1.OnIdleState.ForeColor = System.Drawing.Color.White;
            this.bunifuButton1.OnIdleState.IconLeftImage = null;
            this.bunifuButton1.OnIdleState.IconRightImage = null;

            this.bunifuButton1.OnPressedState.BorderColor = System.Drawing.Color.FromArgb(22, 34, 50);
            this.bunifuButton1.OnPressedState.BorderRadius = 10;
            this.bunifuButton1.OnPressedState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.bunifuButton1.OnPressedState.BorderThickness = 1;
            this.bunifuButton1.OnPressedState.FillColor = System.Drawing.Color.FromArgb(22, 34, 50);
            this.bunifuButton1.OnPressedState.ForeColor = System.Drawing.Color.White;
            this.bunifuButton1.OnPressedState.IconLeftImage = null;
            this.bunifuButton1.OnPressedState.IconRightImage = null;

            this.bunifuButton1.Size = new System.Drawing.Size(300, 45);
            this.bunifuButton1.TabIndex = 4;
            this.bunifuButton1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.bunifuButton1.TextAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.bunifuButton1.TextMarginLeft = 0;
            this.bunifuButton1.TextPadding = new System.Windows.Forms.Padding(0);
            this.bunifuButton1.UseDefaultRadiusAndThickness = true;
            this.bunifuButton1.Click += new System.EventHandler(this.bunifuButton1_Click);

            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panelLoginCard.ResumeLayout(false);
            this.panelRight.ResumeLayout(false);
            this.panelIconCircle.ResumeLayout(false);
            this.panelLeft.ResumeLayout(false);
            this.panelMain.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}