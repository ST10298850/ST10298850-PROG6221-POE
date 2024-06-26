using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using ST10298850_PROG6221_POE.Classes;

namespace RecipeApp
{
    public partial class AddRecipeWindow : Window
    {
        public Recipe? NewRecipe { get; private set; }
        private List<RecipeIngredient> ingredients = new List<RecipeIngredient>();

        public AddRecipeWindow()
        {
            InitializeComponent();
        }

        private void btnAddIngredient_Click(object sender, RoutedEventArgs e)
        {
            string ingredientName = txtIngredientName.Text;
            double.TryParse(txtIngredientQuantity.Text, out double ingredientQuantity);
            string ingredientUnit = txtIngredientUnit.Text;
            double.TryParse(txtIngredientCalories.Text, out double ingredientCalories);
            string foodGroup = ((ComboBoxItem)cmbFoodGroup.SelectedItem)?.Content?.ToString() ?? string.Empty;

            if (!string.IsNullOrEmpty(ingredientName) && ingredientQuantity > 0 && !string.IsNullOrEmpty(ingredientUnit) && ingredientCalories > 0 && !string.IsNullOrEmpty(foodGroup))
            {
                var ingredient = new RecipeIngredient(ingredientName, ingredientQuantity, ingredientUnit, ingredientCalories, foodGroup);
                ingredients.Add(ingredient);

                lstIngredients.Items.Add($"{ingredientName} - {ingredientQuantity} {ingredientUnit} ({ingredientCalories} calories) [{foodGroup}]");
            }
            else
            {
                MessageBox.Show("Please fill in all ingredient fields correctly.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnSaveRecipe_Click(object sender, RoutedEventArgs e)
        {
            string recipeName = txtRecipeName.Text;
            if (!string.IsNullOrEmpty(recipeName) && ingredients.Count > 0)
            {
                NewRecipe = new Recipe(recipeName, ingredients, new List<string>());
                MessageBox.Show($"Recipe '{recipeName}' saved!", "Recipe Saved", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true; // Close the window and return true
            }
            else
            {
                MessageBox.Show("Please enter a recipe name and add at least one ingredient.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false; // Close the window and return false
        }
    }
}
