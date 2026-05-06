using FinanceApp.Domain.Interfaces.Srevices;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceApp.Application.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IAccountService _accounts;


        public StatisticsService(IAccountService accounts)
        {
            _accounts = accounts;
        }
        //public async Task<decimal> GetBalanceForVisibleAccountsAsync()
        //{
        //    var accounts = await _accounts.GetVisibleAccountsAsync();

        //    return accounts.Sum(a => a.Amount);
        //}

        //public async Task<decimal> GetProfitForVisibleAsync(DateTime from, DateTime to)
        //{
        //    var accounts = await _accounts.GetVisibleAccountsAsync();

        //    var tasks = accounts
        //        .Select(a => _accounts.GetProfitAsync(a.Id, from, to));

        //    var results = await Task.WhenAll(tasks);

        //    return results.Sum();
        //}

        public async Task<decimal> GetTotalBalanceAsync()
        {
            var accounts = await _accounts.GetAccountsAsync();

            return accounts.Sum(a => a.Amount);
        }

        public async Task<decimal> GetTotalProfitAsync(DateTime from, DateTime to)
        {
            var accounts = await _accounts.GetAccountsAsync();

            var tasks = accounts
                .Select(a => _accounts.GetProfitAsync(a.Id, from, to));

            var results = await Task.WhenAll(tasks);

            return results.Sum();
        }
    }
}
