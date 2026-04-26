using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace sr
{
    public partial class res : Form
    {
        string connStr = "server=localhost;user=root;password=;database=sr_db;";

        public res()
        {
            InitializeComponent();
            show();
        }

        private void show()
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr))
                {
                    con.Open();

                    string query = "SELECT * FROM passenger";

                    MySqlDataAdapter da = new MySqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    bunifuDataGridView1.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error: " + ex.Message);
            }
        }

        private void bunifuDataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            show();
        }

        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            admin a = new admin();
            a.Show();
            this.Hide();
        }

        private void bunifuGradientPanel1_Click(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void bunifuButton3_Click(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
    }
}