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

        public static Transaction ToModel(this CreateTransactionDto dto)
        {
            return new Transaction
            {
                Id = Guid.NewGuid(),
                Type = dto.Type,
                AccountId = dto.AccountId,
                CategoryId = dto.CategoryId,
                Amount = dto.Amount,
                Description = dto.Description ?? string.Empty,
                Date = dto.Date ?? DateTime.UtcNow
            };
        }

        public static Transaction ToModel(this TransactionDto dto)
        {
            return new Transaction
            {
                Id = dto.Id,
                Type = dto.Type,
                AccountId = dto.AccountId,
                CategoryId = dto.CategoryId,
                Amount = dto.Amount,
                Description = dto.Description ?? string.Empty,
                Date = dto.Date,
                TransferId = dto.TransferId
            };
        }



    }
}
