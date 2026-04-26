using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace sr
{
    public partial class DashboardForm : Form
    {
        string connectionString = "server=localhost;user id=root;password=;database=sr_db;";

        private Panel sidebarPanel;
        private Panel headerPanel;
        private Panel contentPanel;

        private Label lblTotalBuses;
        private Label lblTotalRoutes;
        private Label lblTodaysTrips;
        private Label lblPendingReservations;
        private Label lblConfirmedBookings;
        private Label lblBoardedPassengers;
        private Label lblCancelledBookings;
        private Label lblRevenueToday;

        private Button btnRefreshDashboard;

        private Button btnRouteManagement;
        private Button btnBusManagement;
        private Button btnScheduleManagement;
        private Button btnReservationManagement;
        private Button btnPaymentConfirmation;
        private Button btnCheckInBoarding;
        private Button btnReports;
        private Button btnLogout;

        public DashboardForm()
        {
            InitializeComponent();
            CreateDashboardDesign();
            LoadDashboard();
        }

        private void CreateDashboardDesign()
        {
            this.Controls.Clear();

            this.Text = "Bus Terminal Management System - Dashboard";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ClientSize = new Size(1250, 760);
            this.BackColor = Color.FromArgb(245, 247, 250);
            this.Font = new Font("Segoe UI", 9F);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            CreateSidebar();
            CreateHeader();
            CreateContent();
        }

        private void CreateSidebar()
        {
            sidebarPanel = new Panel();
            sidebarPanel.BackColor = Color.FromArgb(32, 45, 64);
            sidebarPanel.Dock = DockStyle.Left;
            sidebarPanel.Width = 250;
            this.Controls.Add(sidebarPanel);

            Label lblLogo = new Label();
            lblLogo.Text = "BTMS";
            lblLogo.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblLogo.ForeColor = Color.White;
            lblLogo.TextAlign = ContentAlignment.MiddleCenter;
            lblLogo.Dock = DockStyle.Top;
            lblLogo.Height = 70;
            sidebarPanel.Controls.Add(lblLogo);

            Label lblSubtitle = new Label();
            lblSubtitle.Text = "Admin Dashboard";
            lblSubtitle.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            lblSubtitle.ForeColor = Color.Gainsboro;
            lblSubtitle.TextAlign = ContentAlignment.MiddleCenter;
            lblSubtitle.Dock = DockStyle.Top;
            lblSubtitle.Height = 35;
            sidebarPanel.Controls.Add(lblSubtitle);

            btnRouteManagement = CreateMenuButton("Route Management");
            btnBusManagement = CreateMenuButton("Bus Management");
            btnScheduleManagement = CreateMenuButton("Schedule Management");
            btnReservationManagement = CreateMenuButton("Reservation Management");
            btnPaymentConfirmation = CreateMenuButton("Payment Confirmation");
            btnCheckInBoarding = CreateMenuButton("Check-in / Boarding");
            btnReports = CreateMenuButton("Reports");
            btnLogout = CreateMenuButton("Logout");

            btnRouteManagement.Top = 130;
            btnBusManagement.Top = 180;
            btnScheduleManagement.Top = 230;
            btnReservationManagement.Top = 280;
            btnPaymentConfirmation.Top = 330;
            btnCheckInBoarding.Top = 380;
            btnReports.Top = 430;
            btnLogout.Top = 650;

            sidebarPanel.Controls.Add(btnRouteManagement);
            sidebarPanel.Controls.Add(btnBusManagement);
            sidebarPanel.Controls.Add(btnScheduleManagement);
            sidebarPanel.Controls.Add(btnReservationManagement);
            sidebarPanel.Controls.Add(btnPaymentConfirmation);
            sidebarPanel.Controls.Add(btnCheckInBoarding);
            sidebarPanel.Controls.Add(btnReports);
            sidebarPanel.Controls.Add(btnLogout);

            btnRouteManagement.Click += btnRouteManagement_Click;
            btnBusManagement.Click += btnBusManagement_Click;
            btnScheduleManagement.Click += btnScheduleManagement_Click;
            btnReservationManagement.Click += btnReservationManagement_Click;
            btnPaymentConfirmation.Click += btnPaymentConfirmation_Click;
            btnCheckInBoarding.Click += btnCheckInBoarding_Click;
            btnReports.Click += btnReports_Click;
            btnLogout.Click += btnLogout_Click;
        }

        private Button CreateMenuButton(string text)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.Left = 20;
            btn.Width = 210;
            btn.Height = 42;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = Color.FromArgb(45, 62, 85);
            btn.ForeColor = Color.White;
            btn.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btn.TextAlign = ContentAlignment.MiddleLeft;
            btn.Padding = new Padding(15, 0, 0, 0);
            btn.Cursor = Cursors.Hand;

            btn.MouseEnter += (s, e) =>
            {
                btn.BackColor = Color.FromArgb(52, 152, 219);
            };

            btn.MouseLeave += (s, e) =>
            {
                btn.BackColor = Color.FromArgb(45, 62, 85);
            };

            return btn;
        }

        private void CreateHeader()
        {
            headerPanel = new Panel();
            headerPanel.BackColor = Color.White;
            headerPanel.Dock = DockStyle.Top;
            headerPanel.Height = 85;
            headerPanel.Left = 250;
            this.Controls.Add(headerPanel);
            headerPanel.BringToFront();

            Label lblTitle = new Label();
            lblTitle.Text = "Bus Terminal Management System";
            lblTitle.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(32, 45, 64);
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(280, 15);
            this.Controls.Add(lblTitle);
            lblTitle.BringToFront();

            Label lblWelcome = new Label();
            lblWelcome.Text = "Welcome, Admin / Staff";
            lblWelcome.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            lblWelcome.ForeColor = Color.Gray;
            lblWelcome.AutoSize = true;
            lblWelcome.Location = new Point(285, 52);
            this.Controls.Add(lblWelcome);
            lblWelcome.BringToFront();

            btnRefreshDashboard = new Button();
            btnRefreshDashboard.Text = "Refresh Dashboard";
            btnRefreshDashboard.Size = new Size(170, 40);
            btnRefreshDashboard.Location = new Point(1040, 22);
            btnRefreshDashboard.BackColor = Color.FromArgb(46, 204, 113);
            btnRefreshDashboard.ForeColor = Color.White;
            btnRefreshDashboard.FlatStyle = FlatStyle.Flat;
            btnRefreshDashboard.FlatAppearance.BorderSize = 0;
            btnRefreshDashboard.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnRefreshDashboard.Cursor = Cursors.Hand;
            btnRefreshDashboard.Click += btnRefreshDashboard_Click;
            this.Controls.Add(btnRefreshDashboard);
            btnRefreshDashboard.BringToFront();
        }

        private void CreateContent()
        {
            contentPanel = new Panel();
            contentPanel.BackColor = Color.FromArgb(245, 247, 250);
            contentPanel.Location = new Point(250, 85);
            contentPanel.Size = new Size(1000, 675);
            this.Controls.Add(contentPanel);

            Label lblOverview = new Label();
            lblOverview.Text = "Dashboard Overview";
            lblOverview.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblOverview.ForeColor = Color.FromArgb(32, 45, 64);
            lblOverview.AutoSize = true;
            lblOverview.Location = new Point(30, 25);
            contentPanel.Controls.Add(lblOverview);

            lblTotalBuses = CreateCard("Total Buses", "0", 30, 75, Color.FromArgb(52, 152, 219));
            lblTotalRoutes = CreateCard("Total Routes", "0", 270, 75, Color.FromArgb(155, 89, 182));
            lblTodaysTrips = CreateCard("Today's Trips", "0", 510, 75, Color.FromArgb(241, 196, 15));
            lblRevenueToday = CreateCard("Revenue Today", "₱ 0.00", 750, 75, Color.FromArgb(46, 204, 113));

            lblPendingReservations = CreateCard("Pending Reservations", "0", 30, 230, Color.FromArgb(230, 126, 34));
            lblConfirmedBookings = CreateCard("Confirmed Bookings", "0", 270, 230, Color.FromArgb(26, 188, 156));
            lblBoardedPassengers = CreateCard("Boarded Passengers", "0", 510, 230, Color.FromArgb(52, 73, 94));
            lblCancelledBookings = CreateCard("Cancelled Bookings", "0", 750, 230, Color.FromArgb(231, 76, 60));

            CreateQuickActions();
        }

        private Label CreateCard(string title, string value, int x, int y, Color topColor)
        {
            Panel card = new Panel();
            card.BackColor = Color.White;
            card.Location = new Point(x, y);
            card.Size = new Size(210, 125);
            card.BorderStyle = BorderStyle.FixedSingle;
            contentPanel.Controls.Add(card);

            Panel colorBar = new Panel();
            colorBar.BackColor = topColor;
            colorBar.Dock = DockStyle.Top;
            colorBar.Height = 8;
            card.Controls.Add(colorBar);

            Label lblTitle = new Label();
            lblTitle.Text = title;
            lblTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblTitle.ForeColor = Color.Gray;
            lblTitle.AutoSize = false;
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            lblTitle.Location = new Point(5, 25);
            lblTitle.Size = new Size(198, 25);
            card.Controls.Add(lblTitle);

            Label lblValue = new Label();
            lblValue.Text = value;
            lblValue.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblValue.ForeColor = Color.FromArgb(32, 45, 64);
            lblValue.AutoSize = false;
            lblValue.TextAlign = ContentAlignment.MiddleCenter;
            lblValue.Location = new Point(5, 60);
            lblValue.Size = new Size(198, 50);
            card.Controls.Add(lblValue);

            return lblValue;
        }

        private void CreateQuickActions()
        {
            Label lblQuick = new Label();
            lblQuick.Text = "Quick Actions";
            lblQuick.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblQuick.ForeColor = Color.FromArgb(32, 45, 64);
            lblQuick.AutoSize = true;
            lblQuick.Location = new Point(30, 400);
            contentPanel.Controls.Add(lblQuick);

            Button btnQuickSchedule = CreateActionButton("Add Schedule", 30, 455);
            Button btnQuickReservations = CreateActionButton("View Reservations", 230, 455);
            Button btnQuickPayment = CreateActionButton("Confirm Payment", 430, 455);
            Button btnQuickCheckIn = CreateActionButton("Check-in Passenger", 630, 455);
            Button btnQuickReports = CreateActionButton("Generate Reports", 830, 455);

            contentPanel.Controls.Add(btnQuickSchedule);
            contentPanel.Controls.Add(btnQuickReservations);
            contentPanel.Controls.Add(btnQuickPayment);
            contentPanel.Controls.Add(btnQuickCheckIn);
            contentPanel.Controls.Add(btnQuickReports);

            btnQuickSchedule.Click += btnScheduleManagement_Click;
            btnQuickReservations.Click += btnReservationManagement_Click;
            btnQuickPayment.Click += btnPaymentConfirmation_Click;
            btnQuickCheckIn.Click += btnCheckInBoarding_Click;
            btnQuickReports.Click += btnReports_Click;
        }

        private Button CreateActionButton(string text, int x, int y)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.Location = new Point(x, y);
            btn.Size = new Size(160, 55);
            btn.BackColor = Color.FromArgb(32, 45, 64);
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
            return btn;
        }

        private int GetCount(string query)
        {
            int count = 0;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        object result = cmd.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            count = Convert.ToInt32(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Dashboard count error: " + ex.Message);
            }

            return count;
        }

        private decimal GetDecimalValue(string query)
        {
            decimal value = 0;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        object result = cmd.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            value = Convert.ToDecimal(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Dashboard revenue error: " + ex.Message);
            }

            return value;
        }

        private void LoadDashboard()
        {
            lblTotalBuses.Text = GetCount(@"
                SELECT COUNT(*) 
                FROM buses 
                WHERE status = 'Active'
            ").ToString();

            lblTotalRoutes.Text = GetCount(@"
                SELECT COUNT(*) 
                FROM routes 
                WHERE status = 'Active'
            ").ToString();

            lblTodaysTrips.Text = GetCount(@"
                SELECT COUNT(*) 
                FROM schedules 
                WHERE departure_date = CURDATE()
                AND trip_status != 'Cancelled'
            ").ToString();

            lblPendingReservations.Text = GetCount(@"
                SELECT COUNT(*) 
                FROM bookings 
                WHERE reservation_status = 'Pending'
            ").ToString();

            lblConfirmedBookings.Text = GetCount(@"
                SELECT COUNT(*) 
                FROM bookings 
                WHERE reservation_status = 'Confirmed'
            ").ToString();

            lblBoardedPassengers.Text = GetCount(@"
                SELECT COUNT(*) 
                FROM bookings 
                WHERE boarding_status = 'Boarded'
            ").ToString();

            lblCancelledBookings.Text = GetCount(@"
                SELECT COUNT(*) 
                FROM bookings 
                WHERE reservation_status = 'Cancelled'
            ").ToString();

            decimal revenueToday = GetDecimalValue(@"
                SELECT IFNULL(SUM(amount), 0)
                FROM payments
                WHERE payment_status = 'Paid'
                AND DATE(paid_at) = CURDATE()
            ");

            lblRevenueToday.Text = "₱ " + revenueToday.ToString("0.00");
        }

        private void btnRefreshDashboard_Click(object sender, EventArgs e)
        {
            LoadDashboard();
            MessageBox.Show("Dashboard refreshed successfully.", "Dashboard", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnRouteManagement_Click(object sender, EventArgs e)
        {
            RouteManagementForm form = new RouteManagementForm();
            form.Show();
        }

        private void btnBusManagement_Click(object sender, EventArgs e)
        {
            BusManagementForm form = new BusManagementForm();
            form.Show();
        }

        private void btnScheduleManagement_Click(object sender, EventArgs e)
        {
            ScheduleManagementForm form = new ScheduleManagementForm();
            form.Show();
        }

        private void btnReservationManagement_Click(object sender, EventArgs e)
        {
            ReservationManagementForm form = new ReservationManagementForm();
            form.Show();
        }

        private void btnPaymentConfirmation_Click(object sender, EventArgs e)
        {
            PaymentConfirmationForm form = new PaymentConfirmationForm();
            form.Show();
        }

        private void btnCheckInBoarding_Click(object sender, EventArgs e)
        {
            CheckInBoardingForm form = new CheckInBoardingForm();
            form.Show();
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            ReportsForm form = new ReportsForm();
            form.Show();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to logout?",
                "Logout",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                this.Hide();

                login loginForm = new login();
                loginForm.Show();
            }
        }
    }
}