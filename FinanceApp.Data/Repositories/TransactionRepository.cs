using FinanceApp.Domain.Enums;
using FinanceApp.Domain.Interfaces.Repositories;
using FinanceApp.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;


namespace FinanceApp.Data.Repositories
{
    public class TransactionRepository: Repository<Transaction>, ITransactionRepository
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
    }
}
