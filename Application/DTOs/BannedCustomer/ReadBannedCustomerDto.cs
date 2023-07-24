namespace Application.DTOs.BannedCustomer
{
    public class ReadBannedCustomerDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public DateTime? BanDate { get; set; } = null;
        public int EmpId { get; set; }
        public string EmpName { get; set; }
        public DateTime? CreatedDate { get; set; }

    }
}
