using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace sr
{
    public partial class ScheduleManagementForm : Form
    {
        string connectionString = "server=localhost;user id=root;password=;database=sr_db;";

        public ScheduleManagementForm()
        {
            InitializeComponent();
        }

        private void ScheduleManagementForm_Load(object sender, EventArgs e)
        {

            dtpDepartureTime.Format = DateTimePickerFormat.Time;
            dtpDepartureTime.ShowUpDown = true;

            dtpArrivalTime.Format = DateTimePickerFormat.Time;
            dtpArrivalTime.ShowUpDown = true;

            cmbTripStatus.Items.Clear();
            cmbTripStatus.Items.Add("Scheduled");
            cmbTripStatus.Items.Add("Boarding");
            cmbTripStatus.Items.Add("Departed");
            cmbTripStatus.Items.Add("Arrived");
            cmbTripStatus.Items.Add("Cancelled");
            cmbTripStatus.SelectedIndex = 0;

            LoadBuses();
            LoadRoutes();
            LoadSchedules();
            AutoFillFareAndSeats();
        }

        private void LoadBuses()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "SELECT bus_id, bus_number, capacity FROM buses WHERE status = 'Active'";

                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    cmbBus.DataSource = dt;
                    cmbBus.DisplayMember = "bus_number";
                    cmbBus.ValueMember = "bus_id";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading buses: " + ex.Message);
            }
        }

        private void LoadRoutes()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "SELECT route_id, CONCAT(origin, ' → ', destination) AS route_name, fare FROM routes WHERE status = 'Active'";

                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    cmbRoute.DataSource = dt;
                    cmbRoute.DisplayMember = "route_name";
                    cmbRoute.ValueMember = "route_id";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading routes: " + ex.Message);
            }
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
                            s.schedule_id AS 'Schedule ID',
                            b.bus_number AS 'Bus Number',
                            CONCAT(r.origin, ' → ', r.destination) AS 'Route',
                            s.departure_date AS 'Departure Date',
                            s.departure_time AS 'Departure Time',
                            s.arrival_time AS 'Arrival Time',
                            s.fare AS 'Fare',
                            s.available_seats AS 'Available Seats',
                            s.trip_status AS 'Trip Status',
                            s.bus_id,
                            s.route_id
                        FROM schedules s
                        INNER JOIN buses b ON s.bus_id = b.bus_id
                        INNER JOIN routes r ON s.route_id = r.route_id
                        ORDER BY s.departure_date DESC, s.departure_time DESC";

                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dgvSchedules.DataSource = dt;

                    if (dgvSchedules.Columns.Contains("bus_id"))
                        dgvSchedules.Columns["bus_id"].Visible = false;

                    if (dgvSchedules.Columns.Contains("route_id"))
                        dgvSchedules.Columns["route_id"].Visible = false;

                    dgvSchedules.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading schedules: " + ex.Message);
            }
        }

        private int GetBusCapacity(int busId)
        {
            int capacity = 0;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "SELECT capacity FROM buses WHERE bus_id = @bus_id";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@bus_id", busId);

                        object result = cmd.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            capacity = Convert.ToInt32(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error getting bus capacity: " + ex.Message);
            }

            return capacity;
        }

        private decimal GetRouteFare(int routeId)
        {
            decimal fare = 0;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "SELECT fare FROM routes WHERE route_id = @route_id";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@route_id", routeId);

                        object result = cmd.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            fare = Convert.ToDecimal(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error getting route fare: " + ex.Message);
            }

            return fare;
        }

        private void AutoFillFareAndSeats()
        {
            try
            {
                if (cmbBus.SelectedValue == null || cmbRoute.SelectedValue == null)
                    return;

                if (cmbBus.SelectedValue is DataRowView || cmbRoute.SelectedValue is DataRowView)
                    return;

                int busId = Convert.ToInt32(cmbBus.SelectedValue);
                int routeId = Convert.ToInt32(cmbRoute.SelectedValue);

                int capacity = GetBusCapacity(busId);
                decimal fare = GetRouteFare(routeId);

                txtAvailableSeats.Text = capacity.ToString();
                txtFare.Text = fare.ToString("0.00");
            }
            catch
            {
                // avoid error while ComboBox is loading
            }
        }

        private bool ValidateFields()
        {
            if (cmbBus.SelectedValue == null)
            {
                MessageBox.Show("Please select bus.");
                return false;
            }

            if (cmbRoute.SelectedValue == null)
            {
                MessageBox.Show("Please select route.");
                return false;
            }

            if (txtFare.Text.Trim() == "")
            {
                MessageBox.Show("Fare is required.");
                return false;
            }

            if (txtAvailableSeats.Text.Trim() == "")
            {
                MessageBox.Show("Available seats is required.");
                return false;
            }

            decimal fare;
            if (!decimal.TryParse(txtFare.Text.Trim(), out fare))
            {
                MessageBox.Show("Fare must be a valid number.");
                return false;
            }

            int seats;
            if (!int.TryParse(txtAvailableSeats.Text.Trim(), out seats))
            {
                MessageBox.Show("Available seats must be a valid number.");
                return false;
            }

            if (fare <= 0)
            {
                MessageBox.Show("Fare must be greater than zero.");
                return false;
            }

            if (seats <= 0)
            {
                MessageBox.Show("Available seats must be greater than zero.");
                return false;
            }

            if (dtpArrivalTime.Value.TimeOfDay <= dtpDepartureTime.Value.TimeOfDay)
            {
                MessageBox.Show("Arrival time must be later than departure time.");
                return false;
            }

            return true;
        }

        private void ClearFields()
        {
            txtScheduleID.Clear();

            if (cmbBus.Items.Count > 0)
                cmbBus.SelectedIndex = 0;

            if (cmbRoute.Items.Count > 0)
                cmbRoute.SelectedIndex = 0;

            dtpDepartureDate.Value = DateTime.Now;
            dtpDepartureTime.Value = DateTime.Now;
            dtpArrivalTime.Value = DateTime.Now.AddHours(1);

            if (cmbTripStatus.Items.Count > 0)
                cmbTripStatus.SelectedIndex = 0;

            AutoFillFareAndSeats();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateFields())
                return;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                        INSERT INTO schedules 
                        (bus_id, route_id, departure_date, departure_time, arrival_time, fare, available_seats, trip_status)
                        VALUES
                        (@bus_id, @route_id, @departure_date, @departure_time, @arrival_time, @fare, @available_seats, @trip_status)";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@bus_id", Convert.ToInt32(cmbBus.SelectedValue));
                        cmd.Parameters.AddWithValue("@route_id", Convert.ToInt32(cmbRoute.SelectedValue));
                        cmd.Parameters.AddWithValue("@departure_date", dtpDepartureDate.Value.Date);
                        cmd.Parameters.AddWithValue("@departure_time", dtpDepartureTime.Value.ToString("HH:mm:ss"));
                        cmd.Parameters.AddWithValue("@arrival_time", dtpArrivalTime.Value.ToString("HH:mm:ss"));
                        cmd.Parameters.AddWithValue("@fare", Convert.ToDecimal(txtFare.Text.Trim()));
                        cmd.Parameters.AddWithValue("@available_seats", Convert.ToInt32(txtAvailableSeats.Text.Trim()));
                        cmd.Parameters.AddWithValue("@trip_status", cmbTripStatus.Text);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Schedule added successfully.");
                LoadSchedules();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding schedule: " + ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtScheduleID.Text.Trim() == "")
            {
                MessageBox.Show("Please select a schedule to update.");
                return;
            }

            if (!ValidateFields())
                return;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                        UPDATE schedules SET
                            bus_id = @bus_id,
                            route_id = @route_id,
                            departure_date = @departure_date,
                            departure_time = @departure_time,
                            arrival_time = @arrival_time,
                            fare = @fare,
                            available_seats = @available_seats,
                            trip_status = @trip_status
                        WHERE schedule_id = @schedule_id";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@schedule_id", txtScheduleID.Text.Trim());
                        cmd.Parameters.AddWithValue("@bus_id", Convert.ToInt32(cmbBus.SelectedValue));
                        cmd.Parameters.AddWithValue("@route_id", Convert.ToInt32(cmbRoute.SelectedValue));
                        cmd.Parameters.AddWithValue("@departure_date", dtpDepartureDate.Value.Date);
                        cmd.Parameters.AddWithValue("@departure_time", dtpDepartureTime.Value.ToString("HH:mm:ss"));
                        cmd.Parameters.AddWithValue("@arrival_time", dtpArrivalTime.Value.ToString("HH:mm:ss"));
                        cmd.Parameters.AddWithValue("@fare", Convert.ToDecimal(txtFare.Text.Trim()));
                        cmd.Parameters.AddWithValue("@available_seats", Convert.ToInt32(txtAvailableSeats.Text.Trim()));
                        cmd.Parameters.AddWithValue("@trip_status", cmbTripStatus.Text);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Schedule updated successfully.");
                LoadSchedules();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating schedule: " + ex.Message);
            }
        }

        private void btnCancelTrip_Click(object sender, EventArgs e)
        {
            if (txtScheduleID.Text.Trim() == "")
            {
                MessageBox.Show("Please select a schedule to cancel.");
                return;
            }

            DialogResult result = MessageBox.Show(
                "Are you sure you want to cancel this trip?",
                "Confirm Cancel Trip",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.No)
                return;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "UPDATE schedules SET trip_status = 'Cancelled' WHERE schedule_id = @schedule_id";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@schedule_id", txtScheduleID.Text.Trim());
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Trip cancelled successfully.");
                LoadSchedules();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cancelling trip: " + ex.Message);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void cmbBus_SelectedIndexChanged(object sender, EventArgs e)
        {
            AutoFillFareAndSeats();
        }

        private void cmbRoute_SelectedIndexChanged(object sender, EventArgs e)
        {
            AutoFillFareAndSeats();
        }

        private void dgvSchedules_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvSchedules.Rows[e.RowIndex];

                txtScheduleID.Text = row.Cells["Schedule ID"].Value.ToString();

                cmbBus.SelectedValue = Convert.ToInt32(row.Cells["bus_id"].Value);
                cmbRoute.SelectedValue = Convert.ToInt32(row.Cells["route_id"].Value);

                dtpDepartureDate.Value = Convert.ToDateTime(row.Cells["Departure Date"].Value);

                TimeSpan departureTime = TimeSpan.Parse(row.Cells["Departure Time"].Value.ToString());
                TimeSpan arrivalTime = TimeSpan.Parse(row.Cells["Arrival Time"].Value.ToString());

                dtpDepartureTime.Value = DateTime.Today.Add(departureTime);
                dtpArrivalTime.Value = DateTime.Today.Add(arrivalTime);

                txtFare.Text = row.Cells["Fare"].Value.ToString();
                txtAvailableSeats.Text = row.Cells["Available Seats"].Value.ToString();
                cmbTripStatus.Text = row.Cells["Trip Status"].Value.ToString();
            }
        }
    }
}