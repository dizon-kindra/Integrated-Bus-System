using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;


namespace sr
{
    public partial class ReservationManagementForm : Form
    {
        string connectionString = "server=localhost;user id=root;password=;database=sr_db;";

        public ReservationManagementForm()
        {
            InitializeComponent();
            CreateReservationDesign();
        }
        private void CreateReservationDesign()
        {
            this.Controls.Clear();

            this.Text = "Reservation Management";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ClientSize = new Size(1180, 730);
            this.BackColor = Color.FromArgb(245, 247, 250);
            this.Font = new Font("Segoe UI", 9F);

            Panel panelHeader = new Panel();
            panelHeader.BackColor = Color.FromArgb(32, 45, 64);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Height = 65;

            Label lblTitle = new Label();
            lblTitle.Text = "Reservation Management";
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(25, 16);

            panelHeader.Controls.Add(lblTitle);
            this.Controls.Add(panelHeader);

            GroupBox groupBoxInfo = new GroupBox();
            groupBoxInfo.Text = "Booking Information";
            groupBoxInfo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            groupBoxInfo.Location = new Point(20, 80);
            groupBoxInfo.Size = new Size(1140, 255);

            Label lblBookingID = new Label();
            lblBookingID.Text = "Booking ID";
            lblBookingID.Font = new Font("Segoe UI", 9.5F);
            lblBookingID.Location = new Point(25, 35);
            lblBookingID.AutoSize = true;

            txtBookingID = new TextBox();
            txtBookingID.Location = new Point(160, 32);
            txtBookingID.Size = new Size(220, 25);
            txtBookingID.ReadOnly = true;

            Label lblPassengerName = new Label();
            lblPassengerName.Text = "Passenger Name";
            lblPassengerName.Font = new Font("Segoe UI", 9.5F);
            lblPassengerName.Location = new Point(25, 75);
            lblPassengerName.AutoSize = true;

            txtPassengerName = new TextBox();
            txtPassengerName.Location = new Point(160, 72);
            txtPassengerName.Size = new Size(220, 25);

            Label lblPhone = new Label();
            lblPhone.Text = "Phone";
            lblPhone.Font = new Font("Segoe UI", 9.5F);
            lblPhone.Location = new Point(25, 115);
            lblPhone.AutoSize = true;

            txtPhone = new TextBox();
            txtPhone.Location = new Point(160, 112);
            txtPhone.Size = new Size(220, 25);

            Label lblEmail = new Label();
            lblEmail.Text = "Email";
            lblEmail.Font = new Font("Segoe UI", 9.5F);
            lblEmail.Location = new Point(25, 155);
            lblEmail.AutoSize = true;

            txtEmail = new TextBox();
            txtEmail.Location = new Point(160, 152);
            txtEmail.Size = new Size(220, 25);

            Label lblSchedule = new Label();
            lblSchedule.Text = "Schedule";
            lblSchedule.Font = new Font("Segoe UI", 9.5F);
            lblSchedule.Location = new Point(430, 35);
            lblSchedule.AutoSize = true;

            cmbSchedule = new ComboBox();
            cmbSchedule.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSchedule.Location = new Point(570, 32);
            cmbSchedule.Size = new Size(520, 25);

            Label lblSeatNo = new Label();
            lblSeatNo.Text = "Seat No.";
            lblSeatNo.Font = new Font("Segoe UI", 9.5F);
            lblSeatNo.Location = new Point(430, 75);
            lblSeatNo.AutoSize = true;

            txtSeatNo = new TextBox();
            txtSeatNo.Location = new Point(570, 72);
            txtSeatNo.Size = new Size(180, 25);

            Label lblPaymentStatus = new Label();
            lblPaymentStatus.Text = "Payment Status";
            lblPaymentStatus.Font = new Font("Segoe UI", 9.5F);
            lblPaymentStatus.Location = new Point(430, 115);
            lblPaymentStatus.AutoSize = true;

            cmbPaymentStatus = new ComboBox();
            cmbPaymentStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPaymentStatus.Location = new Point(570, 112);
            cmbPaymentStatus.Size = new Size(180, 25);

            Label lblReservationStatus = new Label();
            lblReservationStatus.Text = "Reservation Status";
            lblReservationStatus.Font = new Font("Segoe UI", 9.5F);
            lblReservationStatus.Location = new Point(430, 155);
            lblReservationStatus.AutoSize = true;

            cmbReservationStatus = new ComboBox();
            cmbReservationStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbReservationStatus.Location = new Point(570, 152);
            cmbReservationStatus.Size = new Size(180, 25);

            btnAdd = new Button();
            btnAdd.Text = "Add Booking";
            btnAdd.Location = new Point(160, 200);
            btnAdd.Size = new Size(130, 35);
            btnAdd.BackColor = Color.FromArgb(40, 167, 69);
            btnAdd.ForeColor = Color.White;
            btnAdd.FlatStyle = FlatStyle.Flat;
            btnAdd.Click += btnAdd_Click;

            btnUpdate = new Button();
            btnUpdate.Text = "Update Booking";
            btnUpdate.Location = new Point(305, 200);
            btnUpdate.Size = new Size(135, 35);
            btnUpdate.BackColor = Color.FromArgb(0, 123, 255);
            btnUpdate.ForeColor = Color.White;
            btnUpdate.FlatStyle = FlatStyle.Flat;
            btnUpdate.Click += btnUpdate_Click;

            btnConfirm = new Button();
            btnConfirm.Text = "Confirm Booking";
            btnConfirm.Location = new Point(455, 200);
            btnConfirm.Size = new Size(140, 35);
            btnConfirm.BackColor = Color.FromArgb(23, 162, 184);
            btnConfirm.ForeColor = Color.White;
            btnConfirm.FlatStyle = FlatStyle.Flat;
            btnConfirm.Click += btnConfirm_Click;

            btnCancelBooking = new Button();
            btnCancelBooking.Text = "Cancel Booking";
            btnCancelBooking.Location = new Point(610, 200);
            btnCancelBooking.Size = new Size(140, 35);
            btnCancelBooking.BackColor = Color.FromArgb(220, 53, 69);
            btnCancelBooking.ForeColor = Color.White;
            btnCancelBooking.FlatStyle = FlatStyle.Flat;
            btnCancelBooking.Click += btnCancelBooking_Click;

            btnClear = new Button();
            btnClear.Text = "Clear";
            btnClear.Location = new Point(765, 200);
            btnClear.Size = new Size(110, 35);
            btnClear.BackColor = Color.FromArgb(108, 117, 125);
            btnClear.ForeColor = Color.White;
            btnClear.FlatStyle = FlatStyle.Flat;
            btnClear.Click += btnClear_Click;

            groupBoxInfo.Controls.Add(lblBookingID);
            groupBoxInfo.Controls.Add(txtBookingID);
            groupBoxInfo.Controls.Add(lblPassengerName);
            groupBoxInfo.Controls.Add(txtPassengerName);
            groupBoxInfo.Controls.Add(lblPhone);
            groupBoxInfo.Controls.Add(txtPhone);
            groupBoxInfo.Controls.Add(lblEmail);
            groupBoxInfo.Controls.Add(txtEmail);
            groupBoxInfo.Controls.Add(lblSchedule);
            groupBoxInfo.Controls.Add(cmbSchedule);
            groupBoxInfo.Controls.Add(lblSeatNo);
            groupBoxInfo.Controls.Add(txtSeatNo);
            groupBoxInfo.Controls.Add(lblPaymentStatus);
            groupBoxInfo.Controls.Add(cmbPaymentStatus);
            groupBoxInfo.Controls.Add(lblReservationStatus);
            groupBoxInfo.Controls.Add(cmbReservationStatus);
            groupBoxInfo.Controls.Add(btnAdd);
            groupBoxInfo.Controls.Add(btnUpdate);
            groupBoxInfo.Controls.Add(btnConfirm);
            groupBoxInfo.Controls.Add(btnCancelBooking);
            groupBoxInfo.Controls.Add(btnClear);

            this.Controls.Add(groupBoxInfo);

            GroupBox groupBoxList = new GroupBox();
            groupBoxList.Text = "Booking List";
            groupBoxList.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            groupBoxList.Location = new Point(20, 350);
            groupBoxList.Size = new Size(1140, 360);

            dgvBookings = new DataGridView();
            dgvBookings.Dock = DockStyle.Fill;
            dgvBookings.BackgroundColor = Color.White;
            dgvBookings.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvBookings.AllowUserToAddRows = false;
            dgvBookings.AllowUserToDeleteRows = false;
            dgvBookings.ReadOnly = true;
            dgvBookings.MultiSelect = false;
            dgvBookings.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBookings.CellClick += dgvBookings_CellClick;

            groupBoxList.Controls.Add(dgvBookings);
            this.Controls.Add(groupBoxList);
        }
        private void EnsureComboBoxesCreated()
        {
            if (cmbSchedule == null)
            {
                cmbSchedule = new ComboBox();
                cmbSchedule.Name = "cmbSchedule";
                cmbSchedule.DropDownStyle = ComboBoxStyle.DropDownList;
                cmbSchedule.Font = new Font("Segoe UI", 10F);
                cmbSchedule.Location = new Point(570, 32);
                cmbSchedule.Size = new Size(520, 25);

                Label lblSchedule = new Label();
                lblSchedule.Text = "Schedule";
                lblSchedule.Font = new Font("Segoe UI", 9.5F);
                lblSchedule.Location = new Point(430, 35);
                lblSchedule.AutoSize = true;

                this.Controls.Add(lblSchedule);
                this.Controls.Add(cmbSchedule);
                cmbSchedule.BringToFront();
            }

            if (cmbPaymentStatus == null)
            {
                cmbPaymentStatus = new ComboBox();
                cmbPaymentStatus.Name = "cmbPaymentStatus";
                cmbPaymentStatus.DropDownStyle = ComboBoxStyle.DropDownList;
                cmbPaymentStatus.Font = new Font("Segoe UI", 10F);
                cmbPaymentStatus.Location = new Point(570, 112);
                cmbPaymentStatus.Size = new Size(180, 25);

                Label lblPayment = new Label();
                lblPayment.Text = "Payment Status";
                lblPayment.Font = new Font("Segoe UI", 9.5F);
                lblPayment.Location = new Point(430, 115);
                lblPayment.AutoSize = true;

                this.Controls.Add(lblPayment);
                this.Controls.Add(cmbPaymentStatus);
                cmbPaymentStatus.BringToFront();
            }

            if (cmbReservationStatus == null)
            {
                cmbReservationStatus = new ComboBox();
                cmbReservationStatus.Name = "cmbReservationStatus";
                cmbReservationStatus.DropDownStyle = ComboBoxStyle.DropDownList;
                cmbReservationStatus.Font = new Font("Segoe UI", 10F);
                cmbReservationStatus.Location = new Point(570, 152);
                cmbReservationStatus.Size = new Size(180, 25);

                Label lblReservation = new Label();
                lblReservation.Text = "Reservation Status";
                lblReservation.Font = new Font("Segoe UI", 9.5F);
                lblReservation.Location = new Point(430, 155);
                lblReservation.AutoSize = true;

                this.Controls.Add(lblReservation);
                this.Controls.Add(cmbReservationStatus);
                cmbReservationStatus.BringToFront();
            }
        }
        private void EnsureDataGridViewCreated()
        {
            if (dgvBookings == null)
            {
                dgvBookings = new DataGridView();
                dgvBookings.Name = "dgvBookings";
                dgvBookings.Location = new Point(20, 350);
                dgvBookings.Size = new Size(1140, 330);
                dgvBookings.BackgroundColor = Color.White;
                dgvBookings.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvBookings.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                dgvBookings.AllowUserToAddRows = false;
                dgvBookings.AllowUserToDeleteRows = false;
                dgvBookings.ReadOnly = true;
                dgvBookings.MultiSelect = false;
                dgvBookings.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                dgvBookings.CellClick += new DataGridViewCellEventHandler(dgvBookings_CellClick);

                Label lblList = new Label();
                lblList.Text = "Booking List";
                lblList.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
                lblList.Location = new Point(20, 320);
                lblList.AutoSize = true;

                this.Controls.Add(lblList);
                this.Controls.Add(dgvBookings);
                dgvBookings.BringToFront();
            }
        }

        private void ReservationManagementForm_Load(object sender, EventArgs e)
        {
            LoadPaymentStatus();
            LoadReservationStatus();
            LoadSchedules();
            LoadBookings();
        }

        private void ReservationManagementForm_Load_1(object sender, EventArgs e)
        {
            ReservationManagementForm_Load(sender, e);
        }

        private void LoadPaymentStatus()
        {
            if (cmbPaymentStatus == null)
            {
                MessageBox.Show("cmbPaymentStatus is missing. Please check Designer file.");
                return;
            }

            cmbPaymentStatus.Items.Clear();
            cmbPaymentStatus.Items.Add("Pending");
            cmbPaymentStatus.Items.Add("Paid");
            cmbPaymentStatus.Items.Add("Unpaid");
            cmbPaymentStatus.Items.Add("Refunded");

            if (cmbPaymentStatus.Items.Count > 0)
            {
                cmbPaymentStatus.SelectedIndex = 0;
            }
        }

        private void LoadReservationStatus()
        {
            if (cmbReservationStatus == null)
            {
                MessageBox.Show("cmbReservationStatus is missing. Please check Designer file.");
                return;
            }

            cmbReservationStatus.Items.Clear();
            cmbReservationStatus.Items.Add("Pending");
            cmbReservationStatus.Items.Add("Confirmed");
            cmbReservationStatus.Items.Add("Cancelled");
            cmbReservationStatus.Items.Add("Completed");

            if (cmbReservationStatus.Items.Count > 0)
            {
                cmbReservationStatus.SelectedIndex = 0;
            }
        }

        private void LoadSchedules()
        {
            if (cmbSchedule == null)
            {
                MessageBox.Show("cmbSchedule is missing. Please check Designer file.");
                return;
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                        SELECT 
                            s.schedule_id,
                            CONCAT(
                                b.bus_number, ' | ',
                                r.origin, ' to ', r.destination, ' | ',
                                DATE_FORMAT(s.departure_date, '%Y-%m-%d'), ' ',
                                TIME_FORMAT(s.departure_time, '%h:%i %p'),
                                ' | Fare: ', s.fare,
                                ' | Seats: ', s.available_seats
                            ) AS schedule_name
                        FROM schedules s
                        INNER JOIN buses b ON s.bus_id = b.bus_id
                        INNER JOIN routes r ON s.route_id = r.route_id
                        WHERE s.trip_status != 'Cancelled'
                        ORDER BY s.departure_date ASC, s.departure_time ASC";

                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    cmbSchedule.DataSource = null;
                    cmbSchedule.DataSource = dt;
                    cmbSchedule.DisplayMember = "schedule_name";
                    cmbSchedule.ValueMember = "schedule_id";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading schedules: " + ex.Message);
            }
        }

        private void LoadBookings()
        {
            if (dgvBookings == null)
            {
                MessageBox.Show("dgvBookings is missing. Please check Designer file.");
                return;
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                        SELECT
                            bk.booking_id AS booking_id,
                            bk.passenger_name AS passenger_name,
                            bk.phone AS phone,
                            bk.email AS email,
                            CONCAT(r.origin, ' to ', r.destination) AS route,
                            b.bus_number AS bus_number,
                            bk.seat_no AS seat_no,
                            s.departure_date AS departure_date,
                            s.departure_time AS departure_time,
                            bk.payment_status AS payment_status,
                            bk.reservation_status AS reservation_status,
                            bk.checkin_status AS checkin_status,
                            bk.boarding_status AS boarding_status,
                            bk.created_at AS created_at,
                            bk.schedule_id AS schedule_id
                        FROM bookings bk
                        INNER JOIN schedules s ON bk.schedule_id = s.schedule_id
                        INNER JOIN buses b ON s.bus_id = b.bus_id
                        INNER JOIN routes r ON s.route_id = r.route_id
                        ORDER BY bk.booking_id DESC";

                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dgvBookings.DataSource = dt;

                    if (dgvBookings.Columns.Contains("booking_id"))
                        dgvBookings.Columns["booking_id"].HeaderText = "Booking ID";

                    if (dgvBookings.Columns.Contains("passenger_name"))
                        dgvBookings.Columns["passenger_name"].HeaderText = "Passenger Name";

                    if (dgvBookings.Columns.Contains("phone"))
                        dgvBookings.Columns["phone"].HeaderText = "Phone";

                    if (dgvBookings.Columns.Contains("email"))
                        dgvBookings.Columns["email"].HeaderText = "Email";

                    if (dgvBookings.Columns.Contains("route"))
                        dgvBookings.Columns["route"].HeaderText = "Route";

                    if (dgvBookings.Columns.Contains("bus_number"))
                        dgvBookings.Columns["bus_number"].HeaderText = "Bus No.";

                    if (dgvBookings.Columns.Contains("seat_no"))
                        dgvBookings.Columns["seat_no"].HeaderText = "Seat No.";

                    if (dgvBookings.Columns.Contains("departure_date"))
                        dgvBookings.Columns["departure_date"].HeaderText = "Departure Date";

                    if (dgvBookings.Columns.Contains("departure_time"))
                        dgvBookings.Columns["departure_time"].HeaderText = "Departure Time";

                    if (dgvBookings.Columns.Contains("payment_status"))
                        dgvBookings.Columns["payment_status"].HeaderText = "Payment Status";

                    if (dgvBookings.Columns.Contains("reservation_status"))
                        dgvBookings.Columns["reservation_status"].HeaderText = "Reservation Status";

                    if (dgvBookings.Columns.Contains("checkin_status"))
                        dgvBookings.Columns["checkin_status"].HeaderText = "Check-in Status";

                    if (dgvBookings.Columns.Contains("boarding_status"))
                        dgvBookings.Columns["boarding_status"].HeaderText = "Boarding Status";

                    if (dgvBookings.Columns.Contains("created_at"))
                        dgvBookings.Columns["created_at"].HeaderText = "Created At";

                    if (dgvBookings.Columns.Contains("schedule_id"))
                        dgvBookings.Columns["schedule_id"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading bookings: " + ex.Message);
            }
        }

        private bool ValidateFields()
        {
            if (cmbSchedule == null || cmbSchedule.SelectedValue == null)
            {
                MessageBox.Show("Please select a schedule.");
                return false;
            }

            if (txtPassengerName.Text.Trim() == "")
            {
                MessageBox.Show("Please enter passenger name.");
                txtPassengerName.Focus();
                return false;
            }

            if (txtPhone.Text.Trim() == "")
            {
                MessageBox.Show("Please enter phone number.");
                txtPhone.Focus();
                return false;
            }

            if (txtSeatNo.Text.Trim() == "")
            {
                MessageBox.Show("Please enter seat number.");
                txtSeatNo.Focus();
                return false;
            }

            int seatNo;

            if (!int.TryParse(txtSeatNo.Text.Trim(), out seatNo))
            {
                MessageBox.Show("Seat number must be a valid number.");
                txtSeatNo.Focus();
                return false;
            }

            if (seatNo <= 0)
            {
                MessageBox.Show("Seat number must be greater than zero.");
                txtSeatNo.Focus();
                return false;
            }

            return true;
        }

        private bool IsSeatAlreadyBooked(int scheduleId, int seatNo, string bookingIdToExclude = "")
        {
            bool exists = false;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                        SELECT COUNT(*) 
                        FROM bookings
                        WHERE schedule_id = @schedule_id
                        AND seat_no = @seat_no
                        AND reservation_status != 'Cancelled'";

                    if (bookingIdToExclude != "")
                    {
                        query += " AND booking_id != @booking_id";
                    }

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@schedule_id", scheduleId);
                        cmd.Parameters.AddWithValue("@seat_no", seatNo);

                        if (bookingIdToExclude != "")
                        {
                            cmd.Parameters.AddWithValue("@booking_id", bookingIdToExclude);
                        }

                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        exists = count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error checking seat: " + ex.Message);
            }

            return exists;
        }

        private int GetAvailableSeats(int scheduleId)
        {
            int availableSeats = 0;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                        SELECT available_seats 
                        FROM schedules 
                        WHERE schedule_id = @schedule_id";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@schedule_id", scheduleId);

                        object result = cmd.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            availableSeats = Convert.ToInt32(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error checking available seats: " + ex.Message);
            }

            return availableSeats;
        }

        private void DecreaseAvailableSeats(int scheduleId)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                        UPDATE schedules
                        SET available_seats = available_seats - 1
                        WHERE schedule_id = @schedule_id
                        AND available_seats > 0";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@schedule_id", scheduleId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error decreasing available seats: " + ex.Message);
            }
        }

        private void IncreaseAvailableSeats(int scheduleId)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                        UPDATE schedules
                        SET available_seats = available_seats + 1
                        WHERE schedule_id = @schedule_id";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@schedule_id", scheduleId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error increasing available seats: " + ex.Message);
            }
        }

        private string GetCurrentReservationStatus(string bookingId)
        {
            string status = "";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                        SELECT reservation_status 
                        FROM bookings 
                        WHERE booking_id = @booking_id";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@booking_id", bookingId);

                        object result = cmd.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            status = result.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error checking reservation status: " + ex.Message);
            }

            return status;
        }

        private void ClearFields()
        {
            txtBookingID.Clear();
            txtPassengerName.Clear();
            txtPhone.Clear();
            txtEmail.Clear();
            txtSeatNo.Clear();

            if (cmbSchedule != null && cmbSchedule.Items.Count > 0)
                cmbSchedule.SelectedIndex = 0;

            if (cmbPaymentStatus != null && cmbPaymentStatus.Items.Count > 0)
                cmbPaymentStatus.SelectedIndex = 0;

            if (cmbReservationStatus != null && cmbReservationStatus.Items.Count > 0)
                cmbReservationStatus.SelectedIndex = 0;

            if (dgvBookings != null)
                dgvBookings.ClearSelection();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateFields())
                return;

            int scheduleId = Convert.ToInt32(cmbSchedule.SelectedValue);
            int seatNo = Convert.ToInt32(txtSeatNo.Text.Trim());

            if (GetAvailableSeats(scheduleId) <= 0)
            {
                MessageBox.Show("No available seats for this schedule.");
                return;
            }

            if (IsSeatAlreadyBooked(scheduleId, seatNo))
            {
                MessageBox.Show("This seat is already booked for the selected schedule.");
                return;
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                        INSERT INTO bookings
                        (
                            schedule_id,
                            passenger_name,
                            phone,
                            email,
                            seat_no,
                            payment_status,
                            reservation_status,
                            checkin_status,
                            boarding_status
                        )
                        VALUES
                        (
                            @schedule_id,
                            @passenger_name,
                            @phone,
                            @email,
                            @seat_no,
                            @payment_status,
                            @reservation_status,
                            'Not Checked-in',
                            'Not Boarded'
                        )";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@schedule_id", scheduleId);
                        cmd.Parameters.AddWithValue("@passenger_name", txtPassengerName.Text.Trim());
                        cmd.Parameters.AddWithValue("@phone", txtPhone.Text.Trim());
                        cmd.Parameters.AddWithValue("@email", txtEmail.Text.Trim());
                        cmd.Parameters.AddWithValue("@seat_no", seatNo);
                        cmd.Parameters.AddWithValue("@payment_status", cmbPaymentStatus.Text);
                        cmd.Parameters.AddWithValue("@reservation_status", cmbReservationStatus.Text);

                        cmd.ExecuteNonQuery();
                    }
                }

                DecreaseAvailableSeats(scheduleId);

                MessageBox.Show("Booking added successfully.");

                LoadSchedules();
                LoadBookings();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding booking: " + ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtBookingID.Text.Trim() == "")
            {
                MessageBox.Show("Please select a booking to update.");
                return;
            }

            if (!ValidateFields())
                return;

            int scheduleId = Convert.ToInt32(cmbSchedule.SelectedValue);
            int seatNo = Convert.ToInt32(txtSeatNo.Text.Trim());

            if (IsSeatAlreadyBooked(scheduleId, seatNo, txtBookingID.Text.Trim()))
            {
                MessageBox.Show("This seat is already booked for the selected schedule.");
                return;
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                        UPDATE bookings SET
                            schedule_id = @schedule_id,
                            passenger_name = @passenger_name,
                            phone = @phone,
                            email = @email,
                            seat_no = @seat_no,
                            payment_status = @payment_status,
                            reservation_status = @reservation_status
                        WHERE booking_id = @booking_id";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@booking_id", txtBookingID.Text.Trim());
                        cmd.Parameters.AddWithValue("@schedule_id", scheduleId);
                        cmd.Parameters.AddWithValue("@passenger_name", txtPassengerName.Text.Trim());
                        cmd.Parameters.AddWithValue("@phone", txtPhone.Text.Trim());
                        cmd.Parameters.AddWithValue("@email", txtEmail.Text.Trim());
                        cmd.Parameters.AddWithValue("@seat_no", seatNo);
                        cmd.Parameters.AddWithValue("@payment_status", cmbPaymentStatus.Text);
                        cmd.Parameters.AddWithValue("@reservation_status", cmbReservationStatus.Text);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Booking updated successfully.");

                LoadSchedules();
                LoadBookings();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating booking: " + ex.Message);
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (txtBookingID.Text.Trim() == "")
            {
                MessageBox.Show("Please select a booking to confirm.");
                return;
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                        UPDATE bookings SET
                            reservation_status = 'Confirmed'
                        WHERE booking_id = @booking_id";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@booking_id", txtBookingID.Text.Trim());
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Booking confirmed successfully.");

                LoadBookings();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error confirming booking: " + ex.Message);
            }
        }

        private void btnCancelBooking_Click(object sender, EventArgs e)
        {
            if (txtBookingID.Text.Trim() == "")
            {
                MessageBox.Show("Please select a booking to cancel.");
                return;
            }

            DialogResult result = MessageBox.Show(
                "Are you sure you want to cancel this booking?",
                "Confirm Cancel Booking",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.No)
                return;

            string bookingId = txtBookingID.Text.Trim();
            int scheduleId = Convert.ToInt32(cmbSchedule.SelectedValue);

            string currentStatus = GetCurrentReservationStatus(bookingId);

            if (currentStatus == "Cancelled")
            {
                MessageBox.Show("This booking is already cancelled.");
                return;
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                        UPDATE bookings SET
                            reservation_status = 'Cancelled'
                        WHERE booking_id = @booking_id";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@booking_id", bookingId);
                        cmd.ExecuteNonQuery();
                    }
                }

                IncreaseAvailableSeats(scheduleId);

                MessageBox.Show("Booking cancelled successfully.");

                LoadSchedules();
                LoadBookings();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cancelling booking: " + ex.Message);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void dgvBookings_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            DataGridViewRow row = dgvBookings.Rows[e.RowIndex];

            txtBookingID.Text = row.Cells["booking_id"].Value.ToString();
            txtPassengerName.Text = row.Cells["passenger_name"].Value.ToString();
            txtPhone.Text = row.Cells["phone"].Value.ToString();
            txtEmail.Text = row.Cells["email"].Value == DBNull.Value ? "" : row.Cells["email"].Value.ToString();
            txtSeatNo.Text = row.Cells["seat_no"].Value.ToString();

            cmbPaymentStatus.Text = row.Cells["payment_status"].Value.ToString();
            cmbReservationStatus.Text = row.Cells["reservation_status"].Value.ToString();

            cmbSchedule.SelectedValue = Convert.ToInt32(row.Cells["schedule_id"].Value);
        }
    }
}