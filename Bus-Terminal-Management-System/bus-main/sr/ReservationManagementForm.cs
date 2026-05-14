using System;
using System.Data;
using System.Drawing;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace sr
{
    public partial class ReservationManagementForm : Form
    {
        private readonly HttpClient client = new HttpClient();
        private readonly string apiBaseUrl = "http://localhost:3000/api";

        private Button btnCheckIn;
        private Button btnBoard;

        public ReservationManagementForm()
        {
            InitializeComponent();
            CreateReservationDesign();
        }

        private async void ReservationManagementForm_Load(object sender, EventArgs e)
        {
            LoadPaymentStatus();
            LoadReservationStatus();
            await LoadSchedules();
            await LoadBookings();
        }

        private async void ReservationManagementForm_Load_1(object sender, EventArgs e)
        {
            await LoadSchedules();
            await LoadBookings();
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
            txtPassengerName.ReadOnly = true;

            Label lblPhone = new Label();
            lblPhone.Text = "Phone";
            lblPhone.Font = new Font("Segoe UI", 9.5F);
            lblPhone.Location = new Point(25, 115);
            lblPhone.AutoSize = true;

            txtPhone = new TextBox();
            txtPhone.Location = new Point(160, 112);
            txtPhone.Size = new Size(220, 25);
            txtPhone.ReadOnly = true;

            Label lblEmail = new Label();
            lblEmail.Text = "Email";
            lblEmail.Font = new Font("Segoe UI", 9.5F);
            lblEmail.Location = new Point(25, 155);
            lblEmail.AutoSize = true;

            txtEmail = new TextBox();
            txtEmail.Location = new Point(160, 152);
            txtEmail.Size = new Size(220, 25);
            txtEmail.ReadOnly = true;

            Label lblSchedule = new Label();
            lblSchedule.Text = "Schedule";
            lblSchedule.Font = new Font("Segoe UI", 9.5F);
            lblSchedule.Location = new Point(430, 35);
            lblSchedule.AutoSize = true;

            cmbSchedule = new ComboBox();
            cmbSchedule.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSchedule.Location = new Point(570, 32);
            cmbSchedule.Size = new Size(520, 25);
            cmbSchedule.Enabled = false;

            Label lblSeatNo = new Label();
            lblSeatNo.Text = "Seat No.";
            lblSeatNo.Font = new Font("Segoe UI", 9.5F);
            lblSeatNo.Location = new Point(430, 75);
            lblSeatNo.AutoSize = true;

            txtSeatNo = new TextBox();
            txtSeatNo.Location = new Point(570, 72);
            txtSeatNo.Size = new Size(180, 25);
            txtSeatNo.ReadOnly = true;

            Label lblPaymentStatus = new Label();
            lblPaymentStatus.Text = "Payment Status";
            lblPaymentStatus.Font = new Font("Segoe UI", 9.5F);
            lblPaymentStatus.Location = new Point(430, 115);
            lblPaymentStatus.AutoSize = true;

            cmbPaymentStatus = new ComboBox();
            cmbPaymentStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPaymentStatus.Location = new Point(570, 112);
            cmbPaymentStatus.Size = new Size(180, 25);
            cmbPaymentStatus.Enabled = false;

            Label lblReservationStatus = new Label();
            lblReservationStatus.Text = "Reservation Status";
            lblReservationStatus.Font = new Font("Segoe UI", 9.5F);
            lblReservationStatus.Location = new Point(430, 155);
            lblReservationStatus.AutoSize = true;

            cmbReservationStatus = new ComboBox();
            cmbReservationStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbReservationStatus.Location = new Point(570, 152);
            cmbReservationStatus.Size = new Size(180, 25);
            cmbReservationStatus.Enabled = false;

            btnAdd = new Button();
            btnAdd.Text = "Refresh";
            btnAdd.Location = new Point(160, 200);
            btnAdd.Size = new Size(120, 35);
            btnAdd.BackColor = Color.FromArgb(108, 117, 125);
            btnAdd.ForeColor = Color.White;
            btnAdd.FlatStyle = FlatStyle.Flat;
            btnAdd.Click += btnRefresh_Click;

            btnConfirm = new Button();
            btnConfirm.Text = "Confirm Payment";
            btnConfirm.Location = new Point(295, 200);
            btnConfirm.Size = new Size(150, 35);
            btnConfirm.BackColor = Color.FromArgb(23, 162, 184);
            btnConfirm.ForeColor = Color.White;
            btnConfirm.FlatStyle = FlatStyle.Flat;
            btnConfirm.Click += btnConfirm_Click;

            btnCheckIn = new Button();
            btnCheckIn.Text = "Check-in";
            btnCheckIn.Location = new Point(460, 200);
            btnCheckIn.Size = new Size(120, 35);
            btnCheckIn.BackColor = Color.FromArgb(40, 167, 69);
            btnCheckIn.ForeColor = Color.White;
            btnCheckIn.FlatStyle = FlatStyle.Flat;
            btnCheckIn.Click += btnCheckIn_Click;

            btnBoard = new Button();
            btnBoard.Text = "Board";
            btnBoard.Location = new Point(595, 200);
            btnBoard.Size = new Size(120, 35);
            btnBoard.BackColor = Color.FromArgb(0, 123, 255);
            btnBoard.ForeColor = Color.White;
            btnBoard.FlatStyle = FlatStyle.Flat;
            btnBoard.Click += btnBoard_Click;

            btnClear = new Button();
            btnClear.Text = "Clear";
            btnClear.Location = new Point(730, 200);
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
            groupBoxInfo.Controls.Add(btnConfirm);
            groupBoxInfo.Controls.Add(btnCheckIn);
            groupBoxInfo.Controls.Add(btnBoard);
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

        private void LoadPaymentStatus()
        {
            cmbPaymentStatus.Items.Clear();
            cmbPaymentStatus.Items.Add("Pending");
            cmbPaymentStatus.Items.Add("Paid");
            cmbPaymentStatus.Items.Add("Cancelled");

            if (cmbPaymentStatus.Items.Count > 0)
            {
                cmbPaymentStatus.SelectedIndex = 0;
            }
        }

        private void LoadReservationStatus()
        {
            cmbReservationStatus.Items.Clear();
            cmbReservationStatus.Items.Add("Pending");
            cmbReservationStatus.Items.Add("Confirmed");
            cmbReservationStatus.Items.Add("Cancelled");

            if (cmbReservationStatus.Items.Count > 0)
            {
                cmbReservationStatus.SelectedIndex = 0;
            }
        }

        private async Task LoadSchedules()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(apiBaseUrl + "/admin/schedules");
                string responseBody = await response.Content.ReadAsStringAsync();

                JObject result = JObject.Parse(responseBody);

                if (!response.IsSuccessStatusCode || result["success"]?.ToObject<bool>() != true)
                {
                    string message = result["message"]?.ToString() ?? "Failed to load schedules.";
                    MessageBox.Show(message, "API Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                JArray schedules = (JArray)result["schedules"];

                DataTable dt = new DataTable();
                dt.Columns.Add("schedule_id", typeof(int));
                dt.Columns.Add("schedule_name", typeof(string));

                foreach (JObject schedule in schedules)
                {
                    string tripStatus = schedule["trip_status"]?.ToString() ?? "";

                    if (tripStatus != "Cancelled")
                    {
                        string busNumber = schedule["bus_number"]?.ToString() ?? "";
                        string origin = schedule["origin"]?.ToString() ?? "";
                        string destination = schedule["destination"]?.ToString() ?? "";
                        string departureDate = FormatDate(schedule["departure_date"]?.ToString());
                        string departureTime = schedule["departure_time"]?.ToString() ?? "";
                        string fare = schedule["fare"]?.ToString() ?? "0";
                        string availableSeats = schedule["available_seats"]?.ToString() ?? "0";

                        string scheduleName =
                            busNumber + " | " +
                            origin + " to " + destination + " | " +
                            departureDate + " " + departureTime +
                            " | Fare: " + fare +
                            " | A-Seats: " + availableSeats;

                        dt.Rows.Add(
                            schedule["schedule_id"]?.ToObject<int>() ?? 0,
                            scheduleName
                        );
                    }
                }

                cmbSchedule.DataSource = null;
                cmbSchedule.DataSource = dt;
                cmbSchedule.DisplayMember = "schedule_name";
                cmbSchedule.ValueMember = "schedule_id";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error loading schedules from API.\n\nMake sure Node API is running.\n\n" + ex.Message,
                    "API Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private async Task LoadBookings()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(apiBaseUrl + "/bookings");
                string responseBody = await response.Content.ReadAsStringAsync();

                JObject result = JObject.Parse(responseBody);

                if (!response.IsSuccessStatusCode || result["success"]?.ToObject<bool>() != true)
                {
                    string message = result["message"]?.ToString() ?? "Failed to load bookings.";
                    MessageBox.Show(message, "API Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                JArray bookings = (JArray)result["bookings"];

                DataTable dt = new DataTable();
                dt.Columns.Add("booking_id", typeof(int));
                dt.Columns.Add("passenger_name", typeof(string));
                dt.Columns.Add("phone", typeof(string));
                dt.Columns.Add("email", typeof(string));
                dt.Columns.Add("route", typeof(string));
                dt.Columns.Add("bus_number", typeof(string));
                dt.Columns.Add("seat_no", typeof(int));
                dt.Columns.Add("departure_date", typeof(string));
                dt.Columns.Add("departure_time", typeof(string));
                dt.Columns.Add("payment_status", typeof(string));
                dt.Columns.Add("reservation_status", typeof(string));
                dt.Columns.Add("checkin_status", typeof(string));
                dt.Columns.Add("boarding_status", typeof(string));
                dt.Columns.Add("created_at", typeof(string));
                dt.Columns.Add("schedule_id", typeof(int));

                foreach (JObject booking in bookings)
                {
                    string origin = booking["origin"]?.ToString() ?? "";
                    string destination = booking["destination"]?.ToString() ?? "";

                    dt.Rows.Add(
                        booking["booking_id"]?.ToObject<int>() ?? 0,
                        booking["passenger_name"]?.ToString() ?? "",
                        booking["phone"]?.ToString() ?? "",
                        booking["email"]?.ToString() ?? "",
                        origin + " to " + destination,
                        booking["bus_number"]?.ToString() ?? "",
                        booking["seat_no"]?.ToObject<int>() ?? 0,
                        FormatDate(booking["departure_date"]?.ToString()),
                        booking["departure_time"]?.ToString() ?? "",
                        booking["payment_status"]?.ToString() ?? "",
                        booking["reservation_status"]?.ToString() ?? "",
                        booking["checkin_status"]?.ToString() ?? "",
                        booking["boarding_status"]?.ToString() ?? "",
                        booking["created_at"]?.ToString() ?? "",
                        booking["schedule_id"]?.ToObject<int>() ?? 0
                    );
                }

                dgvBookings.DataSource = dt;

                SetBookingGridHeaders();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error loading bookings from API.\n\nMake sure Node API is running.\n\n" + ex.Message,
                    "API Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void SetBookingGridHeaders()
        {
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

        private string FormatDate(string value)
        {
            DateTime date;

            if (DateTime.TryParse(value, out date))
            {
                return date.ToString("yyyy-MM-dd");
            }

            return value ?? "";
        }

        private async Task RunBookingAction(string endpoint, string successMessage)
        {
            if (string.IsNullOrWhiteSpace(txtBookingID.Text))
            {
                MessageBox.Show("Please select a booking first.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int bookingId = Convert.ToInt32(txtBookingID.Text.Trim());

                var content = new StringContent("{}", Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PutAsync(apiBaseUrl + "/admin/bookings/" + bookingId + endpoint, content);
                string responseBody = await response.Content.ReadAsStringAsync();

                JObject result = JObject.Parse(responseBody);

                if (response.IsSuccessStatusCode && result["success"]?.ToObject<bool>() == true)
                {
                    MessageBox.Show(successMessage, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await LoadBookings();
                    await LoadSchedules();
                    ClearFields();
                }
                else
                {
                    string message = result["message"]?.ToString() ?? "Action failed.";
                    MessageBox.Show(message, "API Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error processing booking action through API.\n\n" + ex.Message,
                    "API Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            await LoadSchedules();
            await LoadBookings();
            ClearFields();
        }

        private async void btnConfirm_Click(object sender, EventArgs e)
        {
            await RunBookingAction(
                "/confirm-payment",
                "Payment confirmed and booking approved successfully."
            );
        }

        private async void btnCheckIn_Click(object sender, EventArgs e)
        {
            await RunBookingAction(
                "/check-in",
                "Passenger checked in successfully."
            );
        }

        private async void btnBoard_Click(object sender, EventArgs e)
        {
            await RunBookingAction(
                "/board",
                "Passenger marked as boarded successfully."
            );
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
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

        private void dgvBookings_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            DataGridViewRow row = dgvBookings.Rows[e.RowIndex];

            txtBookingID.Text = row.Cells["booking_id"].Value?.ToString() ?? "";
            txtPassengerName.Text = row.Cells["passenger_name"].Value?.ToString() ?? "";
            txtPhone.Text = row.Cells["phone"].Value?.ToString() ?? "";
            txtEmail.Text = row.Cells["email"].Value?.ToString() ?? "";
            txtSeatNo.Text = row.Cells["seat_no"].Value?.ToString() ?? "";

            cmbPaymentStatus.Text = row.Cells["payment_status"].Value?.ToString() ?? "";
            cmbReservationStatus.Text = row.Cells["reservation_status"].Value?.ToString() ?? "";

            object scheduleValue = row.Cells["schedule_id"].Value;

            if (scheduleValue != null && scheduleValue != DBNull.Value)
            {
                cmbSchedule.SelectedValue = Convert.ToInt32(scheduleValue);
            }
        }
    }
}