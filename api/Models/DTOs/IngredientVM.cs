using System.ComponentModel.DataAnnotations;

namespace Sandwich.Models.DTOs
{
    public class IngredientVM
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set;}
        [Required]
        public double Price { get; set; }
        [Required]
        public int Calories { get; set; }
        [Required]
        public int TypeId { get; set; }
        public TypeObj Type {get; set; }
    }
}