using System.Collections.Generic;
using System.Collections.ObjectModel;
using ST10298850_PROG6221_POE.Classes;

namespace RecipeApp
{
    public static class SampleDataPopulator
    {
        public static void PopulateSampleData(ObservableCollection<Recipe> recipes)
        {
            var ingredients1 = new List<RecipeIngredient>
    {
        new RecipeIngredient("Whole wheat bread", 2, "slices", 100, "Starchy foods"),
        new RecipeIngredient("Sugar", 1, "cups", 150, "Sweets"),
        new RecipeIngredient("Eggs", 2, "pieces", 70, "Chicken, fish, meat and eggs")
    };

            // Sample steps for a recipe
            var steps1 = new List<string>
    {
        "Toast the bread slices",
        "Mix sugar and eggs",
        "Bake for 30 minutes at 350 degrees"
    };

            // Create a sample recipe and add it to the recipes collection
            var recipe1 = new Recipe("Cake", ingredients1, steps1);
            recipes.Add(recipe1);

            // Example for another recipe
            var ingredients2 = new List<RecipeIngredient>
    {
        new RecipeIngredient("Tomato", 3, "pieces", 20, "Vegetables and fruits"),
        new RecipeIngredient("Cheese", 1, "cups", 200, "Milk and dairy products"),
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

            // Sample Recipe 1
            var ingredients3 = new List<RecipeIngredient>
    {
        new RecipeIngredient("Banana", 2, "pieces", 89, "Vegetables and fruits"),
        new RecipeIngredient("Yogurt", 1, "cups", 154, "Milk and dairy products"),
        new RecipeIngredient("Honey", 2, "tablespoons", 128, "Sweets")
    };

            var steps3 = new List<string>
    {
        "Peel bananas",
        "Blend all ingredients"
    };

            var recipe3 = new Recipe("Banana Smoothie", ingredients3, steps3);
            recipes.Add(recipe3);

            // Sample Recipe 2
            var ingredients4 = new List<RecipeIngredient>
    {
        new RecipeIngredient("Chicken Breast", 200, "grams", 165, "Chicken, fish, meat and eggs"),
        new RecipeIngredient("Olive Oil", 1, "tablespoons", 119, "Fats and oils"),
        new RecipeIngredient("Salt", 0.5, "teaspoons", 0, "Seasonings")
    };

            var steps4 = new List<string>
    {
        "Season chicken breast",
        "Cook chicken breast in pan"
    };

            var recipe4 = new Recipe("Pan-Seared Chicken", ingredients4, steps4);
            recipes.Add(recipe4);

            // Sample Recipe 3
            var ingredients5 = new List<RecipeIngredient>
    {
        new RecipeIngredient("Mixed Vegetables", 3, "cups", 118, "Vegetables and fruits"),
        new RecipeIngredient("Brown rice", 1, "cups", 206, "Starchy foods"),
        new RecipeIngredient("Soy Sauce", 2, "tablespoons", 34, "Seasonings")
    };

            var steps5 = new List<string>
    {
        "Cook brown rice",
        "Stir-fry mixed vegetables",
        "Mix all together with soy sauce"
    };

            var recipe5 = new Recipe("Vegetable Fried Rice", ingredients5, steps5);
            recipes.Add(recipe5);

            // Additional sample recipes can be added here following the same pattern
        }
    }
}
