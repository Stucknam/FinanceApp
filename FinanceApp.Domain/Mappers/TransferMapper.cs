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
    }
}
