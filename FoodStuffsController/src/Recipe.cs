using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodStuffsController.src
{
    class Recipe
    {
        // Define class variables with getters and setters.
        private string _RecipeName;
        private List<RecipeIngredient> _ingredients;
        public string RecipeName 
        { 
            get { return _RecipeName; } 
            set { _RecipeName = value; } 
        }
        public List<RecipeIngredient> ingredients 
        { 
            get { return _ingredients; } 
            set { _ingredients = value; } 
        }

        // Event for interfaces to subscribe to.
        // Trigger interface updage when values change.
        public event EventHandler VariableChangedEvent;

        public Recipe(string recipeName = "New Recipe") 
        {
            this.RecipeName = recipeName;
            this.ingredients = new List<RecipeIngredient>();
        }

        public void addIngredient(RecipeIngredient ingredient) 
        {
            this.ingredients.Add(ingredient);
        }

        public void removeIngredient(RecipeIngredient ingredient) 
        {
            if (ingredients.Contains(ingredient)) ingredients.Remove(ingredient);
        }

        public void removeIngredient(string ingredientName) 
        {
            RecipeIngredient ingredient = ingredients.SingleOrDefault(x => x.ingredientName == ingredientName);

        }

        public string getIngredientString() 
        {
            StringBuilder sb = new StringBuilder();

            foreach (RecipeIngredient ri in ingredients) 
            {
                sb.AppendLine($"{ri.ingredientName} ({ri.ingredientPercentage}%)");
            }

            return sb.ToString();
        }
        
    }

    /// <summary>
    /// A definition for each ingredient within the recipe.
    /// </summary>
    class RecipeIngredient
    {
        private string _ingredientName;
        private double _ingredientPercentage;

        public string ingredientName
        {
            get { return _ingredientName; }
            set { _ingredientName = value; }
        }

        public double ingredientPercentage
        {
            get { return _ingredientPercentage; }
            set { _ingredientPercentage = value; }
        }

        public RecipeIngredient(string ingredient, double percentage) 
        {
            this.ingredientName = ingredient;
            this.ingredientPercentage = percentage;
        }
    }
}
