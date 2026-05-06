using FinanceApp.ConsoleUI.Output;
using FinanceApp.Domain.Interfaces.Srevices;
using FinanceApp.Domain.Models;
using FinanceApp.ConsoleUI.Services;
using Npgsql.PostgresTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Text;


namespace FinanceApp.ConsoleUI.Menu
{
    public class AccountsMenu
    {
        private readonly IStatisticsService _statistics;
        private readonly IConsolePrinter _printer;
        private readonly IAccountService _account;
        private readonly IPaginationService _pager;
        private readonly Services.ISettingsService _settings;
        private readonly ITransactionService _transaction;
        private readonly ITransferService _transfer;
        private readonly ConsoleHelper _helper;

        public AccountsMenu(IStatisticsService statisticsService,
            IConsolePrinter consolePrinter,
            IAccountService accountService,
            IPaginationService paginationService,
            ISettingsService settingsService,
            ITransactionService transactionService,
            ITransferService transferService,
            ConsoleHelper accountHelper)
        {
            _statistics = statisticsService;
            _printer = consolePrinter;
            _account = accountService;
            _pager = paginationService;
            _settings = settingsService;
            _transaction = transactionService;
            _transfer = transferService;
            _helper = accountHelper;
        }

        private void PrintActions()
        {
            _printer.Print("Действия:");
            _printer.Print("1. Добавить счёт\n" +
                "2. Изменить счёт\n" +
                "3. Перевести между счетами\n" +
                "4. Удалить счёт\n" +
                "N. Следующая страница\n" +
                "P. Предыдущая страница\n" +
                "7. Изменить счёт по умолчанию\n" +
                "0. Назад");
        }
        public async Task RunAsync()
        {
            int page = 1;

            while (true)
            {


                var accounts = await _account.GetAccountsAsync();
                accounts = [.. accounts.OrderBy(a => a.Id)];
                var pages = _pager.Paginate(accounts, page, 5);
                int totalPages = pages.TotalPages;
                var defaultAccountId = _settings.DefaultAccountId;

                Console.Clear();
                await PrintHeaderAsync();
                Console.WriteLine();
                _helper.PrintAccountsPageAsync( accounts, page, pages,defaultAccountId);
                Console.WriteLine();

                PrintActions();

                Console.Write("\n>> ");
                string? input = Console.ReadLine();


                switch (input)
                {
                    case "1": await _helper.ExecuteSafeAsync(() => AddAccountAsync()); 
                        break;
                    case "2": await _helper.ExecuteSafeAsync(() => EditAccountAsync(pages, accounts));
                            break;
                    case "3": await _helper.ExecuteSafeAsync(() => TransferAsync(pages, accounts));
                        break;
                    case "4": await _helper.ExecuteSafeAsync(() => DeleteAccountAsync(pages, accounts));
                        break;
                    case "p":
                        if (page > 1)
                            page--;
                        break;
                    case "n":
                        if (page < totalPages)
                            page++; break;
                    case "7": await _helper.ExecuteSafeAsync(() => SetDefaultAccountAsync(accounts, pages));
                        break;

                    case "0": return;
                    default:

                        _printer.Print("Некорректный ввод.Пожалуйста, выберите одну из предложенных опций.", MessageType.Error);
                        Thread.Sleep(1500);
                        break;
                }

            }
        }
        // Установка счёта по умолчанию
        private async Task SetDefaultAccountAsync(List<Account> accounts, PaginationResult<Account> pages)
        {

            var selected = await _helper.SelectAccountAsync("Введите номер счёта, который хотите сделать основным:", pages, accounts);
            if (selected != null)
            {
                _settings.DefaultAccountId = selected.Id; // автоматически вызывает SaveAsync()

                _printer.Print($"Счёт \"{selected.Name}\" теперь основной.", MessageType.Success);
                await Task.Delay(800);
            }
        }

        private async Task EditAccountAsync(PaginationResult<Account> pages, List<Account> accounts)
        {
            var account = await _helper.SelectAccountAsync("Введите номер счёта для редактирования:", pages, accounts);

            if (account == null)
                return;

            _printer.Print($"Редактирование счёта \"{account.Name}\"");

            // Имя
            _printer.Print($"Новое имя (Enter — оставить \"{account.Name}\"):");
            Console.Write(">> ");
            var newName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(newName))
                newName = account.Name;

            // Баланс
            _printer.Print($"Новая сумма (Enter — оставить {account.Amount}):");
            Console.Write(">> ");
            decimal newAmount;
            var amountInput = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(amountInput))
            {
                newAmount = account.Amount;
            }
            else
            {
                if (!decimal.TryParse(amountInput, out newAmount))
                {
                    _printer.Print("Некорректная сумма.", MessageType.Error);
                    await Task.Delay(800);
                    return;
                }
            }

            // Описание
            _printer.Print($"Новое описание (Enter — оставить текущее):");
            Console.Write(">> ");
            var newDescription = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(newDescription))
                newDescription = account.Description;


            // Обновление
            await _account.UpdateAsync(new Account
            {
                Id = account.Id,
                Name = newName,
                Amount = newAmount,
                Description = newDescription,
            });

            _printer.Print("Счёт успешно обновлён.", MessageType.Success);
            await Task.Delay(800);
        }

        private async Task PrintHeaderAsync()
        {
            decimal total = await _statistics.GetTotalBalanceAsync();
            _printer.PrintTitle($"Общий баланс: {total}");
        }




        // Удаление аккаунта с подтверждением
        private async Task DeleteAccountAsync(PaginationResult<Account> pages, List<Account> accounts)
        {
            var account = await _helper.SelectAccountAsync("Введите номер счёта для удаления:", pages, accounts);

            if (account == null)
                return;

            _printer.Print($"Вы действительно хотите удалить счёт \"{account.Name}\"? (y/n)");
            Console.Write(">> ");
            var confirm = Console.ReadLine()?.Trim().ToLower();

            if (confirm != "y" && confirm != "д" && confirm != "yes")
            {
                _printer.Print("Удаление отменено.", MessageType.Accent);
                await Task.Delay(600);
                return;
            }

            // Если удаляем счёт по умолчанию — сбрасываем
            if (_settings.DefaultAccountId == account.Id)
            {
                _settings.DefaultAccountId = null;
            }

            try
            {
                await _account.DeleteAsync(account.Id);
            } catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
            

            _printer.Print("Счёт успешно удалён.", MessageType.Success);
            await Task.Delay(800);
        }

        private async Task AddAccountAsync()
        {
            _printer.Print("Введите название счёта:");
            Console.Write(">> ");
            var name = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                _printer.Print("Название счёта не может быть пустым.", MessageType.Error);
                await Task.Delay(800);
                return;
            }

            _printer.Print("Введите начальную сумму (Enter — 0):");
            Console.Write(">> ");
            var amountInput = Console.ReadLine();
            decimal amount = 0;

            if (!string.IsNullOrWhiteSpace(amountInput))
            {
                if (!decimal.TryParse(amountInput, out amount))
                {
                    _printer.Print("Некорректная сумма.", MessageType.Error);
                    await Task.Delay(800);
                    return;
                }
            }

            _printer.Print("Введите описание (Enter — пропустить):");
            Console.Write(">> ");
            var description = Console.ReadLine()?.Trim() ?? "";

            // Создание счёта
            var created = await _account.CreateAsync(new Account
            {
                Name = name,
                Amount = amount,
                Description = description
            });

            // Если это первый счёт — делаем его основным
            var allAccounts = await _account.GetAccountsAsync();
            if (allAccounts.Count == 1)
            {
                _settings.DefaultAccountId = created.Id;
            }

            _printer.Print("Счёт успешно добавлен.", MessageType.Success);
            await Task.Delay(800);
        }

        private async Task TransferAsync(PaginationResult<Account> pages, List<Account> accounts)
        {
            // 1. Получаем default-счёт (если есть)
            var defaultAccountId = _settings.GetDefaultAccountId();

            var defaultAccount = accounts.FirstOrDefault(a => a.Id == defaultAccountId);

            // 2. Выбор источника
            Account? from;

            if (defaultAccount != null)
            {
                _printer.Print($"Использовать основной счёт \"{defaultAccount.Name}\" как источник? (y/n)");
                Console.Write(">> ");
                var useDefault = Console.ReadLine()?.Trim().ToLower();

                if (useDefault == "y" || useDefault == "д")
                {
                    from = defaultAccount;
                }
                else
                {
                    from = await _helper.SelectAccountAsync("Выберите счёт-источник:", pages, accounts);
                }
            }
            else
            {
                from = await _helper.SelectAccountAsync("Выберите счёт-источник:", pages, accounts);
            }

            if (from == null)
                return;

            // 3. Выбор получателя
            Account? to;

            if (defaultAccount != null && defaultAccount.Id != from.Id)
            {
                _printer.Print($"Использовать основной счёт \"{defaultAccount.Name}\" как получателя? (y/n)");
                Console.Write(">> ");
                var useDefault = Console.ReadLine()?.Trim().ToLower();

                if (useDefault == "y" || useDefault == "д")
                {
                    to = defaultAccount;
                }
                else
                {
                    to = await _helper.SelectAccountAsync("Выберите счёт-получатель:", pages, accounts);
                }
            }
            else
            {
                to = await _helper.SelectAccountAsync("Выберите счёт-получатель:", pages, accounts);
            }

            if (to == null)
                return;

            if (to.Id == from.Id)
            {
                _printer.Print("Нельзя переводить на тот же самый счёт.", MessageType.Error);
                await Task.Delay(800);
                return;
            }

            // 4. Ввод суммы
            _printer.Print("Введите сумму перевода:");
            Console.Write(">> ");
            var amountInput = Console.ReadLine();

            if (!decimal.TryParse(amountInput, out decimal amount) || amount <= 0)
            {
                _printer.Print("Некорректная сумма.", MessageType.Error);
                await Task.Delay(800);
                return;
            }
            try
            {
                var transfer = await _transfer.CreateTransferAsync(from.Id, to.Id, amount, "Перевод");
            }
            catch (Exception ex)
            {

                _printer.Print(ex.Message, MessageType.Error);
                await Task.Delay(800);
                return;
            }
            

            _printer.Print($"Перевод {amount} выполнен: \"{from.Name}\" -> \"{to.Name}\"", MessageType.Success);
            await Task.Delay(800);
        }



    }
}
