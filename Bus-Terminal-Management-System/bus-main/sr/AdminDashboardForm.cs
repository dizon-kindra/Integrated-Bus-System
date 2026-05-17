using System;
using System.Drawing;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace sr
{
    public partial class AdminDashboardForm : Form
    {
        private readonly HttpClient client = new HttpClient();
        private readonly string apiBaseUrl = "http://localhost:3000/api";

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
            _ = ShowDashboardHomeAsync();
        }

        private void AdminDashboardForm_Load(object sender, EventArgs e)
        {
            // Empty load event.
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
            btnPaymentConfirmation.Visible = false;
            panelSidebar.Controls.Add(btnPaymentConfirmation);

            btnCheckInBoarding = CreateSidebarButton("Check-in / Boarding", startY + (buttonHeight + buttonGap) * 6);
            btnCheckInBoarding.Click += btnCheckInBoarding_Click;
            btnCheckInBoarding.Visible = false;
            panelSidebar.Controls.Add(btnCheckInBoarding);

            btnReports = CreateSidebarButton("Reports", startY + (buttonHeight + buttonGap) * 5);
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

        private async Task<JArray> GetArrayFromApi(string endpoint, string arrayName)
        {
            HttpResponseMessage response = await client.GetAsync(apiBaseUrl + endpoint);
            string responseBody = await response.Content.ReadAsStringAsync();

            JObject result = JObject.Parse(responseBody);

            if (!response.IsSuccessStatusCode || result["success"]?.ToObject<bool>() != true)
            {
                string message = result["message"]?.ToString() ?? "API request failed.";
                throw new Exception(message);
            }

            return (JArray)result[arrayName];
        }

        private async Task ShowDashboardHomeAsync()
        {
            panelContent.Controls.Clear();
            lblPageTitle.Text = "Dashboard";

            int totalRoutes = 0;
            int totalBuses = 0;
            int totalSchedules = 0;
            int totalReservations = 0;
            int pendingPayments = 0;
            int paidBookings = 0;
            int boardedPassengers = 0;
            int cancelledBookings = 0;
            decimal totalRevenue = 0;

            try
            {
                JArray routes = await GetArrayFromApi("/admin/routes", "routes");
                JArray buses = await GetArrayFromApi("/admin/buses", "buses");
                JArray schedules = await GetArrayFromApi("/admin/schedules", "schedules");
                JArray bookings = await GetArrayFromApi("/bookings", "bookings");

                totalRoutes = routes.Count;
                totalBuses = buses.Count;
                totalSchedules = schedules.Count;
                totalReservations = bookings.Count;

                foreach (JObject booking in bookings)
                {
                    string paymentStatus = booking["payment_status"]?.ToString() ?? "";
                    string reservationStatus = booking["reservation_status"]?.ToString() ?? "";
                    string boardingStatus = booking["boarding_status"]?.ToString() ?? "";

                    if (paymentStatus == "Pending")
                    {
                        pendingPayments++;
                    }

                    if (paymentStatus == "Paid")
                    {
                        paidBookings++;
                        totalRevenue += booking["total_amount"]?.ToObject<decimal>() ?? 0;
                    }

                    if (boardingStatus == "Boarded")
                    {
                        boardedPassengers++;
                    }

                    if (reservationStatus == "Cancelled")
                    {
                        cancelledBookings++;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Dashboard API error:\n\n" + ex.Message +
                    "\n\nPlease make sure:\n" +
                    "1. XAMPP MySQL is running\n" +
                    "2. Node API is running using npm start\n" +
                    "3. http://localhost:3000/api/test is working",
                    "API Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }

            Label lblDescription = new Label();
            lblDescription.Text = "Click a dashboard card to open the related management module.";
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
            btnRefreshDashboard.Click += async (s, e) =>
            {
                await ShowDashboardHomeAsync();
            };
            panelContent.Controls.Add(btnRefreshDashboard);

            FlowLayoutPanel dashboardCards = new FlowLayoutPanel();
            dashboardCards.Location = new Point(25, 90);
            dashboardCards.Size = new Size(panelContent.Width - 70, panelContent.Height - 130);
            dashboardCards.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dashboardCards.AutoScroll = true;
            dashboardCards.WrapContents = true;
            dashboardCards.FlowDirection = FlowDirection.LeftToRight;
            dashboardCards.BackColor = Color.FromArgb(245, 247, 250);
            dashboardCards.Padding = new Padding(0);
            panelContent.Controls.Add(dashboardCards);

            dashboardCards.Controls.Add(
                CreateStatCard(
                    "Total Routes",
                    totalRoutes.ToString(),
                    "Click to manage routes",
                    Color.FromArgb(52, 152, 219),
                    btnRouteManagement_Click
                )
            );

            dashboardCards.Controls.Add(
                CreateStatCard(
                    "Total Buses",
                    totalBuses.ToString(),
                    "Click to manage buses",
                    Color.FromArgb(46, 204, 113),
                    btnBusManagement_Click
                )
            );

            dashboardCards.Controls.Add(
                CreateStatCard(
                    "Schedules",
                    totalSchedules.ToString(),
                    "Click to manage schedules",
                    Color.FromArgb(155, 89, 182),
                    btnScheduleManagement_Click
                )
            );

            dashboardCards.Controls.Add(
                CreateStatCard(
                    "Reservations",
                    totalReservations.ToString(),
                    "Click to view reservations",
                    Color.FromArgb(241, 196, 15),
                    btnReservationManagement_Click
                )
            );

            dashboardCards.Controls.Add(
                CreateStatCard(
                    "Pending Payments",
                    pendingPayments.ToString(),
                    "Click to confirm payments",
                    Color.FromArgb(230, 126, 34),
                    btnReservationManagement_Click
                )
            );

            dashboardCards.Controls.Add(
                CreateStatCard(
                    "Paid Bookings",
                    paidBookings.ToString(),
                    "Click to view paid bookings",
                    Color.FromArgb(39, 174, 96),
                    btnReservationManagement_Click
                )
            );

            dashboardCards.Controls.Add(
                CreateStatCard(
                    "Boarded",
                    boardedPassengers.ToString(),
                    "Click to view boarded passengers",
                    Color.FromArgb(26, 188, 156),
                    btnReservationManagement_Click
                )
            );

            dashboardCards.Controls.Add(
                CreateStatCard(
                    "Cancelled",
                    cancelledBookings.ToString(),
                    "Click to view cancelled bookings",
                    Color.FromArgb(231, 76, 60),
                    btnReservationManagement_Click
                )
            );

            dashboardCards.Controls.Add(
                CreateStatCard(
                    "Revenue",
                    "₱" + totalRevenue.ToString("N2"),
                    "Click to open reports",
                    Color.FromArgb(44, 62, 80),
                    btnReports_Click
                )
            );
        }

        private Panel CreateStatCard(string title, string value, string description, Color accentColor, EventHandler clickAction)
        {
            Panel card = new Panel();
            card.Size = new Size(250, 135);
            card.Margin = new Padding(0, 0, 25, 25);
            card.BackColor = Color.White;
            card.BorderStyle = BorderStyle.FixedSingle;
            card.Cursor = Cursors.Hand;

            Panel colorBar = new Panel();
            colorBar.BackColor = accentColor;
            colorBar.Dock = DockStyle.Left;
            colorBar.Width = 8;
            card.Controls.Add(colorBar);

            Label lblValue = new Label();
            lblValue.Text = value;
            lblValue.Font = new Font("Segoe UI", 21F, FontStyle.Bold);
            lblValue.ForeColor = accentColor;
            lblValue.AutoSize = true;
            lblValue.Location = new Point(28, 18);
            lblValue.Cursor = Cursors.Hand;
            card.Controls.Add(lblValue);

            Label lblTitle = new Label();
            lblTitle.Text = title;
            lblTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(32, 45, 64);
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(30, 73);
            lblTitle.Cursor = Cursors.Hand;
            card.Controls.Add(lblTitle);

            Label lblDesc = new Label();
            lblDesc.Text = description;
            lblDesc.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            lblDesc.ForeColor = Color.Gray;
            lblDesc.AutoSize = true;
            lblDesc.Location = new Point(32, 101);
            lblDesc.Cursor = Cursors.Hand;
            card.Controls.Add(lblDesc);

            card.Click += clickAction;
            lblValue.Click += clickAction;
            lblTitle.Click += clickAction;
            lblDesc.Click += clickAction;
            colorBar.Click += clickAction;

            card.MouseEnter += (s, e) =>
            {
                card.BackColor = Color.FromArgb(248, 250, 252);
            };

            card.MouseLeave += (s, e) =>
            {
                card.BackColor = Color.White;
            };

            return card;
        }

        private async void btnDashboard_Click(object sender, EventArgs e)
        {
            await ShowDashboardHomeAsync();
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
            LoadFormInPanel(new ReservationManagementForm(), "Reservation Management");
        }

        private void btnCheckInBoarding_Click(object sender, EventArgs e)
        {
            LoadFormInPanel(new ReservationManagementForm(), "Reservation Management");
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