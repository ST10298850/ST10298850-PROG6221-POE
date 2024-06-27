using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ST10298850_PROG6221_POE;
using ST10298850_PROG6221_POE.Classes;
using static ST10298850_PROG6221_POE.Classes.Recipe;

namespace RecipeApp
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<Recipe> recipes = new ObservableCollection<Recipe>();

        public MainWindow()
        {
            InitializeComponent();
            cmbRecipes.ItemsSource = recipes; // Bind the ItemsSource of the ComboBox
            SampleDataPopulator.PopulateSampleData(recipes); // Populate sample data
            RefreshAndSortRecipes();
            foreach (var recipe in recipes)
            {
                recipe.CaloriesNotification += Recipe_CaloriesNotification;
            }
            // Initialize placeholders
            txtFilterIngredient.Text = "Ingredient Name";
            txtFilterIngredient.Foreground = new SolidColorBrush(Colors.Gray);
            txtFilterMaxCalories.Text = "Max Calories";
            txtFilterMaxCalories.Foreground = new SolidColorBrush(Colors.Gray);
            
        }
        // Event handler for the CaloriesNotification event
        private void Recipe_CaloriesNotification(object sender, CaloriesEventArgs e)
        {
            MessageBox.Show(e.Message); // Displaying the message in a simple MessageBox
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

        private void AddRecipeWindow_ExceededCalories(object? sender, CaloriesEventArgs e)
        {
            MessageBox.Show(e.Message, "Calorie Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }


        private void btnAddRecipe_Click(object sender, RoutedEventArgs e)
        {
            AddRecipeWindow addRecipeWindow = new AddRecipeWindow();
            var result = addRecipeWindow.ShowDialog(); // Show the window as a dialog
            if (result == true && addRecipeWindow.NewRecipe != null)
            {
                addRecipeWindow.NewRecipe.ExceededCalories += AddRecipeWindow_ExceededCalories;
                recipes.Add(addRecipeWindow.NewRecipe);
                RefreshAndSortRecipes();
                CheckCalories(addRecipeWindow.NewRecipe); // Check if the new recipe exceeds calorie limit
                //addRecipeWindow.NewRecipe.DisplayCalorieInformation(); // Display calorie information
            }
        }
        private void cmbRecipes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbRecipes.SelectedItem is Recipe selectedRecipe)
            {
                string calorieMessage = selectedRecipe.GetCalorieMessage();
                MessageBox.Show(calorieMessage, "Calorie Information");
                RefreshUI(selectedRecipe); // Update UI with the selected recipe
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


        private void btnFilterRecipes_Click(object sender, RoutedEventArgs e)
        {
            string filterIngredient = txtFilterIngredient.Text == "Ingredient Name" ? string.Empty : txtFilterIngredient.Text;
            string filterFoodGroup = (cmbFilterFoodGroup.SelectedItem as ComboBoxItem)?.Content.ToString();
            double maxCalories = double.TryParse(txtFilterMaxCalories.Text, out double mc) ? mc : double.MaxValue;

            var filteredRecipes = new List<Recipe>();

            foreach (var recipe in recipes)
            {
                bool matchesIngredientFilter = string.IsNullOrEmpty(filterIngredient) ||
                    recipe.Ingredients.Any(ingredient => ingredient.Name.Contains(filterIngredient, StringComparison.OrdinalIgnoreCase));

                bool matchesFoodGroupFilter = string.IsNullOrEmpty(filterFoodGroup) ||
                    recipe.Ingredients.Any(ingredient => ingredient.FoodGroup.Equals(filterFoodGroup, StringComparison.OrdinalIgnoreCase));

                bool matchesCalorieFilter = recipe.CalculateTotalCalories() <= maxCalories;

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
                    txtOutput.Text += recipe.Display() + Environment.NewLine;
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
            const double calorieLimit = 500.0; // Define a calorie limit for recipes
            if (recipe.CalculateTotalCalories() > calorieLimit)
            {
                MessageBox.Show($"The total calories of {recipe.Name} exceed the limit of {calorieLimit} calories.", "Calories Exceeded", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void btnPrintFoodGroups_Click(object sender, RoutedEventArgs e)
        {
            string foodGroupsInfo = GetFoodGroupsInfo();
            txtOutput.Text = foodGroupsInfo;
        }
        private void RefreshAndSortRecipes()
        {
            // Sort the recipes list alphabetically by Name
            var sortedRecipes = new ObservableCollection<Recipe>(recipes.OrderBy(r => r.Name));

            // Reassign the sorted list back to the recipes ObservableCollection
            recipes = sortedRecipes;

            // Rebind the sorted recipes to the combo box
            cmbRecipes.ItemsSource = recipes;

            // Do not auto-select the first item in the combo box
            // if (cmbRecipes.Items.Count > 0)
            // {
            //     cmbRecipes.SelectedIndex = 0;
            // }
        }

        private string GetFoodGroupsInfo()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Food Groups and Examples:");
            sb.AppendLine("1. Starchy foods:");
            sb.AppendLine("   - Examples: Pap, Samp, Brown rice, Potatoes, Whole wheat bread, Whole wheat pasta");
            sb.AppendLine();
            sb.AppendLine("2. Vegetables and fruits:");
            sb.AppendLine("   - Examples: Apple, Pear, Peach, Orange, Mango, Cabbage, Pumpkin, Carrots, Spinach, Broccoli, Cauliflower, Tomato");
            sb.AppendLine();
            sb.AppendLine("3. Dry beans, peas, lentils and soya:");
            sb.AppendLine("   - Examples: Chickpeas, Kidney beans, Green peas, Black beans, Soy beans, Split peas");
            sb.AppendLine();
            sb.AppendLine("4. Chicken, fish, meat and eggs:");
            sb.AppendLine("   - Examples: Skinless chicken, Lean meat, Mince, Canned fish, Frozen fish, Eggs");
            sb.AppendLine();
            sb.AppendLine("5. Milk and dairy products:");
            sb.AppendLine("   - Examples: Low fat milk, Cottage cheese, Plain yoghurt, Amasi");
            sb.AppendLine();
            sb.AppendLine("6. Fats and oils:");
            sb.AppendLine("   - Examples: Avocado, Olive oil, Nuts and seeds, Flax seed");
            sb.AppendLine();
            sb.AppendLine("7. Water:");
            sb.AppendLine("   - Aim to drink 6 to 8 glasses of water each day to help keep your body hydrated.");
            return sb.ToString();
        }
        // MainWindow.xaml.cs

        private void btnFoodGroupInfo_Click(object sender, RoutedEventArgs e)
        {
            string info = "Food Groups and Calories:\n" +
                          "- Fruits and Vegetables: low in calories, high in vitamins.\n" +
                          "- Grains: good source of energy and fiber.\n" +
                          "- Protein Foods: essential for body repair and growth.\n" +
                          "- Dairy: important for calcium and vitamin D.\n\n" +
                          "Calories are a measure of energy. Low-calorie meals are suitable for snacks, " +
                          "moderate-calorie meals for balanced meals, and high-calorie meals should be consumed sparingly.";
            MessageBox.Show(info, "Food Group and Calorie Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}