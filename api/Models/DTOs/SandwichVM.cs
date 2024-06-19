using System.ComponentModel.DataAnnotations;

namespace Sandwich.Models.DTOs
{
    public class SandwichVM
    {
        public int Id { get; set; }
        [Required]
        public int CustomerId { get; set; }
        public List<SandwichIngredientVM>? SandwichIngredients { get; set; }
        public double? Price { get; set; }
        public int? TotalCalories { get; set; }
        public List<IngredientVM>? Ingredients { get; set; }
    }

    public static class SandwichMappingExtensions
    {
        public static SandwichVM ToViewModel(this SandwichObj sandwich)
        {
            return new SandwichVM
            {
                Id = sandwich.Id,
                CustomerId = sandwich.CustomerId,
                SandwichIngredients = sandwich.SandwichIngredients?.Select(si => new SandwichIngredientVM
                {
                    Id = si.Id,
                    SandwichId = si.SandwichId,
                    IngredientId = si.IngredientId,
                    Ingredient = new IngredientVM{
                        Id = si.Ingredient.Id,
                        Name = si.Ingredient.Name,
                        Price = si.Ingredient.Price,
                        Calories = si.Ingredient.Calories,
                        TypeId = si.Ingredient.TypeId
                    }
                }).ToList(),
                Price = sandwich.Price,
                TotalCalories = sandwich.TotalCalories,
                Ingredients = sandwich.Ingredients?.Select(i => new IngredientVM
                {
                    Id = i.Id,
                        Name = i.Name,
                        Price = i.Price,
                        Calories = i.Calories,
                        TypeId = i.TypeId
                }).ToList()
            };
        }
    }
}