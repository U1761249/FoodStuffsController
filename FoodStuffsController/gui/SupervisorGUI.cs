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
                binChart.ChartAreas.Clear();
                binChart.Series.Clear();
                binChart.ChartAreas.Add(new ChartArea()) ;                
                binChart.ChartAreas[0].AxisY.Maximum = 100;
                binChart.ChartAreas[0].AxisX.Maximum = controller.getBins().Count();

                foreach (FeedBin bin in controller.getBins()) 
                {
                    int binNo = bin.getBinNumber();

                    Series s = new Series($"Bin{binNo}");
                                        
                    s.ChartType = SeriesChartType.Column;
                    s.Enabled = true;
                    s.XValueMember = s.Name;
                    s.YValueMembers = "Capacity";
                    s.Label = $"{bin.getVolumePercentage()}%";
                    

                    s.Points.Add(bin.getVolumePercentage());

                    binChart.Series.Add(s);
                }

                
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
