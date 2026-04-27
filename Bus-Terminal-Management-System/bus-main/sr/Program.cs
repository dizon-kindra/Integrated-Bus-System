using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sr
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            //Application.Run(new AdminDashboardForm());
           // Application.Run(new login());
            //Application.Run(new RouteManagementForm());
            //Application.Run(new BusManagementForm());
            // Application.Run(new ScheduleManagementForm());
            // Application.Run(new ReservationManagementForm());
            // Application.Run(new PaymentConfirmationForm());
            //Application.Run(new CheckInBoardingForm());
            //Application.Run(new ReportsForm());
            //Application.Run(new DashboardForm());
        }
    }
}
