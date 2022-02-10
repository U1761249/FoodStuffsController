using FoodStuffsController.src;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodStuffsController.controllers
{
    /// <summary>
    /// Singleton controller for the supervisor users.
    /// </summary>
    class SupervisorGuiController
    {

        private FeedBinController controller;

        private static SupervisorGuiController instance;
        public static SupervisorGuiController getInstance()
        {
            if (instance == null) instance = new SupervisorGuiController();
            return instance;
        }
        private SupervisorGuiController() 
        {
            controller = FeedBinController.getInstance();
        }

        public DataTable getRecipeDataTable() { return controller.getRecipeDataTable(); }


        public void newRecipe() { }

        public void batch() { }

    }
}
