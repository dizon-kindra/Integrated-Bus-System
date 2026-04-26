using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

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

        // Change password if your MySQL root has password.
        // If your XAMPP MySQL has no password, keep password blank.
       
        private string connStr = "server=localhost;user=root;password=;database=sr_db;";

        public RouteManagementForm()
        {
           
            BuildRouteUI();        // mao ni atong custom UI
            LoadRoutes();
            GenerateRouteID();
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

            // Left labels and textboxes
            Label lblRouteID = CreateLabel("Route ID:", 30, 70);
            txtRouteID = CreateTextBox(180, 65);
            txtRouteID.ReadOnly = true;
            txtRouteID.BackColor = Color.FromArgb(235, 235, 235);

            Label lblOrigin = CreateLabel("Origin:", 30, 115);
            txtOrigin = CreateTextBox(180, 110);

            Label lblDestination = CreateLabel("Destination:", 30, 160);
            txtDestination = CreateTextBox(180, 155);

            // Right labels and textboxes
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

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(connStr);
        }

        private void LoadRoutes()
        {
            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    conn.Open();

                    string query = @"
                        SELECT 
                            route_code AS 'Route ID',
                            origin AS 'Origin',
                            destination AS 'Destination',
                            fare AS 'Fare',
                            estimated_duration AS 'Estimated Duration',
                            status AS 'Status'
                        FROM routes
                        ORDER BY route_id DESC";

                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    dgvRoutes.DataSource = table;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading routes:\n" + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GenerateRouteID()
        {
            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    conn.Open();

                    string query = "SELECT IFNULL(MAX(route_id), 0) + 1 FROM routes";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        int nextID = Convert.ToInt32(cmd.ExecuteScalar());
                        txtRouteID.Text = "RT-" + nextID.ToString("000");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating Route ID:\n" + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateFields()
        {
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
            if (!decimal.TryParse(txtFare.Text, out fare))
            {
                MessageBox.Show("Fare must be a valid number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFare.Focus();
                return false;
            }

            if (fare < 0)
            {
                MessageBox.Show("Fare cannot be negative.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateFields())
                return;

            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    conn.Open();

                    string query = @"
                        INSERT INTO routes 
                        (route_code, origin, destination, fare, estimated_duration, status)
                        VALUES
                        (@route_code, @origin, @destination, @fare, @duration, @status)";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@route_code", txtRouteID.Text.Trim());
                        cmd.Parameters.AddWithValue("@origin", txtOrigin.Text.Trim());
                        cmd.Parameters.AddWithValue("@destination", txtDestination.Text.Trim());
                        cmd.Parameters.AddWithValue("@fare", Convert.ToDecimal(txtFare.Text.Trim()));
                        cmd.Parameters.AddWithValue("@duration", txtDuration.Text.Trim());
                        cmd.Parameters.AddWithValue("@status", cmbStatus.Text);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Route added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearFields();
                LoadRoutes();
                GenerateRouteID();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding route:\n" + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRouteID.Text))
            {
                MessageBox.Show("Please select a route to update.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateFields())
                return;

            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    conn.Open();

                    string query = @"
                        UPDATE routes SET
                            origin = @origin,
                            destination = @destination,
                            fare = @fare,
                            estimated_duration = @duration,
                            status = @status
                        WHERE route_code = @route_code";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@route_code", txtRouteID.Text.Trim());
                        cmd.Parameters.AddWithValue("@origin", txtOrigin.Text.Trim());
                        cmd.Parameters.AddWithValue("@destination", txtDestination.Text.Trim());
                        cmd.Parameters.AddWithValue("@fare", Convert.ToDecimal(txtFare.Text.Trim()));
                        cmd.Parameters.AddWithValue("@duration", txtDuration.Text.Trim());
                        cmd.Parameters.AddWithValue("@status", cmbStatus.Text);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Route updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("No route found to update.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }

                ClearFields();
                LoadRoutes();
                GenerateRouteID();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating route:\n" + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDeactivate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRouteID.Text))
            {
                MessageBox.Show("Please select a route to deactivate.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show(
                "Are you sure you want to deactivate this route?",
                "Confirm Deactivate",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result != DialogResult.Yes)
                return;

            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    conn.Open();

                    string query = "UPDATE routes SET status = 'Inactive' WHERE route_code = @route_code";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@route_code", txtRouteID.Text.Trim());

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Route deactivated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("No route found to deactivate.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }

                ClearFields();
                LoadRoutes();
                GenerateRouteID();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deactivating route:\n" + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
            GenerateRouteID();
        }

        private void ClearFields()
        {
            txtOrigin.Clear();
            txtDestination.Clear();
            txtFare.Clear();
            txtDuration.Clear();
            cmbStatus.SelectedIndex = 0;
            txtOrigin.Focus();

            if (dgvRoutes != null)
            {
                dgvRoutes.ClearSelection();
            }
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
    }
}