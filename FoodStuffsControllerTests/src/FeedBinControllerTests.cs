using Microsoft.VisualStudio.TestTools.UnitTesting;
using FoodStuffsController.src;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoodStuffs_Control_System.src;
using System.Data;

namespace FoodStuffsController.src.Tests
{
    /// <summary>
    /// Limited testing of the FeedBinController.
    /// Many functions will not be feasible to UnitTest.
    /// </summary>
    [TestClass()]
    public class FeedBinControllerTests
    {
        FeedBinController controller;
        List<FeedBin> bins;
        List<Recipe> recipes;

        [TestInitialize]
        public void BeforeEach()
        {
            controller = FeedBinController.getInstance(false);

            controller.BinListChangedEvent += this.IgnoreEvent;
            controller.RecipeListChangedEvent += this.IgnoreEvent;


            bins = new List<FeedBin>();
            recipes = new List<Recipe>();

            bins.Add(new FeedBin(1, "Bin 1"));
            bins.Add(new FeedBin(2, "Bin 2"));
            bins.Add(new FeedBin(3, "Bin 3"));

            Recipe R1 = new Recipe("Recipe 1");
            R1.addIngredient(new RecipeIngredient("Ingredient 1", 50));
            R1.addIngredient(new RecipeIngredient("Ingredient 2", 50));

            Recipe R2 = new Recipe("Recipe 2");
            R2.addIngredient(new RecipeIngredient("Ingredient 3", 50));
            R2.addIngredient(new RecipeIngredient("Ingredient 4", 50));

            recipes.Add(R1);
            recipes.Add(R2);

            controller.setBins(bins);
            controller.setRecipse(recipes);            
        }


        //----------------------------------------------------------------------------
        // Define helper functions for testing.

        /// <summary>
        /// Empty function to do nothing when events are thrown.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IgnoreEvent(object sender, EventArgs e) { }

        /// <summary>
        /// Add stock to the bins for testing.
        /// </summary>
        private void AddStock() 
        {
            // Set subscribers to prevent null reference exceptions for the events.
            bins[0].VariableChangedEvent += IgnoreEvent;
            bins[1].VariableChangedEvent += IgnoreEvent;
            bins[2].VariableChangedEvent += IgnoreEvent;

            // Define the bin product names.
            bins[0].setProductName("Ingredient 1");
            bins[1].setProductName("Ingredient 2");

            // Add product to the bins.
            bins[0].addProduct(30);
            bins[1].addProduct(30);

            // Replace the bins list with the modified one.
            controller.setBins(bins);
        }


        //----------------------------------------------------------------------------
        // Test creation of batches based on bin contents.

        /// <summary>
        /// Test that batches cannot be made without enough stock.
        /// </summary>
        [TestMethod()]
        public void canMakeTestNoStock()
        {
            string message = "";
            Assert.IsFalse(controller.canMake("Recipe 1", 20, ref message));
        }

        /// <summary>
        /// Test that batches can be made if there is stock.
        /// </summary>
        [TestMethod()]
        public void canMakeTestHasStock()
        {
            AddStock();
            string message = "";
            Assert.IsTrue(controller.canMake("Recipe 1", 20, ref message));

        }

        /// <summary>
        /// Test that batches cannot be made without enough stock.
        /// </summary>
        [TestMethod()]
        public void makeBatchTestNoStock() 
        {
            Assert.IsFalse(controller.makeBatch("Recipe 1", 50));
        }

        /// <summary>
        /// Test that batches can be made if there is stock.
        /// </summary>
        [TestMethod()]
        public void makeBatchTestHasStock()
        {
            AddStock();
            Assert.IsTrue(controller.makeBatch("Recipe 1", 50));
        }

        //----------------------------------------------------------------------------
        // Test finding bins by ID and product.


        /// <summary>
        /// Test finding of a product that does not exist.
        /// </summary>
        [TestMethod()]
        public void FindByProductTestNoProduct()
        {
            FeedBin Found = controller.FindByProduct("I DO NOT EXIST!");
            Assert.IsNull(Found);
        }

        /// <summary>
        /// Test finding a product that does exist.
        /// </summary>
        [TestMethod()]
        public void FindByProductTestHasProduct()
        {
            FeedBin Found = controller.FindByProduct("Bin 1");
            Assert.IsNotNull(Found);
        }

        /// <summary>
        /// Test finding a bin by number that does not exist.
        /// </summary>
        [TestMethod()]
        public void FindByBinNoTestNoProduct()
        {
            FeedBin Found = controller.FindByBinNo(7);
            Assert.IsNull(Found);
        }

        /// <summary>
        /// Test finding a bin by number that does exist.
        /// </summary>
        [TestMethod()]
        public void FindByBinNoTestHasProduct()
        {
            FeedBin Found = controller.FindByBinNo(1);
            Assert.IsNotNull(Found);
        }


        //----------------------------------------------------------------------------
        //Test other functionality.


        /// <summary>
        /// Test getting the list of bins.
        /// </summary>
        [TestMethod()]
        public void getBinsTest()
        {
            CollectionAssert.AreEqual(bins, controller.getBins());
        }

        /// <summary>
        /// Test replacing the list of bins to a new list.
        /// </summary>
        [TestMethod()]
        public void setBinsTest()
        {
            bins.Add(new FeedBin(4, "New Bin"));
            controller.setBins(bins);
            CollectionAssert.AreEqual(bins, controller.getBins());
        }

        /// <summary>
        /// Test getting the list of recipes.
        /// </summary>
        [TestMethod()]
        public void getRecipesTest()
        {
            CollectionAssert.AreEqual(recipes, controller.getRecipes());
        }

        /// <summary>
        /// Test replacing the list of recipes to a new list.
        /// </summary>
        [TestMethod()]
        public void setRecipseTest()
        {
            Recipe R = new Recipe("New Recipe");
            RecipeIngredient ri1 = new RecipeIngredient("New Ingredient 1", 60);
            RecipeIngredient ri2 = new RecipeIngredient("New Ingredient 2", 40);
            
            R.addIngredient(ri1);
            R.addIngredient(ri2);

            recipes.Add(R);
            controller.setRecipse(recipes);

            CollectionAssert.AreEqual(recipes, controller.getRecipes());
        }
               
        /// <summary>
        /// Test retrieving a list of bins as a string.
        /// </summary>
        [TestMethod()]
        public void StringBinsTest()
        {
            List<string> expected = new List<string>();
            expected.Add("Bin 1: Bin 1");
            expected.Add("Bin 2: Bin 2");
            expected.Add("Bin 3: Bin 3");

            CollectionAssert.AreEqual(expected, controller.StringBins());
        }

        /// <summary>
        /// Test the retrieval of recipe information in a table.
        /// Compare the first row only.
        /// </summary>
        [TestMethod()]
        public void getRecipeDataTableTest()
        {
            DataRow actualRow = controller.getRecipeDataTable().Rows[0];

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Ingredient 1 (50%)");
            sb.AppendLine("Ingredient 2 (50%)");

            List<string> expectedOut = new List<string>();
            List<string> actualOut = new List<string>();

            expectedOut.Add("Recipe 1");
            expectedOut.Add(sb.ToString());
            expectedOut.Add("0mᶟ");

            actualOut.Add(actualRow["Product"].ToString());
            actualOut.Add(actualRow["Ingredients"].ToString());
            actualOut.Add(actualRow["BatchMax"].ToString());

            CollectionAssert.AreEqual(expectedOut, actualOut);
        }

        
    }
}