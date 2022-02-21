using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace FoodStuffs_Control_System.src.Tests
{
    [TestClass()]
    public class FeedBinTests
    {
        // Two bins used throughout the testing.
        FeedBin bin1;
        FeedBin bin2;

        /// <summary>
        /// Setup the bins ready for testing.
        /// </summary>
        [TestInitialize]
        public void BeforeEach() 
        {
            //Define the bins before each test.
            bin1 = new FeedBin(1, "Test Product");
            bin2 = new FeedBin(2, "Changed Product", 20, 40);

            // Subscribe to the event with an empty function.
            bin1.VariableChangedEvent += IgnoreEvent;
            bin2.VariableChangedEvent += IgnoreEvent;
        }

        public void IgnoreEvent(object sender, EventArgs e) 
        { // Do nothing when the event is thrown.
          // Just here to prevent exceptions.
        }

        

        //----------------------------------------------------------------------------
        // Test valid and invalid inputs to Add

        /// <summary>
        /// Test adding product to a bin.
        /// </summary>
        [TestMethod()]
        public void addProductTestHasSpace()
        {
            bin1.AddProduct(20);
            Assert.AreEqual(bin2.GetCurrentVolume(), bin1.GetCurrentVolume());
        }

        /// <summary>
        /// Test that the add function will prevent overfilling a bin.
        /// </summary>
        [TestMethod()]
        public void addProductTestBinOverflow()
        {
            
            Assert.IsFalse(bin1.AddProduct(50));
        }


        //----------------------------------------------------------------------------
        // Test valid and invalid inputs to Remove


        /// <summary>
        /// Test removing product from a bin.
        /// </summary>
        [TestMethod()]
        public void removeProductTestHasEnough()
        {
            bin2.RemoveProduct(20);
            Assert.AreEqual(bin2.GetCurrentVolume(), bin1.GetCurrentVolume());
        }

        /// <summary>
        /// Test removing product from a bin where there isn't enough product.
        /// </summary>
        [TestMethod()]
        public void removeProductTestNotEnough()
        {
            bin2.RemoveProduct(50);
            Assert.AreEqual(bin2.GetCurrentVolume(), bin1.GetCurrentVolume());
        }


        //----------------------------------------------------------------------------
        // Test other functionality.


        /// <summary>
        /// Test setting the product name.
        /// </summary>
        [TestMethod()]
        public void setProductNameTest()
        {
            // Accessing a bin after a name change should work the same as accessing a bin with an initialized name.            

            bin1.SetProductName("Changed Product");
            Assert.AreEqual(bin2.GetProductName(), bin1.GetProductName());
        }

        /// <summary>
        /// Test the ToString method.
        /// </summary>
        [TestMethod()]
        public void ToStringTest()
        {
            Assert.AreEqual("Bin 1: Test Product", bin1.ToString());
        }
                

        /// <summary>
        /// Test the flushing of a bin.
        /// </summary>
        [TestMethod()]
        public void flushTest()
        {
            bin2.Flush();
            Assert.AreEqual(0, bin2.GetCurrentVolume());
        }

        

        /// <summary>
        /// Test the retrieval of the bin number.
        /// </summary>
        [TestMethod()]
        public void getBinNumberTest()
        {
            Assert.AreEqual(1, bin1.GetBinNumber());
        }

        /// <summary>
        /// Test getting the name of the product.
        /// </summary>
        [TestMethod()]
        public void getProductNameTest()
        {
            Assert.AreEqual("Test Product", bin1.GetProductName());
        }

        /// <summary>
        /// Test getting the max capacity for the bin.
        /// </summary>
        [TestMethod()]
        public void getMaxVolumeTest()
        {
            Assert.AreEqual(40, bin1.GetMaxVolume());
        }

        /// <summary>
        /// Test the retrieval of the current volume.
        /// </summary>
        [TestMethod()]
        public void getCurrentVolumeTest()
        {
            Assert.AreEqual(20, bin2.GetCurrentVolume());
        }

        /// <summary>
        /// Test the calculation of the filled percentage.
        /// </summary>
        [TestMethod()]
        public void getVolumePercentageTest()
        {
            Assert.AreEqual(50, bin2.GetVolumePercentage());
        }
    }
}