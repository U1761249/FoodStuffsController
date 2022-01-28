using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FoodStuffsController.gui.MessageBoxes
{
    public partial class InputDialog : Form
    {
        string value;

        public string getValue() { return value; }

        public InputDialog(string title, string promptText)
        {
            InitializeComponent();

            this.Text = title;
            this.lblPrompt.Text = promptText;
        }

        private void tbValue_TextChanged(object sender, EventArgs e)
        {
            value = this.tbValue.Text;
        }
    }
}
