using FoodStuffs_Control_System.src;
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
    public partial class SupervisorGUI : Form
    {

        //___________________________________________________
        // Define variables and a constructor.

        FeedBinController controller;

        public SupervisorGUI()
        {
            InitializeComponent();
            controller = FeedBinController.getInstance();

            controller.BinListChangedEvent += updateValues;

            DataTable dt = controller.getBinsDataTable();

            this.dataGridView1.DataSource = dt;
        }

        //___________________________________________________
        // Define subscriber functions to listen to events.

        private void updateValues(object sender = null, EventArgs e = null)
        {

            DataTable dt = controller.getBinsDataTable();

            this.dataGridView1.DataSource = dt;

            updateGraph();
        }




        //___________________________________________________
        // Define functionality for the GUI

        private void updateGraph()
        {

        }


        //___________________________________________________
        // Action Listeners for the GUI functions..

        private void SupervisorGUI_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
