using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace sr
{
    public partial class bus : Form
    {
        string connStr = "server=localhost;user=root;password=;database=sr_db;";
        int key = 0;

        public bus()
        {
            InitializeComponent();
            pop();
        }

        private void pop()
        {
            using (MySqlConnection con = new MySqlConnection(connStr))
            {
                con.Open();

                string query = "SELECT * FROM bus_add";

                MySqlDataAdapter da = new MySqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                bunifuDataGridView1.DataSource = dt;
            }
        }

        private void btnadd_Click(object sender, EventArgs e)
        {
            if (txtbno.Text == "" || txtsr.Text == "" || txtd.Text == "" ||
                comboBox1.SelectedIndex == -1 || txtarrival.Text == "" || txtp.Text == "")
            {
                MessageBox.Show("Missing information", "Not Insert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr))
                {
                    con.Open();

                    string query = @"INSERT INTO bus_add 
                    (b_no, b_sou, b_des, b_ty, b_time, b_price) 
                    VALUES (@b_no, @b_sou, @b_des, @b_ty, @b_time, @b_price)";

                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@b_no", txtbno.Text.Trim());
                        cmd.Parameters.AddWithValue("@b_sou", txtsr.Text.Trim());
                        cmd.Parameters.AddWithValue("@b_des", txtd.Text.Trim());
                        cmd.Parameters.AddWithValue("@b_ty", comboBox1.Text.Trim());
                        cmd.Parameters.AddWithValue("@b_time", txtarrival.Text.Trim());
                        cmd.Parameters.AddWithValue("@b_price", txtp.Text.Trim());

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Bus added successfully");
                pop();
                reset();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error: " + ex.Message);
            }
        }

        private void reset()
        {
            txtbno.Text = "";
            txtsr.Text = "";
            txtd.Text = "";
            comboBox1.Text = "";
            txtarrival.Text = "";
            txtp.Text = "";
            key = 0;
        }

        private void bunifuDataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (bunifuDataGridView1.SelectedRows.Count == 0)
                return;

            txtbno.Text = bunifuDataGridView1.SelectedRows[0].Cells["b_no"].Value.ToString();
            txtsr.Text = bunifuDataGridView1.SelectedRows[0].Cells["b_sou"].Value.ToString();
            txtd.Text = bunifuDataGridView1.SelectedRows[0].Cells["b_des"].Value.ToString();
            comboBox1.Text = bunifuDataGridView1.SelectedRows[0].Cells["b_ty"].Value.ToString();
            txtarrival.Text = bunifuDataGridView1.SelectedRows[0].Cells["b_time"].Value.ToString();
            txtp.Text = bunifuDataGridView1.SelectedRows[0].Cells["b_price"].Value.ToString();

            key = Convert.ToInt32(bunifuDataGridView1.SelectedRows[0].Cells["ID"].Value);
        }

        private void btnremove_Click(object sender, EventArgs e)
        {
            if (key == 0)
            {
                MessageBox.Show("Select bus to delete", "Bus", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr))
                {
                    con.Open();

                    string query = "DELETE FROM bus_add WHERE ID=@ID";

                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@ID", key);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Bus removed successfully", "Bus", MessageBoxButtons.OK, MessageBoxIcon.Information);
                pop();
                reset();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error: " + ex.Message);
            }
        }

        private void btnedit_Click(object sender, EventArgs e)
        {
            if (key == 0)
            {
                MessageBox.Show("Select bus first", "Bus", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (txtbno.Text == "" || txtsr.Text == "" || txtd.Text == "" ||
                comboBox1.SelectedIndex == -1 || txtarrival.Text == "" || txtp.Text == "")
            {
                MessageBox.Show("Missing information", "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr))
                {
                    con.Open();

                    string query = @"UPDATE bus_add SET 
                    b_no=@b_no,
                    b_sou=@b_sou,
                    b_des=@b_des,
                    b_ty=@b_ty,
                    b_time=@b_time,
                    b_price=@b_price
                    WHERE ID=@ID";

                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@b_no", txtbno.Text.Trim());
                        cmd.Parameters.AddWithValue("@b_sou", txtsr.Text.Trim());
                        cmd.Parameters.AddWithValue("@b_des", txtd.Text.Trim());
                        cmd.Parameters.AddWithValue("@b_ty", comboBox1.Text.Trim());
                        cmd.Parameters.AddWithValue("@b_time", txtarrival.Text.Trim());
                        cmd.Parameters.AddWithValue("@b_price", txtp.Text.Trim());
                        cmd.Parameters.AddWithValue("@ID", key);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Bus updated successfully", "Bus", MessageBoxButtons.OK, MessageBoxIcon.Information);
                pop();
                reset();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error: " + ex.Message);
            }
        }

        private void btnreset_Click(object sender, EventArgs e)
        {
            reset();
        }

        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            home h = new home();
            h.Show();
            this.Hide();
        }

        private void bus_Load(object sender, EventArgs e) { }
        private void panel1_Paint(object sender, PaintEventArgs e) { }
        private void bunifuGradientPanel1_Click(object sender, EventArgs e) { }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) { }
    }
}