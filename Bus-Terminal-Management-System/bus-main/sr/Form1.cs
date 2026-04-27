using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace sr
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            progressbar.Value = 0;
            progressbar.Text = "0%";
            lblLoading.Text = "Preparing system...";

            pictureBoxBus.Image = CreateBusTerminalImage(pictureBoxBus.Width, pictureBoxBus.Height);

            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (progressbar.Value < progressbar.Maximum)
            {
                progressbar.Value += 1;
                progressbar.Text = progressbar.Value + "%";

                if (progressbar.Value < 30)
                {
                    lblLoading.Text = "Loading database connection...";
                }
                else if (progressbar.Value < 60)
                {
                    lblLoading.Text = "Loading terminal modules...";
                }
                else if (progressbar.Value < 90)
                {
                    lblLoading.Text = "Preparing dashboard...";
                }
                else
                {
                    lblLoading.Text = "Almost ready...";
                }
            }

            if (progressbar.Value >= progressbar.Maximum)
            {
                timer1.Stop();

                login log = new login();
                this.Hide();
                log.Show();
            }
        }

        private Image CreateBusTerminalImage(int width, int height)
        {
            Bitmap bmp = new Bitmap(width, height);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;

                using (LinearGradientBrush bg = new LinearGradientBrush(
                    new Rectangle(0, 0, width, height),
                    Color.FromArgb(235, 242, 250),
                    Color.FromArgb(210, 228, 245),
                    LinearGradientMode.Vertical))
                {
                    g.FillRectangle(bg, 0, 0, width, height);
                }

                // Sun / light
                using (SolidBrush sunBrush = new SolidBrush(Color.FromArgb(255, 215, 95)))
                {
                    g.FillEllipse(sunBrush, width - 110, 35, 55, 55);
                }

                // Terminal building shadow
                using (SolidBrush shadow = new SolidBrush(Color.FromArgb(55, 0, 0, 0)))
                {
                    g.FillRectangle(shadow, 52, 90, 365, 145);
                }

                // Terminal building
                using (SolidBrush building = new SolidBrush(Color.FromArgb(32, 45, 64)))
                {
                    g.FillRectangle(building, 45, 80, 365, 145);
                }

                // Building roof
                using (SolidBrush roof = new SolidBrush(Color.FromArgb(52, 152, 219)))
                {
                    Point[] roofPoints =
                    {
                        new Point(25, 80),
                        new Point(225, 25),
                        new Point(430, 80)
                    };

                    g.FillPolygon(roof, roofPoints);
                }

                // Terminal label board
                using (SolidBrush board = new SolidBrush(Color.White))
                {
                    g.FillRoundedRectangle(board, 135, 92, 185, 38, 10);
                }

                using (Font font = new Font("Segoe UI", 14, FontStyle.Bold))
                using (SolidBrush textBrush = new SolidBrush(Color.FromArgb(32, 45, 64)))
                {
                    StringFormat sf = new StringFormat();
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Center;

                    g.DrawString("BUS TERMINAL", font, textBrush, new RectangleF(135, 92, 185, 38), sf);
                }

                // Windows
                using (SolidBrush window = new SolidBrush(Color.FromArgb(180, 220, 245)))
                {
                    g.FillRoundedRectangle(window, 75, 145, 55, 55, 8);
                    g.FillRoundedRectangle(window, 150, 145, 55, 55, 8);
                    g.FillRoundedRectangle(window, 255, 145, 55, 55, 8);
                    g.FillRoundedRectangle(window, 330, 145, 55, 55, 8);
                }

                // Road
                using (SolidBrush road = new SolidBrush(Color.FromArgb(58, 66, 78)))
                {
                    g.FillRectangle(road, 0, height - 95, width, 95);
                }

                // Road line
                using (Pen roadLine = new Pen(Color.White, 4))
                {
                    roadLine.DashPattern = new float[] { 18, 15 };
                    g.DrawLine(roadLine, 15, height - 48, width - 15, height - 48);
                }

                // Bus shadow
                using (SolidBrush busShadow = new SolidBrush(Color.FromArgb(70, 0, 0, 0)))
                {
                    g.FillEllipse(busShadow, 90, height - 98, 320, 30);
                }

                // Bus body
                using (SolidBrush busBody = new SolidBrush(Color.FromArgb(52, 152, 219)))
                {
                    g.FillRoundedRectangle(busBody, 75, height - 190, 335, 105, 20);
                }

                // Bus front
                using (SolidBrush busFront = new SolidBrush(Color.FromArgb(41, 128, 185)))
                {
                    g.FillRoundedRectangle(busFront, 320, height - 190, 90, 105, 20);
                }

                // Bus windows
                using (SolidBrush glass = new SolidBrush(Color.FromArgb(220, 245, 255)))
                {
                    g.FillRoundedRectangle(glass, 100, height - 170, 55, 38, 8);
                    g.FillRoundedRectangle(glass, 165, height - 170, 55, 38, 8);
                    g.FillRoundedRectangle(glass, 230, height - 170, 55, 38, 8);
                    g.FillRoundedRectangle(glass, 330, height - 170, 50, 38, 8);
                }

                // Bus lower stripe
                using (SolidBrush stripe = new SolidBrush(Color.White))
                {
                    g.FillRectangle(stripe, 90, height - 125, 285, 10);
                }

                // Bus door
                using (Pen doorPen = new Pen(Color.White, 3))
                {
                    g.DrawLine(doorPen, 310, height - 178, 310, height - 95);
                }

                // Headlight
                using (SolidBrush light = new SolidBrush(Color.FromArgb(255, 240, 120)))
                {
                    g.FillEllipse(light, 385, height - 120, 18, 12);
                }

                // Wheels
                using (SolidBrush wheel = new SolidBrush(Color.FromArgb(25, 25, 25)))
                {
                    g.FillEllipse(wheel, 125, height - 105, 55, 55);
                    g.FillEllipse(wheel, 310, height - 105, 55, 55);
                }

                using (SolidBrush rim = new SolidBrush(Color.FromArgb(210, 215, 220)))
                {
                    g.FillEllipse(rim, 140, height - 90, 25, 25);
                    g.FillEllipse(rim, 325, height - 90, 25, 25);
                }

                // Decorative text
                using (Font smallFont = new Font("Segoe UI", 10, FontStyle.Bold))
                using (SolidBrush white = new SolidBrush(Color.White))
                {
                    g.DrawString("BTMS EXPRESS", smallFont, white, 105, height - 118);
                }
            }

            return bmp;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pictureBoxBus_Click(object sender, EventArgs e)
        {

        }

        private void bunifuGradientPanel1_Click(object sender, EventArgs e)
        {

        }

        private void progressbar_Click(object sender, EventArgs e)
        {

        }
    }

    public static class GraphicsExtensions
    {
        public static void FillRoundedRectangle(this Graphics g, Brush brush, int x, int y, int width, int height, int radius)
        {
            using (GraphicsPath path = new GraphicsPath())
            {
                int diameter = radius * 2;

                path.AddArc(x, y, diameter, diameter, 180, 90);
                path.AddArc(x + width - diameter, y, diameter, diameter, 270, 90);
                path.AddArc(x + width - diameter, y + height - diameter, diameter, diameter, 0, 90);
                path.AddArc(x, y + height - diameter, diameter, diameter, 90, 90);
                path.CloseFigure();

                g.FillPath(brush, path);
            }
        }
    }
}