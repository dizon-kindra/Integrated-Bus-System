using System;
using System.Windows.Forms;

namespace sr
{
    public partial class home : Form
    {
        public home()
        {
            InitializeComponent();
        }



        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        
        private void bunifuImageButton1_Click_2(object sender, EventArgs e)
        {
            bus b = new bus();
            b.Show();
            this.Hide();
        }

        private void bunifuImageButton2_Click(object sender, EventArgs e)
        {
            search s = new search();
            s.Show();
            this.Hide();
        }

        private void bunifuImageButton4_Click(object sender, EventArgs e)
        {
            time_table tn = new time_table();
            tn.Show();
            this.Hide();
      }

        private void bunifuImageButton5_Click(object sender, EventArgs e)
        {
            login lgn = new login();
            lgn.Show();
            this.Hide();
        }

        private void bunifuImageButton3_Click(object sender, EventArgs e)
        {
            cancel ca = new cancel();
            ca.Show();
            this.Hide();
        }
    }
}
