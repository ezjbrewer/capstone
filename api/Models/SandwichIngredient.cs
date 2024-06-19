using System.ComponentModel.DataAnnotations;

namespace Sandwich.Models;
public class SandwichIngredient
{
    public int Id { get; set; }
    [Required]
    public int SandwichId { get; set; }
    public SandwichObj? Sandwich { get; set; }
    [Required]
    public int IngredientId { get; set; }
    public Ingredient? Ingredient { get; set; }
}