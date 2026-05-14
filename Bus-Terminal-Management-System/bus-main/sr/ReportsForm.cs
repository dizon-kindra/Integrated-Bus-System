using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace sr
{
    public partial class ReportsForm : Form
    {
        private readonly HttpClient client = new HttpClient();
        private readonly string apiBaseUrl = "http://localhost:3000/api";

        PrintDocument printDocument = new PrintDocument();
        int printRowIndex = 0;

        public ReportsForm()
        {
            InitializeComponent();
            printDocument.PrintPage += printDocument_PrintPage;
        }

        private void ReportsForm_Load(object sender, EventArgs e)
        {
            cmbReportType.Items.Clear();
            cmbReportType.Items.Add("Daily Bookings");
            cmbReportType.Items.Add("Cancelled Bookings");
            cmbReportType.Items.Add("Completed Trips");
            cmbReportType.Items.Add("Revenue Report");
            cmbReportType.Items.Add("Passenger Manifest");
            cmbReportType.Items.Add("Trip Report");
            cmbReportType.SelectedIndex = 0;

            dtpFrom.Value = DateTime.Now.Date;
            dtpTo.Value = DateTime.Now.Date;

            lblTotalRecords.Text = "Total Records: 0";
            lblTotalRevenue.Text = "Total Revenue: 0.00";
            lblReportTitle.Text = "Report Type: None";
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

        private DateTime? ParseDate(string value)
        {
            DateTime date;

            if (DateTime.TryParse(value, out date))
            {
                return date.Date;
            }

            return null;
        }

        private bool IsDateInRange(string dateValue)
        {
            DateTime? date = ParseDate(dateValue);

            if (date == null)
            {
                return false;
            }

            return date.Value >= dtpFrom.Value.Date && date.Value <= dtpTo.Value.Date;
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

        private async Task GenerateReport()
        {
            if (cmbReportType.Text == "Daily Bookings")
                await LoadDailyBookings();
            else if (cmbReportType.Text == "Cancelled Bookings")
                await LoadCancelledBookings();
            else if (cmbReportType.Text == "Completed Trips")
                await LoadCompletedTrips();
            else if (cmbReportType.Text == "Revenue Report")
                await LoadRevenueReport();
            else if (cmbReportType.Text == "Passenger Manifest")
                await LoadPassengerManifest();
            else if (cmbReportType.Text == "Trip Report")
                await LoadTripReport();
        }

        private async Task LoadDailyBookings()
        {
            try
            {
                JArray bookings = await GetArrayFromApi("/bookings", "bookings");

                DataTable dt = new DataTable();
                dt.Columns.Add("Booking ID");
                dt.Columns.Add("Passenger Name");
                dt.Columns.Add("Phone");
                dt.Columns.Add("Route");
                dt.Columns.Add("Bus");
                dt.Columns.Add("Seat No");
                dt.Columns.Add("Travel Date");
                dt.Columns.Add("Departure Time");
                dt.Columns.Add("Payment Status");
                dt.Columns.Add("Reservation Status");
                dt.Columns.Add("Date Booked");

                foreach (JObject booking in bookings)
                {
                    string createdAt = booking["created_at"]?.ToString() ?? "";

                    if (!IsDateInRange(createdAt))
                    {
                        continue;
                    }

                    string origin = booking["origin"]?.ToString() ?? "";
                    string destination = booking["destination"]?.ToString() ?? "";

                    dt.Rows.Add(
                        booking["booking_id"]?.ToString() ?? "",
                        booking["passenger_name"]?.ToString() ?? "",
                        booking["phone"]?.ToString() ?? "",
                        origin + " → " + destination,
                        booking["bus_number"]?.ToString() ?? "",
                        booking["seat_no"]?.ToString() ?? "",
                        FormatDate(booking["departure_date"]?.ToString()),
                        booking["departure_time"]?.ToString() ?? "",
                        booking["payment_status"]?.ToString() ?? "",
                        booking["reservation_status"]?.ToString() ?? "",
                        FormatDate(createdAt)
                    );
                }

                ShowReport(dt, false);
                lblReportTitle.Text = "Report Type: Daily Bookings";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading daily bookings report from API:\n" + ex.Message);
            }
        }

        private async Task LoadCancelledBookings()
        {
            try
            {
                JArray bookings = await GetArrayFromApi("/bookings", "bookings");

                DataTable dt = new DataTable();
                dt.Columns.Add("Booking ID");
                dt.Columns.Add("Passenger Name");
                dt.Columns.Add("Phone");
                dt.Columns.Add("Route");
                dt.Columns.Add("Bus");
                dt.Columns.Add("Seat No");
                dt.Columns.Add("Travel Date");
                dt.Columns.Add("Payment Status");
                dt.Columns.Add("Reservation Status");
                dt.Columns.Add("Date Booked");

                foreach (JObject booking in bookings)
                {
                    string status = booking["reservation_status"]?.ToString() ?? "";
                    string createdAt = booking["created_at"]?.ToString() ?? "";

                    if (status != "Cancelled" || !IsDateInRange(createdAt))
                    {
                        continue;
                    }

                    string origin = booking["origin"]?.ToString() ?? "";
                    string destination = booking["destination"]?.ToString() ?? "";

                    dt.Rows.Add(
                        booking["booking_id"]?.ToString() ?? "",
                        booking["passenger_name"]?.ToString() ?? "",
                        booking["phone"]?.ToString() ?? "",
                        origin + " → " + destination,
                        booking["bus_number"]?.ToString() ?? "",
                        booking["seat_no"]?.ToString() ?? "",
                        FormatDate(booking["departure_date"]?.ToString()),
                        booking["payment_status"]?.ToString() ?? "",
                        booking["reservation_status"]?.ToString() ?? "",
                        FormatDate(createdAt)
                    );
                }

                ShowReport(dt, false);
                lblReportTitle.Text = "Report Type: Cancelled Bookings";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading cancelled bookings report from API:\n" + ex.Message);
            }
        }

        private async Task LoadCompletedTrips()
        {
            try
            {
                JArray schedules = await GetArrayFromApi("/admin/schedules", "schedules");

                DataTable dt = new DataTable();
                dt.Columns.Add("Trip ID");
                dt.Columns.Add("Bus");
                dt.Columns.Add("Route");
                dt.Columns.Add("Departure Date");
                dt.Columns.Add("Departure Time");
                dt.Columns.Add("Arrival Time");
                dt.Columns.Add("Fare");
                dt.Columns.Add("Available Seats");
                dt.Columns.Add("Trip Status");

                foreach (JObject schedule in schedules)
                {
                    string tripStatus = schedule["trip_status"]?.ToString() ?? "";
                    string departureDate = schedule["departure_date"]?.ToString() ?? "";

                    if ((tripStatus != "Arrived" && tripStatus != "Completed") || !IsDateInRange(departureDate))
                    {
                        continue;
                    }

                    string origin = schedule["origin"]?.ToString() ?? "";
                    string destination = schedule["destination"]?.ToString() ?? "";

                    dt.Rows.Add(
                        schedule["schedule_id"]?.ToString() ?? "",
                        schedule["bus_number"]?.ToString() ?? "",
                        origin + " → " + destination,
                        FormatDate(departureDate),
                        schedule["departure_time"]?.ToString() ?? "",
                        schedule["arrival_time"]?.ToString() ?? "",
                        schedule["fare"]?.ToString() ?? "",
                        schedule["available_seats"]?.ToString() ?? "",
                        tripStatus
                    );
                }

                ShowReport(dt, false);
                lblReportTitle.Text = "Report Type: Completed Trips";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading completed trips report from API:\n" + ex.Message);
            }
        }

        private async Task LoadRevenueReport()
        {
            try
            {
                JArray bookings = await GetArrayFromApi("/bookings", "bookings");

                DataTable dt = new DataTable();
                dt.Columns.Add("Payment ID");
                dt.Columns.Add("Booking ID");
                dt.Columns.Add("Passenger Name");
                dt.Columns.Add("Route");
                dt.Columns.Add("Bus");
                dt.Columns.Add("Seat No");
                dt.Columns.Add("Amount");
                dt.Columns.Add("Payment Method");
                dt.Columns.Add("Reference No");
                dt.Columns.Add("Payment Status");
                dt.Columns.Add("Date Paid");

                foreach (JObject booking in bookings)
                {
                    string paymentStatus = booking["payment_record_status"]?.ToString();

                    if (string.IsNullOrWhiteSpace(paymentStatus))
                    {
                        paymentStatus = booking["payment_status"]?.ToString() ?? "";
                    }

                    string paidAt = booking["paid_at"]?.ToString() ?? "";

                    if (paymentStatus != "Paid" || !IsDateInRange(paidAt))
                    {
                        continue;
                    }

                    string origin = booking["origin"]?.ToString() ?? "";
                    string destination = booking["destination"]?.ToString() ?? "";

                    dt.Rows.Add(
                        booking["payment_id"]?.ToString() ?? "",
                        booking["booking_id"]?.ToString() ?? "",
                        booking["passenger_name"]?.ToString() ?? "",
                        origin + " → " + destination,
                        booking["bus_number"]?.ToString() ?? "",
                        booking["seat_no"]?.ToString() ?? "",
                        booking["total_amount"]?.ToString() ?? "0",
                        booking["payment_method"]?.ToString() ?? "",
                        booking["reference_no"]?.ToString() ?? "",
                        paymentStatus,
                        FormatDate(paidAt)
                    );
                }

                ShowReport(dt, true);
                lblReportTitle.Text = "Report Type: Revenue Report";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading revenue report from API:\n" + ex.Message);
            }
        }

        private async Task LoadPassengerManifest()
        {
            try
            {
                JArray bookings = await GetArrayFromApi("/bookings", "bookings");

                DataTable dt = new DataTable();
                dt.Columns.Add("Trip ID");
                dt.Columns.Add("Bus");
                dt.Columns.Add("Route");
                dt.Columns.Add("Travel Date");
                dt.Columns.Add("Departure Time");
                dt.Columns.Add("Seat No");
                dt.Columns.Add("Passenger Name");
                dt.Columns.Add("Phone");
                dt.Columns.Add("Payment Status");
                dt.Columns.Add("Reservation Status");
                dt.Columns.Add("Check-in Status");
                dt.Columns.Add("Boarding Status");

                foreach (JObject booking in bookings)
                {
                    string reservationStatus = booking["reservation_status"]?.ToString() ?? "";
                    string departureDate = booking["departure_date"]?.ToString() ?? "";

                    if (reservationStatus == "Cancelled" || !IsDateInRange(departureDate))
                    {
                        continue;
                    }

                    string origin = booking["origin"]?.ToString() ?? "";
                    string destination = booking["destination"]?.ToString() ?? "";

                    dt.Rows.Add(
                        booking["schedule_id"]?.ToString() ?? "",
                        booking["bus_number"]?.ToString() ?? "",
                        origin + " → " + destination,
                        FormatDate(departureDate),
                        booking["departure_time"]?.ToString() ?? "",
                        booking["seat_no"]?.ToString() ?? "",
                        booking["passenger_name"]?.ToString() ?? "",
                        booking["phone"]?.ToString() ?? "",
                        booking["payment_status"]?.ToString() ?? "",
                        reservationStatus,
                        booking["checkin_status"]?.ToString() ?? "",
                        booking["boarding_status"]?.ToString() ?? ""
                    );
                }

                ShowReport(dt, false);
                lblReportTitle.Text = "Report Type: Passenger Manifest";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading passenger manifest from API:\n" + ex.Message);
            }
        }

        private async Task LoadTripReport()
        {
            try
            {
                JArray schedules = await GetArrayFromApi("/admin/schedules", "schedules");
                JArray bookings = await GetArrayFromApi("/bookings", "bookings");

                DataTable dt = new DataTable();
                dt.Columns.Add("Trip ID");
                dt.Columns.Add("Bus");
                dt.Columns.Add("Plate Number");
                dt.Columns.Add("Route");
                dt.Columns.Add("Departure Date");
                dt.Columns.Add("Departure Time");
                dt.Columns.Add("Arrival Time");
                dt.Columns.Add("Fare");
                dt.Columns.Add("Available Seats");
                dt.Columns.Add("Trip Status");
                dt.Columns.Add("Total Bookings");
                dt.Columns.Add("Boarded Passengers");
                dt.Columns.Add("No-show Passengers");

                foreach (JObject schedule in schedules)
                {
                    string departureDate = schedule["departure_date"]?.ToString() ?? "";

                    if (!IsDateInRange(departureDate))
                    {
                        continue;
                    }

                    int scheduleId = schedule["schedule_id"]?.ToObject<int>() ?? 0;
                    int totalBookings = 0;
                    int boardedPassengers = 0;
                    int noShowPassengers = 0;

                    foreach (JObject booking in bookings)
                    {
                        int bookingScheduleId = booking["schedule_id"]?.ToObject<int>() ?? 0;

                        if (bookingScheduleId == scheduleId)
                        {
                            totalBookings++;

                            string boardingStatus = booking["boarding_status"]?.ToString() ?? "";

                            if (boardingStatus == "Boarded")
                            {
                                boardedPassengers++;
                            }
                            else if (boardingStatus == "No-show")
                            {
                                noShowPassengers++;
                            }
                        }
                    }

                    string origin = schedule["origin"]?.ToString() ?? "";
                    string destination = schedule["destination"]?.ToString() ?? "";

                    dt.Rows.Add(
                        scheduleId.ToString(),
                        schedule["bus_number"]?.ToString() ?? "",
                        schedule["plate_number"]?.ToString() ?? "",
                        origin + " → " + destination,
                        FormatDate(departureDate),
                        schedule["departure_time"]?.ToString() ?? "",
                        schedule["arrival_time"]?.ToString() ?? "",
                        schedule["fare"]?.ToString() ?? "",
                        schedule["available_seats"]?.ToString() ?? "",
                        schedule["trip_status"]?.ToString() ?? "",
                        totalBookings.ToString(),
                        boardedPassengers.ToString(),
                        noShowPassengers.ToString()
                    );
                }

                ShowReport(dt, false);
                lblReportTitle.Text = "Report Type: Trip Report";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading trip report from API:\n" + ex.Message);
            }
        }

        private void ShowReport(DataTable dt, bool computeRevenue)
        {
            dgvReports.DataSource = dt;
            lblTotalRecords.Text = "Total Records: " + dt.Rows.Count;

            if (computeRevenue)
            {
                decimal totalRevenue = 0;

                foreach (DataRow row in dt.Rows)
                {
                    if (dt.Columns.Contains("Amount") && row["Amount"] != DBNull.Value)
                    {
                        decimal amount;

                        if (decimal.TryParse(row["Amount"].ToString(), out amount))
                        {
                            totalRevenue += amount;
                        }
                    }
                }

                lblTotalRevenue.Text = "Total Revenue: " + totalRevenue.ToString("0.00");
            }
            else
            {
                lblTotalRevenue.Text = "Total Revenue: 0.00";
            }
        }

        private async void btnGenerate_Click(object sender, EventArgs e)
        {
            if (dtpFrom.Value.Date > dtpTo.Value.Date)
            {
                MessageBox.Show("Date From cannot be greater than Date To.", "Invalid Date", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            await GenerateReport();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            dtpFrom.Value = DateTime.Now.Date;
            dtpTo.Value = DateTime.Now.Date;

            if (cmbReportType.Items.Count > 0)
                cmbReportType.SelectedIndex = 0;

            dgvReports.DataSource = null;
            lblTotalRecords.Text = "Total Records: 0";
            lblTotalRevenue.Text = "Total Revenue: 0.00";
            lblReportTitle.Text = "Report Type: None";
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (dgvReports.Rows.Count == 0 || dgvReports.DataSource == null)
            {
                MessageBox.Show("No report data to print.", "Print Report", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            printRowIndex = 0;

            PrintPreviewDialog previewDialog = new PrintPreviewDialog();
            previewDialog.Document = printDocument;
            previewDialog.Width = 1000;
            previewDialog.Height = 700;
            previewDialog.ShowDialog();
        }

        private void printDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            int x = 50;
            int y = 50;
            int rowHeight = 25;

            Font titleFont = new Font("Arial", 14, FontStyle.Bold);
            Font headerFont = new Font("Arial", 8, FontStyle.Bold);
            Font rowFont = new Font("Arial", 8);

            e.Graphics.DrawString(lblReportTitle.Text, titleFont, Brushes.Black, x, y);
            y += 30;

            e.Graphics.DrawString("Date From: " + dtpFrom.Value.ToString("yyyy-MM-dd") +
                                  "   Date To: " + dtpTo.Value.ToString("yyyy-MM-dd"),
                                  rowFont, Brushes.Black, x, y);
            y += 25;

            e.Graphics.DrawString(lblTotalRecords.Text + "    " + lblTotalRevenue.Text,
                                  rowFont, Brushes.Black, x, y);
            y += 30;

            int colX = x;
            int colWidth = 110;

            for (int i = 0; i < dgvReports.Columns.Count; i++)
            {
                string header = dgvReports.Columns[i].HeaderText;

                if (header.Length > 14)
                    header = header.Substring(0, 14);

                e.Graphics.DrawString(header, headerFont, Brushes.Black, colX, y);
                colX += colWidth;
            }

            y += rowHeight;

            while (printRowIndex < dgvReports.Rows.Count)
            {
                DataGridViewRow row = dgvReports.Rows[printRowIndex];

                if (!row.IsNewRow)
                {
                    colX = x;

                    for (int i = 0; i < dgvReports.Columns.Count; i++)
                    {
                        string value = "";

                        if (row.Cells[i].Value != null)
                            value = row.Cells[i].Value.ToString();

                        if (value.Length > 14)
                            value = value.Substring(0, 14);

                        e.Graphics.DrawString(value, rowFont, Brushes.Black, colX, y);
                        colX += colWidth;
                    }

                    y += rowHeight;
                }

                printRowIndex++;

                if (y > e.MarginBounds.Bottom)
                {
                    e.HasMorePages = true;
                    return;
                }
            }

            e.HasMorePages = false;
        }
    }
}