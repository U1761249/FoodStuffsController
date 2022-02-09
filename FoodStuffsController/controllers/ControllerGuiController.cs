using FoodStuffs_Control_System.src;
using FoodStuffsController.gui.MessageBoxes;
using FoodStuffsController.src;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoodStuffsController.controllers
{
    /// <summary>
    /// Singleton controller for the controller users.
    /// </summary>
    class ControllerGuiController
    {
        public event EventHandler CurrentBinUpdate;

        private static ControllerGuiController instance;
        public static ControllerGuiController getInstance()
        {
            if (instance == null) instance = new ControllerGuiController();
            return instance;
        }

        // Define currentBin as a property
        private FeedBin _currentBin;
        public FeedBin currentBin
        {
            get { return _currentBin; }
            set
            {
                // Unsubscribe the current bin.
                _currentBin.VariableChangedEvent -= CurrentBinChanged;

                // Automatically update the values when the currentBin changes. (Observer)
                _currentBin = value;
                // Subscribe to the VariableChangedEvent from the currentBin
                _currentBin.VariableChangedEvent += CurrentBinChanged;

            }
        }

        private void CurrentBinChanged(object sender = null, EventArgs e = null)
        {
            CurrentBinUpdate(this, null);
        }

        private FeedBinController controller;


        private ControllerGuiController()
        {
            controller = FeedBinController.getInstance();
            _currentBin = controller.getBins()[0];
        }

        public void updateSelectedBin(int index)
        {
            currentBin = controller.getBins()[index];
            CurrentBinUpdate(this, null);
        }

        public void add()
        {
            string value = "";
            if (PopupBoxes.InputBox("Quantity to Add", "How much product to add:", ref value) == DialogResult.OK)
            {
                // Try to convert the input to a double, and add it to the bin.
                try
                {
                    double toAdd = Convert.ToDouble(value);
                    bool added = currentBin.addProduct(toAdd);

                    if (!added) PopupBoxes.ShowError("Error", $"Not enough space in the bin to add {added}mᶟ.");
                }
                catch (Exception err)
                {
                    PopupBoxes.ShowError("Illegal argument", err.Message);
                }
            }
        }

        public void remove()
        {
            string value = "";
            if (PopupBoxes.InputBox("Quantity to Remove", "How much product to remove:", ref value) == DialogResult.OK)
            {
                try
                {
                    double toRemove = Convert.ToDouble(value);
                    double removed = currentBin.removeProduct(toRemove);

                    // Notify the user if the quantity of product could not be removed.
                    if (removed != toRemove) PopupBoxes.ShowError("Warning", $"Only {removed}mᶟ removed of the desired {toRemove}mᶟ.", MessageBoxIcon.Warning);
                }
                catch (Exception err)
                {
                    PopupBoxes.ShowError("Illegal argument", err.Message);
                }
            }
        }

        public void empty()
        {
            DialogResult result = MessageBox.Show("This will empty the bin. \nContinue?", "Flush bin?", MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                currentBin.flush();
            }
        }


    }
}
