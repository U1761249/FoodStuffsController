using Microsoft.VisualStudio.TestTools.UnitTesting;
using FoodStuffsController.src;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodStuffsController.src.Tests
{
    [TestClass()]
    public class RecipeTests
    {

        Recipe r1;

        RecipeIngredient ri1;
        RecipeIngredient ri2;

        /// <summary>
        /// Setup the recipe ready for testing.
        /// </summary>
        [TestInitialize]
        public void BeforeEach()
        {
            r1 = new Recipe("Recipe 1");

            ri1 = new RecipeIngredient("Ingredient 1", 50);
            ri2 = new RecipeIngredient("Ingredient 2", 50);

            r1.addIngredient(ri1);
            r1.addIngredient(ri2);

        }

        //----------------------------------------------------------------------------
        // Test valid and invalid inputs to Remove


        /// <summary>
        /// Attempt to remove an ingredient by object reference.
        /// The name is in the list.
        [TestMethod()]
        public void removeIngredientTestObjectPresent()
        {
            r1.removeIngredient(ri2);
            Assert.AreEqual(ri1, r1.getIngredients()[0]);
        }

        /// <summary>
        /// Attempt to remove an ingredient by name.
        /// The name is in the list.
        [TestMethod()]
        public void removeIngredientTestNamePresent()
        {
            r1.removeIngredient(ri2.getIngredientName());
            Assert.AreEqual(ri1, r1.getIngredients()[0]);
        }

        /// <summary>
        /// Attempt to remove an ingredient by object reference.
        /// The name is not in the list.
        [TestMethod()]
        public void removeIngredientTestObjectMissing()
        {
            RecipeIngredient ri3 = new RecipeIngredient("Ingredient 3", 50);
            List<RecipeIngredient> expected = new List<RecipeIngredient>();
            expected.Add(ri1);
            expected.Add(ri2);

            r1.removeIngredient(ri3);
            CollectionAssert.AreEqual(expected, r1.getIngredients());
        }

        /// <summary>
        /// Attempt to remove an ingredient by name.
        /// The name is not in the list.
        /// </summary>
        [TestMethod()]
        public void removeIngredientTestNameMissing()
        {
            r1.removeIngredient("Ingredient 3");
            List<RecipeIngredient> expected = new List<RecipeIngredient>();
            expected.Add(ri1);
            expected.Add(ri2);

            CollectionAssert.AreEqual(expected, r1.getIngredients());
        }

        //----------------------------------------------------------------------------
        // Test other functionality

        /// <summary>
        /// Test getting the name of the recipe.
        /// </summary>
        [TestMethod()]
        public void getRecipeNameTest()
        {
            Assert.AreEqual("Recipe 1", r1.getRecipeName());
        }

        /// <summary>
        /// Test setting the name of the recipe.
        /// </summary>
        [TestMethod()]
        public void setRecipeNameTest()
        {
            r1.setRecipeName("New Name");
            Assert.AreEqual("New Name", r1.getRecipeName());
        }

        /// <summary>
        /// Test the retrieval of an ingredients list.
        /// </summary>
        [TestMethod()]
        public void getIngredientsTest()
        {
            List<RecipeIngredient> expectedOut = new List<RecipeIngredient>();
            expectedOut.Add(ri1);
            expectedOut.Add(ri2);


            CollectionAssert.AreEqual(expectedOut, r1.getIngredients());
        }

        /// <summary>
        /// Test that the list of RecipeIngredients can be replaced.
        /// </summary>
        [TestMethod()]
        public void setIngredientsTest()
        {
            List<RecipeIngredient> expectedOut = new List<RecipeIngredient>();
            expectedOut.Add(ri1);
            expectedOut.Add(ri2);
            expectedOut.Add(new RecipeIngredient("Ingredient 3", 50));

            r1.setIngredients(expectedOut);
            CollectionAssert.AreEqual(expectedOut, r1.getIngredients());
        }
        
        /// <summary>
        /// Create an expected output and compare to the generated string for ingredients.
        /// </summary>
        [TestMethod()]
        public void getIngredientStringTest()
        {
            string expectedOut;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Ingredient 1 (50%)");
            sb.AppendLine("Ingredient 2 (50%)");

            expectedOut = sb.ToString();

            string actual = r1.getIngredientString();
            Assert.AreEqual(expectedOut, actual, actual);
        }
    }
}