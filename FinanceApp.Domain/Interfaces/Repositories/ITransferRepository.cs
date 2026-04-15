using FinanceApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceApp.Domain.Interfaces.Repositories
{
    public interface ITransferRepository : IRepository<Transfer>
    {
        Task<List<Transfer>> GetTransfersForAccount(Guid accountId);
        Task<decimal> GetIncomingSumAsync(Guid accountId, DateTime from, DateTime to);
        Task<decimal> GetOutgoingSumAsync(Guid accountId, DateTime from, DateTime to);
        Task<List<Transfer>> GetByPeriodAsync(DateTime from, DateTime to);
        Task<List<Transfer>> GetByPeriodForAccount(Guid accountId, DateTime from, DateTime to);
    }
}
