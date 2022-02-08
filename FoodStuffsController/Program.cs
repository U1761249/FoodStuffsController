using FoodStuffsController.controllers;
using FoodStuffsController.src;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoodStuffsController
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            FeedBinController controller = FeedBinController.getInstance();

            Thread t1 = new Thread(GuiController.LaunchController);
            t1.Start();

            Thread t2 = new Thread(GuiController.LaunchSupervisor);
            t2.Start();
        }


    }
}
