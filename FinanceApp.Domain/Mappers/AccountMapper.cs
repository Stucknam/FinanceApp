using FinanceApp.Domain.DTO;
using FinanceApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceApp.Domain.Mappers
{
    public static class AccountMapper
    {
        public static AccountDto ToDto(this Account model)
        {
            return new AccountDto
            {
                Id = model.Id,
                Name = model.Name,
                Amount = model.Amount,
                IconId = model.IconId,
                Color = model.Color,
                Description = model.Description,
                IsDeleted = model.IsDeleted
            };
        }

    }
}
