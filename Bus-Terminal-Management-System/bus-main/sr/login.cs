using System;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace sr
{
    public partial class login : Form
    {
        private string connStr = "server=localhost;user=root;password=;database=sr_db;";

        public login()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            // No action needed
        }

        //private void login_Load(object sender, EventArgs e)
        // {
        //  panel1.BackColor = Color.White;

        // Make password hidden by default
        //  textBox2.UseSystemPasswordChar = true;
        // }
        private void login_Load(object sender, EventArgs e)
        {
            panel1.BackColor = Color.White;
            textBox2.UseSystemPasswordChar = true;

            
        }
        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text.Trim();
            string password = textBox2.Text.Trim();

            if (username == "" || password == "")
            {
                MessageBox.Show("Please enter username and password.", "Login Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string connStr = "server=localhost;user=root;password=;database=sr_db;";

            using (MySqlConnection con = new MySqlConnection(connStr))
            {
                try
                {
                    con.Open();

                    string query = "SELECT pass FROM login WHERE uname = @uname LIMIT 1";

                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@uname", username);

                        object result = cmd.ExecuteScalar();

                        if (result == null || result == DBNull.Value)
                        {
                            MessageBox.Show("Incorrect username or password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        string storedHash = result.ToString();

                        bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(password, storedHash);

                        if (isPasswordCorrect)
                        {
                            MessageBox.Show("Login successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            AdminDashboardForm dashboard = new AdminDashboardForm();
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
            if (bunifuCheckBox1.Checked)
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
            DialogResult result = MessageBox.Show(
                "Are you sure you want to exit?",
                "Exit",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}