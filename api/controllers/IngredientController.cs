using Microsoft.AspNetCore.Mvc;
using Sandwich.Models;
using Sandwich.Data;
using Sandwich.Models.DTOs;

namespace Sandwich.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IngredientController : ControllerBase
    {
        private readonly SandwichDbContext _dbContext;
        public IngredientController(SandwichDbContext context)
        {
            _dbContext = context;
        }
        [HttpGet("{input}")]
        public IActionResult GetIngredients(int input)
        {
            if (input > 3 || input < 1)
            {
                return StatusCode(416, "Range not satisfiable. Input must be values 1, 2, or 3!");
            }

            List<Ingredient> ingredients = new List<Ingredient>();

            switch (input)
            {
                // bread
                case 1:
                ingredients =  _dbContext.Ingredients.Where(i => i.TypeId == 1).ToList();
                break;
                // meat
                case 2:
                ingredients =  _dbContext.Ingredients.Where(i => i.TypeId == 2).ToList();
                break;
                //toppings
                case 3:
                ingredients =  _dbContext.Ingredients.Where(i => i.TypeId == 3 || i.TypeId == 4).ToList();
                break;
            }
            
            
            return Ok(ingredients.Select(b => new IngredientDTO
            {
                Id = b.Id,
                Name = b.Name,
                Price = b.Price,
                Calories = b.Calories,
                TypeId = b.TypeId
            }).ToList());
        }
    }
}