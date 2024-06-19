namespace Sandwich.Models.DTOs
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public double TotalPrice { get; set; }
        public int CustomerId { get; set; }
        public UserProfile? Customer { get; set; }
        public int StatusId { get; set; }
        public Status? Status { get; set; }
        public DateTime OrderReceived { get; set; }
        public List<SandwichDTO>? Sandwiches { get; set; }
        public void CalculateTotalPrice()
        {
            if (Sandwiches != null)
            {
                TotalPrice = Sandwiches.Sum(s => s.Price);
            }
            else
            {
                TotalPrice = 0;
            }
        }
    }
}