using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Sandwich.Models
{
    public class SandwichObj
    {
        public int Id { get; set; }
        
        [Required]
        public int CustomerId { get; set; }
        public UserProfile? Customer { get; set; }

        [Required]
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public Order? Order { get; set; }
        
        public List<SandwichIngredient>? SandwichIngredients { get; set; }

        [NotMapped]
        public double? Price
        {
            get
            {
                // Check if SandwichIngredients is null
                if (SandwichIngredients == null)
                    return null;

                // Calculate the total price
                return Math.Round(SandwichIngredients.Sum(si => si.Ingredient.Price), 2);
            }
        }

        [NotMapped]
        public int? TotalCalories
        {
            get
            {
                return SandwichIngredients?.Sum(si => si.Ingredient.Calories) ?? 0;
            }
        }

        [JsonPropertyName("Ingredients")]
        public List<Ingredient>? Ingredients { get; set; }
    }
}
