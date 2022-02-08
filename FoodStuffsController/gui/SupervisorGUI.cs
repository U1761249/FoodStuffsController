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

            updateValues();
        }

        //___________________________________________________
        // Define subscriber functions to listen to events.

        private void updateValues(object sender = null, EventArgs e = null)
        {
            try
            {
                binChart.BeginInvoke(new InvokeDelegate(updateGraph));
            }
            catch (InvalidOperationException err) { }
        }




        //___________________________________________________
        // Define functionality for the GUI

        private void updateGraph()
        {
            try
            {
                DataTable dt = controller.getBinsDataTable();
                binChart.Series.Clear();

                //// Data arrays.
                //string[] seriesArray = { "Cats", "Dogs" };
                //int[] pointsArray = { 1, 2 };

                //// Set palette.
                //this.binChart.Palette = ChartColorPalette.SeaGreen;

                //// Set title.
                //this.binChart.Titles.Add("Bin Fill Percentages");

                //// Add series.
                //for (int i = 0; i < seriesArray.Length; i++)
                //{
                //    // Add series.
                //    Series series = this.binChart.Series.Add(seriesArray[i]);

                //    // Add point.
                //    series.Points.Add(pointsArray[i]);
                //}

                binChart.DataSource = dt;
                binChart.Series[0].ChartType = SeriesChartType.Bar;
                binChart.Legends[0].Enabled = true;
                binChart.Series[0].XValueMember = "Feed Bin";
                binChart.Series[0].YValueMembers = "Capacity";
                binChart.DataBind();
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
