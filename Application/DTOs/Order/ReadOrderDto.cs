namespace Application.DTOs.Order
{
    public class ReadOrderDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
