using System.ComponentModel.DataAnnotations;

namespace Sandwich.Models
{
    public class Order
    {
        public int Id { get; set; }
        [Required]
        public double TotalPrice { get; set; }
        [Required]
        public int CustomerId { get; set; }
        public UserProfile? Customer { get; set; }
        [Required]
        public int StatusId { get; set; }
        public Status? Status { get; set; }
        public DateTime OrderReceived { get; set; }
        public List<SandwichObj>? Sandwiches { get; set; }
        public void CalculateTotalPrice()
        {
            if (Sandwiches != null && Sandwiches.Any())
            {
                TotalPrice = Sandwiches.Sum(s => s.Price) ?? 0;
            }
            else
            {
                TotalPrice = 0;
            }
        }
    }
}