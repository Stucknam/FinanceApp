using FinanceApp.Domain.Enums;
using FinanceApp.Domain.Interfaces.Repositories;
using FinanceApp.Domain.Models;
using Microsoft.EntityFrameworkCore;


namespace FinanceApp.Data.Repositories
{
    public class TransactionRepository : Repository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(FinanceDbContext context) : base(context) { }

        public async Task<List<Transaction>> GetByPeriodAsync(DateTime from, DateTime to)
        {
            return await _dbSet
                .Where(t => t.Date >= from && t.Date <= to)
                .ToListAsync();
        }

        public async Task<decimal> GetSumAsync(Guid accountId, CategoryType type, DateTime from, DateTime to)
        {
            return await _dbSet
                .Where(t => t.AccountId == accountId && t.Type == type && t.Date >= from && t.Date <= to)
                .SumAsync(t => t.Amount);
        }

        public async Task<List<Transaction>> GetByAccountAndPeriodAsync(Guid accountId, DateTime from, DateTime to)
        {
            return await _dbSet
                .Where(t => t.AccountId == accountId && t.Date >= from && t.Date <= to)
                .ToListAsync();
        }
        public async Task<List<Transaction>> GetLastNonTransferAsync(int count)
        {
            return await _dbSet.Where(t => t.Type != CategoryType.Transfer)
                .OrderByDescending(t => t.Date)
                .Take(count)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalProfitAsync(DateTime from, DateTime to)
        {
            return await _dbSet.Where(t => t.Date >= from && t.Date < to.AddDays(1))
        .GroupBy(t => 1)
        .Select(g =>
            g.Where(t => t.Type == CategoryType.Income).Sum(t => t.Amount) -
            g.Where(t => t.Type == CategoryType.Expense).Sum(t => t.Amount)
        )
        .SingleOrDefaultAsync();
        }
    }
}
