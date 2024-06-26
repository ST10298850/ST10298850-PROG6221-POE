using System.Text;

namespace ST10298850_PROG6221_POE.Classes
{
    public class Recipe
    {
        public string Name { get; set; }
        public List<RecipeIngredient> Ingredients { get; set; }
        public List<string> Steps { get; set; }
        private double scaleAmount = 1;

        public Recipe(string name, List<RecipeIngredient> ingredients, List<string> steps)
        {
            Name = name;
            Ingredients = ingredients;
            Steps = steps;
        }

        public override string ToString()
        {
            return Name;
        }

        public void AddIngredient(RecipeIngredient ingredient)
        {
            Ingredients.Add(ingredient);
        }

        public void AddStep(string step)
        {
            Steps.Add(step);
        }

        public void UpdateScale(double scale)
        {
            scaleAmount = scale;
            foreach (var ingredient in Ingredients)
            {
                ingredient.Quantity = ingredient.OriginalQuantity * scaleAmount;
            }
        }

        // Method to scale the recipe
        public void ScaleRecipe(double scaleFactor)
        {
            foreach (var ingredient in Ingredients)
            {
                ingredient.Scale(scaleFactor);
            }
        }

        public void ResetScale()
        {
            foreach (var ingredient in Ingredients)
            {
                ingredient.ResetQuantity();
            }
            scaleAmount = 1;
        }

        public double CalculateTotalCalories()
        {
            return Ingredients.Sum(ingredient => ingredient.Calories * ingredient.Quantity);
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
            foreach (var step in Steps)
            {
                sb.AppendLine($"- {step}");
            }
            sb.AppendLine($"Total Calories: {CalculateTotalCalories()}");
            sb.AppendLine();
            return sb.ToString();
        }
    }
}
