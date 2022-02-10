using FoodStuffs_Control_System.src;
using FoodStuffsController.gui.MessageBoxes;
using FoodStuffsController.src;
using FoodStuffsController.controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace FoodStuffsController
{
    public partial class ControllerGUI : Form
    {

        //___________________________________________________
        // Define variables and a constructor.



        FeedBinController controller;
        ControllerGuiController cgc;

        private List<string> binStrings;

        // Define a boolean to ignore onChange events if caused by the system.
        // If it is false, the onChange was triggered by a user input.
        bool automatedChange = true;



        /// <summary>
        /// Create a ControllerGUI - the interface used by the bin controller.
        /// </summary>
        public ControllerGUI()
        {
            InitializeComponent();
            Thread.CurrentThread.Name = "ControllerThread";

            // Set the currentBin to the first bin in the controller list.
            controller = FeedBinController.getInstance();

            // Run the updateValues() function when the bin list changes.
            controller.BinListChangedEvent += updateValues;


            // Define the controller for the interface.
            cgc = ControllerGuiController.getInstance();

            // Subscribe to events from the controller.
            cgc.CurrentBinUpdate += updateValues;
                        
            binStrings = controller.StringBins();
            cbBin.DataSource = binStrings;
            cbBin.SelectedIndex = 0;
            
            updateValues();

            automatedChange = false;

        }


        //___________________________________________________
        // Define subscriber functions to listen to events.

        /// <summary>
        /// Update the GUI to display the new information about the currentBin.
        /// </summary>
        /// <param name="sender">Required for events (Unused)</param>
        /// <param name="e">Required for events (Unused)</param>
        private void updateValues(object sender = null, EventArgs e = null)
        {
            //Set the text values for the product name and current volume.
            this.lblProduct.Text = cgc.currentBin.getProductName();
            this.lblStock.Text = $"{cgc.currentBin.getCurrentVolume()}mᶟ";

            // Define the scale and progress of the progress bar to show how full the bin is.
            pbCapacity.Maximum = Convert.ToInt32(cgc.currentBin.getMaxVolume());
            pbCapacity.Value = Convert.ToInt32(cgc.currentBin.getCurrentVolume());


            // Update the cbBins
            List<string> newBinStrings = controller.StringBins();

            if (!binStrings.Equals(newBinStrings))
            {
                automatedChange = true;
                var selectedBin = cbBin.SelectedItem;
                binStrings = newBinStrings;
                cbBin.DataSource = binStrings;

                try { cbBin.SelectedItem = selectedBin; }
                catch (Exception err) { }

                automatedChange = false;
            }

        }



        //___________________________________________________
        // Action Listeners for the GUI functions.


        /// <summary>
        /// Update the currentBin when the user changes the bin.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbBin_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (automatedChange) return; // Ignore the change if it was performed by the software.

            // Update the current bin.
            cgc.updateSelectedBin(cbBin.SelectedIndex);
        }

        /// <summary>
        /// Change the product within a bin.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnChange_Click(object sender, EventArgs e)
        {
            cgc.change();
        }

        /// <summary>
        /// Add a desired quantity of product to a bin.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            cgc.add();
        }

        /// <summary>
        /// Remove a specified quantity of product from the bin.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemove_Click(object sender, EventArgs e)
        {
            cgc.remove();
        }

        /// <summary>
        /// Empty the current bin, if the user confirms the action.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEmpty_Click(object sender, EventArgs e)
        {
            cgc.empty();
        }

        /// <summary>
        /// If either form is closed, exit the application.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ControllerGUI_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        
    }
}
