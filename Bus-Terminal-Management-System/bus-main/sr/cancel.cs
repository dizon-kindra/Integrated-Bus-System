using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace sr
{
    public partial class cancel : Form
    {
        string connStr = "server=localhost;user=root;password=;database=sr_db;";

        public cancel()
        {
            InitializeComponent();
        }

        private void bunifuButton3_Click(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr))
                {
                    con.Open();

                    string query = "SELECT * FROM passenger WHERE p_nm=@name AND p_em=@email";

                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@name", textBox1.Text.Trim());
                        cmd.Parameters.AddWithValue("@email", textBox2.Text.Trim());

                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        bunifuDataGridView1.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error: " + ex.Message);
            }
        }

        private void bunifuDataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            DialogResult res = MessageBox.Show(
                "Are you sure want to delete record?",
                "Delete Record",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (res != DialogResult.Yes)
                return;

            try
            {
                int seatNo = Convert.ToInt32(bunifuDataGridView1.Rows[e.RowIndex].Cells["s_no"].Value);

                using (MySqlConnection con = new MySqlConnection(connStr))
                {
                    con.Open();

                    string deleteQuery = "DELETE FROM passenger WHERE s_no=@s_no";

                    using (MySqlCommand cmd = new MySqlCommand(deleteQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@s_no", seatNo);
                        cmd.ExecuteNonQuery();
                    }

                    string updateSeatQuery = "UPDATE bus_status SET status='A' WHERE seatno=@seatno";

                    using (MySqlCommand cmd = new MySqlCommand(updateSeatQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@seatno", seatNo);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Booking cancelled successfully");

                bunifuButton3_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error: " + ex.Message);
            }
        }

        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            home h = new home();
            h.ShowDialog();
            this.Hide();
        }

        private void bunifuGradientPanel1_Click(object sender, EventArgs e) { }
        private void textBox2_TextChanged(object sender, EventArgs e) { }
    }
}