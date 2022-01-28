using FoodStuffs_Control_System.src;
using FoodStuffsController.gui.MessageBoxes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoodStuffsController
{
    public partial class ControllerGUI : Form
    {

        FeedBin currentBin;


        public ControllerGUI()
        {
            InitializeComponent();
            currentBin = new FeedBin(1, "Wheat");
            updateValues();
        }

        public void updateValues()
        {
            this.lblProduct.Text = currentBin.getProductName();
            this.lblStock.Text = currentBin.getcurrentVolume().ToString();
        }

        // Action Listeners for the GUI functions.
        private void btnAdd_Click(object sender, EventArgs e)
        {
            string value = "";
            if (PopupBoxes.InputBox("Quantity to Add", "How much product to add:", ref value) == DialogResult.OK)
            {
                try
                {
                    double toAdd = Convert.ToDouble(value);
                    bool added = currentBin.addProduct(toAdd);

                    if (!added) PopupBoxes.ShowError("Error", "Not enough space in the bin to add " + toAdd);

                    updateValues();
                }
                catch (Exception err)
                {
                    PopupBoxes.ShowError("Illegal argument", err.Message);
                }
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {

        }

        private void btnEmpty_Click(object sender, EventArgs e)
        {

        }
    }
}
