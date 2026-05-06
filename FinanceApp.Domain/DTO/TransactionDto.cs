using FinanceApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceApp.Domain.DTO
{
    public class TransactionDto
    {
        public Guid Id { get; set; }
        public CategoryType Type { get; set; }   // income | expense | transfer
        public Guid AccountId { get; set; }
        public Guid? CategoryId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string? Description { get; set; }
        public Guid? TransferId { get; set; }
    }

}
