using FinanceApp.Domain.DTO;
using FinanceApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceApp.Domain.Mappers
{
    public static class CategoryMapper
    {
        public static CategoryDto ToDto(this Category model)
        {
            return new CategoryDto
            {
                Id = model.Id,
                Name = model.Name,
                Type = model.Type,
                ColorId = model.ColorId,
                IconId = model.IconId,
            };
        }
    }
}
