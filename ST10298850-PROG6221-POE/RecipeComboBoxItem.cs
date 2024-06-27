using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ST10298850_PROG6221_POE.Classes
{
    public class RecipeComboBoxItem
    {
        public Recipe Recipe { get; set; }

        public string DisplayText
        {
            get
            {
                var foodGroups = string.Join(", ", Recipe.Ingredients.Select(i => i.FoodGroup).Distinct());
                return $"{Recipe.Name} - Food Groups: {foodGroups} - Total Calories: {Recipe.CalculateTotalCalories()}";
            }
        }

        public override string ToString()
        {
            return DisplayText;
        }
    }
}

