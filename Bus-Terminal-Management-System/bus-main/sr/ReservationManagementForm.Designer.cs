namespace sr
{
    partial class ReservationManagementForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.GroupBox groupBoxInfo;
        private System.Windows.Forms.GroupBox groupBoxList;

        private System.Windows.Forms.Label lblBookingID;
        private System.Windows.Forms.TextBox txtBookingID;

        private System.Windows.Forms.Label lblPassengerName;
        private System.Windows.Forms.TextBox txtPassengerName;

        private System.Windows.Forms.Label lblPhone;
        private System.Windows.Forms.TextBox txtPhone;

        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox txtEmail;

        private System.Windows.Forms.Label lblSchedule;
        private System.Windows.Forms.ComboBox cmbSchedule;

        private System.Windows.Forms.Label lblSeatNo;
        private System.Windows.Forms.TextBox txtSeatNo;

        private System.Windows.Forms.Label lblPaymentStatus;
        private System.Windows.Forms.ComboBox cmbPaymentStatus;

        private System.Windows.Forms.Label lblReservationStatus;
        private System.Windows.Forms.ComboBox cmbReservationStatus;

        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.Button btnCancelBooking;
        private System.Windows.Forms.Button btnClear;

        private System.Windows.Forms.DataGridView dgvBookings;

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
            // 
            // ReservationManagementForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "ReservationManagementForm";
            this.Load += new System.EventHandler(this.ReservationManagementForm_Load);
            this.ResumeLayout(false);

        }
    }
}