using CommunityToolkit.Mvvm.ComponentModel;
using FinanceApp.Domain.Interfaces.Srevices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using FinanceApp.Components.AccountCard;
using FinanceApp.Components.DateRangeSelector;


namespace FinanceApp.Pages.Dashboard
{
    public partial class DashboardViewModel : ObservableObject
    {
        //public SummaryBlockViewModel Summary { get; }
        public DateRangeSelectorViewModel DateRange { get; }
        public ObservableCollection<AccountCardViewModel> Accounts { get; } = new();
        //public IncomeSectionViewModel IncomeSection { get; }
        //public ExpenseSectionViewModel ExpenseSection { get; }

        private readonly IAccountService _accountService;

        public DashboardViewModel(
            IAccountService accountService,
            ITransactionService transactionService)
        {
            _accountService = accountService;

            DateRange = new DateRangeSelectorViewModel();
            //Summary = new SummaryBlockViewModel(transactionService, DateRange);
            //IncomeSection = new IncomeSectionViewModel(transactionService, DateRange);
            //ExpenseSection = new ExpenseSectionViewModel(transactionService, DateRange);
        }

        public async Task InitializeAsync()
        {
            var accounts = await _accountService.GetAccountsAsync();

            foreach (var acc in accounts)
                Accounts.Add(new AccountCardViewModel(acc, _accountService, DateRange));
        }
    }
}
