using FinanceApp.Domain.DTO;
using FinanceApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceApp.Domain.Mappers
{
    public static class TransactionMapper
    {
        public static TransactionDto ToDto(this Transaction model)
        {
            return new TransactionDto
            {
                Id = model.Id,
                Type = model.Type,
                AccountId = model.AccountId,
                CategoryId = model.CategoryId,
                Amount = model.Amount,
                Date = model.Date,
                Description = model.Description,
                TransferId = model.TransferId
            };
        }
    }
}
