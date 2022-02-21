using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodStuffsController.src
{
    public class Recipe
    {
        // Define class variables with getters and setters.
        private string recipeName;
        private List<RecipeIngredient> ingredients;
        public string getRecipeName() { return this.recipeName; }
        public void setRecipeName(string newName) { this.recipeName = newName; }
        public List<RecipeIngredient> getIngredients() { return this.ingredients; }
        public void setIngredients(List<RecipeIngredient> newIngredients) { this.ingredients = newIngredients; }

        // Event for interfaces to subscribe to.
        // Trigger interface updage when values change.
        public event EventHandler VariableChangedEvent;

        public Recipe(string recipeName = "New Recipe")
        {
            this.recipeName = recipeName;
            this.ingredients = new List<RecipeIngredient>();
        }

        public void AddIngredient(RecipeIngredient ingredient)
        {
            this.ingredients.Add(ingredient);
        }

        public void RemoveIngredient(RecipeIngredient ingredient)
        {
            if (ingredients.Contains(ingredient)) ingredients.Remove(ingredient);
        }

        public void RemoveIngredient(string ingredientName)
        {
            RecipeIngredient ingredient = ingredients.SingleOrDefault(x => x.GetIngredientName() == ingredientName);

        }

        public string GetIngredientString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (RecipeIngredient ri in ingredients)
            {
                sb.AppendLine($"{ri.GetIngredientName()} ({ri.GetIngredientPercentage()}%)");
            }

            return sb.ToString();
        }

    }

    /// <summary>
    /// A definition for each ingredient within the recipe.
    /// </summary>
    public class RecipeIngredient
    {
        private string ingredientName;
        private double ingredientPercentage;

        public string GetIngredientName() { return this.ingredientName; }
        public void SetIngredientName(string newName) { this.ingredientName = newName; }
        public double GetIngredientPercentage() { return this.ingredientPercentage; }
        public void SetIngredientPercentage(double newPercentage) { this.ingredientPercentage = newPercentage; }

        public RecipeIngredient(string ingredient, double percentage)
        {
            this.ingredientName = ingredient;
            this.ingredientPercentage = percentage;
        }
    }
}
