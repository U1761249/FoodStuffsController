using FoodStuffs_Control_System.src;
using FoodStuffsController.sql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;

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


        // Trigger an event when the lists are changed.
        public event EventHandler BinListChangedEvent;
        public event EventHandler RecipeListChangedEvent;

        private static FeedBinController instance;
        private List<FeedBin> bins;
        private List<Recipe> recipes;

        private DBManager dbconn;

        private readonly object _LOCKED = new object();

        private FeedBinController()
        {
            dbconn = new DBManager();
            dbconn.queryDatabase("Test");

            bins = new List<FeedBin>();
            recipes = new List<Recipe>();
            PopulateBins();
            PopulateRecipes();
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
            Monitor.Enter(_LOCKED);
            try
            {
                AddBin(new FeedBin(1, "Wheaty Bits"));
                AddBin(new FeedBin(2, "Meaty Bits"));
                AddBin(new FeedBin(3, "Gravy Bits"));
            }
            finally { Monitor.Exit(_LOCKED); }
            //TODO: Make this pull from the database.
        }
        /// <summary>
        /// Populate the recipes with initial values.
        /// </summary>
        private void PopulateRecipes()
        {
            Monitor.Enter(_LOCKED);
            try
            {
                Recipe r1 = new Recipe("Recipe 1");
                r1.addIngredient(new RecipeIngredient("Wheaty Bits", 50));
                r1.addIngredient(new RecipeIngredient("Gravy Bits", 50));

                Recipe r2 = new Recipe("Recipe 2");
                r2.addIngredient(new RecipeIngredient("Meaty Bits", 35));
                r2.addIngredient(new RecipeIngredient("Gravy Bits", 65));

                Recipe r3 = new Recipe("Recipe 3");
                r3.addIngredient(new RecipeIngredient("Wheaty Bits", 20));
                r3.addIngredient(new RecipeIngredient("Meaty Bits", 20));
                r3.addIngredient(new RecipeIngredient("Gravy Bits", 60));

                Recipe r4 = new Recipe("Recipe 4");
                r4.addIngredient(new RecipeIngredient("Meaty Bits", 35));
                r4.addIngredient(new RecipeIngredient("Tasty Bits", 65));

                recipes.Add(r1);
                recipes.Add(r2);
                recipes.Add(r3);
                recipes.Add(r4);
            }
            finally { Monitor.Exit(_LOCKED); }
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
            Monitor.Enter(_LOCKED);
            try
            {
                bins.Add(bin);
                bin.VariableChangedEvent += BinVariableChanged;
            }
            finally { Monitor.Exit(_LOCKED); }
        }

        private void AddRecipe(Recipe recipe)
        {
            Monitor.Enter(_LOCKED);
            try
            {
                recipes.Add(recipe);
                recipe.VariableChangedEvent += RecipeVariableChanged;
            }
            finally { Monitor.Exit(_LOCKED); }
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

        private void RecipeVariableChanged(object sender = null, EventArgs e = null)
        {
            RecipeListChangedEvent(this, null);
        }


        //___________________________________________________
        // Define class functionalty.



        //Functions to manipulate the bins list.
        /// <summary>
        /// Get the bins list.
        /// </summary>
        /// <returns></returns>
        public List<FeedBin> getBins()
        {
            return bins;
        }

        /// <summary>
        /// Override the bins list with a new list.
        /// </summary>
        /// <param name="newBins"></param>
        public void setBins(List<FeedBin> newBins)
        {
            Monitor.Enter(_LOCKED);
            try
            {
                bins = newBins;
                BinListChangedEvent(this, null);
            }
            finally { Monitor.Exit(_LOCKED); }
        }

        public List<Recipe> getRecipes() { return recipes; }

        public void setRecipse(List<Recipe> newRecipes)
        {
            Monitor.Enter(_LOCKED);
            try
            {
                recipes = newRecipes;
                RecipeListChangedEvent(this, null);
            }
            finally { Monitor.Exit(_LOCKED); }
        }

        public List<string> getRecipeList()
        {
            Monitor.Enter(_LOCKED);
            try
            {
                List<string> recipeStrings = new List<string>();
                foreach (Recipe r in recipes)
                {
                    recipeStrings.Add(r.RecipeName);
                }
                return recipeStrings;
            }
            finally { Monitor.Exit(_LOCKED); }
        }

        /// <summary>
        /// Create a list of all bins using their ToString method.
        /// </summary>
        /// <returns></returns>
        public List<string> StringBins()
        {
            Monitor.Enter(_LOCKED);
            try
            {
                List<string> strings = new List<string>();

                foreach (FeedBin bin in bins) { strings.Add(bin.ToString()); }

                return strings;
            }
            finally { Monitor.Exit(_LOCKED); }
        }

        public DataTable getBinsDataTable()
        {
            Monitor.Enter(_LOCKED);
            try
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
            finally { Monitor.Exit(_LOCKED); }
        }

        public DataTable getRecipeDataTable()
        {
            Monitor.Enter(_LOCKED);
            try
            {
                DataTable dt = new DataTable();

                dt.Columns.Add("Product");
                dt.Columns.Add("Ingredients");
                dt.Columns.Add("BatchMax");

                foreach (Recipe recipe in recipes)
                {
                    string message = "";

                    DataRow row = dt.NewRow();
                    // Get the recipe name.
                    row["Product"] = recipe.RecipeName;
                    // Get a list of ingredients and ratios.
                    row["Ingredients"] = recipe.getIngredientString();
                    // Get the maximum size of a batch - rounded to 2dp
                    row["BatchMax"] = Math.Round(getMaxBatch(recipe, ref message), 2) + "mᶟ";

                    dt.Rows.Add(row);
                }

                return dt;
            }
            finally { Monitor.Exit(_LOCKED); }
        }

        private double getMaxBatch(Recipe recipe, ref string message)
        {
            Monitor.Enter(_LOCKED);
            try
            {
                double maxBatch = double.PositiveInfinity;

                foreach (RecipeIngredient ri in recipe.ingredients)
                {
                    FeedBin ingredientBin = FindByProduct(ri.ingredientName);
                    // Return 0 if there is an ingredient with no current bin.
                    if (ingredientBin == null)
                    {
                        message = $"No bin contains product {ri.ingredientName}";
                        return 0;
                    }

                    double maxIngredient = (ingredientBin.getCurrentVolume() / ri.ingredientPercentage) * 100;
                    if (maxIngredient < maxBatch)
                    {
                        maxBatch = maxIngredient;

                        if (maxBatch == 0)
                        {
                            message = $"The batch cannot be made, {ri.ingredientName} is missing.";
                        }
                        else
                        {
                            message = $"The batch size is limited to {maxBatch} by the availability of {ri.ingredientName}";
                        }
                    }
                }
                if (maxBatch == double.PositiveInfinity) maxBatch = 0;
                return maxBatch;
            }
            finally { Monitor.Exit(_LOCKED); }
        }

        /// <summary>
        /// Calculate if the recipe can be made at the desired size.
        /// </summary>
        /// <param name="recipeName"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public bool canMake(string recipeName, double size, ref string message)
        {
            Monitor.Enter(_LOCKED);
            try
            {
                Recipe found = recipes.Find(r => r.RecipeName == recipeName);
                if (found == null)
                {
                    message = "The recipe could not be found.";
                    return false;
                }

                return getMaxBatch(found, ref message) >= size;
            }
            finally { Monitor.Exit(_LOCKED); }
        }

        /// <summary>
        /// Make a batch of the given recipe to the specified size.
        /// </summary>
        /// <param name="recipeName"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public bool makeBatch(string recipeName, double size)
        {
            Monitor.Enter(_LOCKED);
            try
            {

                // Find the recipe from the recipes list.
                Recipe found = recipes.Find(r => r.RecipeName == recipeName);
                if (found == null)
                {
                    return false;
                }

                // Loop over each ingredient in the recipe.
                foreach (RecipeIngredient ri in found.ingredients)
                {
                    // Find the bin that contains the recipe.
                    FeedBin ingredientBin = FindByProduct(ri.ingredientName);
                    // Stop if there is an ingredient with no current bin.
                    if (ingredientBin == null)
                    {
                        return false;
                    }

                    // Calculate and remove the correct portion of ingredient from the bin.
                    double toRemove = (size / 100) * ri.ingredientPercentage;
                    if (ingredientBin.removeProduct(toRemove) != toRemove)
                    {
                        // One bin does not have enough ingredient. Stop making the batch.
                        return false;
                    }
                }

                // The batch was successfully made.
                return true;
            }
            finally { Monitor.Exit(_LOCKED); }
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
    }
}
