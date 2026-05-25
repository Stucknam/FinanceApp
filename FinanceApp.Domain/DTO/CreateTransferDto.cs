namespace FinanceApp.Domain.DTO
{
    public class CreateTransferDto
    {
        public Guid AccountFromId { get; set; }
        public Guid AccountToId { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public DateTime? Date { get; set; }
    }
}
