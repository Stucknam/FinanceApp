using FinanceApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceApp.Domain.Interfaces.Srevices
{
    public interface IAccountService
    {
        public Task<List<Account>> GetAccountsAsync();
        public Task<Account?> GetByIdAsync(Guid id);



        Task<decimal> GetBalanceAsync(Guid accountId);
        Task<decimal> GetProfitAsync(Guid accountId, DateTime from, DateTime to);


        public Task<Account> CreateAsync(Account account);
        public Task UpdateAsync(Account account);
        public Task DeleteAsync(Guid id);   

    }
}
