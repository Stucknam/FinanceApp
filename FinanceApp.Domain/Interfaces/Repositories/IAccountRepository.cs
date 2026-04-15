using FinanceApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace FinanceApp.Domain.Interfaces.Repositories
{
    public interface IAccountRepository: IRepository<Account>
    {
        public Task<decimal> GetBalanceAsync(Guid accountId);
    }
}
