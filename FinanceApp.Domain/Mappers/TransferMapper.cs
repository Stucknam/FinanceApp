using FinanceApp.Domain.DTO;
using FinanceApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceApp.Domain.Mappers
{
    public static class TransferMapper
    {
        public static TransferDto ToDto(this Transfer model)
        {
            return new TransferDto
            {
                Id = model.Id,
                AccountFromId = model.AccountFromId,
                AccountToId = model.AccountToId,
                Amount = model.Amount,
                Description = model.Description,
                Date = model.Date
            };

        }
        public static Transfer ToModel(this CreateTransferDto dto)
        {
            return new Transfer
            {
                Id = Guid.NewGuid(),
                AccountFromId = dto.AccountFromId,
                AccountToId = dto.AccountToId,
                Amount = dto.Amount,
                Description = dto.Description ?? string.Empty,
                Date = dto.Date ?? DateTime.UtcNow
            };
        }
        public static Transfer ToModel(this TransferDto dto)
        {
            return new Transfer
            {
                Id = dto.Id,
                AccountFromId = dto.AccountFromId,
                AccountToId = dto.AccountToId,
                Amount = dto.Amount,
                Description = dto.Description ?? string.Empty,
                Date = dto.Date
            };
        }

    }
}
