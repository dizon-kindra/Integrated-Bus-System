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
    public partial class recipt : Form
    {
        public string tdate, sno,bno, sou, des, bustype,arrival,nm,em,ag,gen,mno,price;

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        public Image img = null;

        private void recipt_Load(object sender, EventArgs e)
        {
            labeldate.Text = tdate;
            lblseat.Text = sno;
            lblbno.Text = bno;
            lblsou.Text = sou;
            lbldes.Text = des;
            lblbt.Text = bustype;
            lblat.Text = arrival;
            lblnm.Text = nm;
            lblemail.Text = em;
            lblage.Text = ag;
           lblgender.Text = gen;
            lblmobile.Text = mno;
            lblprice.Text = price;

        }

        private void bunifuImageButton1_MouseHover(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(bunifuImageButton1, "print");
        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            print(this.panel1);
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Rectangle pagearea = e.PageBounds;
            e.Graphics.DrawImage(memorying, (pagearea.Width / 4) - (this.panel1.Width / 4), this.panel1.Location.Y);
        }

        
        public recipt()
        {
            InitializeComponent();
            tdate = DateTime.Now.ToString("dd/MM/yyyy");
        }
        private void print(Panel pnl)
        {
            //            printerSettings ps== new printerSettings();
            panel1 = pnl;
            getprintarea(pnl);
            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.ShowDialog();
        }
        private Bitmap memorying;
        private void getprintarea(Panel pnl)
        {
            memorying = new Bitmap(pnl.Width, pnl.Height);
            pnl.DrawToBitmap(memorying, new Rectangle(2, 2, pnl.Width, pnl.Height));
        }

    }
}
