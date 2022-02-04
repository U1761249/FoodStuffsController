using FoodStuffs_Control_System.src;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace FoodStuffsController.src
{
    /// <summary>
    /// Shared Singleton resource that controls the system.
    /// Could be offloaded as a Web Service, or expernal software accessed via Java RMI or via Web Sockets.
    /// </summary>
    class FeedBinController
    {

        //___________________________________________________
        // Define variables and a constructor.


        // Trigger an event when the bin list is changed.
        public event EventHandler BinListChangedEvent;

        private static FeedBinController instance;
        private List<FeedBin> bins;


        private FeedBinController()
        {
            bins = new List<FeedBin>();
            PopulateBins();
        }


        //___________________________________________________
        // Define instance getter and bin population.


        /// <summary>
        /// Get the instance of the conctroller.
        /// Create the singleton instance if it does not exist.
        /// </summary>
        /// <returns></returns>
        public static FeedBinController getInstance()
        {
            if (instance == null) instance = new FeedBinController();
            return instance;
        }

        /// <summary>
        /// Populate bins with initial values.
        /// </summary>
        private void PopulateBins()
        {
            AddBin(new FeedBin(1, "Wheaty Bits"));
            AddBin(new FeedBin(2, "Meaty Bits"));
            AddBin(new FeedBin(3, "Gravy Bits"));

            //TODO: Make this pull from the database.
        }


        //___________________________________________________
        // Define subscriber functions to listen to events.


        /// <summary>
        /// Add the bin to the list.
        /// Subscribe the BinListChangedEvent to the bin VariableChangedEvent.
        /// </summary>
        /// <param name="bin"></param>
        private void AddBin(FeedBin bin) 
        {
            bins.Add(bin);
            bin.VariableChangedEvent += BinVariableChanged;
        }

        /// <summary>
        /// Trigger the BinListChangedEvent when a VariableChangedEvent is triggered.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BinVariableChanged(object sender = null, EventArgs e = null) 
        { 
            BinListChangedEvent(this, null); 
        }


        //___________________________________________________
        // Define class functionalty.



        //Functions to manipulate the bins list.
        /// <summary>
        /// Get the bins list.
        /// </summary>
        /// <returns></returns>
        public List<FeedBin> getBins() { return bins; }

        /// <summary>
        /// Override the bins list with a new list.
        /// </summary>
        /// <param name="newBins"></param>
        public void setBins(List<FeedBin> newBins) 
        { 
            bins = newBins;
            BinListChangedEvent(this, null);
        }

        /// <summary>
        /// Create a list of all bins using their ToString method.
        /// </summary>
        /// <returns></returns>
        public List<string> StringBins()
        {
            List<string> strings = new List<string>();

            SortByBinNo();
            foreach (FeedBin bin in bins) { strings.Add(bin.ToString()); }

            return strings;
        }

        public DataTable getBinsDataTable() 
        {
            DataTable dt = new DataTable();
            dt.Clear();
            dt.Columns.Add("BinNo");
            dt.Columns.Add("Product");
            dt.Columns.Add("CurrentVolume");
            dt.Columns.Add("MaxVolume");

            foreach (FeedBin bin in bins) 
            {
                DataRow row = dt.NewRow();
                row["BinNo"] = bin.getBinNumber();
                row["Product"] = bin.getProductName();
                row["CurrentVolume"] = bin.getCurrentVolume();
                row["MaxVolume"] = bin.getMaxVolume();

                dt.Rows.Add(row);
            }

            return dt;
        }

        /// <summary>
        /// Search the list for a bin containing a product with the given name.
        /// Return null if not found.
        /// </summary>
        /// <param name="product"></param>
        /// <returns>Found value, or Null</returns>
        public FeedBin FindByProduct(string product)
        {
            // Find the first bin with the desired product.
            FeedBin found = bins.Find(bin => bin.getProductName() == product);
            return found;
        }

        /// <summary>
        /// Search the list for a bin with the given number.
        /// Return null if not found.
        /// </summary>
        /// <param name="binNo"></param>
        /// <returns>Found value, or Null</returns>
        public FeedBin FindByBinNo(int binNo)
        {
            // Find the first bin with the desired ID.
            FeedBin found = bins.Find(bin => bin.getBinNumber() == binNo);
            return found;
        }

        /// <summary>
        /// Sort the bins in order of ID (Ascending)
        /// </summary>
        public void SortByBinNo()
        {
            bins.Sort((b1, b2) => b1.getBinNumber().CompareTo(b2.getBinNumber()));
        }


        /// <summary>
        /// Sort the bins in order of Product Name (Ascending)
        /// </summary>
        public void SortByProduct()
        {
            bins.Sort((b1, b2) => b1.getProductName().CompareTo(b2.getProductName()));
        }

        /// <summary>
        /// Sort the bins in order of Volume Percentage (Ascending)
        /// </summary>
        public void SortByPercentage()
        {
            bins.Sort((b1, b2) => b1.getVolumePercentage().CompareTo(b2.getVolumePercentage()));
        }

        /// <summary>
        /// Update a bin within the bins list.
        /// </summary>
        /// <param name="updated"></param>
        public void updateBin()
        {
            BinListChangedEvent(this, null);
        }


    }
}
