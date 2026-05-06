using FinanceApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceApp.ConsoleUI.Services
{
    public class SettingsDto
    {
        public Guid? DefaultAccountId { get; set; }
        public List<Guid>? HiddenAccounts { get; set; }
        public ApplicationTheme Theme { get; set; }

    }
}
