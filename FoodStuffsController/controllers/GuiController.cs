using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace FoodStuffsController.controllers
{
    /// <summary>
    /// Used to control external GUI functions (E.G. CRUD operations)
    /// </summary>
    static class GuiController
    {


        public static void LaunchSupervisor()
        {
            Application.Run(new SupervisorGUI());
        }

        public static void LaunchController()
        {
            Application.Run(new ControllerGUI());
        }
    }
}
