using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace sr
{
    public partial class search : Form
    {
        string connStr = "server=localhost;user=root;password=;database=sr_db;";

        public search()
        {
            InitializeComponent();
        }

        void getdata()
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr))
                {
                    con.Open();

                    string query = "SELECT * FROM bus_add";

                    MySqlDataAdapter dp = new MySqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    dp.Fill(dt);

                    bunifuDataGridView1.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error: " + ex.Message);
            }
        }

        private void btnsearch_Click(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr))
                {
                    con.Open();

                    string query = "SELECT * FROM bus_add WHERE b_sou=@source AND b_des=@destination";

                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@source", txt1.Text.Trim());
                        cmd.Parameters.AddWithValue("@destination", txt2.Text.Trim());

                        MySqlDataAdapter dp = new MySqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        dp.Fill(dt);

                        bunifuDataGridView1.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error: " + ex.Message, "MySQL Connect", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void bunifuDataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (bunifuDataGridView1.CurrentRow == null)
                return;

            book b = new book();

            b.txtbno.Text = bunifuDataGridView1.CurrentRow.Cells["b_no"].Value.ToString();
            b.txtsr.Text = bunifuDataGridView1.CurrentRow.Cells["b_sou"].Value.ToString();
            b.txtdes.Text = bunifuDataGridView1.CurrentRow.Cells["b_des"].Value.ToString();
            b.txtbt.Text = bunifuDataGridView1.CurrentRow.Cells["b_ty"].Value.ToString();
            b.txtarrivaltime.Text = bunifuDataGridView1.CurrentRow.Cells["b_time"].Value.ToString();
            b.txtprice.Text = bunifuDataGridView1.CurrentRow.Cells["b_price"].Value.ToString();

            b.Show();
            this.Hide();
        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            string no1 = txt1.Text;
            string no2 = txt2.Text;

            txt1.Text = no2;
            txt2.Text = no1;
        }

        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            home h = new home();
            h.Show();
            this.Hide();
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void bunifuDataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void panel1_Paint(object sender, PaintEventArgs e) { }
    }
}