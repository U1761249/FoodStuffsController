using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodStuffs_Control_System.src
{
    class FeedBin
    {
        private bool ignoreUpdateEvent = true;

        //FeedBin instance properties
        private int _binNumber;
        private string _productName;
        private double _maxVolume;
        private double _currentVolume;

        // Call the VariableChangedEvent whenever a variable changes.
        private int binNumber { get { return _binNumber; } 
            set 
            { 
                _binNumber = value;                     // Underscored variables store the data.
                if (!ignoreUpdateEvent)                 // Check if the event should be triggered (Only false when the FeedBin is created)
                    VariableChangedEvent(this, null);   // Trigger the event to update interfaces.
            } 
        }
        private string productName { get { return _productName; } 
            set 
            { 
                _productName = value;
                if (!ignoreUpdateEvent)
                    VariableChangedEvent(this, null); 
            } 
        }
        private double maxVolume { get { return _maxVolume; } 
            set 
            { 
                _maxVolume = value;
                if (!ignoreUpdateEvent)
                    VariableChangedEvent(this, null); 
            } 
        }
        private double currentVolume { get { return _currentVolume; } 
            set 
            { 
                _currentVolume = value;
                if (!ignoreUpdateEvent)
                    VariableChangedEvent(this, null); 
            } 
        }

        // Event for interfaces to subscribe to. Trigger interface updage when values change.
        public event EventHandler VariableChangedEvent;



        /// <summary>
        /// Constructor for a FeedBin
        /// </summary>
        /// <param name="binNo">Unique number for a bin.</param>
        /// <param name="prodName">Name of the product within a bin.</param>
        public FeedBin(int binNo, string prodName)
        {
            binNumber = binNo;          // bin identifier
            productName = prodName;     // product in bin
            maxVolume = 40.0;           // maximum capacity in cubic metres
            currentVolume = 0.0;        // bin starts in the empty state

            ignoreUpdateEvent = false;  // Start listening to the VariableUpdateEvent after construction.
        }


        /// <summary>
        /// Change the name of the product assigned to the bin.
        /// Can only be done if the bin is empty.
        /// </summary>
        /// <param name="newName">New name of the product.</param>
        /// <returns>True if the name was changed.</returns>
        public bool setProductName(string newName)
        {
            if (currentVolume == 0.0)
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
        public void flush()
        {

            currentVolume = 0.0;
        }

        /// <summary>
        /// Add product to the bin if there is enough room.
        /// </summary>
        /// <param name="volume"></param>
        /// <returns></returns>
        public bool addProduct(double volume)
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
        public double removeProduct(double volume)
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

        // Getters for each of the values.
        public int getBinNumber() { return binNumber; }
        public string getProductName() { return productName; }
        public double getMaxVolume() { return maxVolume; }
        public double getCurrentVolume() { return currentVolume; }

        public double getVolumePercentage() { return (maxVolume / 100) * currentVolume; }

        /// <summary>
        /// Override the default ToString method.
        /// </summary>
        /// <returns></returns>
        override
        public string ToString() { return $"Bin {binNumber}: {productName}"; }
    }
}
