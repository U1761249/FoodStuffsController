using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodStuffs_Control_System.src
{
    public class FeedBin
    {

        //___________________________________________________
        // Define variables and a constructor.

        private bool ignoreUpdateEvent = true;

        //FeedBin instance properties
        private int _binNumber;
        private string _productName;
        private double _maxVolume;
        private double _currentVolume;

        // Event for interfaces to subscribe to.
        // Trigger interface updage when values change.
        public event EventHandler VariableChangedEvent;

        // Call the VariableChangedEvent whenever a variable changes.
        private int binNumber
        {
            get { return _binNumber; }
            set
            {
                _binNumber = value;                     // Underscored variables store the data.
                if (!ignoreUpdateEvent)                 // Check if the event should be triggered (Only false when the FeedBin is created)
                    VariableChangedEvent(this, null);   // Trigger the event to update interfaces.
            }
        }
        private string productName
        {
            get { return _productName; }
            set
            {
                _productName = value;
                if (!ignoreUpdateEvent)
                    VariableChangedEvent(this, null);
            }
        }
        private double maxVolume
        {
            get { return _maxVolume; }
            set
            {
                _maxVolume = value;
                if (!ignoreUpdateEvent)
                    VariableChangedEvent(this, null);
            }
        }
        private double currentVolume
        {
            get { return _currentVolume; }
            set
            {
                _currentVolume = value;
                if (!ignoreUpdateEvent)
                    VariableChangedEvent(this, null);
            }
        }



        /// <summary>
        /// Constructor for a FeedBin
        /// </summary>
        /// <param name="binNo">Unique number for a bin.</param>
        /// <param name="prodName">Name of the product within a bin.</param>
        public FeedBin(int binNo, string prodName, double currentVolume = 0, double maxVolume = 40)
        {
            this.binNumber = binNo;          // bin identifier
            this.productName = prodName;     // product in bin
            this.maxVolume = maxVolume;           // maximum capacity in cubic metres, default is 40
            this.currentVolume = currentVolume;        // current bin capacity, default is 0

            ignoreUpdateEvent = false;  // Start listening to the VariableUpdateEvent after construction.

        }




        //___________________________________________________
        // Define class functionality.

        /// <summary>
        /// Override the default ToString method.
        /// </summary>
        /// <returns></returns>
        override
        public string ToString()
        { return $"Bin {binNumber}: {productName}"; }

        /// <summary>
        /// Change the name of the product assigned to the bin.
        /// Can only be done if the bin is empty.
        /// </summary>
        /// <param name="newName">New name of the product.</param>
        /// <returns>True if the name was changed.</returns>
        public bool SetProductName(string newName)
        {
            if (currentVolume == 0.0 && !String.IsNullOrWhiteSpace(newName))
            {
                productName = newName;
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Empty the bin.
        /// </summary>
        public void Flush()
        {

            currentVolume = 0.0;
        }

        /// <summary>
        /// Add product to the bin if there is enough room.
        /// </summary>
        /// <param name="volume"></param>
        /// <returns></returns>
        public bool AddProduct(double volume)
        {
            if (maxVolume >= currentVolume + volume)
            {
                currentVolume += volume;
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Removes a given volume of product from a bin.
        /// Removes as much as possible if insufficient product.
        /// </summary>
        /// <param name="volume"></param>
        /// <returns>The actual quantity of product removed.</returns>
        public double RemoveProduct(double volume)
        {
            if (currentVolume >= volume)
            {
                currentVolume -= volume;
            }
            else
            {
                volume = currentVolume;
                currentVolume = 0.0;
            }
            return volume;  // actual amount removed - may be less than requested
        }



        //___________________________________________________
        // Getters for each of the values.
        public int GetBinNumber() { return binNumber; }
        public string GetProductName() { return productName; }
        public double GetMaxVolume() { return maxVolume; }
        public double GetCurrentVolume() { return currentVolume; }

        public double GetVolumePercentage()
        {
            if (currentVolume == 0) return 0;
            return (currentVolume / maxVolume) * 100;
        }




    }
}
