﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace FoodStuffsController.gui.MessageBoxes
{
    class PopupBoxes
    {
        public static void ShowError(string error = "Error.", string message = "An unexpected error has occured.", MessageBoxIcon icon = MessageBoxIcon.Error)
        {
            MessageBox.Show(message, error, MessageBoxButtons.OK, icon);
        }


        public static DialogResult InputBox(string title, string promptText, ref string value)
        {
            InputDialog id = new InputDialog(title, promptText);
            DialogResult dialogResult = id.ShowDialog();
            value = id.getValue();
            return dialogResult;
        }

    }
}
