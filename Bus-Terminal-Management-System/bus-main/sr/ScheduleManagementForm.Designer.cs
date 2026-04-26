namespace sr
{
    partial class ScheduleManagementForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblScheduleID;
        private System.Windows.Forms.Label lblBus;
        private System.Windows.Forms.Label lblRoute;
        private System.Windows.Forms.Label lblDepartureDate;
        private System.Windows.Forms.Label lblDepartureTime;
        private System.Windows.Forms.Label lblArrivalTime;
        private System.Windows.Forms.Label lblFare;
        private System.Windows.Forms.Label lblAvailableSeats;
        private System.Windows.Forms.Label lblTripStatus;

        private System.Windows.Forms.TextBox txtScheduleID;
        private System.Windows.Forms.ComboBox cmbBus;
        private System.Windows.Forms.ComboBox cmbRoute;
        private System.Windows.Forms.DateTimePicker dtpDepartureDate;
        private System.Windows.Forms.DateTimePicker dtpDepartureTime;
        private System.Windows.Forms.DateTimePicker dtpArrivalTime;
        private System.Windows.Forms.TextBox txtFare;
        private System.Windows.Forms.TextBox txtAvailableSeats;
        private System.Windows.Forms.ComboBox cmbTripStatus;

        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnCancelTrip;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.DataGridView dgvSchedules;

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
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblScheduleID = new System.Windows.Forms.Label();
            this.lblBus = new System.Windows.Forms.Label();
            this.lblRoute = new System.Windows.Forms.Label();
            this.lblDepartureDate = new System.Windows.Forms.Label();
            this.lblDepartureTime = new System.Windows.Forms.Label();
            this.lblArrivalTime = new System.Windows.Forms.Label();
            this.lblFare = new System.Windows.Forms.Label();
            this.lblAvailableSeats = new System.Windows.Forms.Label();
            this.lblTripStatus = new System.Windows.Forms.Label();

            this.txtScheduleID = new System.Windows.Forms.TextBox();
            this.cmbBus = new System.Windows.Forms.ComboBox();
            this.cmbRoute = new System.Windows.Forms.ComboBox();
            this.dtpDepartureDate = new System.Windows.Forms.DateTimePicker();
            this.dtpDepartureTime = new System.Windows.Forms.DateTimePicker();
            this.dtpArrivalTime = new System.Windows.Forms.DateTimePicker();
            this.txtFare = new System.Windows.Forms.TextBox();
            this.txtAvailableSeats = new System.Windows.Forms.TextBox();
            this.cmbTripStatus = new System.Windows.Forms.ComboBox();

            this.btnAdd = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnCancelTrip = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.dgvSchedules = new System.Windows.Forms.DataGridView();

            ((System.ComponentModel.ISupportInitialize)(this.dgvSchedules)).BeginInit();
            this.SuspendLayout();

            // Form
            this.ClientSize = new System.Drawing.Size(1050, 700);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Schedule / Trip Management";
            this.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Load += new System.EventHandler(this.ScheduleManagementForm_Load);

            // Title
            this.lblTitle.Text = "Schedule / Trip Management";
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(30, 45, 70);
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(30, 20);

            // Schedule ID
            this.lblScheduleID.Text = "Schedule ID";
            this.lblScheduleID.Location = new System.Drawing.Point(35, 85);
            this.lblScheduleID.AutoSize = true;

            this.txtScheduleID.Location = new System.Drawing.Point(160, 80);
            this.txtScheduleID.Size = new System.Drawing.Size(200, 25);
            this.txtScheduleID.ReadOnly = true;

            // Bus
            this.lblBus.Text = "Bus";
            this.lblBus.Location = new System.Drawing.Point(35, 125);
            this.lblBus.AutoSize = true;

            this.cmbBus.Location = new System.Drawing.Point(160, 120);
            this.cmbBus.Size = new System.Drawing.Size(200, 25);
            this.cmbBus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBus.SelectedIndexChanged += new System.EventHandler(this.cmbBus_SelectedIndexChanged);

            // Route
            this.lblRoute.Text = "Route";
            this.lblRoute.Location = new System.Drawing.Point(35, 165);
            this.lblRoute.AutoSize = true;

            this.cmbRoute.Location = new System.Drawing.Point(160, 160);
            this.cmbRoute.Size = new System.Drawing.Size(200, 25);
            this.cmbRoute.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRoute.SelectedIndexChanged += new System.EventHandler(this.cmbRoute_SelectedIndexChanged);

            // Departure Date
            this.lblDepartureDate.Text = "Departure Date";
            this.lblDepartureDate.Location = new System.Drawing.Point(35, 205);
            this.lblDepartureDate.AutoSize = true;

            this.dtpDepartureDate.Location = new System.Drawing.Point(160, 200);
            this.dtpDepartureDate.Size = new System.Drawing.Size(200, 25);
            this.dtpDepartureDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;

            // Departure Time
            this.lblDepartureTime.Text = "Departure Time";
            this.lblDepartureTime.Location = new System.Drawing.Point(410, 85);
            this.lblDepartureTime.AutoSize = true;

            this.dtpDepartureTime.Location = new System.Drawing.Point(545, 80);
            this.dtpDepartureTime.Size = new System.Drawing.Size(180, 25);
            this.dtpDepartureTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpDepartureTime.ShowUpDown = true;

            // Arrival Time
            this.lblArrivalTime.Text = "Arrival Time";
            this.lblArrivalTime.Location = new System.Drawing.Point(410, 125);
            this.lblArrivalTime.AutoSize = true;

            this.dtpArrivalTime.Location = new System.Drawing.Point(545, 120);
            this.dtpArrivalTime.Size = new System.Drawing.Size(180, 25);
            this.dtpArrivalTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpArrivalTime.ShowUpDown = true;

            // Fare
            this.lblFare.Text = "Fare";
            this.lblFare.Location = new System.Drawing.Point(410, 165);
            this.lblFare.AutoSize = true;

            this.txtFare.Location = new System.Drawing.Point(545, 160);
            this.txtFare.Size = new System.Drawing.Size(180, 25);
            this.txtFare.ReadOnly = true;

            // Available Seats
            this.lblAvailableSeats.Text = "Available Seats";
            this.lblAvailableSeats.Location = new System.Drawing.Point(410, 205);
            this.lblAvailableSeats.AutoSize = true;

            this.txtAvailableSeats.Location = new System.Drawing.Point(545, 200);
            this.txtAvailableSeats.Size = new System.Drawing.Size(180, 25);
            this.txtAvailableSeats.ReadOnly = true;

            // Trip Status
            this.lblTripStatus.Text = "Trip Status";
            this.lblTripStatus.Location = new System.Drawing.Point(765, 85);
            this.lblTripStatus.AutoSize = true;

            this.cmbTripStatus.Location = new System.Drawing.Point(850, 80);
            this.cmbTripStatus.Size = new System.Drawing.Size(150, 25);
            this.cmbTripStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            // Buttons
            this.btnAdd.Text = "Add Schedule";
            this.btnAdd.Location = new System.Drawing.Point(35, 260);
            this.btnAdd.Size = new System.Drawing.Size(150, 40);
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(40, 167, 69);
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);

            this.btnUpdate.Text = "Update Schedule";
            this.btnUpdate.Location = new System.Drawing.Point(200, 260);
            this.btnUpdate.Size = new System.Drawing.Size(160, 40);
            this.btnUpdate.BackColor = System.Drawing.Color.FromArgb(0, 123, 255);
            this.btnUpdate.ForeColor = System.Drawing.Color.White;
            this.btnUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);

            this.btnCancelTrip.Text = "Cancel Trip";
            this.btnCancelTrip.Location = new System.Drawing.Point(375, 260);
            this.btnCancelTrip.Size = new System.Drawing.Size(140, 40);
            this.btnCancelTrip.BackColor = System.Drawing.Color.FromArgb(220, 53, 69);
            this.btnCancelTrip.ForeColor = System.Drawing.Color.White;
            this.btnCancelTrip.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelTrip.Click += new System.EventHandler(this.btnCancelTrip_Click);

            this.btnClear.Text = "Clear";
            this.btnClear.Location = new System.Drawing.Point(530, 260);
            this.btnClear.Size = new System.Drawing.Size(120, 40);
            this.btnClear.BackColor = System.Drawing.Color.FromArgb(108, 117, 125);
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);

            // DataGridView
            this.dgvSchedules.Location = new System.Drawing.Point(35, 330);
            this.dgvSchedules.Size = new System.Drawing.Size(965, 320);
            this.dgvSchedules.BackgroundColor = System.Drawing.Color.White;
            this.dgvSchedules.AllowUserToAddRows = false;
            this.dgvSchedules.AllowUserToDeleteRows = false;
            this.dgvSchedules.ReadOnly = true;
            this.dgvSchedules.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSchedules.MultiSelect = false;
            this.dgvSchedules.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvSchedules.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSchedules_CellClick);

            // Add controls
            this.Controls.Add(this.lblTitle);

            this.Controls.Add(this.lblScheduleID);
            this.Controls.Add(this.txtScheduleID);
            this.Controls.Add(this.lblBus);
            this.Controls.Add(this.cmbBus);
            this.Controls.Add(this.lblRoute);
            this.Controls.Add(this.cmbRoute);
            this.Controls.Add(this.lblDepartureDate);
            this.Controls.Add(this.dtpDepartureDate);

            this.Controls.Add(this.lblDepartureTime);
            this.Controls.Add(this.dtpDepartureTime);
            this.Controls.Add(this.lblArrivalTime);
            this.Controls.Add(this.dtpArrivalTime);
            this.Controls.Add(this.lblFare);
            this.Controls.Add(this.txtFare);
            this.Controls.Add(this.lblAvailableSeats);
            this.Controls.Add(this.txtAvailableSeats);
            this.Controls.Add(this.lblTripStatus);
            this.Controls.Add(this.cmbTripStatus);

            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnCancelTrip);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.dgvSchedules);

            ((System.ComponentModel.ISupportInitialize)(this.dgvSchedules)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}