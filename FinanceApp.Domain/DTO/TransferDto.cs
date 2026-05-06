using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceApp.Domain.DTO
{
    public class TransferDto
    {
        public Guid Id { get; set; }
        public Guid AccountFromId { get; set; }
        public Guid AccountToId { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }
    }

}
