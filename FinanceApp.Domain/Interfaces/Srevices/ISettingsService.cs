using FinanceApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceApp.Domain.Interfaces.Srevices
{
    public interface ISettingsService
    {
        Guid? DefaultAccountId { get; set; } // Счет по умолчанию для транзакций
        List<Guid> HiddenAccounts { get; set; } // Счета скрытые из представления и итоговой суммы

        ApplicationTheme Theme { get; set; } // Light | Dark

        Task LoadAsync();
        Task SaveAsync();

    }
}
