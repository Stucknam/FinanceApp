using FinanceApp.Domain.Defaults;
using FinanceApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FinanceApp.Domain.DTO
{
    public class CategoryDto
    {
        public Guid Id { get; set; } 
        public string? Name { get; set; } 
        public string? IconId { get; set; } 
        public string? ColorId { get; set; }
        public CategoryType Type { get; set; } // income | expence
    }
}
