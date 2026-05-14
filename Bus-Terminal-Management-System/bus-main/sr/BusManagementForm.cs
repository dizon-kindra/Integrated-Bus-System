using System;
using System.Data;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace sr
{
    public partial class BusManagementForm : Form
    {
        private readonly HttpClient client = new HttpClient();
        private readonly string apiBaseUrl = "http://localhost:3000/api";

        public BusManagementForm()
        {
            InitializeComponent();
        }

        private async void BusManagementForm_Load(object sender, EventArgs e)
        {
            cmbBusType.Items.Clear();
            cmbBusType.Items.Add("Airconditioned");
            cmbBusType.Items.Add("Ordinary");
            cmbBusType.Items.Add("Mini Bus");

            cmbStatus.Items.Clear();
            cmbStatus.Items.Add("Active");
            cmbStatus.Items.Add("Inactive");
            cmbStatus.Items.Add("Maintenance");

            if (cmbBusType.Items.Count > 0)
            {
                cmbBusType.SelectedIndex = 0;
            }

            if (cmbStatus.Items.Count > 0)
            {
                cmbStatus.SelectedIndex = 0;
            }

            await LoadBuses();
        }

        private async System.Threading.Tasks.Task LoadBuses()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(apiBaseUrl + "/admin/buses");
                string responseBody = await response.Content.ReadAsStringAsync();

                JObject result = JObject.Parse(responseBody);

                if (!response.IsSuccessStatusCode || result["success"]?.ToObject<bool>() != true)
                {
                    string message = result["message"]?.ToString() ?? "Failed to load buses.";
                    MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                JArray buses = (JArray)result["buses"];

                DataTable dt = new DataTable();
                dt.Columns.Add("bus_id", typeof(int));
                dt.Columns.Add("bus_number", typeof(string));
                dt.Columns.Add("plate_number", typeof(string));
                dt.Columns.Add("capacity", typeof(int));
                dt.Columns.Add("bus_type", typeof(string));
                dt.Columns.Add("status", typeof(string));

                foreach (JObject bus in buses)
                {
                    dt.Rows.Add(
                        bus["bus_id"]?.ToObject<int>() ?? 0,
                        bus["bus_number"]?.ToString() ?? "",
                        bus["plate_number"]?.ToString() ?? "",
                        bus["capacity"]?.ToObject<int>() ?? 0,
                        bus["bus_type"]?.ToString() ?? "",
                        bus["status"]?.ToString() ?? ""
                    );
                }

                dgvBuses.DataSource = dt;

                if (dgvBuses.Columns.Count > 0)
                {
                    dgvBuses.Columns["bus_id"].HeaderText = "Bus ID";
                    dgvBuses.Columns["bus_number"].HeaderText = "Bus Number";
                    dgvBuses.Columns["plate_number"].HeaderText = "Plate Number";
                    dgvBuses.Columns["capacity"].HeaderText = "Capacity";
                    dgvBuses.Columns["bus_type"].HeaderText = "Bus Type";
                    dgvBuses.Columns["status"].HeaderText = "Status";

                    dgvBuses.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dgvBuses.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    dgvBuses.MultiSelect = false;
                    dgvBuses.ReadOnly = true;
                }
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

        private void ClearFields()
        {
            txtBusID.Clear();
            txtBusNumber.Clear();
            txtPlateNumber.Clear();
            txtCapacity.Clear();

            if (cmbBusType.Items.Count > 0)
            {
                cmbBusType.SelectedIndex = 0;
            }

            if (cmbStatus.Items.Count > 0)
            {
                cmbStatus.SelectedIndex = 0;
            }

            txtBusNumber.Focus();
        }

        private bool ValidateFields()
        {
            if (string.IsNullOrWhiteSpace(txtBusNumber.Text))
            {
                MessageBox.Show("Please enter bus number.");
                txtBusNumber.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPlateNumber.Text))
            {
                MessageBox.Show("Please enter plate number.");
                txtPlateNumber.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtCapacity.Text))
            {
                MessageBox.Show("Please enter capacity.");
                txtCapacity.Focus();
                return false;
            }

            int capacity;

            if (!int.TryParse(txtCapacity.Text.Trim(), out capacity))
            {
                MessageBox.Show("Capacity must be a valid number.");
                txtCapacity.Focus();
                return false;
            }

            if (capacity <= 0)
            {
                MessageBox.Show("Capacity must be greater than zero.");
                txtCapacity.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(cmbBusType.Text))
            {
                MessageBox.Show("Please select bus type.");
                cmbBusType.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(cmbStatus.Text))
            {
                MessageBox.Show("Please select status.");
                cmbStatus.Focus();
                return false;
            }

            return true;
        }

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateFields())
            {
                return;
            }

            try
            {
                var busData = new
                {
                    bus_number = txtBusNumber.Text.Trim(),
                    plate_number = txtPlateNumber.Text.Trim(),
                    capacity = Convert.ToInt32(txtCapacity.Text.Trim()),
                    bus_type = cmbBusType.Text.Trim(),
                    status = cmbStatus.Text.Trim()
                };

                string json = JsonConvert.SerializeObject(busData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(apiBaseUrl + "/admin/buses", content);
                string responseBody = await response.Content.ReadAsStringAsync();

                JObject result = JObject.Parse(responseBody);

                if (response.IsSuccessStatusCode && result["success"]?.ToObject<bool>() == true)
                {
                    MessageBox.Show("Bus added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await LoadBuses();
                    ClearFields();
                }
                else
                {
                    string message = result["message"]?.ToString() ?? "Error adding bus.";
                    MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error adding bus through API.\n\n" + ex.Message,
                    "API Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBusID.Text))
            {
                MessageBox.Show("Please select a bus to update.");
                return;
            }

            if (!ValidateFields())
            {
                return;
            }

            try
            {
                int busId = Convert.ToInt32(txtBusID.Text.Trim());

                var busData = new
                {
                    bus_number = txtBusNumber.Text.Trim(),
                    plate_number = txtPlateNumber.Text.Trim(),
                    capacity = Convert.ToInt32(txtCapacity.Text.Trim()),
                    bus_type = cmbBusType.Text.Trim(),
                    status = cmbStatus.Text.Trim()
                };

                string json = JsonConvert.SerializeObject(busData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PutAsync(apiBaseUrl + "/admin/buses/" + busId, content);
                string responseBody = await response.Content.ReadAsStringAsync();

                JObject result = JObject.Parse(responseBody);

                if (response.IsSuccessStatusCode && result["success"]?.ToObject<bool>() == true)
                {
                    MessageBox.Show("Bus updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await LoadBuses();
                    ClearFields();
                }
                else
                {
                    string message = result["message"]?.ToString() ?? "Error updating bus.";
                    MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error updating bus through API.\n\n" + ex.Message,
                    "API Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private async void btnDeactivate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBusID.Text))
            {
                MessageBox.Show("Please select a bus to deactivate.");
                return;
            }

            DialogResult resultConfirm = MessageBox.Show(
                "Are you sure you want to deactivate this bus?",
                "Confirm Deactivate",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (resultConfirm == DialogResult.No)
            {
                return;
            }

            try
            {
                int busId = Convert.ToInt32(txtBusID.Text.Trim());

                var busData = new
                {
                    bus_number = txtBusNumber.Text.Trim(),
                    plate_number = txtPlateNumber.Text.Trim(),
                    capacity = Convert.ToInt32(txtCapacity.Text.Trim()),
                    bus_type = cmbBusType.Text.Trim(),
                    status = "Inactive"
                };

                string json = JsonConvert.SerializeObject(busData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PutAsync(apiBaseUrl + "/admin/buses/" + busId, content);
                string responseBody = await response.Content.ReadAsStringAsync();

                JObject result = JObject.Parse(responseBody);

                if (response.IsSuccessStatusCode && result["success"]?.ToObject<bool>() == true)
                {
                    MessageBox.Show("Bus deactivated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await LoadBuses();
                    ClearFields();
                }
                else
                {
                    string message = result["message"]?.ToString() ?? "Error deactivating bus.";
                    MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error deactivating bus through API.\n\n" + ex.Message,
                    "API Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void dgvBuses_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvBuses.Rows[e.RowIndex].Cells["bus_id"].Value != null)
            {
                DataGridViewRow row = dgvBuses.Rows[e.RowIndex];

                txtBusID.Text = row.Cells["bus_id"].Value.ToString();
                txtBusNumber.Text = row.Cells["bus_number"].Value.ToString();
                txtPlateNumber.Text = row.Cells["plate_number"].Value.ToString();
                txtCapacity.Text = row.Cells["capacity"].Value.ToString();
                cmbBusType.Text = row.Cells["bus_type"].Value.ToString();
                cmbStatus.Text = row.Cells["status"].Value.ToString();
            }
        }
    }
}