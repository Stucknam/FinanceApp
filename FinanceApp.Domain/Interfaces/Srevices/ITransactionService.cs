using FinanceApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceApp.Domain.Interfaces.Srevices
{
    public interface ITransactionService
    {
        Task<List<Transaction>> GetByAccountAsync(Guid accountId, DateTime from, DateTime to);
        Task<List<Transaction>> GetByPeriodAsync(DateTime from, DateTime to);

        Task<decimal> GetIncomeSumAsync(Guid accountId, DateTime from, DateTime to);
        Task<decimal> GetExpenseSumAsync(Guid accountId, DateTime from, DateTime to);

        Task<Transaction> CreateAsync(Transaction transaction);
        Task UpdateAsync(Transaction transaction);
        Task DeleteAsync(Guid id);

        Task<List<Transaction>> GetAllAsync();

        Task<List<Transaction>> GetLastNonTransferAsync(int count);

    }
}
