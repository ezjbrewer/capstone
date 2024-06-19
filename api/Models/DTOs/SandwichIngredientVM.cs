using System.ComponentModel.DataAnnotations;

namespace Sandwich.Models.DTOs
{
    public class SandwichIngredientVM
    {
        public int Id { get; set; }
        [Required]
        public int SandwichId { get; set; }
        [Required]
        public int IngredientId { get; set; }
        public IngredientVM Ingredient { get; set; }
    }
}