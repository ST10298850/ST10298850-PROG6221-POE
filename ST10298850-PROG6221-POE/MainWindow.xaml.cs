using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ST10298850_PROG6221_POE.Classes;

namespace RecipeApp
{
    public partial class MainWindow : Window
    {
        private List<Recipe> recipes = new List<Recipe>();

        public MainWindow()
        {
            InitializeComponent();
            PopulateSampleData(); // Example: Populate initial data
        }

        private void btnScaleRecipe_Click(object sender, RoutedEventArgs e)
        {
            if (cmbRecipes.SelectedItem is Recipe selectedRecipe && cmbScaleFactor.SelectedItem is ComboBoxItem selectedScaleFactorItem)
            {
                if (double.TryParse(selectedScaleFactorItem.Tag.ToString(), out double scaleFactor))
                {
                    selectedRecipe.ScaleRecipe(scaleFactor);
                    RefreshUI(selectedRecipe); // Update UI with the scaled recipe
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
            recipes.Add(new Recipe("Pasta Carbonara", new List<RecipeIngredient>
            {
                new RecipeIngredient("Spaghetti", 200, "g", 300, "Pasta"),
                new RecipeIngredient("Bacon", 150, "g", 250, "Meat"),
                new RecipeIngredient("Egg", 2, "units", 150, "Dairy"),
                new RecipeIngredient("Parmesan", 50, "g", 200, "Dairy")
            }, new List<string>
            {
                "Boil spaghetti until al dente",
                "Fry bacon until crispy",
                "Mix eggs and Parmesan cheese",
                "Combine all ingredients and serve immediately"
            }));

            recipes.Add(new Recipe("Chicken Stir-Fry", new List<RecipeIngredient>
            {
                new RecipeIngredient("Chicken Breast", 300, "g", 250, "Meat"),
                new RecipeIngredient("Bell Pepper", 2, "units", 50, "Vegetable"),
                new RecipeIngredient("Broccoli", 1, "head", 100, "Vegetable"),
                new RecipeIngredient("Soy Sauce", 50, "ml", 20, "Condiment")
            }, new List<string>
            {
                "Cut chicken into strips",
                "Stir-fry chicken until cooked",
                "Add vegetables and stir-fry until tender",
                "Season with soy sauce and serve hot"
            }));

            cmbRecipes.ItemsSource = recipes;
        }

        private void btnAddRecipe_Click(object sender, RoutedEventArgs e)
        {
            AddRecipeWindow addRecipeWindow = new AddRecipeWindow();
            addRecipeWindow.ShowDialog(); // Show the window as a dialog
            if (addRecipeWindow.NewRecipe != null)
            {
                recipes.Add(addRecipeWindow.NewRecipe);
                cmbRecipes.ItemsSource = null; // Refresh ComboBox
                cmbRecipes.ItemsSource = recipes;
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
            foreach (var recipe in recipes)
            {
                recipe.ResetScale();
            }
            if (cmbRecipes.SelectedItem is Recipe selectedRecipe)
            {
                RefreshUI(selectedRecipe);
            }
            else
            {
                DisplayAllRecipes();
            }
        }

        private void cmbRecipes_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (cmbRecipes.SelectedItem != null && cmbRecipes.SelectedItem is Recipe selectedRecipe)
            {
                txtOutput.Clear();
                txtOutput.Text += selectedRecipe.Display();
            }
        }
    }
}
