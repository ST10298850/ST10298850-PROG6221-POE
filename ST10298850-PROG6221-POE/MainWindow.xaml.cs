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
            recipes = new ObservableCollection<Recipe>();
            cmbRecipes.ItemsSource = recipes; // Bind the ItemsSource of the ComboBox
            PopulateSampleData(); // Populate sample data

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
        private void PopulateSampleData()
        {
            // Sample ingredients for a recipe
            var ingredients1 = new List<RecipeIngredient>
    {
        new RecipeIngredient("Flour", 2, "cups", 100, "Grains"),
        new RecipeIngredient("Sugar", 1, "cups", 150, "Sweets"),
        new RecipeIngredient("Eggs", 2, "pieces", 70, "Protein")
    };

            // Sample steps for a recipe
            var steps1 = new List<string>
    {
        "Mix all ingredients",
        "Bake for 30 minutes at 350 degrees"
    };

            // Create a sample recipe and add it to the recipes collection
            var recipe1 = new Recipe("Cake", ingredients1, steps1);
            recipes.Add(recipe1);

            // Example for another recipe
            var ingredients2 = new List<RecipeIngredient>
    {
        new RecipeIngredient("Tomato", 3, "pieces", 20, "Vegetables"),
        new RecipeIngredient("Cheese", 1, "cups", 200, "Dairy"),
        new RecipeIngredient("Basil", 5, "leaves", 5, "Herbs")
    };

            var steps2 = new List<string>
    {
        "Slice tomatoes and cheese",
        "Layer tomatoes, cheese, and basil",
        "Serve fresh"
    };

            var recipe2 = new Recipe("Caprese Salad", ingredients2, steps2);
            recipes.Add(recipe2);

            // No need to manually add items to cmbRecipes since it's bound to the recipes collection
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
                txtOutput.Text += recipe.Display() + Environment.NewLine;
            }
        }
        private void btnClearFilters_Click(object sender, RoutedEventArgs e)
        {
            // Clear filter inputs
            txtFilterIngredient.Text = "Ingredient Name";
            txtFilterMaxCalories.Text = "Max Calories";
            cmbFilterFoodGroup.SelectedIndex = -1;

            // Reset the ItemsSource of cmbRecipes to the full list of recipes
            cmbRecipes.ItemsSource = recipes;

            // Optionally, you can also reset the foreground color of the filter TextBoxes
            txtFilterIngredient.Foreground = new SolidColorBrush(Colors.Gray);
            txtFilterMaxCalories.Foreground = new SolidColorBrush(Colors.Gray);

            // Display all recipes in the output TextBox
            DisplayAllRecipes();
        }


        private void btnResetScale_Click(object sender, RoutedEventArgs e)
        {
            foreach (var recipe in recipes)
            {
                recipe.ResetScale();
            }
            DisplayAllRecipes();
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
            string filterIngredient = txtFilterIngredient.Text;
            string filterFoodGroup = (cmbFilterFoodGroup.SelectedItem as ComboBoxItem)?.Content.ToString();
            double maxCalories = double.TryParse(txtFilterMaxCalories.Text, out double mc) ? mc : double.MaxValue;

            var filteredRecipes = new List<Recipe>();

            foreach (var recipe in recipes)
            {
                bool matchesIngredientFilter = string.IsNullOrEmpty(filterIngredient);
                bool matchesFoodGroupFilter = string.IsNullOrEmpty(filterFoodGroup);
                bool matchesCalorieFilter = recipe.CalculateTotalCalories() <= maxCalories;

                if (!matchesIngredientFilter)
                {
                    foreach (var ingredient in recipe.Ingredients)
                    {
                        if (ingredient.Name.Contains(filterIngredient, StringComparison.OrdinalIgnoreCase))
                        {
                            matchesIngredientFilter = true;
                            break;
                        }
                    }
                }

                if (!matchesFoodGroupFilter)
                {
                    foreach (var ingredient in recipe.Ingredients)
                    {
                        if (ingredient.FoodGroup.Equals(filterFoodGroup, StringComparison.OrdinalIgnoreCase))
                        {
                            matchesFoodGroupFilter = true;
                            break;
                        }
                    }
                }

                if (matchesIngredientFilter && matchesFoodGroupFilter && matchesCalorieFilter)
                {
                    filteredRecipes.Add(recipe);
                }
            }

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
