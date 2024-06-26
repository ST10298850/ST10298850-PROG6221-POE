using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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
                RefreshUI(selectedRecipe);
            }
        }

        private void cmbRecipes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbRecipes.SelectedItem is Recipe selectedRecipe)
            {
                txtOutput.Text = selectedRecipe.Display();
            }
        }
    }
}
