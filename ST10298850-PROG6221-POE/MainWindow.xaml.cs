using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ST10298850_PROG6221_POE;
using ST10298850_PROG6221_POE.Classes;

namespace RecipeApp
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<Recipe> recipes = new ObservableCollection<Recipe>();

        public MainWindow()
        {
            InitializeComponent();
            PopulateSampleData();
            cmbRecipes.ItemsSource = recipes; // Bind the ComboBox to the ObservableCollection

            // Initialize placeholders
            txtFilterIngredient.Text = "Ingredient Name";
            txtFilterIngredient.Foreground = new SolidColorBrush(Colors.Gray);
            txtFilterMaxCalories.Text = "Max Calories";
            txtFilterMaxCalories.Foreground = new SolidColorBrush(Colors.Gray);
        }

        private void btnScaleRecipe_Click(object sender, RoutedEventArgs e)
        {
            if (cmbRecipes.SelectedItem is Recipe selectedRecipe && cmbScaleFactor.SelectedItem is ComboBoxItem selectedScaleFactorItem)
            {
                if (double.TryParse(selectedScaleFactorItem.Tag.ToString(), out double scaleFactor))
                {
                    selectedRecipe.ScaleRecipe(scaleFactor);
                    RefreshUI(selectedRecipe); // Update UI with the scaled recipe
                    CheckCalories(selectedRecipe); // Check if the scaled recipe exceeds calorie limit
                }
                else
                {
                    MessageBox.Show("Invalid scale factor selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a recipe and a scale factor.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RefreshUI(Recipe recipe)
        {
            txtOutput.Text = recipe.Display();
        }

        private void PopulateSampleData()
        {
            // Your sample data population logic remains unchanged
        }

        private void AddRecipeWindow_ExceededCalories(object sender, EventArgs e)
        {
            MessageBox.Show($"The total calories of this recipe exceed the limit.", "Calories Exceeded", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void btnAddRecipe_Click(object sender, RoutedEventArgs e)
        {
            AddRecipeWindow addRecipeWindow = new AddRecipeWindow();
            var result = addRecipeWindow.ShowDialog(); // Show the window as a dialog
            if (result == true && addRecipeWindow.NewRecipe != null)
            {
                addRecipeWindow.NewRecipe.ExceededCalories += AddRecipeWindow_ExceededCalories;
                recipes.Add(addRecipeWindow.NewRecipe);
                CheckCalories(addRecipeWindow.NewRecipe); // Check if the new recipe exceeds calorie limit
            }
        }

        private void btnDisplayRecipes_Click(object sender, RoutedEventArgs e)
        {
            DisplayAllRecipes();
        }

        private void DisplayAllRecipes()
        {
            txtOutput.Clear();
            foreach (var recipe in recipes)
            {
                txtOutput.Text += recipe.Display();
            }
        }

        private void btnResetScale_Click(object sender, RoutedEventArgs e)
        {
            if (cmbRecipes.SelectedItem is Recipe selectedRecipe)
            {
                selectedRecipe.ResetScale();
                RefreshUI(selectedRecipe); // Update UI with the reset recipe
            }
        }

        private void cmbRecipes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbRecipes.SelectedItem is Recipe selectedRecipe)
            {
                txtOutput.Text = selectedRecipe.Display();
            }
        }

        private void btnFilterRecipes_Click(object sender, RoutedEventArgs e)
        {
            string filterIngredient = txtFilterIngredient.Text != "Ingredient Name" ? txtFilterIngredient.Text : string.Empty;
            double maxCalories = double.TryParse(txtFilterMaxCalories.Text, out double mc) ? mc : double.MaxValue;

            var filteredRecipes = recipes.Where(recipe =>
                recipe.Ingredients.Any(ingredient => ingredient.Name.Contains(filterIngredient, StringComparison.OrdinalIgnoreCase)) &&
                recipe.CalculateTotalCalories() <= maxCalories).ToList();

            cmbRecipes.ItemsSource = filteredRecipes;

            if (filteredRecipes.Count == 0)
            {
                txtOutput.Text = "No recipes match the filter criteria.";
            }
            else
            {
                txtOutput.Clear();
                foreach (var recipe in filteredRecipes)
                {
                    txtOutput.Text += recipe.Display();
                }
            }
        }
        private void btnViewSteps_Click(object sender, RoutedEventArgs e)
        {
            if (cmbRecipes.SelectedItem is Recipe selectedRecipe)
            {
                RecipeDisplay recipedisplay = new RecipeDisplay(selectedRecipe);
                recipedisplay.Show();
            }
            else
            {
                MessageBox.Show("Please select a recipe.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Placeholder simulation methods
        private void txtFilterIngredient_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtFilterIngredient.Text == "Ingredient Name")
            {
                txtFilterIngredient.Text = "";
                txtFilterIngredient.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void txtFilterIngredient_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFilterIngredient.Text))
            {
                txtFilterIngredient.Text = "Ingredient Name";
                txtFilterIngredient.Foreground = new SolidColorBrush(Colors.Gray);
            }
        }

        private void txtFilterMaxCalories_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtFilterMaxCalories.Text == "Max Calories")
            {
                txtFilterMaxCalories.Text = "";
                txtFilterMaxCalories.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void txtFilterMaxCalories_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFilterMaxCalories.Text))
            {
                txtFilterMaxCalories.Text = "Max Calories";
                txtFilterMaxCalories.Foreground = new SolidColorBrush(Colors.Gray);
            }
        }

        // Additional method to check calories after scaling or adding a recipe
        private void CheckCalories(Recipe recipe)
        {
            if (recipe.CalculateTotalCalories() > 300)
            {
                MessageBox.Show($"The total calories of the recipe '{recipe.Name}' exceed 300. Total Calories: {recipe.CalculateTotalCalories()}", "Calories Exceeded", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
