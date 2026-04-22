using FinanceApp.ConsoleUI.Menu;
using FinanceApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Runtime;
using System.Security.Principal;
using System.Text;

namespace FinanceApp.ConsoleUI.Output
{
    public class ConsoleHelper
    {
        private readonly IConsolePrinter _printer;

        public ConsoleHelper(IConsolePrinter printer)
        {
            _printer = printer;
        }

        public async Task<Account?> SelectAccountAsync(string prompt, PaginationResult<Account> pages, List<Account> accounts)
        {

            if (!pages.Items.Any())
            {
                _printer.Print("На этой странице нет счетов.", MessageType.Error);
                return null;
            }

            _printer.Print(prompt);
            _printer.Print("Введите номер счёта или 'a' чтобы выбрать из всех счетов.");
            Console.Write(">> ");
            var input = Console.ReadLine()?.Trim().ToLower();

            // Выбор из всех счетов
            if (input == "a")
            {
                return await SelectFromAllAccountsAsync(accounts);
            }

            // Выбор со страницы
            if (!int.TryParse(input, out int number))
            {
                _printer.Print("Неверный ввод. Введите число или 'a'.", MessageType.Error);
                return null;
            }

            var list = pages.Items.ToList();

            if (number < 1 || number > list.Count)
            {
                _printer.Print("Неверный номер счёта.", MessageType.Error);
                return null;
            }

            return list[number - 1];
        }

        private async Task<Account?> SelectFromAllAccountsAsync(List<Account> accounts)
        { 
            if (accounts.Count == 0)
            {
                _printer.Print("Нет доступных счетов.", MessageType.Error);
                await Task.Delay(800);
                return null;
            }

            _printer.Print("Выберите счёт из полного списка:");

            int i = 1;
            foreach (var acc in accounts)
            {
                _printer.Print($"[{i}] {acc.Name} — {acc.Amount} руб.");
                i++;
            }

            Console.Write(">> ");
            var input = Console.ReadLine();

            if (!int.TryParse(input, out int number) || number < 1 || number > accounts.Count)
            {
                _printer.Print("Неверный номер.", MessageType.Error);
                await Task.Delay(800);
                return null;
            }

            return accounts[number - 1];
        }

        public void PrintAccountsPageAsync(List<Account> accounts, int page, PaginationResult<Account> pages, Guid? defaultAccountId)
        {

            if (accounts.Count == 0)
            {
                _printer.Print("Нет счетов для отображения.", MessageType.Accent);
                return ;
            }

            _printer.Print($"Страница {page}/{pages.TotalPages}");
            Console.WriteLine();

            int i = 0;
            foreach (var account in pages.Items)
            {


                var type = (account.Id == defaultAccountId) ? MessageType.Accent : MessageType.Default;

                _printer.Print($"[{++i}] {account.Name,-20} {account.Amount}руб.", type);
            }
        }

        public async Task ExecuteSafeAsync(Func<Task> action)
        {
            try
            {
                await action();
            }
            catch (Exception ex)
            {
                _printer.Print($"Ошибка: {ex.Message}", MessageType.Error);
                await Task.Delay(1000);
            }
        }
    }
}
