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
                dataGridView1.BeginInvoke(new InvokeDelegate(updateGraph));
            }
            catch (InvalidOperationException err) { }
            //updateGraph();
        }




        //___________________________________________________
        // Define functionality for the GUI

        private void updateGraph()
        {
            Console.WriteLine(Thread.CurrentThread.Name);

            DataTable dt = controller.getBinsDataTable();

            this.dataGridView1.DataSource = dt;
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
