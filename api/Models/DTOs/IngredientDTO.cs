namespace Sandwich.Models.DTOs;

public class IngredientDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public int Calories { get; set; }
    public int TypeId { get; set; }
    public List<SandwichIngredientDTO> SandwichIngredients { get; set;}
}