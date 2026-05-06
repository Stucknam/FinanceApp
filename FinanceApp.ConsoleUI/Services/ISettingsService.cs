using FinanceApp.Domain.Enums;
using FinanceApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceApp.ConsoleUI.Services
{
    public interface ISettingsService
    {
        Guid? DefaultAccountId { get; set; } // Счет по умолчанию для транзакций
        List<Guid> HiddenAccounts { get; set; } // Счета скрытые из представления и итоговой суммы

        ApplicationTheme Theme { get; set; } // Light | Dark

        Task LoadAsync();
        Task SaveAsync();

        public Guid? GetDefaultAccountId();
        public void SetDefaultAccountId(Guid accountId);

        public void HideAccount(Guid accountId);
        public void ShowAccount(Guid accountId);

    }
}
