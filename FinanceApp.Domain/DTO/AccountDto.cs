using FinanceApp.Domain.Defaults;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FinanceApp.Domain.DTO
{
    public class AccountDto
    {
            public Guid Id { get; set; }
            public string? Name { get; set; } 
            public decimal Amount { get; set; }
            public string? IconId { get; set; }
            public string? Color { get; set; }
            public string? Description { get; set; }
            public bool IsDeleted { get; set; }
    }
}
