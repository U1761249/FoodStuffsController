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
            controller = FeedBinController.GetInstance();
        }

        /// <summary>
        /// Get information about the recipes in the list in the form of a DataTable.
        /// </summary>
        /// <returns></returns>
        public DataTable GetRecipeDataTable() { return controller.GetRecipeDataTable(); }

        /// <summary>
        /// Define a new recipe.
        /// </summary>
        public void NewRecipe()
        {
            Recipe r = new Recipe();
            if (PopupBoxes.NewRecipe(ref r) == DialogResult.OK)
            {
                controller.AddNewRecipe(r);
            }
        }

        /// <summary>
        /// Make a batch.
        /// </summary>
        public void Batch()
        {
            KeyValuePair<string, double> batchInfo = new KeyValuePair<string, double>("", 0);
            if (PopupBoxes.MakeBatch(ref batchInfo, controller.GetRecipeList()) == DialogResult.OK)
            {
                if (batchInfo.Value <= 0)
                {
                    PopupBoxes.ShowError("Invalid Argument", "The specified quanitiy to make was invalid");
                    return;
                }

                string message = "";
                if (controller.CanMake(batchInfo.Key, batchInfo.Value, ref message))
                {
                    bool success = controller.MakeBatch(batchInfo.Key, batchInfo.Value);
                    if (!success) PopupBoxes.ShowError();
                }
                else PopupBoxes.ShowError("Invalid Batch", message, MessageBoxIcon.Information);
            }
        }

    }
}
