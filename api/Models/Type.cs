using System.ComponentModel.DataAnnotations;

namespace Sandwich.Models
{
    public class TypeObj
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        public List<Ingredient>? Ingredients { get; set; }
    }
}
