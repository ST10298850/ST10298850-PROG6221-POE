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
        private List<string> steps = new List<string>();

        public AddRecipeWindow()
        {
            InitializeComponent();
        }

        private void btnAddIngredient_Click(object sender, RoutedEventArgs e)
        {
            // Assuming you have TextBoxes for ingredient name, quantity, calories, and a ComboBox for food group
            var name = txtIngredientName.Text;
            var quantityText = txtIngredientQuantity.Text;
            var caloriesText = txtIngredientCalories.Text;
            //var unit = txtIngredientUnit.Text; // Assuming you have a TextBox for unit
            var unit = ((ComboBoxItem)cmbIngredientUnit.SelectedItem)?.Content.ToString();

            var foodGroup = cmbFoodGroup.SelectedItem as ComboBoxItem; // Assuming you have a ComboBox for food group

            if (!string.IsNullOrWhiteSpace(name) &&
                double.TryParse(quantityText, out double quantity) &&
                double.TryParse(caloriesText, out double calories) &&
                !string.IsNullOrWhiteSpace(unit) &&
                foodGroup != null)
            {
                var ingredient = new RecipeIngredient(name, quantity, unit, calories, foodGroup.Content.ToString());
                ingredients.Add(ingredient);
                lstIngredients.Items.Add(ingredient.ToString()); // Assuming you have a ListBox named lstIngredients
                ClearIngredientInputs();
            }
            else
            {
                MessageBox.Show("Please fill in all fields correctly.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnSaveRecipe_Click(object sender, RoutedEventArgs e)
        {
            string recipeName = txtRecipeName.Text;
            if (!string.IsNullOrEmpty(recipeName) && ingredients.Count > 0 && steps.Count > 0)
            {
                NewRecipe = new Recipe(recipeName, ingredients, steps);
                MessageBox.Show($"Recipe '{recipeName}' saved!", "Recipe Saved", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Please enter a recipe name, add at least one ingredient, and add at least one step.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void btnAddStep_Click(object sender, RoutedEventArgs e)
        {
            var step = txtStep.Text; // Assuming you have a TextBox named txtStep for the step description
            if (!string.IsNullOrWhiteSpace(step))
            {
                steps.Add(step);
                lstSteps.Items.Add(step); // Assuming you have a ListBox named lstSteps
                txtStep.Clear(); // Clear the step input field after adding
            }
            else
            {
                MessageBox.Show("Please enter a step.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearIngredientInputs()
        {
            // Clear the ingredient input fields after adding
            txtIngredientName.Clear();
            txtIngredientQuantity.Clear();
            txtIngredientCalories.Clear();
            cmbFoodGroup.SelectedIndex = -1; // Reset the ComboBox selection
        }
    }
}
