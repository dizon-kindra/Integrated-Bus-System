using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace sr
{
    public partial class BusManagementForm : Form
    {
        private string connStr = "server=localhost;user=root;password=;database=sr_db;";

        public BusManagementForm()
        {
            InitializeComponent();
        }

        private void BusManagementForm_Load(object sender, EventArgs e)
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

            LoadBuses();
        }

        private void LoadBuses()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    string query = @"
                        SELECT 
                            bus_id,
                            bus_number,
                            plate_number,
                            capacity,
                            bus_type,
                            status
                        FROM buses
                        ORDER BY bus_id DESC";

                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

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
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading buses: " + ex.Message);
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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateFields())
            {
                return;
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    string query = @"
                        INSERT INTO buses 
                        (
                            bus_number, 
                            plate_number, 
                            capacity, 
                            bus_type, 
                            status
                        )
                        VALUES
                        (
                            @bus_number, 
                            @plate_number, 
                            @capacity, 
                            @bus_type, 
                            @status
                        )";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@bus_number", txtBusNumber.Text.Trim());
                        cmd.Parameters.AddWithValue("@plate_number", txtPlateNumber.Text.Trim());
                        cmd.Parameters.AddWithValue("@capacity", Convert.ToInt32(txtCapacity.Text.Trim()));
                        cmd.Parameters.AddWithValue("@bus_type", cmbBusType.Text.Trim());
                        cmd.Parameters.AddWithValue("@status", cmbStatus.Text.Trim());

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Bus added successfully.");
                LoadBuses();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding bus: " + ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
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
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    string query = @"
                        UPDATE buses SET
                            bus_number = @bus_number,
                            plate_number = @plate_number,
                            capacity = @capacity,
                            bus_type = @bus_type,
                            status = @status
                        WHERE bus_id = @bus_id";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@bus_id", Convert.ToInt32(txtBusID.Text.Trim()));
                        cmd.Parameters.AddWithValue("@bus_number", txtBusNumber.Text.Trim());
                        cmd.Parameters.AddWithValue("@plate_number", txtPlateNumber.Text.Trim());
                        cmd.Parameters.AddWithValue("@capacity", Convert.ToInt32(txtCapacity.Text.Trim()));
                        cmd.Parameters.AddWithValue("@bus_type", cmbBusType.Text.Trim());
                        cmd.Parameters.AddWithValue("@status", cmbStatus.Text.Trim());

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Bus updated successfully.");
                LoadBuses();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating bus: " + ex.Message);
            }
        }

        private void btnDeactivate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBusID.Text))
            {
                MessageBox.Show("Please select a bus to deactivate.");
                return;
            }

            DialogResult result = MessageBox.Show(
                "Are you sure you want to deactivate this bus?",
                "Confirm Deactivate",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.No)
            {
                return;
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    string query = "UPDATE buses SET status = 'Inactive' WHERE bus_id = @bus_id";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@bus_id", Convert.ToInt32(txtBusID.Text.Trim()));
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Bus deactivated successfully.");
                LoadBuses();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deactivating bus: " + ex.Message);
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