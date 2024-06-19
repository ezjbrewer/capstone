using Microsoft.AspNetCore.Mvc;
using Sandwich.Models;
using Sandwich.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Sandwich.Models.DTOs;

namespace Sandwich.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SandwichController : ControllerBase
    {
        private readonly SandwichDbContext _dbContext;

        public SandwichController(SandwichDbContext context)
        {
            _dbContext = context;
        }

        [HttpGet("users/{userId}")]
        public IActionResult GetSandwichesByUser(int userId)
        {
            UserProfile doesUserExists = _dbContext.UserProfiles.FirstOrDefault(u => u.Id == userId);

            if (doesUserExists == null)
            {
                return NotFound("User does not exist");
            }

            List<SandwichDTO> sandwiches = _dbContext.Sandwiches
                .Include(s => s.SandwichIngredients)
                .ThenInclude(si => si.Ingredient)
                .Where(s => s.CustomerId == userId)
                .Select(s => new SandwichDTO
                {
                    Id = s.Id,
                    CustomerId = s.CustomerId,
                    SandwichIngredients = s.SandwichIngredients.Select(si => new SandwichIngredientDTO
                    {
                        Id = si.Id,
                        SandwichId = si.SandwichId,
                        IngredientId = si.IngredientId,
                        Ingredient = new IngredientDTO
                        {
                            Id = si.Ingredient.Id,
                            Name = si.Ingredient.Name,
                            Price = si.Ingredient.Price,
                            Calories = si.Ingredient.Calories,
                            TypeId = si.Ingredient.TypeId
                        }
                    }).ToList()
                })
                .ToList();

            return Ok(sandwiches);
        }
        
        [HttpDelete("{sandwichId}")]
        public IActionResult DeleteSandwich(int sandwichId)
        {
            SandwichObj sandwichToDelete = _dbContext.Sandwiches.FirstOrDefault(s => s.Id == sandwichId);

            if (sandwichToDelete == null)
            {
                return NotFound();
            }

            _dbContext.Sandwiches.Remove(sandwichToDelete);
            _dbContext.SaveChanges();

            return Accepted("Sandwich successfully deleted");
        }

        [HttpPost("/api/Sandwich/post")]
        public IActionResult PostSandwich([FromBody] SandwichObj sandwich)
        {
            if (sandwich.Ingredients == null || !sandwich.Ingredients.Any())
            {
                return BadRequest("Ingredients are required.");
            }
            
            SandwichObj newSandwich = new SandwichObj
            {
                CustomerId = sandwich.CustomerId,
                OrderId = sandwich.OrderId 
            };

            _dbContext.Sandwiches.Add(newSandwich);
            _dbContext.SaveChanges();

            List<SandwichIngredient> newSandwichIngredients = sandwich.Ingredients.Select(i => new SandwichIngredient
            {
                SandwichId = newSandwich.Id,
                IngredientId = i.Id,
            })
            .ToList();

            _dbContext.SandwichIngredients.AddRange(newSandwichIngredients);
            _dbContext.SaveChanges();

            foreach (var ingredient in sandwich.Ingredients)
            {
                newSandwich.SandwichIngredients.Add(new SandwichIngredient
                {
                    SandwichId = newSandwich.Id,
                    IngredientId = ingredient.Id
                });
            }

            List<SandwichIngredient> sandwichIngredients = _dbContext.SandwichIngredients
                                    .Include(si => si.Ingredient)
                                    .Where(si => si.SandwichId == sandwich.Id)
                                    .ToList();

            newSandwich.SandwichIngredients = sandwichIngredients;

            var sandwichVM = newSandwich.ToViewModel();

            return Ok(sandwichVM);
        }

        [HttpGet("{id}")]
        public IActionResult GetSandwichById(int id)
        {
            SandwichObj sandwich = _dbContext.Sandwiches.FirstOrDefault(s => s.Id == id);

            if (sandwich == null)
            {
                return NotFound("Sandwich not found!");
            }

            sandwich.SandwichIngredients = _dbContext.SandwichIngredients.Include(si => si.Ingredient).Where(si => si.SandwichId == id).ToList();
            
            return Ok(sandwich);
        }

        [HttpPut("{sandwichId}")]
        public IActionResult UpdateSandwich(int sandwichId, [FromBody] SandwichObj updatedSandwich)
        {

            if (sandwichId != updatedSandwich.Id)
            {
                return BadRequest("Mismatch between sandwichId in the URL and in the request body.");
            }

            SandwichObj existingSandwich = _dbContext.Sandwiches
                .Include(s => s.SandwichIngredients)
                    .ThenInclude(si => si.Ingredient)
                .FirstOrDefault(s => s.Id == sandwichId);

            if (existingSandwich == null)
            {
                return NotFound("Sandwich not found.");
            }

            var existingIngredientIds = existingSandwich.SandwichIngredients
                .Select(si => si.IngredientId)
                .ToList();

            foreach (var existingIngredient in existingSandwich.SandwichIngredients.ToList())
            {
                if (!updatedSandwich.Ingredients.Any(i => i.Id == existingIngredient.IngredientId))
                {
                    _dbContext.SandwichIngredients.Remove(existingIngredient);
                }
            }

            foreach (var ingredient in updatedSandwich.Ingredients)
            {
                if (!existingIngredientIds.Contains(ingredient.Id))
                {
                    existingSandwich.SandwichIngredients.Add(new SandwichIngredient
                    {
                        SandwichId = existingSandwich.Id,
                        IngredientId = ingredient.Id
                    });
                }
            }

            _dbContext.SaveChanges();

            existingSandwich = _dbContext.Sandwiches
                .Include(s => s.SandwichIngredients)
                    .ThenInclude(si => si.Ingredient)
                .FirstOrDefault(s => s.Id == existingSandwich.Id);

            return Ok(existingSandwich);
        }
    }
}