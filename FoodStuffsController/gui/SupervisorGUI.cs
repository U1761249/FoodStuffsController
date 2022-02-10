using FoodStuffs_Control_System.src;
using FoodStuffsController.controllers;
using FoodStuffsController.src;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace FoodStuffsController
{
    public partial class SupervisorGUI : Form
    {
        // Define a delegate to use BeginInvoke to update values using the thread it was created on.
        public delegate void InvokeDelegate();

        //___________________________________________________
        // Define variables and a constructor.

        FeedBinController controller;
        SupervisorGuiController sgc;

        public SupervisorGUI()
        {
            InitializeComponent();
            
            // Give a name to the current thread for debuggind.
            Thread.CurrentThread.Name = "SupervisorThread";
            
            // Get the instance for the FeedBinController.
            controller = FeedBinController.getInstance();

            controller.BinListChangedEvent += updateValues;

            // Define the controller for the interface.
            sgc = SupervisorGuiController.getInstance();

            // Subscribe to events from the controller.


        }

        //___________________________________________________
        // Define subscriber functions to listen to events.

        private void updateValues(object sender = null, EventArgs e = null)
        {
            try
            {
                binChart.BeginInvoke(new InvokeDelegate(updateDisplay));
                binChart.BeginInvoke(new InvokeDelegate(updateGraph));
            }
            catch (InvalidOperationException err) { }
        }




        //___________________________________________________
        // Define functionality for the GUI

        /// <summary>
        /// Update the display data to the current data.
        /// </summary>
        private void updateDisplay() 
        {
            gvRecipe.DataSource = sgc.getRecipeDataTable();

            for (int i = 0; i < gvRecipe.Columns.Count; i++)
            {
                gvRecipe.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                gvRecipe.Columns[i].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            }
            // Set the last column to size Fill
            gvRecipe.Columns[gvRecipe.Columns.Count -1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            // Resize rows
            gvRecipe.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }

        /// <summary>
        /// Convert the data from the controller bin list into a graph.
        /// </summary>
        private void updateGraph()
        {
            try
            {
                binChart.ChartAreas.Clear();
                binChart.Series.Clear();
                binChart.ChartAreas.Add(new ChartArea()) ;                
                binChart.ChartAreas[0].AxisY.Maximum = 100;
                binChart.ChartAreas[0].AxisX.Maximum = controller.getBins().Count() + 1;

                binChart.ChartAreas[0].AxisX.LabelStyle.Enabled = false;
                binChart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;

                binChart.ChartAreas[0].AxisY.Title = "Capacity";
                binChart.ChartAreas[0].AxisY.LabelStyle.Enabled = true;
                binChart.ChartAreas[0].AxisY.MajorGrid.Enabled = true;


                foreach (FeedBin bin in controller.getBins()) 
                {
                    int binNo = bin.getBinNumber();

                    Series s = new Series($"Bin{binNo}");
                                        
                    s.ChartType = SeriesChartType.Column;
                    s.Enabled = true;
                    s.YValueMembers = "Capacity";

                    // Change the label based on capacity
                    double volume = bin.getVolumePercentage();
                    if (volume == 0) { s.Label = "EMPTY"; }
                    else if (volume == 100) { s.Label = "FULL"; }
                    else { s.Label = $"{volume}%"; }

                    


                    s.Points.Add(new DataPoint(binNo, bin.getVolumePercentage()));
                        
                    binChart.Series.Add(s);
                }

                
            }
            catch (Exception e) { }
        }


        //___________________________________________________
        // Action Listeners for the GUI functions..

        

        private void SupervisorGUI_Shown(object sender, EventArgs e)
        {
            updateValues();
        }

        private void btnNewRecipe_Click(object sender, EventArgs e)
        {
            sgc.newRecipe();
        }

        private void btnBatch_Click(object sender, EventArgs e)
        {
            sgc.batch();
        }
        
        private void SupervisorGUI_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
