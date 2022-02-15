using FoodStuffsController.src;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoodStuffsController.gui.MessageBoxes
{
    public partial class NewRecipe : Form
    {
        FeedBinController controller;
        List<string> ingredients;
        DataTable contents;


        public NewRecipe()
        {
            InitializeComponent();
            controller = FeedBinController.getInstance();
            contents = new DataTable();
            ingredients = controller.getIngredientList();

            UpdateListView();

            contents.Columns.Add("Ingredient");
            contents.Columns.Add("Percentage");

            gvRecipeContents.DataSource = contents;
            gvRecipeContents.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            gvRecipeContents.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void UpdateListView()
        {
            lbIngredientList.Items.Clear();
            foreach (string i in ingredients) { lbIngredientList.Items.Add(i); }
        }

        public Recipe getRecipe()
        {
            Recipe r = new Recipe();
            r.setRecipeName(tbRecipeName.Text);
            List<RecipeIngredient> ingredientsList = new List<RecipeIngredient>();
            for (int i = 0; i < contents.Rows.Count; i++)
            {
                ingredientsList.Add(
                    new RecipeIngredient(
                        contents.Rows[i][0].ToString(),
                        Convert.ToDouble(contents.Rows[i][1])
                    )
                );
            }
            r.setIngredients(ingredientsList);
            return r;
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            string ingredient = "";
            if (PopupBoxes.InputBox("New Product", "What is the new product:", ref ingredient) == DialogResult.OK)
            {
                if (!String.IsNullOrWhiteSpace(ingredient))
                {
                    ingredients.Add(ingredient);
                    UpdateListView();
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (lbIngredientList.SelectedItem == null)
            {
                PopupBoxes.ShowError("No ingredient selected.", "An ingredient must be selected to add.");
                return;
            }

            string ingredientName = lbIngredientList.SelectedItem.ToString();
            string sQuantity = "";
            if (PopupBoxes.InputBox("Ingredient Quantity", $"What percentage of the batch is made up of {ingredientName}:", ref sQuantity) == DialogResult.OK)
            {
                try
                {
                    double dQuantity = Convert.ToDouble(sQuantity);
                    DataRow r = contents.NewRow();
                    r["Ingredient"] = ingredientName;
                    r["Percentage"] = dQuantity;

                    contents.Rows.Add(r);
                }
                catch (Exception err)
                {
                    PopupBoxes.ShowError();
                }
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {

        }
    }
}
