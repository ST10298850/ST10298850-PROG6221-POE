using ST10298850_PROG6221_POE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

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
            TriggerCalorieCheck();
        }

        // Methods
        public void AddIngredient(RecipeIngredient ingredient)
        {
            Ingredients.Add(ingredient);
            TriggerCalorieCheck();
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
            TriggerCalorieCheck();
        }

        public void ResetScale()
        {
            scaleAmount = 1;
            foreach (var ingredient in Ingredients)
            {
                ingredient.ResetQuantity();
            }

        }
        public event EventHandler<CaloriesEventArgs>? ExceededCalories;

        public event CaloriesNotificationHandler CaloriesNotification;

        protected virtual void OnCaloriesNotification(CaloriesEventArgs e)
        {
            CaloriesNotification?.Invoke(this, e);
        }

        private void TriggerCalorieCheck()
        {
            string message = GetCalorieMessage();
            OnCaloriesNotification(new CaloriesEventArgs(message));

        }
        public string GetCalorieMessage()
        {
            double totalCalories = CalculateTotalCalories();
            if (totalCalories < 200)
            {
                return "This recipe is low in calories, suitable for a snack.";
            }
            else if (totalCalories >= 200 && totalCalories <= 500)
            {
                return "This recipe has moderate calories, suitable for a balanced meal.";
            }
            else
            {
                return "This recipe is high in calories and should be consumed sparingly.";
            }
        }

        public double CalculateTotalCalories()
        {
            return Ingredients.Sum(ingredient => ingredient.Calories * (ingredient.Quantity / ingredient.OriginalQuantity));
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
        //            return Ingredients.Sum(ingredient => ingredient.Calories * ingredient.Quantity / ingredient.OriginalQuantity);


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

}//Complete

//----------------------------------------------------------------------------------------REFERENCES--------------------------------------------------------------------------------------------
//C# Tutorial (C Sharp). (n.d.). Retrieved May 27, 2024, from https://www.w3schools.com/cs/index.php 

//Chand, M. (2023, April 2). How to create a list in C#? Retrieved May 28, 2024, from https://www.c-sharpcorner.com/UploadFile/mahesh/create-a-list-in-C-Sharp/

//Sort a list alphabetically. (2021, February). Stack Overflow. Retrieved May 29, 2024, from https://stackoverflow.com/questions/6965337/sort-a-list-alphabetically

//----------------------------------------------------------------------------------------New References--------------------------------------------------------------------------------------------

//Adegeo. (2023, July 5).Create a new app with Visual Studio tutorial - WPF .NET. Microsoft Learn. https://learn.microsoft.com/en-us/dotnet/desktop/wpf/get-started/create-app-visual-studio?view=netdesktop-8.0

//Welcome - The complete WPF tutorial. (n.d.). https://wpf-tutorial.com/
//----------------------------------------------------------------------------------------REFERENCES--------------------------------------------------------------------------------------------

//public void DisplayCalorieInformation()
//{
//    var sb = new StringBuilder();
//    sb.AppendLine("Calorie Information:");
//    sb.AppendLine("- Less than 200 calories: Suitable for a snack.");
//    sb.AppendLine("- Between 200 and 500 calories: Suitable for a balanced meal.");
//    sb.AppendLine("- More than 500 calories: Should be consumed sparingly.");
//    MessageBox.Show(sb.ToString(), "Calorie Information", MessageBoxButton.OK, MessageBoxImage.Information);
//}

//private void CheckCalories()
//{
//    var totalCalories = CalculateTotalCalories();
//    string message = "";

//    if (totalCalories < 200)
//    {
//        message = "This recipe is low in calories, suitable for a snack.";
//    }
//    else if (totalCalories >= 200 && totalCalories <= 500)
//    {
//        message = "This recipe has moderate calories, suitable for a balanced meal.";
//    }
//    else if (totalCalories > 500)
//    {
//        message = "This recipe is high in calories and should be consumed sparingly.";
//    }

//    MessageBox.Show(message, "Calorie Notification", MessageBoxButton.OK, MessageBoxImage.Information);
//}