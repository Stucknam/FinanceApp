using FinanceApp.Domain.Interfaces.Repositories;
using FinanceApp.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceApp.Data.Repositories
{
    public class AccountRepository : Repository<Account>, IAccountRepository
    {
        public AccountRepository(FinanceDbContext context) : base(context) { }

        public async Task<decimal> GetBalanceAsync(Guid accountId)
        {
            return await _dbSet
                .Where(a => a.Id == accountId)
                .Select(a => a.Amount)
                .FirstOrDefaultAsync();
        }

    }
}
