using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceApp.Domain.Interfaces.Srevices
{
    public interface IStatisticsService
    {
        Task<decimal> GetTotalBalanceAsync();
        Task<decimal> GetBalanceForVisibleAccountsAsync();
        Task<decimal> GetTotalProfitAsync(DateTime from, DateTime to);
        Task<decimal> GetProfitForVisibleAsync(DateTime from, DateTime to);
    }
}
