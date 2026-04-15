using FinanceApp.Domain.Interfaces.Repositories;
using FinanceApp.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceApp.Data.Repositories
{
    public class TransferRepository : Repository<Transfer>, ITransferRepository
    {
        public TransferRepository(FinanceDbContext context) : base(context) { }

        public async Task<List<Transfer>> GetByPeriodAsync(DateTime from, DateTime to)
        {
            return await _dbSet.Where(t => t.Date >= from && t.Date <= to).ToListAsync();
        }

        public async Task<List<Transfer>> GetByPeriodForAccount(Guid accountId, DateTime from, DateTime to)
        {
            return await _dbSet.Where(t => 
            (t.AccountFromId == accountId || t.AccountToId == accountId)
            && t.Date >= from && t.Date <= to)
                .ToListAsync();
        }

        public async Task<decimal> GetIncomingSumAsync(Guid accountId, DateTime from, DateTime to)
        {
            return await _dbSet
                .Where(t => t.AccountToId == accountId && t.Date >= from && t.Date <= to)
                .SumAsync(t => t.Amount);
        }

        public async Task<decimal> GetOutgoingSumAsync(Guid accountId, DateTime from, DateTime to)
        {
            return await _dbSet
                .Where(t => t.AccountFromId == accountId && t.Date >= from && t.Date <= to)
                .SumAsync(t => t.Amount);
        }

        public async Task<List<Transfer>> GetTransfersForAccount(Guid accountId)
        {
            return await _dbSet
                .Where(t => t.AccountFromId == accountId ||  t.AccountToId == accountId)
                .ToListAsync();
        }
    }
}
