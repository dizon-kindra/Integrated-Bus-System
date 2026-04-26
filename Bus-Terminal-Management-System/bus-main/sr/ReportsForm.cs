using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace sr
{
    public partial class ReportsForm : Form
    {
        string connectionString = "server=localhost;user id=root;password=;database=sr_db;";

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

        private void GenerateReport()
        {
            if (cmbReportType.Text == "Daily Bookings")
                LoadDailyBookings();
            else if (cmbReportType.Text == "Cancelled Bookings")
                LoadCancelledBookings();
            else if (cmbReportType.Text == "Completed Trips")
                LoadCompletedTrips();
            else if (cmbReportType.Text == "Revenue Report")
                LoadRevenueReport();
            else if (cmbReportType.Text == "Passenger Manifest")
                LoadPassengerManifest();
            else if (cmbReportType.Text == "Trip Report")
                LoadTripReport();
        }

        private void LoadDailyBookings()
        {
            try
            {
                string query = @"
                    SELECT
                        bk.booking_id AS 'Booking ID',
                        bk.passenger_name AS 'Passenger Name',
                        bk.phone AS 'Phone',
                        CONCAT(r.origin, ' → ', r.destination) AS 'Route',
                        b.bus_number AS 'Bus',
                        bk.seat_no AS 'Seat No',
                        s.departure_date AS 'Travel Date',
                        s.departure_time AS 'Departure Time',
                        bk.payment_status AS 'Payment Status',
                        bk.reservation_status AS 'Reservation Status',
                        bk.created_at AS 'Date Booked'
                    FROM bookings bk
                    INNER JOIN schedules s ON bk.schedule_id = s.schedule_id
                    INNER JOIN buses b ON s.bus_id = b.bus_id
                    INNER JOIN routes r ON s.route_id = r.route_id
                    WHERE DATE(bk.created_at) BETWEEN @fromDate AND @toDate
                    ORDER BY bk.created_at DESC";

                LoadData(query, false);
                lblReportTitle.Text = "Report Type: Daily Bookings";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading daily bookings report: " + ex.Message);
            }
        }

        private void LoadCancelledBookings()
        {
            try
            {
                string query = @"
                    SELECT
                        bk.booking_id AS 'Booking ID',
                        bk.passenger_name AS 'Passenger Name',
                        bk.phone AS 'Phone',
                        CONCAT(r.origin, ' → ', r.destination) AS 'Route',
                        b.bus_number AS 'Bus',
                        bk.seat_no AS 'Seat No',
                        s.departure_date AS 'Travel Date',
                        bk.payment_status AS 'Payment Status',
                        bk.reservation_status AS 'Reservation Status',
                        bk.created_at AS 'Date Booked'
                    FROM bookings bk
                    INNER JOIN schedules s ON bk.schedule_id = s.schedule_id
                    INNER JOIN buses b ON s.bus_id = b.bus_id
                    INNER JOIN routes r ON s.route_id = r.route_id
                    WHERE bk.reservation_status = 'Cancelled'
                    AND DATE(bk.created_at) BETWEEN @fromDate AND @toDate
                    ORDER BY bk.created_at DESC";

                LoadData(query, false);
                lblReportTitle.Text = "Report Type: Cancelled Bookings";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading cancelled bookings report: " + ex.Message);
            }
        }

        private void LoadCompletedTrips()
        {
            try
            {
                string query = @"
                    SELECT
                        s.schedule_id AS 'Trip ID',
                        b.bus_number AS 'Bus',
                        CONCAT(r.origin, ' → ', r.destination) AS 'Route',
                        s.departure_date AS 'Departure Date',
                        s.departure_time AS 'Departure Time',
                        s.arrival_time AS 'Arrival Time',
                        s.fare AS 'Fare',
                        s.available_seats AS 'Available Seats',
                        s.trip_status AS 'Trip Status'
                    FROM schedules s
                    INNER JOIN buses b ON s.bus_id = b.bus_id
                    INNER JOIN routes r ON s.route_id = r.route_id
                    WHERE s.trip_status = 'Arrived'
                    AND s.departure_date BETWEEN @fromDate AND @toDate
                    ORDER BY s.departure_date DESC, s.departure_time DESC";

                LoadData(query, false);
                lblReportTitle.Text = "Report Type: Completed Trips";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading completed trips report: " + ex.Message);
            }
        }

        private void LoadRevenueReport()
        {
            try
            {
                string query = @"
                    SELECT
                        p.payment_id AS 'Payment ID',
                        bk.booking_id AS 'Booking ID',
                        bk.passenger_name AS 'Passenger Name',
                        CONCAT(r.origin, ' → ', r.destination) AS 'Route',
                        b.bus_number AS 'Bus',
                        bk.seat_no AS 'Seat No',
                        p.amount AS 'Amount',
                        p.payment_method AS 'Payment Method',
                        p.reference_no AS 'Reference No',
                        p.payment_status AS 'Payment Status',
                        p.paid_at AS 'Date Paid'
                    FROM payments p
                    INNER JOIN bookings bk ON p.booking_id = bk.booking_id
                    INNER JOIN schedules s ON bk.schedule_id = s.schedule_id
                    INNER JOIN buses b ON s.bus_id = b.bus_id
                    INNER JOIN routes r ON s.route_id = r.route_id
                    WHERE p.payment_status = 'Paid'
                    AND DATE(p.paid_at) BETWEEN @fromDate AND @toDate
                    ORDER BY p.paid_at DESC";

                LoadData(query, true);
                lblReportTitle.Text = "Report Type: Revenue Report";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading revenue report: " + ex.Message);
            }
        }

        private void LoadPassengerManifest()
        {
            try
            {
                string query = @"
                    SELECT
                        s.schedule_id AS 'Trip ID',
                        b.bus_number AS 'Bus',
                        CONCAT(r.origin, ' → ', r.destination) AS 'Route',
                        s.departure_date AS 'Travel Date',
                        s.departure_time AS 'Departure Time',
                        bk.seat_no AS 'Seat No',
                        bk.passenger_name AS 'Passenger Name',
                        bk.phone AS 'Phone',
                        bk.payment_status AS 'Payment Status',
                        bk.reservation_status AS 'Reservation Status',
                        bk.checkin_status AS 'Check-in Status',
                        bk.boarding_status AS 'Boarding Status'
                    FROM bookings bk
                    INNER JOIN schedules s ON bk.schedule_id = s.schedule_id
                    INNER JOIN buses b ON s.bus_id = b.bus_id
                    INNER JOIN routes r ON s.route_id = r.route_id
                    WHERE s.departure_date BETWEEN @fromDate AND @toDate
                    AND bk.reservation_status != 'Cancelled'
                    ORDER BY s.departure_date ASC, s.departure_time ASC, bk.seat_no ASC";

                LoadData(query, false);
                lblReportTitle.Text = "Report Type: Passenger Manifest";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading passenger manifest: " + ex.Message);
            }
        }

        private void LoadTripReport()
        {
            try
            {
                string query = @"
                    SELECT
                        s.schedule_id AS 'Trip ID',
                        b.bus_number AS 'Bus',
                        b.plate_number AS 'Plate Number',
                        CONCAT(r.origin, ' → ', r.destination) AS 'Route',
                        s.departure_date AS 'Departure Date',
                        s.departure_time AS 'Departure Time',
                        s.arrival_time AS 'Arrival Time',
                        s.fare AS 'Fare',
                        s.available_seats AS 'Available Seats',
                        s.trip_status AS 'Trip Status',
                        COUNT(bk.booking_id) AS 'Total Bookings',
                        SUM(CASE WHEN bk.boarding_status = 'Boarded' THEN 1 ELSE 0 END) AS 'Boarded Passengers',
                        SUM(CASE WHEN bk.boarding_status = 'No-show' THEN 1 ELSE 0 END) AS 'No-show Passengers'
                    FROM schedules s
                    INNER JOIN buses b ON s.bus_id = b.bus_id
                    INNER JOIN routes r ON s.route_id = r.route_id
                    LEFT JOIN bookings bk ON s.schedule_id = bk.schedule_id
                    WHERE s.departure_date BETWEEN @fromDate AND @toDate
                    GROUP BY 
                        s.schedule_id,
                        b.bus_number,
                        b.plate_number,
                        r.origin,
                        r.destination,
                        s.departure_date,
                        s.departure_time,
                        s.arrival_time,
                        s.fare,
                        s.available_seats,
                        s.trip_status
                    ORDER BY s.departure_date DESC, s.departure_time DESC";

                LoadData(query, false);
                lblReportTitle.Text = "Report Type: Trip Report";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading trip report: " + ex.Message);
            }
        }

        private void LoadData(string query, bool computeRevenue)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@fromDate", dtpFrom.Value.Date);
                    cmd.Parameters.AddWithValue("@toDate", dtpTo.Value.Date);

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dgvReports.DataSource = dt;

                    lblTotalRecords.Text = "Total Records: " + dt.Rows.Count;

                    if (computeRevenue)
                    {
                        decimal totalRevenue = 0;

                        foreach (DataRow row in dt.Rows)
                        {
                            if (row["Amount"] != DBNull.Value)
                                totalRevenue += Convert.ToDecimal(row["Amount"]);
                        }

                        lblTotalRevenue.Text = "Total Revenue: " + totalRevenue.ToString("0.00");
                    }
                    else
                    {
                        lblTotalRevenue.Text = "Total Revenue: 0.00";
                    }
                }
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (dtpFrom.Value.Date > dtpTo.Value.Date)
            {
                MessageBox.Show("Date From cannot be greater than Date To.", "Invalid Date", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            GenerateReport();
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