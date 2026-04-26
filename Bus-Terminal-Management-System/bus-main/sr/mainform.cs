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
    public partial class mainform : Form
    {
        public mainform()
        {
            InitializeComponent();
        }

        private void homeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool IsOpen = false;
            foreach(Form m in Application.OpenForms)
            {
                if(m.Text=="home")
                {
                    IsOpen = true;
                    m.Focus();
                    break;
                }
            }
            if(IsOpen==false)
            {
                home h = new home();
                h.MdiParent = this;
                h.Show();
                

            }
            

        }

        private void aboutUsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool IsOpen = false;
            foreach (Form m in Application.OpenForms)
            {
                if (m.Text == "about")
                {
                    IsOpen = true;
                    m.Focus();
                    break;
                }
            }
            if (IsOpen == false)
            {
                about a = new about();
                a.MdiParent = this;
                a.Show();
            }
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool IsOpen = false;
            foreach (Form m in Application.OpenForms)
            {
                if (m.Text == "help")
                {
                    IsOpen = true;
                    m.Focus();
                    break;
                }
            }
            if (IsOpen == false)
            {
                helps h = new helps();
                h.MdiParent = this;
                h.Show();
            }
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void mainform_Load(object sender, EventArgs e)
        {

        }
    }
}
