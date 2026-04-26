using System;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace sr
{
    public partial class AdminDashboardForm : Form
    {
        private string connectionString = "server=localhost;database=sr_db;uid=root;pwd=;";

        private Panel panelSidebar;
        private Panel panelMain;
        private Panel panelTop;
        private Panel panelContent;

        private Label lblSystemTitle;
        private Label lblPageTitle;

        private Button btnDashboard;
        private Button btnRouteManagement;
        private Button btnBusManagement;
        private Button btnScheduleManagement;
        private Button btnReservationManagement;
        private Button btnPaymentConfirmation;
        private Button btnCheckInBoarding;
        private Button btnReports;
        private Button btnLogout;

        public AdminDashboardForm()
        {
            InitializeComponent();
            CreateDashboardDesign();
            ShowDashboardHome();
        }

        private void AdminDashboardForm_Load(object sender, EventArgs e)
        {
            // Empty load event
        }

        private void CreateDashboardDesign()
        {
            this.Text = "Bus Terminal Management System";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;
            this.MinimumSize = new Size(1200, 750);
            this.BackColor = Color.FromArgb(245, 247, 250);
            this.Font = new Font("Segoe UI", 10F);

            this.Controls.Clear();

            panelSidebar = new Panel();
            panelSidebar.Dock = DockStyle.Left;
            panelSidebar.Width = 260;
            panelSidebar.BackColor = Color.FromArgb(32, 45, 64);
            this.Controls.Add(panelSidebar);

            Label lblAdmin = new Label();
            lblAdmin.Text = "Admin Dashboard";
            lblAdmin.ForeColor = Color.LightGray;
            lblAdmin.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            lblAdmin.AutoSize = false;
            lblAdmin.TextAlign = ContentAlignment.MiddleCenter;
            lblAdmin.Location = new Point(0, 20);
            lblAdmin.Size = new Size(260, 30);
            panelSidebar.Controls.Add(lblAdmin);

            Label lblLogo = new Label();
            lblLogo.Text = "BTMS";
            lblLogo.ForeColor = Color.White;
            lblLogo.Font = new Font("Segoe UI", 28F, FontStyle.Bold);
            lblLogo.AutoSize = false;
            lblLogo.TextAlign = ContentAlignment.MiddleCenter;
            lblLogo.Location = new Point(0, 55);
            lblLogo.Size = new Size(260, 60);
            panelSidebar.Controls.Add(lblLogo);

            Label lblSubtitle = new Label();
            lblSubtitle.Text = "Bus Terminal Management";
            lblSubtitle.ForeColor = Color.LightGray;
            lblSubtitle.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            lblSubtitle.AutoSize = false;
            lblSubtitle.TextAlign = ContentAlignment.MiddleCenter;
            lblSubtitle.Location = new Point(0, 112);
            lblSubtitle.Size = new Size(260, 25);
            panelSidebar.Controls.Add(lblSubtitle);

            int startY = 165;
            int buttonHeight = 44;
            int buttonGap = 8;

            btnDashboard = CreateSidebarButton("Dashboard", startY);
            btnDashboard.Click += btnDashboard_Click;
            panelSidebar.Controls.Add(btnDashboard);

            btnRouteManagement = CreateSidebarButton("Route Management", startY + (buttonHeight + buttonGap) * 1);
            btnRouteManagement.Click += btnRouteManagement_Click;
            panelSidebar.Controls.Add(btnRouteManagement);

            btnBusManagement = CreateSidebarButton("Bus Management", startY + (buttonHeight + buttonGap) * 2);
            btnBusManagement.Click += btnBusManagement_Click;
            panelSidebar.Controls.Add(btnBusManagement);

            btnScheduleManagement = CreateSidebarButton("Schedule Management", startY + (buttonHeight + buttonGap) * 3);
            btnScheduleManagement.Click += btnScheduleManagement_Click;
            panelSidebar.Controls.Add(btnScheduleManagement);

            btnReservationManagement = CreateSidebarButton("Reservation Management", startY + (buttonHeight + buttonGap) * 4);
            btnReservationManagement.Click += btnReservationManagement_Click;
            panelSidebar.Controls.Add(btnReservationManagement);

            btnPaymentConfirmation = CreateSidebarButton("Payment Confirmation", startY + (buttonHeight + buttonGap) * 5);
            btnPaymentConfirmation.Click += btnPaymentConfirmation_Click;
            panelSidebar.Controls.Add(btnPaymentConfirmation);

            btnCheckInBoarding = CreateSidebarButton("Check-in / Boarding", startY + (buttonHeight + buttonGap) * 6);
            btnCheckInBoarding.Click += btnCheckInBoarding_Click;
            panelSidebar.Controls.Add(btnCheckInBoarding);

            btnReports = CreateSidebarButton("Reports", startY + (buttonHeight + buttonGap) * 7);
            btnReports.Click += btnReports_Click;
            panelSidebar.Controls.Add(btnReports);

            btnLogout = CreateSidebarButton("Logout", 0);
            btnLogout.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            btnLogout.BackColor = Color.FromArgb(220, 53, 69);
            btnLogout.Click += btnLogout_Click;
            panelSidebar.Controls.Add(btnLogout);

            panelMain = new Panel();
            panelMain.Location = new Point(260, 0);
            panelMain.Size = new Size(this.ClientSize.Width - 260, this.ClientSize.Height);
            panelMain.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panelMain.BackColor = Color.FromArgb(245, 247, 250);
            this.Controls.Add(panelMain);

            panelSidebar.BringToFront();

            panelTop = new Panel();
            panelTop.Location = new Point(0, 0);
            panelTop.Size = new Size(panelMain.Width, 80);
            panelTop.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panelTop.BackColor = Color.White;
            panelMain.Controls.Add(panelTop);

            lblSystemTitle = new Label();
            lblSystemTitle.Text = "Bus Terminal Management System";
            lblSystemTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblSystemTitle.ForeColor = Color.FromArgb(32, 45, 64);
            lblSystemTitle.AutoSize = true;
            lblSystemTitle.Location = new Point(30, 15);
            panelTop.Controls.Add(lblSystemTitle);

            lblPageTitle = new Label();
            lblPageTitle.Text = "Dashboard";
            lblPageTitle.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            lblPageTitle.ForeColor = Color.Gray;
            lblPageTitle.AutoSize = true;
            lblPageTitle.Location = new Point(33, 50);
            panelTop.Controls.Add(lblPageTitle);

            panelContent = new Panel();
            panelContent.Location = new Point(0, 80);
            panelContent.Size = new Size(panelMain.Width, panelMain.Height - 80);
            panelContent.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panelContent.BackColor = Color.FromArgb(245, 247, 250);
            panelContent.Padding = new Padding(20);
            panelContent.AutoScroll = true;
            panelMain.Controls.Add(panelContent);

            panelTop.BringToFront();

            this.Resize += AdminDashboardForm_Resize;
            AdminDashboardForm_Resize(null, null);
        }

        private Button CreateSidebarButton(string text, int y)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.Size = new Size(210, 44);
            btn.Location = new Point(25, y);
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = Color.FromArgb(44, 62, 85);
            btn.ForeColor = Color.White;
            btn.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btn.TextAlign = ContentAlignment.MiddleLeft;
            btn.Padding = new Padding(18, 0, 0, 0);
            btn.Cursor = Cursors.Hand;

            btn.MouseEnter += (s, e) =>
            {
                if (btn != btnLogout)
                {
                    btn.BackColor = Color.FromArgb(52, 152, 219);
                }
            };

            btn.MouseLeave += (s, e) =>
            {
                if (btn == btnLogout)
                {
                    btn.BackColor = Color.FromArgb(220, 53, 69);
                }
                else
                {
                    btn.BackColor = Color.FromArgb(44, 62, 85);
                }
            };

            return btn;
        }

        private void AdminDashboardForm_Resize(object sender, EventArgs e)
        {
            if (panelMain != null)
            {
                panelMain.Location = new Point(260, 0);
                panelMain.Size = new Size(this.ClientSize.Width - 260, this.ClientSize.Height);
            }

            if (panelTop != null && panelMain != null)
            {
                panelTop.Location = new Point(0, 0);
                panelTop.Size = new Size(panelMain.Width, 80);
            }

            if (panelContent != null && panelMain != null)
            {
                panelContent.Location = new Point(0, 80);
                panelContent.Size = new Size(panelMain.Width, panelMain.Height - 80);
            }

            if (btnLogout != null && panelSidebar != null)
            {
                btnLogout.Location = new Point(25, panelSidebar.Height - 70);
            }
        }

        private void LoadFormInPanel(Form childForm, string pageTitle)
        {
            panelContent.Controls.Clear();

            lblPageTitle.Text = pageTitle;

            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            childForm.AutoScroll = true;

            panelContent.Controls.Add(childForm);
            childForm.Show();
        }

        // ================= DATABASE HELPERS =================

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

        // ================= DASHBOARD HOME =================

        private void ShowDashboardHome()
        {
            panelContent.Controls.Clear();
            lblPageTitle.Text = "Dashboard";

            // ================= GET DATA FROM DATABASE =================
            int totalRoutes = GetCount("SELECT COUNT(*) FROM routes");
            int totalBuses = GetCount("SELECT COUNT(*) FROM buses");
            int totalSchedules = GetCount("SELECT COUNT(*) FROM schedules");
            int totalReservations = GetCount("SELECT COUNT(*) FROM bookings");

            int pendingPayments = GetCount("SELECT COUNT(*) FROM payments WHERE payment_status = 'Pending'");
            int paidBookings = GetCount("SELECT COUNT(*) FROM payments WHERE payment_status = 'Paid'");
            int boardedPassengers = GetCount("SELECT COUNT(*) FROM bookings WHERE boarding_status = 'Boarded'");
            int cancelledBookings = GetCount("SELECT COUNT(*) FROM bookings WHERE reservation_status = 'Cancelled'");

            decimal totalRevenue = GetDecimalValue("SELECT IFNULL(SUM(amount), 0) FROM payments WHERE payment_status = 'Paid'");

            Label lblDescription = new Label();
            lblDescription.Text = "Monitor routes, buses, schedules, reservations, payments, boarding, and reports.";
            lblDescription.Font = new Font("Segoe UI", 11F, FontStyle.Regular);
            lblDescription.ForeColor = Color.DimGray;
            lblDescription.AutoSize = true;
            lblDescription.Location = new Point(30, 25);
            panelContent.Controls.Add(lblDescription);

            Button btnRefreshDashboard = new Button();
            btnRefreshDashboard.Text = "Refresh";
            btnRefreshDashboard.Size = new Size(130, 40);
            btnRefreshDashboard.Location = new Point(panelContent.Width - 180, 18);
            btnRefreshDashboard.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRefreshDashboard.BackColor = Color.FromArgb(52, 152, 219);
            btnRefreshDashboard.ForeColor = Color.White;
            btnRefreshDashboard.FlatStyle = FlatStyle.Flat;
            btnRefreshDashboard.FlatAppearance.BorderSize = 0;
            btnRefreshDashboard.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnRefreshDashboard.Cursor = Cursors.Hand;
            btnRefreshDashboard.Click += (s, e) =>
            {
                ShowDashboardHome();
            };
            panelContent.Controls.Add(btnRefreshDashboard);

            Panel cardRoutes = CreateStatCard("Total Routes", totalRoutes.ToString(), "Active terminal routes", new Point(25, 90), Color.FromArgb(52, 152, 219));
            Panel cardBuses = CreateStatCard("Total Buses", totalBuses.ToString(), "Registered buses", new Point(285, 90), Color.FromArgb(46, 204, 113));
            Panel cardSchedules = CreateStatCard("Schedules", totalSchedules.ToString(), "Scheduled trips", new Point(545, 90), Color.FromArgb(155, 89, 182));
            Panel cardReservations = CreateStatCard("Reservations", totalReservations.ToString(), "Passenger bookings", new Point(805, 90), Color.FromArgb(241, 196, 15));

            Panel cardPending = CreateStatCard("Pending Payments", pendingPayments.ToString(), "Needs confirmation", new Point(25, 250), Color.FromArgb(230, 126, 34));
            Panel cardPaid = CreateStatCard("Paid Bookings", paidBookings.ToString(), "Confirmed payments", new Point(285, 250), Color.FromArgb(39, 174, 96));
            Panel cardBoarded = CreateStatCard("Boarded", boardedPassengers.ToString(), "Boarded passengers", new Point(545, 250), Color.FromArgb(26, 188, 156));
            Panel cardCancelled = CreateStatCard("Cancelled", cancelledBookings.ToString(), "Cancelled bookings", new Point(805, 250), Color.FromArgb(231, 76, 60));

            Panel cardRevenue = CreateStatCard("Revenue", "₱" + totalRevenue.ToString("N2"), "Total paid amount", new Point(25, 410), Color.FromArgb(44, 62, 80));

            panelContent.Controls.Add(cardRoutes);
            panelContent.Controls.Add(cardBuses);
            panelContent.Controls.Add(cardSchedules);
            panelContent.Controls.Add(cardReservations);
            panelContent.Controls.Add(cardPending);
            panelContent.Controls.Add(cardPaid);
            panelContent.Controls.Add(cardBoarded);
            panelContent.Controls.Add(cardCancelled);
            panelContent.Controls.Add(cardRevenue);

            Panel panelActivity = new Panel();
            panelActivity.BackColor = Color.White;
            panelActivity.BorderStyle = BorderStyle.FixedSingle;
            panelActivity.Location = new Point(285, 410);
            panelActivity.Size = new Size(520, 210);
            panelContent.Controls.Add(panelActivity);

            Label lblActivityTitle = new Label();
            lblActivityTitle.Text = "Today's Activity";
            lblActivityTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblActivityTitle.ForeColor = Color.FromArgb(32, 45, 64);
            lblActivityTitle.AutoSize = true;
            lblActivityTitle.Location = new Point(20, 18);
            panelActivity.Controls.Add(lblActivityTitle);

            Label lblActivity = new Label();
            lblActivity.Text =
                "• " + totalSchedules + " scheduled trip(s) available\n" +
                "• " + totalReservations + " total passenger reservation(s)\n" +
                "• " + pendingPayments + " booking(s) still pending payment\n" +
                "• " + boardedPassengers + " passenger(s) already boarded\n" +
                "• Revenue collected: ₱" + totalRevenue.ToString("N2");
            lblActivity.Font = new Font("Segoe UI", 11F, FontStyle.Regular);
            lblActivity.ForeColor = Color.DimGray;
            lblActivity.AutoSize = true;
            lblActivity.Location = new Point(25, 65);
            panelActivity.Controls.Add(lblActivity);

            Panel panelQuick = new Panel();
            panelQuick.BackColor = Color.White;
            panelQuick.BorderStyle = BorderStyle.FixedSingle;
            panelQuick.Location = new Point(830, 410);
            panelQuick.Size = new Size(495, 210);
            panelContent.Controls.Add(panelQuick);

            Label lblQuickTitle = new Label();
            lblQuickTitle.Text = "Quick Actions";
            lblQuickTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblQuickTitle.ForeColor = Color.FromArgb(32, 45, 64);
            lblQuickTitle.AutoSize = true;
            lblQuickTitle.Location = new Point(20, 18);
            panelQuick.Controls.Add(lblQuickTitle);

            Button btnQuickRoute = CreateQuickButton("Manage Routes", new Point(25, 65));
            btnQuickRoute.Click += btnRouteManagement_Click;
            panelQuick.Controls.Add(btnQuickRoute);

            Button btnQuickSchedule = CreateQuickButton("Manage Schedules", new Point(250, 65));
            btnQuickSchedule.Click += btnScheduleManagement_Click;
            panelQuick.Controls.Add(btnQuickSchedule);

            Button btnQuickPayment = CreateQuickButton("Confirm Payments", new Point(25, 125));
            btnQuickPayment.Click += btnPaymentConfirmation_Click;
            panelQuick.Controls.Add(btnQuickPayment);

            Button btnQuickBoarding = CreateQuickButton("Check-in / Boarding", new Point(250, 125));
            btnQuickBoarding.Click += btnCheckInBoarding_Click;
            panelQuick.Controls.Add(btnQuickBoarding);
        }

        private Panel CreateStatCard(string title, string value, string description, Point location, Color accentColor)
        {
            Panel card = new Panel();
            card.Size = new Size(235, 125);
            card.Location = location;
            card.BackColor = Color.White;
            card.BorderStyle = BorderStyle.FixedSingle;

            Panel colorBar = new Panel();
            colorBar.BackColor = accentColor;
            colorBar.Dock = DockStyle.Left;
            colorBar.Width = 8;
            card.Controls.Add(colorBar);

            Label lblValue = new Label();
            lblValue.Text = value;
            lblValue.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblValue.ForeColor = accentColor;
            lblValue.AutoSize = true;
            lblValue.Location = new Point(25, 15);
            card.Controls.Add(lblValue);

            Label lblTitle = new Label();
            lblTitle.Text = title;
            lblTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(32, 45, 64);
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(28, 68);
            card.Controls.Add(lblTitle);

            Label lblDesc = new Label();
            lblDesc.Text = description;
            lblDesc.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            lblDesc.ForeColor = Color.Gray;
            lblDesc.AutoSize = true;
            lblDesc.Location = new Point(30, 95);
            card.Controls.Add(lblDesc);

            return card;
        }

        private Button CreateQuickButton(string text, Point location)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.Size = new Size(190, 42);
            btn.Location = location;
            btn.BackColor = Color.FromArgb(44, 62, 85);
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;

            btn.MouseEnter += (s, e) =>
            {
                btn.BackColor = Color.FromArgb(52, 152, 219);
            };

            btn.MouseLeave += (s, e) =>
            {
                btn.BackColor = Color.FromArgb(44, 62, 85);
            };

            return btn;
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            ShowDashboardHome();
        }

        private void btnRouteManagement_Click(object sender, EventArgs e)
        {
            LoadFormInPanel(new RouteManagementForm(), "Route Management");
        }

        private void btnBusManagement_Click(object sender, EventArgs e)
        {
            LoadFormInPanel(new BusManagementForm(), "Bus Management");
        }

        private void btnScheduleManagement_Click(object sender, EventArgs e)
        {
            LoadFormInPanel(new ScheduleManagementForm(), "Schedule / Trip Management");
        }

        private void btnReservationManagement_Click(object sender, EventArgs e)
        {
            LoadFormInPanel(new ReservationManagementForm(), "Reservation Management");
        }

        private void btnPaymentConfirmation_Click(object sender, EventArgs e)
        {
            LoadFormInPanel(new PaymentConfirmationForm(), "Payment Confirmation");
        }

        private void btnCheckInBoarding_Click(object sender, EventArgs e)
        {
            LoadFormInPanel(new CheckInBoardingForm(), "Check-in / Boarding");
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            LoadFormInPanel(new ReportsForm(), "Reports Dashboard");
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
                login log = new login();
                log.Show();

                this.Hide();
            }
        }
    }
}