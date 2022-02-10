using FoodStuffs_Control_System.src;
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

        public SupervisorGUI()
        {
            InitializeComponent();
                        
            Thread.CurrentThread.Name = "SupervisorThread";
            controller = FeedBinController.getInstance();

            controller.BinListChangedEvent += updateValues;


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

        private void SupervisorGUI_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void SupervisorGUI_Shown(object sender, EventArgs e)
        {
            updateValues();
        }

    }
}
