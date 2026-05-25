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
                IconId = model.IconId,
                ColorId = model.ColorId
            };
        }

        public static Category ToModel(this CreateCategoryDto dto)
        {
            return new Category
            {
                Id = Guid.NewGuid(),
                Name = dto.Name ?? string.Empty,
                IconId = dto.IconId,
                ColorId = dto.ColorId,
                Type = dto.Type
            };
        }

        public static Category ToModel(this CategoryDto dto)
        {
            return new Category
            {
                Id = dto.Id,
                Name = dto.Name ?? string.Empty,
                IconId = dto.IconId,
                ColorId = dto.ColorId,
                Type = dto.Type
            };
        }
    }

}

