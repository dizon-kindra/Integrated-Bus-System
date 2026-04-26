using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
namespace sr
{
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
            
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void login_Load(object sender, EventArgs e)
        {
            panel1.BackColor = Color.White;
        }
        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            string connStr = "server=localhost;user=root;password=;database=sr_db;";

            using (MySqlConnection con = new MySqlConnection(connStr))
            {
                try
                {
                    con.Open();

                    string query = "SELECT COUNT(*) FROM login WHERE uname=@uname AND pass=@pass";

                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@uname", textBox1.Text.Trim());
                        cmd.Parameters.AddWithValue("@pass", textBox2.Text.Trim());

                        int count = Convert.ToInt32(cmd.ExecuteScalar());

                        if (count > 0)
                        {
                            MessageBox.Show("Login successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            DashboardForm dashboard = new DashboardForm();
                            dashboard.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Incorrect username or password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Database Error: " + ex.Message);
                }
            }
        }
        private void bunifuCheckBox1_CheckedChanged(object sender, Bunifu.UI.WinForms.BunifuCheckBox.CheckedChangedEventArgs e)
        {
            if(bunifuCheckBox1.Checked)
            {
                textBox2.UseSystemPasswordChar = false;
            }
            else
            {
                textBox2.UseSystemPasswordChar = true;
            }
        }

        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

       
    }
    }
