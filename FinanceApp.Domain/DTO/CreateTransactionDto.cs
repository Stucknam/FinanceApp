using FinanceApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceApp.Domain.DTO
{
    public class CreateTransactionDto
    {
        public CategoryType Type { get; set; }
        public Guid AccountId { get; set; }
        public Guid? CategoryId { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public DateTime? Date { get; set; }
    }
}
