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
        FeedBinController controller;


        // Define currentBin as a property
        private FeedBin _currentBin;
        private FeedBin currentBin
        {
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

        public SupervisorGUI()
        {
            InitializeComponent();
        }















        private void updateValues(object sender = null, EventArgs e = null) 
        {
            updateGraph();
        }

        private void updateGraph() 
        { 
            
        }

        private void SupervisorGUI_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
