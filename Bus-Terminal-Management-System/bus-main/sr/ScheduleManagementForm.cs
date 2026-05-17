using System;
using System.Data;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace sr
{
    public partial class ScheduleManagementForm : Form
    {
        private readonly HttpClient client = new HttpClient();
        private readonly string apiBaseUrl = "http://localhost:3000/api";

        public ScheduleManagementForm()
        {
            InitializeComponent();
        }

        private async void ScheduleManagementForm_Load(object sender, EventArgs e)
        {
            // Time format without seconds: 03:10 PM
            dtpDepartureTime.Format = DateTimePickerFormat.Custom;
            dtpDepartureTime.CustomFormat = "hh:mm tt";
            dtpDepartureTime.ShowUpDown = true;

            dtpArrivalTime.Format = DateTimePickerFormat.Custom;
            dtpArrivalTime.CustomFormat = "hh:mm tt";
            dtpArrivalTime.ShowUpDown = true;

            cmbTripStatus.Items.Clear();
            cmbTripStatus.Items.Add("Scheduled");
            cmbTripStatus.Items.Add("Departed");
            cmbTripStatus.Items.Add("Arrived");
            cmbTripStatus.Items.Add("Completed");
            cmbTripStatus.Items.Add("Cancelled");
            cmbTripStatus.SelectedIndex = 0;

            await LoadBuses();
            await LoadRoutes();
            await LoadSchedules();
            AutoFillFareAndSeats();
        }

        private async Task LoadBuses()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(apiBaseUrl + "/admin/buses");
                string responseBody = await response.Content.ReadAsStringAsync();

                JObject result = JObject.Parse(responseBody);

                if (!response.IsSuccessStatusCode || result["success"]?.ToObject<bool>() != true)
                {
                    string message = result["message"]?.ToString() ?? "Failed to load buses.";
                    MessageBox.Show(message, "API Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                JArray buses = (JArray)result["buses"];

                DataTable dt = new DataTable();
                dt.Columns.Add("bus_id", typeof(int));
                dt.Columns.Add("bus_number", typeof(string));
                dt.Columns.Add("capacity", typeof(int));

                foreach (JObject bus in buses)
                {
                    string status = bus["status"]?.ToString() ?? "";

                    if (status == "Active")
                    {
                        dt.Rows.Add(
                            bus["bus_id"]?.ToObject<int>() ?? 0,
                            bus["bus_number"]?.ToString() ?? "",
                            bus["capacity"]?.ToObject<int>() ?? 0
                        );
                    }
                }

                cmbBus.DataSource = dt;
                cmbBus.DisplayMember = "bus_number";
                cmbBus.ValueMember = "bus_id";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error loading buses from API.\n\nMake sure Node API is running.\n\n" + ex.Message,
                    "API Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private async Task LoadRoutes()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(apiBaseUrl + "/admin/routes");
                string responseBody = await response.Content.ReadAsStringAsync();

                JObject result = JObject.Parse(responseBody);

                if (!response.IsSuccessStatusCode || result["success"]?.ToObject<bool>() != true)
                {
                    string message = result["message"]?.ToString() ?? "Failed to load routes.";
                    MessageBox.Show(message, "API Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                JArray routes = (JArray)result["routes"];

                DataTable dt = new DataTable();
                dt.Columns.Add("route_id", typeof(int));
                dt.Columns.Add("route_name", typeof(string));
                dt.Columns.Add("fare", typeof(decimal));

                foreach (JObject route in routes)
                {
                    string status = route["status"]?.ToString() ?? "";

                    if (status == "Active")
                    {
                        string origin = route["origin"]?.ToString() ?? "";
                        string destination = route["destination"]?.ToString() ?? "";

                        dt.Rows.Add(
                            route["route_id"]?.ToObject<int>() ?? 0,
                            origin + " → " + destination,
                            route["fare"]?.ToObject<decimal>() ?? 0
                        );
                    }
                }

                cmbRoute.DataSource = dt;
                cmbRoute.DisplayMember = "route_name";
                cmbRoute.ValueMember = "route_id";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error loading routes from API.\n\nMake sure Node API is running.\n\n" + ex.Message,
                    "API Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
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
                dt.Columns.Add("Schedule ID", typeof(int));
                dt.Columns.Add("Bus Number", typeof(string));
                dt.Columns.Add("Route", typeof(string));
                dt.Columns.Add("Departure Date", typeof(string));
                dt.Columns.Add("Departure Time", typeof(string));
                dt.Columns.Add("Arrival Time", typeof(string));
                dt.Columns.Add("Fare", typeof(decimal));
                dt.Columns.Add("Available Seats", typeof(int));
                dt.Columns.Add("Trip Status", typeof(string));
                dt.Columns.Add("bus_id", typeof(int));
                dt.Columns.Add("route_id", typeof(int));

                foreach (JObject schedule in schedules)
                {
                    string origin = schedule["origin"]?.ToString() ?? "";
                    string destination = schedule["destination"]?.ToString() ?? "";

                    dt.Rows.Add(
                        schedule["schedule_id"]?.ToObject<int>() ?? 0,
                        schedule["bus_number"]?.ToString() ?? "",
                        origin + " → " + destination,
                        FormatDate(schedule["departure_date"]?.ToString()),
                        FormatTime(schedule["departure_time"]?.ToString()),
                        FormatTime(schedule["arrival_time"]?.ToString()),
                        schedule["fare"]?.ToObject<decimal>() ?? 0,
                        schedule["available_seats"]?.ToObject<int>() ?? 0,
                        schedule["trip_status"]?.ToString() ?? "",
                        schedule["bus_id"]?.ToObject<int>() ?? 0,
                        schedule["route_id"]?.ToObject<int>() ?? 0
                    );
                }

                dgvSchedules.DataSource = dt;

                if (dgvSchedules.Columns.Contains("bus_id"))
                    dgvSchedules.Columns["bus_id"].Visible = false;

                if (dgvSchedules.Columns.Contains("route_id"))
                    dgvSchedules.Columns["route_id"].Visible = false;

                dgvSchedules.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
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

        private string FormatDate(string value)
        {
            DateTime date;

            if (DateTime.TryParse(value, out date))
            {
                return date.ToString("yyyy-MM-dd");
            }

            return value ?? "";
        }

        private string FormatTime(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "";

            DateTime time;

            if (DateTime.TryParse(value, out time))
            {
                return time.ToString("hh:mm tt");
            }

            TimeSpan timeSpan;

            if (TimeSpan.TryParse(value, out timeSpan))
            {
                return DateTime.Today.Add(timeSpan).ToString("hh:mm tt");
            }

            return value;
        }

        private int GetSelectedBusCapacity()
        {
            try
            {
                if (cmbBus.SelectedItem is DataRowView row)
                {
                    return Convert.ToInt32(row["capacity"]);
                }
            }
            catch
            {
                // Ignore while ComboBox is loading
            }

            return 0;
        }

        private decimal GetSelectedRouteFare()
        {
            try
            {
                if (cmbRoute.SelectedItem is DataRowView row)
                {
                    return Convert.ToDecimal(row["fare"]);
                }
            }
            catch
            {
                // Ignore while ComboBox is loading
            }

            return 0;
        }

        private void AutoFillFareAndSeats()
        {
            try
            {
                if (cmbBus.SelectedValue == null || cmbRoute.SelectedValue == null)
                    return;

                if (cmbBus.SelectedValue is DataRowView || cmbRoute.SelectedValue is DataRowView)
                    return;

                int capacity = GetSelectedBusCapacity();
                decimal fare = GetSelectedRouteFare();

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

            if (dgvSchedules != null)
            {
                dgvSchedules.ClearSelection();
            }
        }

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateFields())
                return;

            try
            {
                var scheduleData = new
                {
                    bus_id = Convert.ToInt32(cmbBus.SelectedValue),
                    route_id = Convert.ToInt32(cmbRoute.SelectedValue),
                    departure_date = dtpDepartureDate.Value.ToString("yyyy-MM-dd"),

                    // Save to API/database as 03:10 PM, no seconds
                    departure_time = dtpDepartureTime.Value.ToString("hh:mm tt"),
                    arrival_time = dtpArrivalTime.Value.ToString("hh:mm tt"),

                    fare = Convert.ToDecimal(txtFare.Text.Trim()),
                    trip_status = cmbTripStatus.Text
                };

                string json = JsonConvert.SerializeObject(scheduleData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(apiBaseUrl + "/admin/schedules", content);
                string responseBody = await response.Content.ReadAsStringAsync();

                JObject result = JObject.Parse(responseBody);

                if (response.IsSuccessStatusCode && result["success"]?.ToObject<bool>() == true)
                {
                    MessageBox.Show("Schedule added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await LoadSchedules();
                    ClearFields();
                }
                else
                {
                    string message = result["message"]?.ToString() ?? "Error adding schedule.";
                    MessageBox.Show(message, "API Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding schedule through API:\n" + ex.Message, "API Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
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
                int scheduleId = Convert.ToInt32(txtScheduleID.Text.Trim());

                var scheduleData = new
                {
                    bus_id = Convert.ToInt32(cmbBus.SelectedValue),
                    route_id = Convert.ToInt32(cmbRoute.SelectedValue),
                    departure_date = dtpDepartureDate.Value.ToString("yyyy-MM-dd"),

                    // Save to API/database as 03:10 PM, no seconds
                    departure_time = dtpDepartureTime.Value.ToString("hh:mm tt"),
                    arrival_time = dtpArrivalTime.Value.ToString("hh:mm tt"),

                    fare = Convert.ToDecimal(txtFare.Text.Trim()),
                    available_seats = Convert.ToInt32(txtAvailableSeats.Text.Trim()),
                    trip_status = cmbTripStatus.Text
                };

                string json = JsonConvert.SerializeObject(scheduleData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PutAsync(apiBaseUrl + "/admin/schedules/" + scheduleId, content);
                string responseBody = await response.Content.ReadAsStringAsync();

                JObject result = JObject.Parse(responseBody);

                if (response.IsSuccessStatusCode && result["success"]?.ToObject<bool>() == true)
                {
                    MessageBox.Show("Schedule updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await LoadSchedules();
                    ClearFields();
                }
                else
                {
                    string message = result["message"]?.ToString() ?? "Error updating schedule.";
                    MessageBox.Show(message, "API Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating schedule through API:\n" + ex.Message, "API Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnCancelTrip_Click(object sender, EventArgs e)
        {
            if (txtScheduleID.Text.Trim() == "")
            {
                MessageBox.Show("Please select a schedule to cancel.");
                return;
            }

            DialogResult resultConfirm = MessageBox.Show(
                "Are you sure you want to cancel this trip?",
                "Confirm Cancel Trip",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (resultConfirm == DialogResult.No)
                return;

            try
            {
                int scheduleId = Convert.ToInt32(txtScheduleID.Text.Trim());

                var statusData = new
                {
                    trip_status = "Cancelled"
                };

                string json = JsonConvert.SerializeObject(statusData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PutAsync(apiBaseUrl + "/admin/schedules/" + scheduleId + "/status", content);
                string responseBody = await response.Content.ReadAsStringAsync();

                JObject result = JObject.Parse(responseBody);

                if (response.IsSuccessStatusCode && result["success"]?.ToObject<bool>() == true)
                {
                    MessageBox.Show("Trip cancelled successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await LoadSchedules();
                    ClearFields();
                }
                else
                {
                    string message = result["message"]?.ToString() ?? "Error cancelling trip.";
                    MessageBox.Show(message, "API Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cancelling trip through API:\n" + ex.Message, "API Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                DateTime departureDate;
                if (DateTime.TryParse(row.Cells["Departure Date"].Value.ToString(), out departureDate))
                {
                    dtpDepartureDate.Value = departureDate;
                }

                DateTime departureTime;
                DateTime arrivalTime;

                if (DateTime.TryParse(row.Cells["Departure Time"].Value.ToString(), out departureTime))
                {
                    dtpDepartureTime.Value = departureTime;
                }

                if (DateTime.TryParse(row.Cells["Arrival Time"].Value.ToString(), out arrivalTime))
                {
                    dtpArrivalTime.Value = arrivalTime;
                }

                txtFare.Text = row.Cells["Fare"].Value.ToString();
                txtAvailableSeats.Text = row.Cells["Available Seats"].Value.ToString();
                cmbTripStatus.Text = row.Cells["Trip Status"].Value.ToString();
            }
        }
    }
}