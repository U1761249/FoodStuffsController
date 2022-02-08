using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodStuffsController.controllers
{
    /// <summary>
    /// Singleton controller for the controller users.
    /// </summary>
    class ControllerGuiController
    {
        private static ControllerGuiController instance;
        public static ControllerGuiController getInstance()
        {
            if (instance == null) instance = new FeedBinController();
            return instance;
        }
        private ControllerGuiController() { }


    }
}
