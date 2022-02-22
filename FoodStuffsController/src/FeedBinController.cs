using FoodStuffs_Control_System.src;
using FoodStuffsController.gui.MessageBoxes;
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
    public class FeedBinController
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

        private FeedBinController(bool databasePull = true)
        {
            dbconn = new DBManager();

            bins = new List<FeedBin>();
            recipes = new List<Recipe>();

            // Pull from the database if it is required.
            // ONLY FALSE DURING TESTING.
            if (databasePull)
            {
                PopulateBins();
                PopulateRecipes();
            }
        }


        //___________________________________________________
        // Define instance getter and bin population.


        /// <summary>
        /// Get the instance of the conctroller.
        /// Create the singleton instance if it does not exist.
        /// </summary>
        /// <returns></returns>
        public static FeedBinController GetInstance(bool databasePull = true)
        {
            if (instance == null) instance = new FeedBinController(databasePull);
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

                DataTable binTable = dbconn.QueryDatabase(
                    "SELECT binNo, prodName, currentVolume, maxVolume from bins " +
                    "left join products on bins.prodNo = products.prodNo"
                    );

                for (int r = 0; r < binTable.Rows.Count; r++)
                {
                    int binNo = Convert.ToInt32(binTable.Rows[r][0].ToString());
                    string productName = binTable.Rows[r][1].ToString();
                    double curVol = Convert.ToDouble(binTable.Rows[r][2].ToString());
                    double maxVol = Convert.ToDouble(binTable.Rows[r][3].ToString());

                    FeedBin newBin = new FeedBin(binNo, productName, curVol, maxVol);
                    AddBin(newBin);
                }

            }
            finally { Monitor.Exit(_LOCKED); }
        }

        /// <summary>
        /// Populate the recipes with initial values.
        /// </summary>
        private void PopulateRecipes()
        {
            Monitor.Enter(_LOCKED);
            try
            {
                this.recipes = new List<Recipe>();
                DataTable recipeTable = dbconn.QueryDatabase(
                    "SELECT productName, prodName, volume from recipe " +
                    "left join recipeIngredients on recipe.recipeID = recipeIngredients.recipeID " +
                    "left join products on recipeIngredients.prodNo = products.prodNo"
                    );

                Recipe currentRecipe = null;

                for (int r = 0; r < recipeTable.Rows.Count; r++)
                {

                    string productName = recipeTable.Rows[r][0].ToString();
                    string ingredientName = recipeTable.Rows[r][1].ToString();
                    double volume = Convert.ToDouble(recipeTable.Rows[r][2].ToString());

                    if (currentRecipe == null) { currentRecipe = new Recipe(productName); }

                    else if (currentRecipe.getRecipeName() != productName)
                    {
                        AddRecipe(currentRecipe);
                        currentRecipe = new Recipe(productName);
                    }

                    currentRecipe.AddIngredient(new RecipeIngredient(ingredientName, volume));

                }
                AddRecipe(currentRecipe);

            }
            finally { Monitor.Exit(_LOCKED); }
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

        /// <summary>
        /// Add a recipe to the recipes list and subscribe to its events.
        /// </summary>
        /// <param name="recipe"></param>
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
        /// Add a new recipe to the database and update the recipe list.
        /// </summary>
        /// <param name="recipe"></param>
        public void AddNewRecipe(Recipe recipe)
        {
            string addRecipeQuery = $"INSERT INTO recipe (productName) VALUES ('{recipe.getRecipeName()}');";
            string addRIQuery = "";

            if (!dbconn.UpdateDatabase(addRecipeQuery)) { return; }

            DataTable result = dbconn.QueryDatabase($"SELECT recipeID FROM recipe WHERE productName = '{recipe.getRecipeName()}'");
            string recipeID = result.Rows[0][0].ToString();

            foreach (RecipeIngredient ri in recipe.getIngredients())
            {
                string ingredientName = ri.GetIngredientName();
                // Create a new ingredient if one does not exist (Newly added)
                if (!dbconn.DatabaseContains($"SELECT * FROM products WHERE prodName = '{ingredientName}';"))
                {
                    string newIngredientQuery = $"INSERT INTO products (prodName) VALUES ('{ingredientName}')";
                    dbconn.UpdateDatabase(newIngredientQuery);
                }
                addRIQuery += $"INSERT INTO recipeIngredients (prodNo, recipeID, volume) VALUES (" +
                    $"(SELECT prodNo FROM products WHERE prodName = '{ingredientName}'), '{recipeID}', '{ri.GetIngredientPercentage()}');";
            }

            dbconn.UpdateDatabase(addRIQuery);

            PopulateRecipes();
            RecipeListChangedEvent(this, null);
        }

        /// <summary>
        /// Trigger the BinListChangedEvent when a VariableChangedEvent is triggered.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BinVariableChanged(object sender = null, EventArgs e = null)
        {
            UpdateDatabase((FeedBin)sender);

            BinListChangedEvent(this, null);
        }

        /// <summary>
        /// Trigger the RecipeListChangedEvent when a recipe variable changes. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RecipeVariableChanged(object sender = null, EventArgs e = null)
        {
            RecipeListChangedEvent(this, null);
        }


        //___________________________________________________
        // Define class functionalty.

        /// <summary>
        /// Perform a series of database SQL queries to update the database for the provided bin.
        /// </summary>
        /// <param name="bin"></param>
        private void UpdateDatabase(FeedBin bin)
        {
            string hasProductQuery = $"SELECT * FROM products WHERE prodName = '{bin.GetProductName()}'";
            string hasBinQuery = $"SELECT * FROM bins WHERE binNo = {bin.GetBinNumber()}";
            string insertProductQuery = $"INSERT INTO products (prodName) VALUES('{bin.GetProductName()}')";
            string insertQuery = $"" +
                $"INSERT INTO bins (prodNo, currentVolume, maxVolume) values " +
                $"((SELECT prodNo FROM products WHERE prodName = '{bin.GetProductName()}'), {bin.GetCurrentVolume()}, {bin.GetMaxVolume()});";

            string updateQuery = $"" +
            $"Update bins SET prodNo = " +
            $"(SELECT prodNo FROM products WHERE prodName = '{bin.GetProductName()}'), " +
            $"currentVolume = {bin.GetCurrentVolume()} WHERE binNo = {bin.GetBinNumber()};";

            bool hasProduct = dbconn.DatabaseContains(hasProductQuery);

            if (!hasProduct)
            {
                // Add the new product to the database.
                hasProduct = dbconn.UpdateDatabase(insertProductQuery);
            }
            // Check again in case the database could not be updated.
            if (!hasProduct)
            {
                // Notify the user if a product was not added to the bin.
                PopupBoxes.ShowError("Database Update Error", $"An unexpected error prevented {bin.GetProductName()} from being added to the database.");
            }
            else
            {
                bool success;
                // Insert a new bin if one does not exist with the current bin number.
                if (!dbconn.DatabaseContains(hasBinQuery))
                {
                    success = dbconn.UpdateDatabase(insertQuery);
                }
                // Update the bin with the new values.
                else
                {
                    success = dbconn.UpdateDatabase(updateQuery);
                }

                if (!success)
                {
                    PopupBoxes.ShowError("Database Update Error", $"An unexpected error prevented changes to bin {bin.GetBinNumber()} from being made to the database.");
                }
            }
        }

        //Functions to manipulate the bins list.
        /// <summary>
        /// Get the bins list.
        /// </summary>
        /// <returns></returns>
        public List<FeedBin> GetBins()
        {
            return bins;
        }

        /// <summary>
        /// Override the bins list with a new list.
        /// </summary>
        /// <param name="newBins"></param>
        public void SetBins(List<FeedBin> newBins)
        {
            Monitor.Enter(_LOCKED);
            try
            {
                bins = newBins;
                BinListChangedEvent(this, null);
            }
            finally { Monitor.Exit(_LOCKED); }
        }

        public List<Recipe> GetRecipes() { return recipes; }

        /// <summary>
        /// Override the recipes list with a new list.
        /// </summary>
        /// <param name="newRecipes"></param>
        public void SetRecipse(List<Recipe> newRecipes)
        {
            Monitor.Enter(_LOCKED);
            try
            {
                recipes = newRecipes;
                RecipeListChangedEvent(this, null);
            }
            finally { Monitor.Exit(_LOCKED); }
        }

        /// <summary>
        /// Get a list of all recipe names.
        /// </summary>
        /// <returns></returns>
        public List<string> GetRecipeList()
        {
            Monitor.Enter(_LOCKED);
            try
            {
                List<string> recipeStrings = new List<string>();
                foreach (Recipe r in recipes)
                {
                    recipeStrings.Add(r.getRecipeName());
                }
                return recipeStrings;
            }
            finally { Monitor.Exit(_LOCKED); }
        }

        /// <summary>
        /// Get a list of all ingredients within the database.
        /// </summary>
        /// <returns></returns>
        public List<string> GetIngredientList()
        {
            List<string> ingredientList = new List<string>();
            Monitor.Enter(_LOCKED);
            try
            {
                DataTable results = dbconn.QueryDatabase("SELECT prodName FROM products");

                for (int r = 0; r < results.Rows.Count; r++)
                {
                    ingredientList.Add(results.Rows[r][0].ToString());
                }

                return ingredientList;
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

        /// <summary>
        /// Get information about the recipes in a data table.
        /// </summary>
        /// <returns></returns>
        public DataTable GetRecipeDataTable()
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
                    row["Product"] = recipe.getRecipeName();
                    // Get a list of ingredients and ratios.
                    row["Ingredients"] = recipe.GetIngredientString();
                    // Get the maximum size of a batch - rounded to 2dp
                    row["BatchMax"] = Math.Round(GetMaxBatch(recipe, ref message), 2) + "mᶟ";

                    dt.Rows.Add(row);
                }

                return dt;
            }
            finally { Monitor.Exit(_LOCKED); }
        }

        /// <summary>
        /// Calculate the largest possible batch of a recipe given the current contents of the bins.
        /// </summary>
        /// <param name="recipe"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private double GetMaxBatch(Recipe recipe, ref string message)
        {
            Monitor.Enter(_LOCKED);
            try
            {
                double maxBatch = double.PositiveInfinity;

                foreach (RecipeIngredient ri in recipe.getIngredients())
                {
                    FeedBin ingredientBin = FindByProduct(ri.GetIngredientName());
                    // Return 0 if there is an ingredient with no current bin.
                    if (ingredientBin == null)
                    {
                        message = $"No bin contains product {ri.GetIngredientName()}";
                        return 0;
                    }

                    double maxIngredient = (ingredientBin.GetCurrentVolume() / ri.GetIngredientPercentage()) * 100;
                    if (maxIngredient < maxBatch)
                    {
                        maxBatch = maxIngredient;

                        if (maxBatch == 0)
                        {
                            message = $"The batch cannot be made, {ri.GetIngredientName()} is missing.";
                        }
                        else
                        {
                            message = $"The batch size is limited to {string.Format("{0:N2}%", maxBatch)} by the availability of {ri.GetIngredientName()}";
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
        public bool CanMake(string recipeName, double size, ref string message)
        {
            Monitor.Enter(_LOCKED);
            try
            {
                Recipe found = recipes.Find(r => r.getRecipeName() == recipeName);
                if (found == null)
                {
                    message = "The recipe could not be found.";
                    return false;
                }

                return GetMaxBatch(found, ref message) >= size;
            }
            finally { Monitor.Exit(_LOCKED); }
        }

        /// <summary>
        /// Make a batch of the given recipe to the specified size.
        /// </summary>
        /// <param name="recipeName"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public bool MakeBatch(string recipeName, double size)
        {
            Monitor.Enter(_LOCKED);
            try
            {

                // Find the recipe from the recipes list.
                Recipe found = recipes.Find(r => r.getRecipeName() == recipeName);
                if (found == null)
                {
                    return false;
                }

                // Loop over each ingredient in the recipe.
                foreach (RecipeIngredient ri in found.getIngredients())
                {
                    // Find the bin that contains the recipe.
                    FeedBin ingredientBin = FindByProduct(ri.GetIngredientName());
                    // Stop if there is an ingredient with no current bin.
                    if (ingredientBin == null)
                    {
                        return false;
                    }

                    // Calculate and remove the correct portion of ingredient from the bin.
                    double toRemove = (size / 100) * ri.GetIngredientPercentage();
                    if (ingredientBin.RemoveProduct(toRemove) != toRemove)
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
            FeedBin found = bins.Find(bin => bin.GetProductName() == product);
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
            FeedBin found = bins.Find(bin => bin.GetBinNumber() == binNo);
            return found;
        }
    }
}
