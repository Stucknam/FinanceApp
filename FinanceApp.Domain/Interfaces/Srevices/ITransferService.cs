using FinanceApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceApp.Domain.Interfaces.Srevices
{
    public interface ITransferService
    {
        Task<List<Transfer>> GetByAccountAsync(Guid accountId, DateTime from, DateTime to);
        Task<List<Transfer>> GetByPeriodAsync(DateTime from, DateTime to);

        Task<decimal> GetIncomingSumAsync(Guid accountId, DateTime from, DateTime to);
        Task<decimal> GetOutgoingSumAsync(Guid accountId, DateTime from, DateTime to);

        Task<Transfer> CreateAsync(Transfer transfer);
        Task UpdateAsync(Transfer transfer);
        Task DeleteAsync(Guid transferId);

    }
}
