using ST10298850_PROG6221_POE.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ST10298850_PROG6221_POE
{
    /// <summary>
    /// Interaction logic for RecipeDisplay.xaml
    /// </summary>
    public partial class RecipeDisplay : Window
    {
        public RecipeDisplay(Recipe recipe) // Add this constructor
        {
            InitializeComponent();
            foreach (var step in recipe.Steps)
            {
                var checkBox = new CheckBox { Content = step, IsChecked = false };
                lstSteps.Items.Add(checkBox);
            }
        }
    }

}
