using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using FoodStuffsController.src;

namespace FoodStuffsController.gui.MessageBoxes
{
    class PopupBoxes
    {
        /// <summary>
        /// Show an error message if something happened.
        /// </summary>
        /// <param name="error"></param>
        /// <param name="message"></param>
        /// <param name="icon"></param>
        public static void ShowError(string error = "Error.", string message = "An unexpected error has occured.", MessageBoxIcon icon = MessageBoxIcon.Error)
        {
            MessageBox.Show(message, error, MessageBoxButtons.OK, icon);
        }

        /// <summary>
        /// Display and populate an InputBox.
        /// Use a reference to the value to pull the data out.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="promptText"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DialogResult InputBox(string title, string promptText, ref string value)
        {
            InputDialog id = new InputDialog(title, promptText);
            DialogResult dialogResult = id.ShowDialog();
            value = id.GetValue();
            return dialogResult;
        }

        public static DialogResult MakeBatch(ref KeyValuePair<string, double> value, List<string> recipes)
        {
            BatchDialog bd = new BatchDialog(recipes);
            DialogResult dialogResult = bd.ShowDialog();
            value = bd.getValue();
            return dialogResult;
        }

        public static DialogResult NewRecipe(ref Recipe r)
        {
            NewRecipe nr = new NewRecipe();
            DialogResult dialogResult = nr.ShowDialog();
            r = nr.GetRecipe();
            return dialogResult;
        }

        public static DialogResult ChangeProduct(ref string value)
        {
            ChangeProduct cp = new ChangeProduct();
            DialogResult dialogResult = cp.ShowDialog();
            value = cp.GetValue();
            return dialogResult;
        }

    }
}
