namespace ST10298850_PROG6221_POE.Classes
{
    public class RecipeIngredient
    {
        public string Name { get; set; }
        public double Quantity { get; set; }
        public string Unit { get; set; }
        public double Calories { get; set; }
        public string FoodGroup { get; set; }
        public double OriginalQuantity { get; private set; }

        public RecipeIngredient(string name, double quantity, string unit, double calories, string foodGroup)
        {
            Name = name;
            Quantity = quantity;
            Unit = unit;
            Calories = calories;
            FoodGroup = foodGroup;
            OriginalQuantity = quantity;
        }

        public void ResetQuantity()
        {
            Quantity = OriginalQuantity;
        }
        public void Scale(double scaleFactor)
        {
            Quantity = OriginalQuantity * scaleFactor;
        }

        public override string ToString()
        {
            return $"{Name} - {Quantity} {Unit} ({Calories} calories) [{FoodGroup}]";
        }
    }
}
