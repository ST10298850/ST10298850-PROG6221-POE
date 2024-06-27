using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ST10298850_PROG6221_POE.Classes
{
    public class Recipe
    {
        // Properties
        public string Name { get; set; }
        public List<RecipeIngredient> Ingredients { get; set; }
        public List<string> Steps { get; set; }
        private double scaleAmount = 1;

        // Constructor
        public Recipe(string name, List<RecipeIngredient> ingredients, List<string> steps)
        {
            Name = name;
            Ingredients = ingredients;
            Steps = steps;
            CheckCalories();
        }

        // Methods
        public void AddIngredient(RecipeIngredient ingredient)
        {
            Ingredients.Add(ingredient);
            CheckCalories();
        }

        public void AddStep(string step)
        {
            Steps.Add(step);
        }

        public override string ToString()
        {
            return Name;
        }

        public void ScaleRecipe(double scaleFactor)
        {
            scaleAmount *= scaleFactor;
            foreach (var ingredient in Ingredients)
            {
                ingredient.Scale(scaleFactor);
            }
            CheckCalories();
        }

        public void ResetScale()
        {
            scaleAmount = 1;
            foreach (var ingredient in Ingredients)
            {
                ingredient.ResetQuantity();
            }
            CheckCalories();
        }

        public event CaloriesNotificationHandler CaloriesNotification;

        protected virtual void OnCaloriesNotification(CaloriesEventArgs e)
        {
            CaloriesNotification?.Invoke(this, e);
        }

        public void CheckCalories()
        {
            double totalCalories = CalculateTotalCalories();
            if (totalCalories < 100) // Assuming 100 is the threshold for "too low"
            {
                OnCaloriesNotification(new CaloriesEventArgs("Calories too low"));
            }
            else if (totalCalories > 300)
            {
                OnCaloriesNotification(new CaloriesEventArgs("Calories exceed 300"));
            }
        }


        public double CalculateTotalCalories()
        {
            // Adjusted to calculate total calories considering the scaleAmount
            return Ingredients.Sum(ingredient => ingredient.Calories * ingredient.Quantity / ingredient.OriginalQuantity);
        }

        public string Display()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Recipe: {Name}");
            sb.AppendLine(new string('-', 20));
            sb.AppendLine("Ingredients:");
            foreach (var ingredient in Ingredients)
            {
                sb.AppendLine($"- {ingredient}");
            }
            sb.AppendLine("Steps:");
            for (int i = 0; i < Steps.Count; i++)
            {
                sb.AppendLine($"{i + 1}. {Steps[i]}");
            }
            sb.AppendLine($"Total Calories: {CalculateTotalCalories()}"); // Display total calories
            sb.AppendLine();
            return sb.ToString();
        }

        // Update the event handler to use CaloriesEventArgs, which will include the message
        public event EventHandler<CaloriesEventArgs>? ExceededCalories;


        // Define CaloriesEventArgs to pass messages
        public class CaloriesEventArgs : EventArgs
        {
            public string Message { get; }

            public CaloriesEventArgs(string message)
            {
                Message = message;
            }
        }
    }

}
