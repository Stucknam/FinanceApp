using FinanceApp.Application.Services;
using FinanceApp.ConsoleUI.Output;
using FinanceApp.Domain.Enums;
using FinanceApp.Domain.Interfaces.Srevices;
using FinanceApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Runtime;
using System.Text;
using System.Text.RegularExpressions;

namespace FinanceApp.ConsoleUI.Menu
{
    public class TransactionsMenu
    {
        private readonly ITransactionService _transactionService;
        private readonly IConsolePrinter _printer;
        private readonly ISettingsService _settings;
        private readonly IPaginationService _pager;
        private readonly ITransferService _transfer;
        private readonly IAccountService _account;
        private readonly ConsoleHelper _helper;

        public TransactionsMenu(
            ITransactionService transactionService,
            ITransferService transferService,
            IAccountService accountService,
            IConsolePrinter printer,
            ISettingsService settingsService,
            IPaginationService paginationService,
            ConsoleHelper helper
            )
        {
            _transactionService = transactionService;
            _transfer = transferService;
            _printer = printer;
            _settings = settingsService;
            _helper = helper;
            _pager = paginationService;
            _account = accountService;

        }

        private void ShowMenu()
        {
            _printer.Print("Действия:" +
                    "\n1. Добавить доход" +
                    "\n2. Добавить расход" +
                    "\n3. Последние транзакции" +
                    "\n4. Последние переводы" +
                    "\nN. Следующая страница" +
                    "\nP. Предыдущая страница" +
                    "\n0. Назад");

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
                _printer.PrintTitle("Доходы и расходы");

                _helper.PrintAccountsPageAsync(accounts, page, pages, defaultAccountId);

                Console.WriteLine();

                ShowMenu();
                
                Console.Write(">> ");

                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        await _helper.ExecuteSafeAsync(() => AddIncomeAsync(pages, accounts));
                        break;

                    case "2":
                        await _helper.ExecuteSafeAsync(() => AddExpenseAsync(pages, accounts));
                        break;

                    case "3":
                        await _helper.ExecuteSafeAsync(() => ShowLastTransactionsAsync(accounts));
                        break;
                    case "4": _helper.ExecuteSafeAsync(() => ShowLastTransfersAsync(accounts)).Wait(); 
                        break;
                    case "p":
                        if (page > 1)
                            page--;
                        break;
                    case "n":
                        if (page < totalPages)
                            page++; break;
                    case "0":
                        return;

                    default:

                        _printer.Print("Некорректный ввод.Пожалуйста, выберите одну из предложенных опций.", MessageType.Error);
                        Thread.Sleep(1500);
                        break;
                }
            }
        }
        // Добавить доход
        private async Task AddIncomeAsync(PaginationResult<Account> pages, List<Account> accounts)
        {
            var account = await _helper.SelectAccountAsync("Введите номер счёта для добавления дохода", pages, accounts);
            if (account == null || account.Id == Guid.Empty) return;
            
            _printer.Print("Введите сумму дохода:");
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

            Console.Write("Описание: ");
            var description = Console.ReadLine()!;

            var transaction = new Transaction
            {
                AccountId = account.Id,
                Amount = amount,
                Description = description ?? string.Empty,
                Type = CategoryType.Income,
            };

            await _transactionService.CreateAsync(transaction);

            Console.WriteLine("Доход добавлен.");
            await Task.Delay(800);
        }
        // Добавить расход
        private async Task AddExpenseAsync(PaginationResult<Account> pages, List<Account> accounts)
        {
            var account = await _helper.SelectAccountAsync("Введите номер счёта для добавления расхода", pages, accounts);
            if (account == null || account.Id == Guid.Empty) return;

            _printer.Print("Введите сумму расхода:");
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

            Console.Write("Описание: ");
            var description = Console.ReadLine()!;

            var transaction = new Transaction
            {
                AccountId = account.Id,
                Amount = amount,
                Description = description ?? string.Empty,
                Type = CategoryType.Expense,
            };

            await _transactionService.CreateAsync(transaction);

            Console.WriteLine("Расход добавлен.");
            await Task.Delay(800);
        }

        // Показать последние 10 транзакций
        private async Task ShowLastTransactionsAsync(List<Account> accounts)
        {
            Console.Clear();
            _printer.PrintTitle("Последние транзакции");

            var list = await _transactionService.GetLastNonTransferAsync(10);

            if (list.Count == 0)
            {
                _printer.Print("Транзакций пока нет.", MessageType.Accent);
                await Task.Delay(800);
                return;
            }

            Console.WriteLine("Дата        | Счет               |     Сумма | Описание");
            Console.WriteLine("--------------------------------------------------------------------------");
            

            foreach (var t in list)
            {
               
                    var accountName = accounts.FirstOrDefault(a => a.Id == t.AccountId)?.Name ?? "Неизвестный счёт";
                    PrintTransaction(t, accountName);
                
               
            }

                Console.ReadKey();
        }

        public static void PrintTransaction(Transaction t, string accountName)
        {
            // Формируем сумму со знаком

            var sign = t.Type switch
            {
                CategoryType.Income => "+",
                CategoryType.Expense => "-",
                _ => throw new ArgumentException("Ошибка: неверный тип транзакции."),
            };

             // Выводим левую часть строки (без суммы)
            Console.Write($"{t.Date:dd.MM HH:mm} | {accountName,18} | ");

            // Красим сумму
            if (sign == "+")
                Console.ForegroundColor = ConsoleColor.Green;
            else
                Console.ForegroundColor = ConsoleColor.Red;

            
            Console.Write($"{sign}{t.Amount,8}"); // 8 — ширина колонки

            Console.ResetColor();

            // Остальная часть строки
            Console.WriteLine($" | {t.Description}");
        }

        private async Task ShowLastTransfersAsync(List<Account> accounts)
        {
            Console.Clear();
            _printer.PrintTitle("Последние переводы");

            var transfers = await _transfer.GetLastTransfersAsync(10);

            if (transfers.Count == 0)
            {
                _printer.Print("Переводов пока нет.", MessageType.Accent);
                await Task.Delay(800);
                return;
            }

            Console.WriteLine("Дата        | Откуда            ->  Куда               |   Сумма | Описание");
            Console.WriteLine("----------------------------------------------------------------------------");

            foreach (var tr in transfers)
            {
                var fromName = accounts.FirstOrDefault(a => a.Id == tr.AccountFromId)?.Name ?? "???";
                var toName = accounts.FirstOrDefault(a => a.Id == tr.AccountToId)?.Name ?? "???";

                PrintTransferRow(tr, fromName, toName);
            }

            Console.ReadKey();
        }

        private static void PrintTransferRow(Transfer tr, string fromName, string toName)
        {
            Console.Write($"{tr.Date:dd.MM HH:mm} | {fromName,-18} -> {toName,-18} | ");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"{tr.Amount,8}");
            Console.ResetColor();

            Console.WriteLine($" | {tr.Description}");
        }

    }
}
