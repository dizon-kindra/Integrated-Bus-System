namespace sr
{
    partial class CheckInBoardingForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 760);
            this.Name = "CheckInBoardingForm";
            this.Text = "Check-in / Boarding";
            this.Load += new System.EventHandler(this.CheckInBoardingForm_Load);

            this.ResumeLayout(false);
        }
    }
}