using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sr
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            timer1.Start();
            progressbar.Value = 0;
            
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            progressbar.Value = progressbar.Value + 1;
            progressbar.Text = progressbar.Value + "%";
            if (progressbar.Value >= progressbar.Maximum)
            {
                timer1.Stop();
              
                login log = new login();
                this.Hide();
                log.Show();
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void bunifuGradientPanel1_Click(object sender, EventArgs e)
        {

        }
    }
}
