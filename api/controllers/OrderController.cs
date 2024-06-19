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
    public class OrderController : ControllerBase
    {
        private readonly SandwichDbContext _dbContext;
        public OrderController(SandwichDbContext context)
        {
            _dbContext = context;
        }

        [HttpGet("allOrders")]
        public IActionResult GetAllOrders()
        {
            var orders = _dbContext.Orders
                        .Include(o => o.Sandwiches)
                            .ThenInclude(s => s.SandwichIngredients)
                                .ThenInclude(si => si.Ingredient)
                        .Select(o => new OrderDTO
                        {
                            Id = o.Id,
                            TotalPrice = o.TotalPrice,
                            CustomerId = o.CustomerId,
                            Customer = null,
                            StatusId = o.StatusId,
                            Status = null,
                            OrderReceived = o.OrderReceived,
                            Sandwiches = o.Sandwiches.Select(s => new SandwichDTO
                            {
                                Id = s.Id,
                                SandwichIngredients = s.SandwichIngredients.Select(si => new SandwichIngredientDTO
                                {
                                    Id = si.Id,
                                    SandwichId = si.SandwichId,
                                    Sandwich = null,
                                    IngredientId = si.IngredientId,
                                    Ingredient = new IngredientDTO
                                    {
                                        Id = si.Ingredient.Id,
                                        Name = si.Ingredient.Name,
                                        Price = si.Ingredient.Price,
                                        Calories = si.Ingredient.Calories,
                                        TypeId = si.Ingredient.TypeId,
                                        SandwichIngredients = null
                                    }
                                }).ToList(),
                                CustomerId = s.CustomerId
                            }).ToList()
                        }).ToList();
            foreach (var order in orders)
            {
                order.CalculateTotalPrice();
            }

            return Ok(orders);
        }

        [HttpGet("{id}")]
        public IActionResult GetOrderById(int id)
        {
           Order order = _dbContext.Orders
            .Include(o => o.Sandwiches)
                .ThenInclude(s => s.SandwichIngredients)
                    .ThenInclude(si => si.Ingredient)
            .FirstOrDefault(o => o.Id == id);

            if (order == null)
            {
                return NotFound("Order was not found.");
            }

            OrderDTO orderDTO = new OrderDTO
            {
                Id = order.Id,
                TotalPrice = order.TotalPrice,
                CustomerId = order.CustomerId,
                StatusId = order.StatusId,
                OrderReceived = order.OrderReceived,
                Sandwiches = order.Sandwiches.Select(s => new SandwichDTO
                {
                    Id = s.Id,
                    SandwichIngredients = s.SandwichIngredients.Select(si => new SandwichIngredientDTO
                    {
                        Id = si.Id,
                        SandwichId = si.SandwichId,
                        Ingredient = new IngredientDTO
                        {
                            Id = si.Ingredient.Id,
                            Name = si.Ingredient.Name,
                            Price = si.Ingredient.Price,
                            Calories = si.Ingredient.Calories,
                            TypeId = si.Ingredient.TypeId
                        }
                    }).ToList(),
                    CustomerId = s.CustomerId
                }).ToList()                
            };

            orderDTO.CalculateTotalPrice();            

            return Ok(orderDTO);
        }

        [HttpPost]
        public IActionResult InitiateNewOrder(Order newOrder)
        {
            newOrder.OrderReceived = DateTime.Now;
            _dbContext.Orders.Add(newOrder);
            _dbContext.SaveChanges();

            return Ok(newOrder);
        }
    }   
}