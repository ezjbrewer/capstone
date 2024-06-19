namespace Sandwich.Models.DTOs;

public class SandwichIngredientDTO
{
    public int Id { get; set; }
    public int SandwichId { get; set; }
    public SandwichDTO Sandwich { get; set; }
    public int IngredientId { get; set; }
    public IngredientDTO Ingredient { get; set; }
}