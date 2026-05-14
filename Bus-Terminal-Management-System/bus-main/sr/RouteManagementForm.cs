using System;
using System.Data;
using System.Drawing;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace sr
{
    public partial class RouteManagementForm : Form
    {
        private TextBox txtRouteID;
        private TextBox txtOrigin;
        private TextBox txtDestination;
        private TextBox txtFare;
        private TextBox txtDuration;
        private ComboBox cmbStatus;

        private Button btnAdd;
        private Button btnUpdate;
        private Button btnDeactivate;
        private Button btnClear;

        private DataGridView dgvRoutes;

        private readonly HttpClient client = new HttpClient();
        private readonly string apiBaseUrl = "http://localhost:3000/api";

        public RouteManagementForm()
        {
            BuildRouteUI();
            _ = LoadRoutes();
        }

        private void BuildRouteUI()
        {
            this.Text = "Route Management";
            this.Size = new Size(1100, 720);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(245, 247, 250);
            this.Font = new Font("Segoe UI", 10);

            Label lblTitle = new Label();
            lblTitle.Text = "🚌  Route Management";
            lblTitle.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(30, 80, 150);
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(30, 20);
            this.Controls.Add(lblTitle);

            Panel panelInfo = new Panel();
            panelInfo.Location = new Point(30, 75);
            panelInfo.Size = new Size(1020, 280);
            panelInfo.BackColor = Color.White;
            panelInfo.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(panelInfo);

            Label lblInfo = new Label();
            lblInfo.Text = "Route Information";
            lblInfo.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblInfo.ForeColor = Color.FromArgb(30, 90, 170);
            lblInfo.AutoSize = true;
            lblInfo.Location = new Point(20, 15);
            panelInfo.Controls.Add(lblInfo);

            Label lblRouteID = CreateLabel("Route ID:", 30, 70);
            txtRouteID = CreateTextBox(180, 65);
            txtRouteID.ReadOnly = true;
            txtRouteID.BackColor = Color.FromArgb(235, 235, 235);

            Label lblOrigin = CreateLabel("Origin:", 30, 115);
            txtOrigin = CreateTextBox(180, 110);

            Label lblDestination = CreateLabel("Destination:", 30, 160);
            txtDestination = CreateTextBox(180, 155);

            Label lblFare = CreateLabel("Fare:", 560, 70);
            txtFare = CreateTextBox(710, 65);

            Label lblDuration = CreateLabel("Estimated Duration:", 560, 115);
            txtDuration = CreateTextBox(710, 110);

            Label lblStatus = CreateLabel("Status:", 560, 160);
            cmbStatus = new ComboBox();
            cmbStatus.Location = new Point(710, 155);
            cmbStatus.Size = new Size(260, 32);
            cmbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbStatus.Items.Add("Active");
            cmbStatus.Items.Add("Inactive");
            cmbStatus.SelectedIndex = 0;

            panelInfo.Controls.Add(lblRouteID);
            panelInfo.Controls.Add(txtRouteID);
            panelInfo.Controls.Add(lblOrigin);
            panelInfo.Controls.Add(txtOrigin);
            panelInfo.Controls.Add(lblDestination);
            panelInfo.Controls.Add(txtDestination);
            panelInfo.Controls.Add(lblFare);
            panelInfo.Controls.Add(txtFare);
            panelInfo.Controls.Add(lblDuration);
            panelInfo.Controls.Add(txtDuration);
            panelInfo.Controls.Add(lblStatus);
            panelInfo.Controls.Add(cmbStatus);

            btnAdd = CreateButton("Add Route", 180, 215, Color.FromArgb(25, 103, 210));
            btnUpdate = CreateButton("Update Route", 350, 215, Color.FromArgb(25, 103, 210));
            btnDeactivate = CreateButton("Deactivate", 520, 215, Color.FromArgb(25, 103, 210));
            btnClear = CreateButton("Clear", 690, 215, Color.Gray);

            btnAdd.Click += BtnAdd_Click;
            btnUpdate.Click += BtnUpdate_Click;
            btnDeactivate.Click += BtnDeactivate_Click;
            btnClear.Click += BtnClear_Click;

            panelInfo.Controls.Add(btnAdd);
            panelInfo.Controls.Add(btnUpdate);
            panelInfo.Controls.Add(btnDeactivate);
            panelInfo.Controls.Add(btnClear);

            Panel panelGrid = new Panel();
            panelGrid.Location = new Point(30, 380);
            panelGrid.Size = new Size(1020, 285);
            panelGrid.BackColor = Color.White;
            panelGrid.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(panelGrid);

            Label lblRoutesList = new Label();
            lblRoutesList.Text = "Routes List";
            lblRoutesList.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblRoutesList.ForeColor = Color.FromArgb(30, 90, 170);
            lblRoutesList.AutoSize = true;
            lblRoutesList.Location = new Point(20, 15);
            panelGrid.Controls.Add(lblRoutesList);

            dgvRoutes = new DataGridView();
            dgvRoutes.Location = new Point(20, 60);
            dgvRoutes.Size = new Size(980, 200);
            dgvRoutes.AllowUserToAddRows = false;
            dgvRoutes.AllowUserToDeleteRows = false;
            dgvRoutes.ReadOnly = true;
            dgvRoutes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRoutes.MultiSelect = false;
            dgvRoutes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRoutes.BackgroundColor = Color.White;
            dgvRoutes.BorderStyle = BorderStyle.Fixed3D;
            dgvRoutes.RowHeadersVisible = false;

            dgvRoutes.EnableHeadersVisualStyles = false;
            dgvRoutes.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(225, 235, 248);
            dgvRoutes.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dgvRoutes.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvRoutes.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgvRoutes.DefaultCellStyle.SelectionBackColor = Color.FromArgb(180, 210, 250);
            dgvRoutes.DefaultCellStyle.SelectionForeColor = Color.Black;

            dgvRoutes.CellClick += DgvRoutes_CellClick;

            panelGrid.Controls.Add(dgvRoutes);
        }

        private Label CreateLabel(string text, int x, int y)
        {
            Label label = new Label();
            label.Text = text;
            label.Location = new Point(x, y);
            label.Size = new Size(150, 30);
            label.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            label.ForeColor = Color.Black;
            return label;
        }

        private TextBox CreateTextBox(int x, int y)
        {
            TextBox textBox = new TextBox();
            textBox.Location = new Point(x, y);
            textBox.Size = new Size(260, 32);
            textBox.Font = new Font("Segoe UI", 10);
            return textBox;
        }

        private Button CreateButton(string text, int x, int y, Color backColor)
        {
            Button button = new Button();
            button.Text = text;
            button.Location = new Point(x, y);
            button.Size = new Size(150, 40);
            button.BackColor = backColor;
            button.ForeColor = Color.White;
            button.FlatStyle = FlatStyle.Flat;
            button.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            button.Cursor = Cursors.Hand;
            return button;
        }

        private async System.Threading.Tasks.Task LoadRoutes()
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

                DataTable table = new DataTable();
                table.Columns.Add("route_id", typeof(int));
                table.Columns.Add("Route ID", typeof(string));
                table.Columns.Add("Origin", typeof(string));
                table.Columns.Add("Destination", typeof(string));
                table.Columns.Add("Fare", typeof(decimal));
                table.Columns.Add("Estimated Duration", typeof(string));
                table.Columns.Add("Status", typeof(string));

                foreach (JObject route in routes)
                {
                    table.Rows.Add(
                        route["route_id"]?.ToObject<int>() ?? 0,
                        route["route_code"]?.ToString() ?? "",
                        route["origin"]?.ToString() ?? "",
                        route["destination"]?.ToString() ?? "",
                        route["fare"]?.ToObject<decimal>() ?? 0,
                        route["estimated_duration"]?.ToString() ?? "",
                        route["status"]?.ToString() ?? ""
                    );
                }

                dgvRoutes.DataSource = table;

                if (dgvRoutes.Columns["route_id"] != null)
                {
                    dgvRoutes.Columns["route_id"].Visible = false;
                }

                GenerateRouteIDFromGrid();
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

        private void GenerateRouteIDFromGrid()
        {
            int maxId = 0;

            if (dgvRoutes.DataSource is DataTable table)
            {
                foreach (DataRow row in table.Rows)
                {
                    int currentId;
                    if (int.TryParse(row["route_id"].ToString(), out currentId))
                    {
                        if (currentId > maxId)
                        {
                            maxId = currentId;
                        }
                    }
                }
            }

            txtRouteID.Text = "RT-" + (maxId + 1).ToString("000");
        }

        private bool ValidateFields()
        {
            if (string.IsNullOrWhiteSpace(txtRouteID.Text))
            {
                MessageBox.Show("Route ID is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtOrigin.Text))
            {
                MessageBox.Show("Please enter origin.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtOrigin.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtDestination.Text))
            {
                MessageBox.Show("Please enter destination.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDestination.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtFare.Text))
            {
                MessageBox.Show("Please enter fare.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFare.Focus();
                return false;
            }

            decimal fare;
            if (!decimal.TryParse(txtFare.Text.Trim(), out fare))
            {
                MessageBox.Show("Fare must be a valid number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFare.Focus();
                return false;
            }

            if (fare <= 0)
            {
                MessageBox.Show("Fare must be greater than zero.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFare.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtDuration.Text))
            {
                MessageBox.Show("Please enter estimated duration.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDuration.Focus();
                return false;
            }

            if (cmbStatus.SelectedIndex == -1)
            {
                MessageBox.Show("Please select status.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbStatus.Focus();
                return false;
            }

            return true;
        }

        private async void BtnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateFields())
            {
                return;
            }

            try
            {
                var routeData = new
                {
                    route_code = txtRouteID.Text.Trim(),
                    origin = txtOrigin.Text.Trim(),
                    destination = txtDestination.Text.Trim(),
                    fare = Convert.ToDecimal(txtFare.Text.Trim()),
                    estimated_duration = txtDuration.Text.Trim(),
                    status = cmbStatus.Text.Trim()
                };

                string json = JsonConvert.SerializeObject(routeData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(apiBaseUrl + "/admin/routes", content);
                string responseBody = await response.Content.ReadAsStringAsync();

                JObject result = JObject.Parse(responseBody);

                if (response.IsSuccessStatusCode && result["success"]?.ToObject<bool>() == true)
                {
                    MessageBox.Show("Route added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearFields();
                    await LoadRoutes();
                }
                else
                {
                    string message = result["message"]?.ToString() ?? "Error adding route.";
                    MessageBox.Show(message, "API Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding route through API:\n" + ex.Message, "API Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRouteID.Text))
            {
                MessageBox.Show("Please select a route to update.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateFields())
            {
                return;
            }

            try
            {
                if (dgvRoutes.CurrentRow == null || dgvRoutes.CurrentRow.Cells["route_id"].Value == null)
                {
                    MessageBox.Show("Please select a route from the table.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int routeId = Convert.ToInt32(dgvRoutes.CurrentRow.Cells["route_id"].Value);

                var routeData = new
                {
                    route_code = txtRouteID.Text.Trim(),
                    origin = txtOrigin.Text.Trim(),
                    destination = txtDestination.Text.Trim(),
                    fare = Convert.ToDecimal(txtFare.Text.Trim()),
                    estimated_duration = txtDuration.Text.Trim(),
                    status = cmbStatus.Text.Trim()
                };

                string json = JsonConvert.SerializeObject(routeData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PutAsync(apiBaseUrl + "/admin/routes/" + routeId, content);
                string responseBody = await response.Content.ReadAsStringAsync();

                JObject result = JObject.Parse(responseBody);

                if (response.IsSuccessStatusCode && result["success"]?.ToObject<bool>() == true)
                {
                    MessageBox.Show("Route updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearFields();
                    await LoadRoutes();
                }
                else
                {
                    string message = result["message"]?.ToString() ?? "Error updating route.";
                    MessageBox.Show(message, "API Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating route through API:\n" + ex.Message, "API Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void BtnDeactivate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRouteID.Text))
            {
                MessageBox.Show("Please select a route to deactivate.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirm = MessageBox.Show(
                "Are you sure you want to deactivate this route?",
                "Confirm Deactivate",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirm != DialogResult.Yes)
            {
                return;
            }

            try
            {
                if (dgvRoutes.CurrentRow == null || dgvRoutes.CurrentRow.Cells["route_id"].Value == null)
                {
                    MessageBox.Show("Please select a route from the table.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int routeId = Convert.ToInt32(dgvRoutes.CurrentRow.Cells["route_id"].Value);

                var routeData = new
                {
                    route_code = txtRouteID.Text.Trim(),
                    origin = txtOrigin.Text.Trim(),
                    destination = txtDestination.Text.Trim(),
                    fare = Convert.ToDecimal(txtFare.Text.Trim()),
                    estimated_duration = txtDuration.Text.Trim(),
                    status = "Inactive"
                };

                string json = JsonConvert.SerializeObject(routeData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PutAsync(apiBaseUrl + "/admin/routes/" + routeId, content);
                string responseBody = await response.Content.ReadAsStringAsync();

                JObject result = JObject.Parse(responseBody);

                if (response.IsSuccessStatusCode && result["success"]?.ToObject<bool>() == true)
                {
                    MessageBox.Show("Route deactivated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearFields();
                    await LoadRoutes();
                }
                else
                {
                    string message = result["message"]?.ToString() ?? "Error deactivating route.";
                    MessageBox.Show(message, "API Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deactivating route through API:\n" + ex.Message, "API Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
            GenerateRouteIDFromGrid();
        }

        private void ClearFields()
        {
            txtOrigin.Clear();
            txtDestination.Clear();
            txtFare.Clear();
            txtDuration.Clear();

            if (cmbStatus.Items.Count > 0)
            {
                cmbStatus.SelectedIndex = 0;
            }

            txtOrigin.Focus();

            if (dgvRoutes != null)
            {
                dgvRoutes.ClearSelection();
            }

            GenerateRouteIDFromGrid();
        }

        private void DgvRoutes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvRoutes.Rows[e.RowIndex].Cells["Route ID"].Value != null)
            {
                DataGridViewRow row = dgvRoutes.Rows[e.RowIndex];

                txtRouteID.Text = row.Cells["Route ID"].Value.ToString();
                txtOrigin.Text = row.Cells["Origin"].Value.ToString();
                txtDestination.Text = row.Cells["Destination"].Value.ToString();
                txtFare.Text = row.Cells["Fare"].Value.ToString();
                txtDuration.Text = row.Cells["Estimated Duration"].Value.ToString();
                cmbStatus.Text = row.Cells["Status"].Value.ToString();
            }
        }

        private void RouteManagementForm_Load(object sender, EventArgs e)
        {
            // Not used because UI is built in constructor.
        }
    }
}