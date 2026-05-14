using System;
using System.Drawing;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace sr
{
    public partial class login : Form
    {
        private readonly HttpClient client = new HttpClient();

        public login()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            // No action needed
        }

        private void login_Load(object sender, EventArgs e)
        {
            panel1.BackColor = Color.White;
            textBox2.UseSystemPasswordChar = true;
        }

        private async void bunifuButton1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text.Trim();
            string password = textBox2.Text.Trim();

            if (username == "" || password == "")
            {
                MessageBox.Show(
                    "Please enter username and password.",
                    "Login Required",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            try
            {
                string apiUrl = "http://localhost:3000/api/admin/login";

                var loginData = new
                {
                    username = username,
                    password = password
                };

                string json = JsonConvert.SerializeObject(loginData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                string responseBody = await response.Content.ReadAsStringAsync();

                dynamic result = JsonConvert.DeserializeObject(responseBody);

                if (response.IsSuccessStatusCode && result.success == true)
                {
                    MessageBox.Show(
                        "Login successfully",
                        "Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );

                    AdminDashboardForm dashboard = new AdminDashboardForm();
                    dashboard.Show();

                    this.Hide();
                }
                else
                {
                    string message = "Incorrect username or password.";

                    if (result != null && result.message != null)
                    {
                        message = result.message.ToString();
                    }

                    MessageBox.Show(
                        message,
                        "Login Failed",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Cannot connect to the Node API.\n\nPlease make sure:\n" +
                    "1. XAMPP MySQL is running\n" +
                    "2. Node API is running using npm start\n" +
                    "3. API URL is http://localhost:3000/api/admin/login\n\n" +
                    "Error: " + ex.Message,
                    "Connection Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
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