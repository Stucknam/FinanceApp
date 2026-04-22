using CommunityToolkit.Mvvm.ComponentModel;
using FinanceApp.Domain.Interfaces.Srevices;
using FinanceApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;
using FinanceApp.Components.DateRangeSelector;

namespace FinanceApp.Components.AccountCard
{
    public partial class AccountCardViewModel : ObservableObject
    {
        [ObservableProperty] private string name;
        [ObservableProperty] private decimal amount;
        [ObservableProperty] private decimal change;
        [ObservableProperty] private string color;

        private readonly IAccountService _accountService;
        private readonly Account _account;
        private readonly DateRangeSelectorViewModel _dateRange;

        public AccountCardViewModel(
            Account account,
            IAccountService accountService,
            DateRangeSelectorViewModel dateRange)
        {
            _account = account;
            _accountService = accountService;
            _dateRange = dateRange;

            name = account.Name;
            amount = account.Amount;
            color = account.Color;

            // Подписка на изменение диапазона дат
            _dateRange.PropertyChanged += async (_, __) => await LoadChangeAsync();
        }

        public async Task InitializeAsync()
        {
            await LoadChangeAsync();
        }

        private async Task LoadChangeAsync()
        {
            Change = await _accountService.GetProfitAsync(
                _account.Id,
                _dateRange.Start,
                _dateRange.End
            );
        }
    }
}
