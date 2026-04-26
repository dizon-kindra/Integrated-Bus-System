using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace sr
{
    public partial class CheckInBoardingForm : Form
    {
        string connectionString = "server=localhost;user id=root;password=;database=sr_db;";

        private Panel panelHeader;
        private Label lblTitle;

        private GroupBox groupSearch;
        private TextBox txtSearch;
        private Button btnSearch;
        private Button btnRefresh;

        private GroupBox groupTrip;
        private ComboBox cmbSchedule;
        private Button btnLoadTrip;

        private GroupBox groupDetails;
        private TextBox txtBookingID;
        private TextBox txtPassengerName;
        private TextBox txtPhone;
        private TextBox txtSeatNo;
        private TextBox txtPaymentStatus;
        private TextBox txtReservationStatus;
        private TextBox txtCheckinStatus;
        private TextBox txtBoardingStatus;

        private Button btnCheckIn;
        private Button btnBoard;
        private Button btnNoShow;
        private Button btnClear;

        private DataGridView dgvPassengers;

        public CheckInBoardingForm()
        {
            InitializeComponent();
            CreateDesign();
        }

        private void CheckInBoardingForm_Load(object sender, EventArgs e)
        {
            if (cmbSchedule == null || dgvPassengers == null)
            {
                CreateDesign();
            }

            LoadSchedules();
            LoadPassengers();
        }

        private void CheckInBoardingForm_Load_1(object sender, EventArgs e)
        {
            CheckInBoardingForm_Load(sender, e);
        }

        private void CreateDesign()
        {
            this.Controls.Clear();

            this.Text = "Check-in / Boarding";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ClientSize = new Size(1200, 760);
            this.BackColor = Color.FromArgb(245, 247, 250);
            this.Font = new Font("Segoe UI", 9F);

            panelHeader = new Panel();
            panelHeader.BackColor = Color.FromArgb(32, 45, 64);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Height = 65;

            lblTitle = new Label();
            lblTitle.Text = "Check-in / Boarding";
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(25, 16);

            panelHeader.Controls.Add(lblTitle);
            this.Controls.Add(panelHeader);

            groupSearch = new GroupBox();
            groupSearch.Text = "Search Passenger";
            groupSearch.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            groupSearch.Location = new Point(20, 85);
            groupSearch.Size = new Size(560, 95);

            Label lblSearch = new Label();
            lblSearch.Text = "Search Booking / Passenger / Phone / Seat";
            lblSearch.Font = new Font("Segoe UI", 9F);
            lblSearch.AutoSize = true;
            lblSearch.Location = new Point(18, 30);

            txtSearch = new TextBox();
            txtSearch.Font = new Font("Segoe UI", 10F);
            txtSearch.Location = new Point(18, 52);
            txtSearch.Size = new Size(310, 25);

            btnSearch = new Button();
            btnSearch.Text = "Search";
            btnSearch.Location = new Point(345, 50);
            btnSearch.Size = new Size(90, 30);
            btnSearch.BackColor = Color.FromArgb(52, 152, 219);
            btnSearch.ForeColor = Color.White;
            btnSearch.FlatStyle = FlatStyle.Flat;
            btnSearch.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnSearch.Click += btnSearch_Click;

            btnRefresh = new Button();
            btnRefresh.Text = "Refresh";
            btnRefresh.Location = new Point(445, 50);
            btnRefresh.Size = new Size(90, 30);
            btnRefresh.BackColor = Color.FromArgb(127, 140, 141);
            btnRefresh.ForeColor = Color.White;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnRefresh.Click += btnRefresh_Click;

            groupSearch.Controls.Add(lblSearch);
            groupSearch.Controls.Add(txtSearch);
            groupSearch.Controls.Add(btnSearch);
            groupSearch.Controls.Add(btnRefresh);
            this.Controls.Add(groupSearch);

            groupTrip = new GroupBox();
            groupTrip.Text = "Trip Filter";
            groupTrip.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            groupTrip.Location = new Point(600, 85);
            groupTrip.Size = new Size(580, 95);

            Label lblSchedule = new Label();
            lblSchedule.Text = "Schedule / Trip";
            lblSchedule.Font = new Font("Segoe UI", 9F);
            lblSchedule.AutoSize = true;
            lblSchedule.Location = new Point(18, 30);

            cmbSchedule = new ComboBox();
            cmbSchedule.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSchedule.Font = new Font("Segoe UI", 10F);
            cmbSchedule.Location = new Point(18, 52);
            cmbSchedule.Size = new Size(380, 25);

            btnLoadTrip = new Button();
            btnLoadTrip.Text = "Load Trip Passengers";
            btnLoadTrip.Location = new Point(415, 50);
            btnLoadTrip.Size = new Size(140, 30);
            btnLoadTrip.BackColor = Color.FromArgb(46, 204, 113);
            btnLoadTrip.ForeColor = Color.White;
            btnLoadTrip.FlatStyle = FlatStyle.Flat;
            btnLoadTrip.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnLoadTrip.Click += btnLoadTrip_Click;

            groupTrip.Controls.Add(lblSchedule);
            groupTrip.Controls.Add(cmbSchedule);
            groupTrip.Controls.Add(btnLoadTrip);
            this.Controls.Add(groupTrip);

            groupDetails = new GroupBox();
            groupDetails.Text = "Booking Details";
            groupDetails.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            groupDetails.Location = new Point(20, 195);
            groupDetails.Size = new Size(1160, 185);

            CreateLabel(groupDetails, "Booking ID", 20, 32);
            txtBookingID = CreateTextBox(groupDetails, 140, 28);

            CreateLabel(groupDetails, "Passenger Name", 20, 68);
            txtPassengerName = CreateTextBox(groupDetails, 140, 64);

            CreateLabel(groupDetails, "Phone", 20, 104);
            txtPhone = CreateTextBox(groupDetails, 140, 100);

            CreateLabel(groupDetails, "Seat No.", 20, 140);
            txtSeatNo = CreateTextBox(groupDetails, 140, 136);

            CreateLabel(groupDetails, "Payment Status", 350, 32);
            txtPaymentStatus = CreateTextBox(groupDetails, 480, 28);

            CreateLabel(groupDetails, "Reservation Status", 350, 68);
            txtReservationStatus = CreateTextBox(groupDetails, 480, 64);

            CreateLabel(groupDetails, "Check-in Status", 350, 104);
            txtCheckinStatus = CreateTextBox(groupDetails, 480, 100);

            CreateLabel(groupDetails, "Boarding Status", 350, 140);
            txtBoardingStatus = CreateTextBox(groupDetails, 480, 136);

            btnCheckIn = CreateButton("Mark Checked-in", 720, 28, Color.FromArgb(52, 152, 219));
            btnCheckIn.Click += btnCheckIn_Click;

            btnBoard = CreateButton("Board Passenger", 920, 28, Color.FromArgb(46, 204, 113));
            btnBoard.Click += btnBoard_Click;

            btnNoShow = CreateButton("Mark No-show", 720, 85, Color.FromArgb(231, 76, 60));
            btnNoShow.Click += btnNoShow_Click;

            btnClear = CreateButton("Clear", 920, 85, Color.FromArgb(127, 140, 141));
            btnClear.Click += btnClear_Click;

            groupDetails.Controls.Add(btnCheckIn);
            groupDetails.Controls.Add(btnBoard);
            groupDetails.Controls.Add(btnNoShow);
            groupDetails.Controls.Add(btnClear);

            this.Controls.Add(groupDetails);

            dgvPassengers = new DataGridView();
            dgvPassengers.Location = new Point(20, 400);
            dgvPassengers.Size = new Size(1160, 330);
            dgvPassengers.BackgroundColor = Color.White;
            dgvPassengers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPassengers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPassengers.MultiSelect = false;
            dgvPassengers.ReadOnly = true;
            dgvPassengers.AllowUserToAddRows = false;
            dgvPassengers.AllowUserToDeleteRows = false;
            dgvPassengers.RowHeadersVisible = false;
            dgvPassengers.CellClick += dgvPassengers_CellClick;

            this.Controls.Add(dgvPassengers);
        }

        private void CreateLabel(Control parent, string text, int x, int y)
        {
            Label lbl = new Label();
            lbl.Text = text;
            lbl.Font = new Font("Segoe UI", 9F);
            lbl.AutoSize = true;
            lbl.Location = new Point(x, y);
            parent.Controls.Add(lbl);
        }

        private TextBox CreateTextBox(Control parent, int x, int y)
        {
            TextBox txt = new TextBox();
            txt.Font = new Font("Segoe UI", 10F);
            txt.Location = new Point(x, y);
            txt.Size = new Size(180, 25);
            txt.ReadOnly = true;
            parent.Controls.Add(txt);
            return txt;
        }

        private Button CreateButton(string text, int x, int y, Color color)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.Location = new Point(x, y);
            btn.Size = new Size(180, 35);
            btn.BackColor = color;
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            return btn;
        }

        private void LoadSchedules()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                        SELECT 
                            s.schedule_id,
                            CONCAT(
                                'Trip #', s.schedule_id, ' | ',
                                b.bus_number, ' | ',
                                r.origin, ' to ', r.destination, ' | ',
                                DATE_FORMAT(s.departure_date, '%Y-%m-%d'), ' ',
                                TIME_FORMAT(s.departure_time, '%h:%i %p')
                            ) AS schedule_name
                        FROM schedules s
                        INNER JOIN buses b ON s.bus_id = b.bus_id
                        INNER JOIN routes r ON s.route_id = r.route_id
                        WHERE s.trip_status != 'Cancelled'
                        ORDER BY s.departure_date ASC, s.departure_time ASC";

                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    cmbSchedule.DataSource = dt;
                    cmbSchedule.DisplayMember = "schedule_name";
                    cmbSchedule.ValueMember = "schedule_id";
                    cmbSchedule.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading schedules: " + ex.Message);
            }
        }

        private void LoadPassengers(string search = "", int scheduleId = 0)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                        SELECT
                            bk.booking_id,
                            bk.passenger_name,
                            bk.phone,
                            bk.email,
                            CONCAT(r.origin, ' to ', r.destination) AS route,
                            b.bus_number,
                            bk.seat_no,
                            s.departure_date,
                            s.departure_time,
                            bk.payment_status,
                            bk.reservation_status,
                            bk.checkin_status,
                            bk.boarding_status,
                            bk.schedule_id
                        FROM bookings bk
                        INNER JOIN schedules s ON bk.schedule_id = s.schedule_id
                        INNER JOIN buses b ON s.bus_id = b.bus_id
                        INNER JOIN routes r ON s.route_id = r.route_id
                        WHERE 1 = 1";

                    if (search != "")
                    {
                        query += @"
                            AND (
                                bk.booking_id LIKE @search OR
                                bk.passenger_name LIKE @search OR
                                bk.phone LIKE @search OR
                                bk.seat_no LIKE @search
                            )";
                    }

                    if (scheduleId > 0)
                    {
                        query += " AND bk.schedule_id = @schedule_id";
                    }

                    query += " ORDER BY s.departure_date ASC, s.departure_time ASC, bk.seat_no ASC";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        if (search != "")
                        {
                            cmd.Parameters.AddWithValue("@search", "%" + search + "%");
                        }

                        if (scheduleId > 0)
                        {
                            cmd.Parameters.AddWithValue("@schedule_id", scheduleId);
                        }

                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dgvPassengers.DataSource = dt;
                        FormatGrid();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading passengers: " + ex.Message);
            }
        }

        private void FormatGrid()
        {
            if (dgvPassengers.Columns.Contains("schedule_id"))
                dgvPassengers.Columns["schedule_id"].Visible = false;

            if (dgvPassengers.Columns.Contains("booking_id"))
                dgvPassengers.Columns["booking_id"].HeaderText = "Booking ID";

            if (dgvPassengers.Columns.Contains("passenger_name"))
                dgvPassengers.Columns["passenger_name"].HeaderText = "Passenger";

            if (dgvPassengers.Columns.Contains("phone"))
                dgvPassengers.Columns["phone"].HeaderText = "Phone";

            if (dgvPassengers.Columns.Contains("email"))
                dgvPassengers.Columns["email"].HeaderText = "Email";

            if (dgvPassengers.Columns.Contains("route"))
                dgvPassengers.Columns["route"].HeaderText = "Route";

            if (dgvPassengers.Columns.Contains("bus_number"))
                dgvPassengers.Columns["bus_number"].HeaderText = "Bus";

            if (dgvPassengers.Columns.Contains("seat_no"))
                dgvPassengers.Columns["seat_no"].HeaderText = "Seat No.";

            if (dgvPassengers.Columns.Contains("departure_date"))
                dgvPassengers.Columns["departure_date"].HeaderText = "Date";

            if (dgvPassengers.Columns.Contains("departure_time"))
                dgvPassengers.Columns["departure_time"].HeaderText = "Time";

            if (dgvPassengers.Columns.Contains("payment_status"))
                dgvPassengers.Columns["payment_status"].HeaderText = "Payment";

            if (dgvPassengers.Columns.Contains("reservation_status"))
                dgvPassengers.Columns["reservation_status"].HeaderText = "Reservation";

            if (dgvPassengers.Columns.Contains("checkin_status"))
                dgvPassengers.Columns["checkin_status"].HeaderText = "Check-in";

            if (dgvPassengers.Columns.Contains("boarding_status"))
                dgvPassengers.Columns["boarding_status"].HeaderText = "Boarding";
        }

        private void ClearFields()
        {
            txtBookingID.Clear();
            txtPassengerName.Clear();
            txtPhone.Clear();
            txtSeatNo.Clear();
            txtPaymentStatus.Clear();
            txtReservationStatus.Clear();
            txtCheckinStatus.Clear();
            txtBoardingStatus.Clear();
        }

        private bool HasSelectedBooking()
        {
            if (txtBookingID.Text.Trim() == "")
            {
                MessageBox.Show("Please select a passenger booking first.");
                return false;
            }

            return true;
        }

        private bool IsAllowedForCheckIn()
        {
            if (txtPaymentStatus.Text != "Paid")
            {
                MessageBox.Show("Passenger cannot check-in because payment is not paid.");
                return false;
            }

            if (txtReservationStatus.Text != "Confirmed" && txtReservationStatus.Text != "Completed")
            {
                MessageBox.Show("Passenger cannot check-in because reservation is not confirmed.");
                return false;
            }

            if (txtBoardingStatus.Text == "No-show")
            {
                MessageBox.Show("Passenger is already marked as No-show.");
                return false;
            }

            return true;
        }

        private void UpdateBookingStatus(string checkinStatus, string boardingStatus)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                        UPDATE bookings SET
                            checkin_status = @checkin_status,
                            boarding_status = @boarding_status,
                            reservation_status = CASE 
                                WHEN @boarding_status = 'Boarded' THEN 'Completed'
                                ELSE reservation_status
                            END
                        WHERE booking_id = @booking_id";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@booking_id", txtBookingID.Text.Trim());
                        cmd.Parameters.AddWithValue("@checkin_status", checkinStatus);
                        cmd.Parameters.AddWithValue("@boarding_status", boardingStatus);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Booking status updated successfully.");
                LoadPassengers();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating booking status: " + ex.Message);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadPassengers(txtSearch.Text.Trim());
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            LoadSchedules();
            LoadPassengers();
            ClearFields();
        }

        private void btnLoadTrip_Click(object sender, EventArgs e)
        {
            if (cmbSchedule.SelectedValue == null)
            {
                MessageBox.Show("Please select schedule.");
                return;
            }

            if (cmbSchedule.SelectedValue is DataRowView)
            {
                return;
            }

            int scheduleId = Convert.ToInt32(cmbSchedule.SelectedValue);
            LoadPassengers("", scheduleId);
        }

        private void btnCheckIn_Click(object sender, EventArgs e)
        {
            if (!HasSelectedBooking())
                return;

            if (!IsAllowedForCheckIn())
                return;

            if (txtCheckinStatus.Text == "Checked-in")
            {
                MessageBox.Show("Passenger is already checked-in.");
                return;
            }

            UpdateBookingStatus("Checked-in", "Not Boarded");
        }

        private void btnBoard_Click(object sender, EventArgs e)
        {
            if (!HasSelectedBooking())
                return;

            if (!IsAllowedForCheckIn())
                return;

            if (txtCheckinStatus.Text != "Checked-in")
            {
                MessageBox.Show("Passenger must be checked-in first before boarding.");
                return;
            }

            if (txtBoardingStatus.Text == "Boarded")
            {
                MessageBox.Show("Passenger is already boarded.");
                return;
            }

            UpdateBookingStatus("Checked-in", "Boarded");
        }

        private void btnNoShow_Click(object sender, EventArgs e)
        {
            if (!HasSelectedBooking())
                return;

            if (txtBoardingStatus.Text == "Boarded")
            {
                MessageBox.Show("Cannot mark as No-show because passenger is already boarded.");
                return;
            }

            DialogResult result = MessageBox.Show(
                "Mark this passenger as No-show?",
                "Confirm No-show",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.No)
                return;

            UpdateBookingStatus("Not Checked-in", "No-show");
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void dgvPassengers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvPassengers.Rows[e.RowIndex].Cells["booking_id"].Value != null)
            {
                DataGridViewRow row = dgvPassengers.Rows[e.RowIndex];

                txtBookingID.Text = row.Cells["booking_id"].Value.ToString();
                txtPassengerName.Text = row.Cells["passenger_name"].Value.ToString();
                txtPhone.Text = row.Cells["phone"].Value.ToString();
                txtSeatNo.Text = row.Cells["seat_no"].Value.ToString();
                txtPaymentStatus.Text = row.Cells["payment_status"].Value.ToString();
                txtReservationStatus.Text = row.Cells["reservation_status"].Value.ToString();
                txtCheckinStatus.Text = row.Cells["checkin_status"].Value.ToString();
                txtBoardingStatus.Text = row.Cells["boarding_status"].Value.ToString();
            }
        }
    }
}