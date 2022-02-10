using FoodStuffsController.gui.MessageBoxes;
using FoodStuffsController.src;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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


        public void newRecipe() 
        {
            
        }

        public void batch() 
        { 
        KeyValuePair<string, double> batchInfo = new KeyValuePair<string, double>("", 0);
            if (PopupBoxes.MakeBatch(ref batchInfo, controller.getRecipeList()) == DialogResult.OK) 
            {
                PopupBoxes.ShowError(batchInfo.Value.ToString(), batchInfo.Key, MessageBoxIcon.Information);
            }
        }

    }
}
