using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace sr
{
    public partial class time_table : Form
    {
        string connStr = "server=localhost;user=root;password=;database=sr_db;";

        public time_table()
        {
            InitializeComponent();
            getdata();
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

        private void btnswap_Click(object sender, EventArgs e)
        {
            string no1 = txt1.Text;
            string no2 = txt2.Text;

            txt1.Text = no2;
            txt2.Text = no1;
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            home h = new home();
            h.Show();
            this.Hide();
        }

        private void bunifuGradientPanel1_Click(object sender, EventArgs e) { }
    }
}