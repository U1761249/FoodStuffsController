using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

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
            value = id.getValue();
            return dialogResult;
        }

    }
}
