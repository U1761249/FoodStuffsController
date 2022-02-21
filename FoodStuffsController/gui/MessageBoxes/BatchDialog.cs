using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoodStuffsController.gui.MessageBoxes
{
    public partial class BatchDialog : Form
    {

        string recipe;
        double quantity;

        public BatchDialog(List<string> recipes)
        {
            InitializeComponent();
            recipe = "";
            quantity = 0;
            lbRecipes.DataSource = recipes;
        }

        public KeyValuePair<string, double> getValue() 
        {
            return new KeyValuePair<string, double> ( recipe, quantity );
        }

        private void TbQuantity_TextChanged(object sender, EventArgs e)
        {
            try 
            {
                double quantity = Convert.ToDouble(tbQuantity.Text);
                this.quantity = quantity;
            }
            catch (FormatException err) 
            {
                // remove the last chatacter from the text box.
                //tbQuantity.Text = tbQuantity.Text.Substring(0, tbQuantity.Text.Length - 1);
                tbQuantity.Text = new string(tbQuantity.Text.Where(c => char.IsDigit(c)).ToArray());
            }
        }

        private void LbRecipes_SelectedIndexChanged(object sender, EventArgs e)
        {
            recipe = lbRecipes.SelectedItem.ToString();
        }
    }
}
