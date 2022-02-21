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

        // Define a delegate to use BeginInvoke to update values using the thread it was created on.
        public delegate void InvokeDelegate();

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

            // Give a name to the current thread for debuggind.
            Thread.CurrentThread.Name = "ControllerThread";

            // Get the instance for the FeedBinController.
            controller = FeedBinController.GetInstance();

            // Run the updateValues() function when the bin list changes.
            controller.BinListChangedEvent += UpdateValues;


            // Define the controller for the interface.
            cgc = ControllerGuiController.getInstance();

            // Subscribe to events from the controller.
            cgc.CurrentBinUpdate += UpdateValues;

            binStrings = controller.StringBins();
            cbBin.DataSource = binStrings;
            cbBin.SelectedIndex = 0;

            UpdateDisplay();

            automatedChange = false;

        }


        //___________________________________________________
        // Define subscriber functions to listen to events.

        /// <summary>
        /// Update the GUI to display the new information about the currentBin.
        /// </summary>
        /// <param name="sender">Required for events (Unused)</param>
        /// <param name="e">Required for events (Unused)</param>
        private void UpdateValues(object sender = null, EventArgs e = null)
        {
            try
            {
                // Invoke the updateDisplay method to prevent cross-thread access to display properties.
                tableLayoutPanel1.BeginInvoke(new InvokeDelegate(UpdateDisplay));
            }
            catch (Exception err) { }


        }

        private void UpdateDisplay()
        {
            //Set the text values for the product name and current volume.
            this.lblProduct.Text = cgc.currentBin.GetProductName();
            this.lblStock.Text = $"{cgc.currentBin.GetCurrentVolume()}mᶟ";

            // Define the scale and progress of the progress bar to show how full the bin is.
            pbCapacity.Maximum = Convert.ToInt32(cgc.currentBin.GetMaxVolume());
            pbCapacity.Value = Convert.ToInt32(cgc.currentBin.GetCurrentVolume());


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
        private void CbBin_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (automatedChange) return; // Ignore the change if it was performed by the software.

            // Update the current bin.
            cgc.UpdateSelectedBin(cbBin.SelectedIndex);
        }

        /// <summary>
        /// Change the product within a bin.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnChange_Click(object sender, EventArgs e)
        {
            cgc.Change();
        }

        /// <summary>
        /// Add a desired quantity of product to a bin.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            cgc.Add();
        }

        /// <summary>
        /// Remove a specified quantity of product from the bin.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRemove_Click(object sender, EventArgs e)
        {
            cgc.Remove();
        }

        /// <summary>
        /// Empty the current bin, if the user confirms the action.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnEmpty_Click(object sender, EventArgs e)
        {
            cgc.Empty();
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
