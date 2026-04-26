using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace sr
{
    public partial class PaymentConfirmationForm : Form
    {
        string connectionString = "server=localhost;user id=root;password=;database=sr_db;";

        private Panel panelHeader;
        private Label lblTitle;

        private GroupBox groupBoxPayment;
        private Label lblPaymentID;
        private Label lblBooking;
        private Label lblAmount;
        private Label lblPaymentMethod;
        private Label lblReferenceNo;
        private Label lblPaymentStatus;

        private TextBox txtPaymentID;
        private ComboBox cmbBooking;
        private TextBox txtAmount;
        private ComboBox cmbPaymentMethod;
        private TextBox txtReferenceNo;
        private ComboBox cmbPaymentStatus;

        private Button btnAdd;
        private Button btnConfirmPayment;
        private Button btnRejectPayment;
        private Button btnClear;

        private DataGridView dgvPayments;

        public PaymentConfirmationForm()
        {
            InitializeComponent();
            CreatePaymentConfirmationDesign();
            LoadPaymentMethods();
            LoadPaymentStatuses();
            LoadBookings();
            LoadPayments();
        }

        private void CreatePaymentConfirmationDesign()
        {
            this.Controls.Clear();

            this.Text = "Payment Confirmation";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ClientSize = new Size(1180, 730);
            this.BackColor = Color.FromArgb(245, 247, 250);
            this.Font = new Font("Segoe UI", 9F);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            panelHeader = new Panel();
            panelHeader.BackColor = Color.FromArgb(32, 45, 64);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Height = 65;

            lblTitle = new Label();
            lblTitle.Text = "Payment Confirmation";
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(25, 16);

            panelHeader.Controls.Add(lblTitle);
            this.Controls.Add(panelHeader);

            groupBoxPayment = new GroupBox();
            groupBoxPayment.Text = "Payment Information";
            groupBoxPayment.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            groupBoxPayment.ForeColor = Color.FromArgb(32, 45, 64);
            groupBoxPayment.Location = new Point(25, 85);
            groupBoxPayment.Size = new Size(1130, 220);
            groupBoxPayment.BackColor = Color.White;

            lblPaymentID = CreateLabel("Payment ID:", 25, 40);
            txtPaymentID = CreateTextBox(150, 37, 180);
            txtPaymentID.ReadOnly = true;
            txtPaymentID.BackColor = Color.FromArgb(235, 235, 235);

            lblBooking = CreateLabel("Booking:", 370, 40);
            cmbBooking = CreateComboBox(500, 37, 580);

            lblAmount = CreateLabel("Amount:", 25, 85);
            txtAmount = CreateTextBox(150, 82, 180);

            lblPaymentMethod = CreateLabel("Payment Method:", 370, 85);
            cmbPaymentMethod = CreateComboBox(500, 82, 220);

            lblReferenceNo = CreateLabel("Reference No.:", 25, 130);
            txtReferenceNo = CreateTextBox(150, 127, 180);

            lblPaymentStatus = CreateLabel("Payment Status:", 370, 130);
            cmbPaymentStatus = CreateComboBox(500, 127, 220);

            btnAdd = CreateButton("Add Payment", 760, 80, Color.FromArgb(46, 204, 113));
            btnAdd.Click += btnAdd_Click;

            btnConfirmPayment = CreateButton("Confirm Payment", 900, 80, Color.FromArgb(52, 152, 219));
            btnConfirmPayment.Click += btnConfirmPayment_Click;

            btnRejectPayment = CreateButton("Reject Payment", 760, 130, Color.FromArgb(231, 76, 60));
            btnRejectPayment.Click += btnRejectPayment_Click;

            btnClear = CreateButton("Clear", 900, 130, Color.FromArgb(127, 140, 141));
            btnClear.Click += btnClear_Click;

            groupBoxPayment.Controls.Add(lblPaymentID);
            groupBoxPayment.Controls.Add(txtPaymentID);
            groupBoxPayment.Controls.Add(lblBooking);
            groupBoxPayment.Controls.Add(cmbBooking);
            groupBoxPayment.Controls.Add(lblAmount);
            groupBoxPayment.Controls.Add(txtAmount);
            groupBoxPayment.Controls.Add(lblPaymentMethod);
            groupBoxPayment.Controls.Add(cmbPaymentMethod);
            groupBoxPayment.Controls.Add(lblReferenceNo);
            groupBoxPayment.Controls.Add(txtReferenceNo);
            groupBoxPayment.Controls.Add(lblPaymentStatus);
            groupBoxPayment.Controls.Add(cmbPaymentStatus);
            groupBoxPayment.Controls.Add(btnAdd);
            groupBoxPayment.Controls.Add(btnConfirmPayment);
            groupBoxPayment.Controls.Add(btnRejectPayment);
            groupBoxPayment.Controls.Add(btnClear);

            this.Controls.Add(groupBoxPayment);

            dgvPayments = new DataGridView();
            dgvPayments.Location = new Point(25, 325);
            dgvPayments.Size = new Size(1130, 375);
            dgvPayments.BackgroundColor = Color.White;
            dgvPayments.BorderStyle = BorderStyle.None;
            dgvPayments.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPayments.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPayments.MultiSelect = false;
            dgvPayments.ReadOnly = true;
            dgvPayments.AllowUserToAddRows = false;
            dgvPayments.AllowUserToDeleteRows = false;
            dgvPayments.RowHeadersVisible = false;
            dgvPayments.EnableHeadersVisualStyles = false;
            dgvPayments.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(32, 45, 64);
            dgvPayments.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvPayments.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvPayments.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
            dgvPayments.DefaultCellStyle.SelectionBackColor = Color.FromArgb(52, 152, 219);
            dgvPayments.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvPayments.CellClick += dgvPayments_CellClick;

            this.Controls.Add(dgvPayments);
        }

        private Label CreateLabel(string text, int x, int y)
        {
            Label label = new Label();
            label.Text = text;
            label.Location = new Point(x, y);
            label.Size = new Size(120, 25);
            label.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label.ForeColor = Color.FromArgb(32, 45, 64);
            return label;
        }

        private TextBox CreateTextBox(int x, int y, int width)
        {
            TextBox textBox = new TextBox();
            textBox.Location = new Point(x, y);
            textBox.Size = new Size(width, 25);
            textBox.Font = new Font("Segoe UI", 9F);
            return textBox;
        }

        private ComboBox CreateComboBox(int x, int y, int width)
        {
            ComboBox comboBox = new ComboBox();
            comboBox.Location = new Point(x, y);
            comboBox.Size = new Size(width, 25);
            comboBox.Font = new Font("Segoe UI", 9F);
            comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            return comboBox;
        }

        private Button CreateButton(string text, int x, int y, Color color)
        {
            Button button = new Button();
            button.Text = text;
            button.Location = new Point(x, y);
            button.Size = new Size(125, 35);
            button.BackColor = color;
            button.ForeColor = Color.White;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            button.Cursor = Cursors.Hand;
            return button;
        }

        private void LoadPaymentMethods()
        {
            cmbPaymentMethod.Items.Clear();
            cmbPaymentMethod.Items.Add("Cash");
            cmbPaymentMethod.Items.Add("GCash");
            cmbPaymentMethod.Items.Add("Bank Transfer");
            cmbPaymentMethod.Items.Add("Card");
            cmbPaymentMethod.SelectedIndex = 0;
        }

        private void LoadPaymentStatuses()
        {
            cmbPaymentStatus.Items.Clear();
            cmbPaymentStatus.Items.Add("Pending");
            cmbPaymentStatus.Items.Add("Paid");
            cmbPaymentStatus.Items.Add("Rejected");
            cmbPaymentStatus.Items.Add("Refunded");
            cmbPaymentStatus.SelectedIndex = 0;
        }

        private void LoadBookings()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                        SELECT 
                            booking_id,
                            CONCAT('Booking #', booking_id, ' - ', passenger_name, ' | Seat ', seat_no) AS booking_name
                        FROM bookings
                        WHERE reservation_status <> 'Cancelled'
                        ORDER BY booking_id DESC";

                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    cmbBooking.DataSource = dt;
                    cmbBooking.DisplayMember = "booking_name";
                    cmbBooking.ValueMember = "booking_id";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading bookings: " + ex.Message);
            }
        }

        private void LoadPayments()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                        SELECT
                            p.payment_id,
                            p.booking_id,
                            bk.passenger_name,
                            CONCAT(r.origin, ' to ', r.destination) AS route,
                            b.bus_number,
                            bk.seat_no,
                            p.amount,
                            p.payment_method,
                            p.reference_no,
                            p.payment_status,
                            p.paid_at,
                            bk.reservation_status
                        FROM payments p
                        INNER JOIN bookings bk ON p.booking_id = bk.booking_id
                        INNER JOIN schedules s ON bk.schedule_id = s.schedule_id
                        INNER JOIN buses b ON s.bus_id = b.bus_id
                        INNER JOIN routes r ON s.route_id = r.route_id
                        ORDER BY p.payment_id DESC";

                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dgvPayments.DataSource = dt;

                    if (dgvPayments.Columns.Count > 0)
                    {
                        dgvPayments.Columns["payment_id"].HeaderText = "Payment ID";
                        dgvPayments.Columns["booking_id"].HeaderText = "Booking ID";
                        dgvPayments.Columns["passenger_name"].HeaderText = "Passenger";
                        dgvPayments.Columns["route"].HeaderText = "Route";
                        dgvPayments.Columns["bus_number"].HeaderText = "Bus No.";
                        dgvPayments.Columns["seat_no"].HeaderText = "Seat";
                        dgvPayments.Columns["amount"].HeaderText = "Amount";
                        dgvPayments.Columns["payment_method"].HeaderText = "Method";
                        dgvPayments.Columns["reference_no"].HeaderText = "Reference No.";
                        dgvPayments.Columns["payment_status"].HeaderText = "Payment Status";
                        dgvPayments.Columns["paid_at"].HeaderText = "Paid At";
                        dgvPayments.Columns["reservation_status"].HeaderText = "Reservation Status";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading payments: " + ex.Message);
            }
        }

        private bool ValidateFields()
        {
            if (cmbBooking.SelectedValue == null)
            {
                MessageBox.Show("Please select booking.");
                return false;
            }

            if (txtAmount.Text.Trim() == "")
            {
                MessageBox.Show("Please enter amount.");
                txtAmount.Focus();
                return false;
            }

            decimal amount;

            if (!decimal.TryParse(txtAmount.Text.Trim(), out amount))
            {
                MessageBox.Show("Amount must be a valid number.");
                txtAmount.Focus();
                return false;
            }

            if (amount <= 0)
            {
                MessageBox.Show("Amount must be greater than zero.");
                txtAmount.Focus();
                return false;
            }

            if (cmbPaymentMethod.Text.Trim() == "")
            {
                MessageBox.Show("Please select payment method.");
                return false;
            }

            if (cmbPaymentStatus.Text.Trim() == "")
            {
                MessageBox.Show("Please select payment status.");
                return false;
            }

            return true;
        }

        private void ClearFields()
        {
            txtPaymentID.Clear();
            txtAmount.Clear();
            txtReferenceNo.Clear();

            if (cmbBooking.Items.Count > 0)
                cmbBooking.SelectedIndex = 0;

            if (cmbPaymentMethod.Items.Count > 0)
                cmbPaymentMethod.SelectedIndex = 0;

            if (cmbPaymentStatus.Items.Count > 0)
                cmbPaymentStatus.SelectedIndex = 0;
        }

        private void UpdateBookingPaymentStatus(int bookingId, string paymentStatus)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string reservationStatus = "Pending";

                    if (paymentStatus == "Paid")
                    {
                        reservationStatus = "Confirmed";
                    }
                    else if (paymentStatus == "Rejected")
                    {
                        reservationStatus = "Pending";
                    }
                    else if (paymentStatus == "Refunded")
                    {
                        reservationStatus = "Cancelled";
                    }

                    string query = @"
                        UPDATE bookings SET
                            payment_status = @payment_status,
                            reservation_status = @reservation_status
                        WHERE booking_id = @booking_id";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@payment_status", paymentStatus);
                        cmd.Parameters.AddWithValue("@reservation_status", reservationStatus);
                        cmd.Parameters.AddWithValue("@booking_id", bookingId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating booking status: " + ex.Message);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateFields())
                return;

            try
            {
                int bookingId = Convert.ToInt32(cmbBooking.SelectedValue);
                string paymentStatus = cmbPaymentStatus.Text;

                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                        INSERT INTO payments
                        (booking_id, amount, payment_method, reference_no, payment_status, paid_at)
                        VALUES
                        (@booking_id, @amount, @payment_method, @reference_no, @payment_status,
                        CASE WHEN @payment_status = 'Paid' THEN NOW() ELSE NULL END)";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@booking_id", bookingId);
                        cmd.Parameters.AddWithValue("@amount", Convert.ToDecimal(txtAmount.Text.Trim()));
                        cmd.Parameters.AddWithValue("@payment_method", cmbPaymentMethod.Text);
                        cmd.Parameters.AddWithValue("@reference_no", txtReferenceNo.Text.Trim());
                        cmd.Parameters.AddWithValue("@payment_status", paymentStatus);
                        cmd.ExecuteNonQuery();
                    }
                }

                UpdateBookingPaymentStatus(bookingId, paymentStatus);

                MessageBox.Show("Payment added successfully.");
                LoadPayments();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding payment: " + ex.Message);
            }
        }

        private void btnConfirmPayment_Click(object sender, EventArgs e)
        {
            if (txtPaymentID.Text.Trim() == "")
            {
                MessageBox.Show("Please select a payment to confirm.");
                return;
            }

            DialogResult result = MessageBox.Show(
                "Are you sure you want to confirm this payment?",
                "Confirm Payment",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.No)
                return;

            try
            {
                int paymentId = Convert.ToInt32(txtPaymentID.Text.Trim());
                int bookingId = Convert.ToInt32(cmbBooking.SelectedValue);

                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                        UPDATE payments SET
                            payment_status = 'Paid',
                            paid_at = NOW()
                        WHERE payment_id = @payment_id";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@payment_id", paymentId);
                        cmd.ExecuteNonQuery();
                    }
                }

                UpdateBookingPaymentStatus(bookingId, "Paid");

                MessageBox.Show("Payment confirmed successfully. Reservation is now confirmed.");
                LoadPayments();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error confirming payment: " + ex.Message);
            }
        }

        private void btnRejectPayment_Click(object sender, EventArgs e)
        {
            if (txtPaymentID.Text.Trim() == "")
            {
                MessageBox.Show("Please select a payment to reject.");
                return;
            }

            DialogResult result = MessageBox.Show(
                "Are you sure you want to reject this payment?",
                "Reject Payment",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.No)
                return;

            try
            {
                int paymentId = Convert.ToInt32(txtPaymentID.Text.Trim());
                int bookingId = Convert.ToInt32(cmbBooking.SelectedValue);

                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                        UPDATE payments SET
                            payment_status = 'Rejected',
                            paid_at = NULL
                        WHERE payment_id = @payment_id";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@payment_id", paymentId);
                        cmd.ExecuteNonQuery();
                    }
                }

                UpdateBookingPaymentStatus(bookingId, "Rejected");

                MessageBox.Show("Payment rejected successfully.");
                LoadPayments();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error rejecting payment: " + ex.Message);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void dgvPayments_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvPayments.Rows[e.RowIndex];

                txtPaymentID.Text = row.Cells["payment_id"].Value.ToString();
                cmbBooking.SelectedValue = Convert.ToInt32(row.Cells["booking_id"].Value);
                txtAmount.Text = row.Cells["amount"].Value.ToString();
                cmbPaymentMethod.Text = row.Cells["payment_method"].Value.ToString();
                txtReferenceNo.Text = row.Cells["reference_no"].Value == DBNull.Value ? "" : row.Cells["reference_no"].Value.ToString();
                cmbPaymentStatus.Text = row.Cells["payment_status"].Value.ToString();
            }
        }

        private void PaymentConfirmationForm_Load(object sender, EventArgs e)
        {

        }
    }
}