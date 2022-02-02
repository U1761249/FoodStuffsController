﻿using FoodStuffs_Control_System.src;
using FoodStuffsController.gui.MessageBoxes;
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

namespace FoodStuffsController
{
    public partial class ControllerGUI : Form
    {
        FeedBinController controller;


        // Define currentBin as a property
        private FeedBin _currentBin;
        private FeedBin currentBin { 
            get { return _currentBin; }
            set 
            {
                // Automatically update the values when the currentBin changes. (Observer)
                _currentBin = value;
                // Subscribe to the VariableChangedEvent from the currentBin
                _currentBin.VariableChangedEvent += updateValues;
                updateValues();
            }
        }

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

            // Set the currentBin to the first bin in the controller list.
            controller = FeedBinController.getInstance();

            binStrings = controller.StringBins();
            cbBin.DataSource = binStrings;
            cbBin.SelectedIndex = 0;

            automatedChange = false;
                        
            currentBin = controller.getBins()[0];

            //updateValues();
        }

        /// <summary>
        /// Update the GUI to display the new information about the currentBin.
        /// </summary>
        public void updateValues(object sender = null, EventArgs e = null)
        {
            //Set the text values for the product name and current volume.
            this.lblProduct.Text = currentBin.getProductName();
            this.lblStock.Text = $"{currentBin.getCurrentVolume()}mᶟ";

            // Define the scale and progress of the progress bar to show how full the bin is.
            pbCapacity.Maximum = Convert.ToInt32(currentBin.getMaxVolume());
            pbCapacity.Value = Convert.ToInt32(currentBin.getCurrentVolume());


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
            currentBin = controller.getBins()[cbBin.SelectedIndex];
            
            //updateValues();
        }


        /// <summary>
        /// Add a desired quantity of product to a bin.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
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

                    //updateValues();
                }
                catch (Exception err)
                {
                    PopupBoxes.ShowError("Illegal argument", err.Message);
                }
            }
        }

        /// <summary>
        /// Remove a specified quantity of product from the bin.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemove_Click(object sender, EventArgs e)
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

                    //updateValues();
                }
                catch (Exception err)
                {
                    PopupBoxes.ShowError("Illegal argument", err.Message);
                }
            }
        }

        /// <summary>
        /// Empty the current bin, if the user confirms the action.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEmpty_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("This will empty the bin. \nContinue?", "Flush bin?", MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                currentBin.flush();
                //updateValues();
            }
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
