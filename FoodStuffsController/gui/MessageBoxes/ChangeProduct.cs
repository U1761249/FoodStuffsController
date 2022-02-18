using FoodStuffsController.src;
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

    public partial class ChangeProduct : Form
    {
        FeedBinController controller = FeedBinController.getInstance();
        string value;
        public string getValue() { return value; }

        public ChangeProduct()
        {
            InitializeComponent();
            lbProducts.DataSource = controller.getIngredientList();
        }

        private void lbProducts_SelectedIndexChanged(object sender, EventArgs e)
        {
            value = lbProducts.SelectedItem.ToString();
        }
    }
}
