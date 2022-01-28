using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace FoodStuffsController.controllers
{
    class GuiController
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
