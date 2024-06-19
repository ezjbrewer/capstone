namespace Sandwich.Models.DTOs
{
    public class SandwichDTO
    {
        public int Id { get; set; }

        public List<SandwichIngredientDTO> SandwichIngredients { get; set; }

        public double Price
        {
            get
            {
                double totalPrice = SandwichIngredients?.Where(si => si.Ingredient != null)
                                                        .Sum(si => si.Ingredient.Price) ?? 0;
                return Math.Round(totalPrice, 2);
            }
        }

        public int TotalCalories
        {
            get
            {
                return SandwichIngredients?.Where(si => si.Ingredient != null)
                                           .Sum(si => si.Ingredient.Calories) ?? 0;
            }
        }

        public int CustomerId { get; set; }
    }
}