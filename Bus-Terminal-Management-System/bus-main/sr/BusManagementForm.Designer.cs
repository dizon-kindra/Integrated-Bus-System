namespace sr
{
    partial class BusManagementForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panelForm;

        private System.Windows.Forms.GroupBox groupBoxBusInfo;
        private System.Windows.Forms.GroupBox groupBoxBusList;

        private System.Windows.Forms.Label lblBusID;
        private System.Windows.Forms.Label lblBusNumber;
        private System.Windows.Forms.Label lblPlateNumber;
        private System.Windows.Forms.Label lblCapacity;
        private System.Windows.Forms.Label lblBusType;
        private System.Windows.Forms.Label lblStatus;

        private System.Windows.Forms.TextBox txtBusID;
        private System.Windows.Forms.TextBox txtBusNumber;
        private System.Windows.Forms.TextBox txtPlateNumber;
        private System.Windows.Forms.TextBox txtCapacity;

        private System.Windows.Forms.ComboBox cmbBusType;
        private System.Windows.Forms.ComboBox cmbStatus;

        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnDeactivate;
        private System.Windows.Forms.Button btnClear;

        private System.Windows.Forms.DataGridView dgvBuses;

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
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelForm = new System.Windows.Forms.Panel();

            this.groupBoxBusInfo = new System.Windows.Forms.GroupBox();
            this.groupBoxBusList = new System.Windows.Forms.GroupBox();

            this.lblBusID = new System.Windows.Forms.Label();
            this.lblBusNumber = new System.Windows.Forms.Label();
            this.lblPlateNumber = new System.Windows.Forms.Label();
            this.lblCapacity = new System.Windows.Forms.Label();
            this.lblBusType = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();

            this.txtBusID = new System.Windows.Forms.TextBox();
            this.txtBusNumber = new System.Windows.Forms.TextBox();
            this.txtPlateNumber = new System.Windows.Forms.TextBox();
            this.txtCapacity = new System.Windows.Forms.TextBox();

            this.cmbBusType = new System.Windows.Forms.ComboBox();
            this.cmbStatus = new System.Windows.Forms.ComboBox();

            this.btnAdd = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnDeactivate = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();

            this.dgvBuses = new System.Windows.Forms.DataGridView();

            this.panelHeader.SuspendLayout();
            this.panelForm.SuspendLayout();
            this.groupBoxBusInfo.SuspendLayout();
            this.groupBoxBusList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBuses)).BeginInit();
            this.SuspendLayout();

            // panelHeader
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(34, 45, 65);
            this.panelHeader.Controls.Add(this.lblTitle);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(1100, 70);

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(25, 17);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Text = "Bus Management";

            // panelForm
            this.panelForm.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelForm.Controls.Add(this.groupBoxBusInfo);
            this.panelForm.Controls.Add(this.groupBoxBusList);
            this.panelForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelForm.Location = new System.Drawing.Point(0, 70);
            this.panelForm.Name = "panelForm";
            this.panelForm.Size = new System.Drawing.Size(1100, 630);

            // groupBoxBusInfo
            this.groupBoxBusInfo.BackColor = System.Drawing.Color.White;
            this.groupBoxBusInfo.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.groupBoxBusInfo.Location = new System.Drawing.Point(25, 25);
            this.groupBoxBusInfo.Name = "groupBoxBusInfo";
            this.groupBoxBusInfo.Size = new System.Drawing.Size(1050, 210);
            this.groupBoxBusInfo.TabStop = false;
            this.groupBoxBusInfo.Text = "Bus Information";

            this.groupBoxBusInfo.Controls.Add(this.lblBusID);
            this.groupBoxBusInfo.Controls.Add(this.txtBusID);
            this.groupBoxBusInfo.Controls.Add(this.lblBusNumber);
            this.groupBoxBusInfo.Controls.Add(this.txtBusNumber);
            this.groupBoxBusInfo.Controls.Add(this.lblPlateNumber);
            this.groupBoxBusInfo.Controls.Add(this.txtPlateNumber);
            this.groupBoxBusInfo.Controls.Add(this.lblCapacity);
            this.groupBoxBusInfo.Controls.Add(this.txtCapacity);
            this.groupBoxBusInfo.Controls.Add(this.lblBusType);
            this.groupBoxBusInfo.Controls.Add(this.cmbBusType);
            this.groupBoxBusInfo.Controls.Add(this.lblStatus);
            this.groupBoxBusInfo.Controls.Add(this.cmbStatus);
            this.groupBoxBusInfo.Controls.Add(this.btnAdd);
            this.groupBoxBusInfo.Controls.Add(this.btnUpdate);
            this.groupBoxBusInfo.Controls.Add(this.btnDeactivate);
            this.groupBoxBusInfo.Controls.Add(this.btnClear);

            // lblBusID
            this.lblBusID.AutoSize = true;
            this.lblBusID.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblBusID.Location = new System.Drawing.Point(30, 40);
            this.lblBusID.Text = "Bus ID";

            // txtBusID
            this.txtBusID.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtBusID.Location = new System.Drawing.Point(140, 37);
            this.txtBusID.Name = "txtBusID";
            this.txtBusID.ReadOnly = true;
            this.txtBusID.Size = new System.Drawing.Size(210, 25);

            // lblBusNumber
            this.lblBusNumber.AutoSize = true;
            this.lblBusNumber.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblBusNumber.Location = new System.Drawing.Point(30, 80);
            this.lblBusNumber.Text = "Bus Number";

            // txtBusNumber
            this.txtBusNumber.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtBusNumber.Location = new System.Drawing.Point(140, 77);
            this.txtBusNumber.Name = "txtBusNumber";
            this.txtBusNumber.Size = new System.Drawing.Size(210, 25);

            // lblPlateNumber
            this.lblPlateNumber.AutoSize = true;
            this.lblPlateNumber.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblPlateNumber.Location = new System.Drawing.Point(30, 120);
            this.lblPlateNumber.Text = "Plate Number";

            // txtPlateNumber
            this.txtPlateNumber.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtPlateNumber.Location = new System.Drawing.Point(140, 117);
            this.txtPlateNumber.Name = "txtPlateNumber";
            this.txtPlateNumber.Size = new System.Drawing.Size(210, 25);

            // lblCapacity
            this.lblCapacity.AutoSize = true;
            this.lblCapacity.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblCapacity.Location = new System.Drawing.Point(400, 40);
            this.lblCapacity.Text = "Capacity";

            // txtCapacity
            this.txtCapacity.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtCapacity.Location = new System.Drawing.Point(510, 37);
            this.txtCapacity.Name = "txtCapacity";
            this.txtCapacity.Size = new System.Drawing.Size(210, 25);

            // lblBusType
            this.lblBusType.AutoSize = true;
            this.lblBusType.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblBusType.Location = new System.Drawing.Point(400, 80);
            this.lblBusType.Text = "Bus Type";

            // cmbBusType
            this.cmbBusType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBusType.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbBusType.FormattingEnabled = true;
            this.cmbBusType.Location = new System.Drawing.Point(510, 77);
            this.cmbBusType.Name = "cmbBusType";
            this.cmbBusType.Size = new System.Drawing.Size(210, 25);

            // lblStatus
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblStatus.Location = new System.Drawing.Point(400, 120);
            this.lblStatus.Text = "Status";

            // cmbStatus
            this.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatus.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Location = new System.Drawing.Point(510, 117);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(210, 25);

            // btnAdd
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(40, 167, 69);
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.Location = new System.Drawing.Point(770, 35);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(120, 35);
            this.btnAdd.Text = "Add Bus";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);

            // btnUpdate
            this.btnUpdate.BackColor = System.Drawing.Color.FromArgb(0, 123, 255);
            this.btnUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdate.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnUpdate.ForeColor = System.Drawing.Color.White;
            this.btnUpdate.Location = new System.Drawing.Point(910, 35);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(120, 35);
            this.btnUpdate.Text = "Update Bus";
            this.btnUpdate.UseVisualStyleBackColor = false;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);

            // btnDeactivate
            this.btnDeactivate.BackColor = System.Drawing.Color.FromArgb(220, 53, 69);
            this.btnDeactivate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeactivate.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnDeactivate.ForeColor = System.Drawing.Color.White;
            this.btnDeactivate.Location = new System.Drawing.Point(770, 90);
            this.btnDeactivate.Name = "btnDeactivate";
            this.btnDeactivate.Size = new System.Drawing.Size(120, 35);
            this.btnDeactivate.Text = "Deactivate";
            this.btnDeactivate.UseVisualStyleBackColor = false;
            this.btnDeactivate.Click += new System.EventHandler(this.btnDeactivate_Click);

            // btnClear
            this.btnClear.BackColor = System.Drawing.Color.FromArgb(108, 117, 125);
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.Location = new System.Drawing.Point(910, 90);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(120, 35);
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);

            // groupBoxBusList
            this.groupBoxBusList.BackColor = System.Drawing.Color.White;
            this.groupBoxBusList.Controls.Add(this.dgvBuses);
            this.groupBoxBusList.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.groupBoxBusList.Location = new System.Drawing.Point(25, 255);
            this.groupBoxBusList.Name = "groupBoxBusList";
            this.groupBoxBusList.Size = new System.Drawing.Size(1050, 350);
            this.groupBoxBusList.TabStop = false;
            this.groupBoxBusList.Text = "Bus List";

            // dgvBuses
            this.dgvBuses.AllowUserToAddRows = false;
            this.dgvBuses.AllowUserToDeleteRows = false;
            this.dgvBuses.BackgroundColor = System.Drawing.Color.White;
            this.dgvBuses.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBuses.Location = new System.Drawing.Point(20, 30);
            this.dgvBuses.Name = "dgvBuses";
            this.dgvBuses.ReadOnly = true;
            this.dgvBuses.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvBuses.MultiSelect = false;
            this.dgvBuses.Size = new System.Drawing.Size(1010, 300);
            this.dgvBuses.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBuses_CellClick);

            // BusManagementForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(1100, 700);
            this.Controls.Add(this.panelForm);
            this.Controls.Add(this.panelHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "BusManagementForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Bus Management";
            this.Load += new System.EventHandler(this.BusManagementForm_Load);

            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelForm.ResumeLayout(false);
            this.groupBoxBusInfo.ResumeLayout(false);
            this.groupBoxBusInfo.PerformLayout();
            this.groupBoxBusList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBuses)).EndInit();
            this.ResumeLayout(false);
        }
    }
}