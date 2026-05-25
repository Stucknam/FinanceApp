using FinanceApp.Domain.Enums;
using FinanceApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceApp.Domain.Interfaces.Repositories
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
        Task<List<Transaction>> GetByPeriodAsync(DateTime from, DateTime to);
        Task<decimal> GetSumAsync(Guid accountId, CategoryType type, DateTime from, DateTime to);
        Task<List<Transaction>> GetByAccountAndPeriodAsync(Guid accountId, DateTime from, DateTime to);
        Task<List<Transaction>> GetLastNonTransferAsync(int count);
        Task<decimal> GetTotalProfitAsync(DateTime from, DateTime to);


    }
}
