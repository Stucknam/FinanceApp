using FinanceApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceApp.Domain.DTO
{
    public class CreateCategoryDto
    {
        public string? Name { get; set; }
        public string? IconId { get; set; }
        public string? ColorId { get; set; }
        public CategoryType Type { get; set; }
    }
}
