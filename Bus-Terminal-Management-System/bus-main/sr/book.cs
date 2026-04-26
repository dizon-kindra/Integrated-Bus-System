using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace sr
{
    public partial class book : Form
    {
        static int[] bookedseat;
        static int[] tempbookseat;

        private string gender = "";
        string connStr = "server=localhost;user=root;password=;database=sr_db;";

        public book()
        {
            InitializeComponent();

            bookedseat = new int[29];
            tempbookseat = new int[29];

            for (int i = 0; i < tempbookseat.Length; i++)
            {
                tempbookseat[i] = 0;
            }

            alreadybooked();
        }

        private void alreadybooked()
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr))
                {
                    string query = "SELECT * FROM bus_status ORDER BY seatno ASC";

                    MySqlDataAdapter da = new MySqlDataAdapter(query, con);
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    int rows = ds.Tables[0].Rows.Count;

                    for (int i = 0; i < rows && i < 28; i++)
                    {
                        string status = ds.Tables[0].Rows[i]["status"].ToString();

                        Button btn = this.Controls.Find("button" + (i + 1), true).FirstOrDefault() as Button;

                        if (btn == null)
                            continue;

                        if (status == "B")
                        {
                            bookedseat[i] = 1;
                            btn.BackColor = Color.Red;
                            btn.Enabled = false;
                        }
                        else
                        {
                            bookedseat[i] = 0;
                            btn.BackColor = Color.Gray;
                            btn.Enabled = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error: " + ex.Message);
            }
        }

        private void SaveSelectedSeats()
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr))
                {
                    con.Open();

                    for (int i = 0; i < 29; i++)
                    {
                        if (tempbookseat[i] == 1)
                        {
                            string query = "UPDATE bus_status SET status='B' WHERE seatno=@seatno";

                            using (MySqlCommand cmd = new MySqlCommand(query, con))
                            {
                                cmd.Parameters.AddWithValue("@seatno", i + 1);
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }

                MessageBox.Show("Seat booked successfully");
                alreadybooked();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error: " + ex.Message);
            }
        }

        private void ToggleSeat(int index, Button btn)
        {
            if (tempbookseat[index] == 0)
            {
                btn.BackColor = Color.Green;
                tempbookseat[index] = 1;
                txtseat.Text = (index + 1).ToString();
            }
            else
            {
                btn.BackColor = Color.Gray;
                tempbookseat[index] = 0;
                txtseat.Text = "";
            }
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            SaveSelectedSeats();
        }

        private void bunifuButton3_Click(object sender, EventArgs e)
        {
            string res = "";

            if (rbtmale.Checked)
                res = rbtmale.Text;
            else if (rbtfemale.Checked)
                res = rbtfemale.Text;

            if (txtseat.Text == "" || txtbno.Text == "" || txtsr.Text == "" ||
                txtdes.Text == "" || txtbt.Text == "" || txtarrivaltime.Text == "" ||
                txtprice.Text == "" || bunifuDatePicker1.Text == "" || txtnm.Text == "" ||
                txtem.Text == "" || txtage.Text == "" || txtmob.Text == "" || res == "")
            {
                MessageBox.Show("Missing information", "Not Insert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr))
                {
                    con.Open();

                    string query = @"INSERT INTO passenger
                    (s_no, b_no, b_sr, b_des, b_ty, b_ar, b_price, b_trav, p_nm, p_em, p_ag, p_mob, p_gen)
                    VALUES
                    (@s_no, @b_no, @b_sr, @b_des, @b_ty, @b_ar, @b_price, @b_trav, @p_nm, @p_em, @p_ag, @p_mob, @p_gen)";

                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@s_no", txtseat.Text.Trim());
                        cmd.Parameters.AddWithValue("@b_no", txtbno.Text.Trim());
                        cmd.Parameters.AddWithValue("@b_sr", txtsr.Text.Trim());
                        cmd.Parameters.AddWithValue("@b_des", txtdes.Text.Trim());
                        cmd.Parameters.AddWithValue("@b_ty", txtbt.Text.Trim());
                        cmd.Parameters.AddWithValue("@b_ar", txtarrivaltime.Text.Trim());
                        cmd.Parameters.AddWithValue("@b_price", txtprice.Text.Trim());
                        cmd.Parameters.AddWithValue("@b_trav", bunifuDatePicker1.Text);
                        cmd.Parameters.AddWithValue("@p_nm", txtnm.Text.Trim());
                        cmd.Parameters.AddWithValue("@p_em", txtem.Text.Trim());
                        cmd.Parameters.AddWithValue("@p_ag", txtage.Text.Trim());
                        cmd.Parameters.AddWithValue("@p_mob", txtmob.Text.Trim());
                        cmd.Parameters.AddWithValue("@p_gen", res);

                        cmd.ExecuteNonQuery();
                    }
                }

                SaveSelectedSeats();

                MessageBox.Show("Booking successfully...", "Book", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error: " + ex.Message);
            }
        }

        private void bunifuButton4_Click(object sender, EventArgs e)
        {
            if (txtseat.Text == "" || txtbno.Text == "" || txtsr.Text == "" ||
                txtdes.Text == "" || txtbt.Text == "" || txtarrivaltime.Text == "" ||
                txtprice.Text == "" || bunifuDatePicker1.Text == "" || txtnm.Text == "" ||
                txtem.Text == "" || txtage.Text == "" || txtmob.Text == "")
            {
                MessageBox.Show("Missing information", "Not Insert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            recipt rs = new recipt();

            if (rbtmale.Checked)
                gender = "male";

            if (rbtfemale.Checked)
                gender = "female";

            rs.tdate = bunifuDatePicker1.Text;
            rs.sno = txtseat.Text;
            rs.bno = txtbno.Text;
            rs.sou = txtsr.Text;
            rs.des = txtdes.Text;
            rs.bustype = txtbt.Text;
            rs.arrival = txtarrivaltime.Text;
            rs.nm = txtnm.Text;
            rs.em = txtem.Text;
            rs.ag = txtage.Text;
            rs.gen = gender;
            rs.mno = txtmob.Text;
            rs.price = txtprice.Text;

            rs.ShowDialog();
        }

        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            search s = new search();
            s.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e) { ToggleSeat(0, button1); }
        private void button2_Click(object sender, EventArgs e) { ToggleSeat(1, button2); }
        private void button3_Click(object sender, EventArgs e) { ToggleSeat(2, button3); }
        private void button4_Click(object sender, EventArgs e) { ToggleSeat(3, button4); }
        private void button5_Click(object sender, EventArgs e) { ToggleSeat(4, button5); }
        private void button6_Click(object sender, EventArgs e) { ToggleSeat(5, button6); }
        private void button7_Click(object sender, EventArgs e) { ToggleSeat(6, button7); }
        private void button8_Click(object sender, EventArgs e) { ToggleSeat(7, button8); }
        private void button9_Click(object sender, EventArgs e) { ToggleSeat(8, button9); }
        private void button10_Click(object sender, EventArgs e) { ToggleSeat(9, button10); }
        private void button11_Click(object sender, EventArgs e) { ToggleSeat(10, button11); }
        private void button12_Click(object sender, EventArgs e) { ToggleSeat(11, button12); }
        private void button13_Click(object sender, EventArgs e) { ToggleSeat(12, button13); }
        private void button14_Click(object sender, EventArgs e) { ToggleSeat(13, button14); }
        private void button15_Click(object sender, EventArgs e) { ToggleSeat(14, button15); }
        private void button16_Click(object sender, EventArgs e) { ToggleSeat(15, button16); }
        private void button17_Click(object sender, EventArgs e) { ToggleSeat(16, button17); }
        private void button18_Click(object sender, EventArgs e) { ToggleSeat(17, button18); }
        private void button19_Click(object sender, EventArgs e) { ToggleSeat(18, button19); }
        private void button20_Click(object sender, EventArgs e) { ToggleSeat(19, button20); }
        private void button21_Click(object sender, EventArgs e) { ToggleSeat(20, button21); }
        private void button22_Click(object sender, EventArgs e) { ToggleSeat(21, button22); }
        private void button23_Click(object sender, EventArgs e) { ToggleSeat(22, button23); }
        private void button24_Click(object sender, EventArgs e) { ToggleSeat(23, button24); }
        private void button25_Click(object sender, EventArgs e) { ToggleSeat(24, button25); }
        private void button26_Click(object sender, EventArgs e) { ToggleSeat(25, button26); }
        private void button27_Click(object sender, EventArgs e) { ToggleSeat(26, button27); }
        private void button28_Click(object sender, EventArgs e) { ToggleSeat(27, button28); }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("Enter only numbers");
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("Enter only numbers");
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void txtseat_TextChanged(object sender, EventArgs e) { }
        private void bunifuButton202_Click(object sender, EventArgs e) { }
        private void textBox2_TextChanged(object sender, EventArgs e) { }
        private void button29_Click(object sender, EventArgs e) { }
        private void label12_Click(object sender, EventArgs e) { }
        private void bunifuGradientPanel1_Click(object sender, EventArgs e) { }
        private void book_Load(object sender, EventArgs e) { }
    }
}