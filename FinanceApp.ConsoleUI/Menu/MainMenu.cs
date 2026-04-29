using FinanceApp.ConsoleUI.Output;
using FinanceApp.Domain.Interfaces.Srevices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;

namespace FinanceApp.ConsoleUI.Menu
{

    public class MainMenu
    {
        private readonly IConsolePrinter _printer;

        private readonly AccountsMenu _accountsMenu;
        private readonly TransactionsMenu _transactionsMenu;
        public MainMenu(IConsolePrinter printer, AccountsMenu accountsMenu, TransactionsMenu transactionsMenu)
        {
            _printer = printer;
            _accountsMenu = accountsMenu;
            _transactionsMenu = transactionsMenu;
        }

        private void ShowMainMenu()
        {
            Console.Write(
                "Что вы хотите сделать?" +
                "\n\t1. Управление счетами" +
                "\n\t2. Управление транзакциями" +
                "\n[q] - Выход \n" +
                ">> "
                );
        }
        public async Task RunAsync()
        {
            Console.Title = "Мои финансы";

            

            _printer.Print("Добро пожаловать в мои финансы!", MessageType.Accent);
            _printer.Print("Здесь вы можете учитывать свои доходы и расходы и видеть статистику по счетам.", MessageType.Accent);

            
            while (true) 
            {
                _printer.PrintTitle("Мои финансы - главное меню");
                _printer.Print($"Дата: {DateOnly.FromDateTime(DateTime.UtcNow)}");
                ShowMainMenu();


                var input = Console.ReadLine();

                switch (input?.ToLower())
                {
                    case "1": await _accountsMenu.RunAsync();
                        break;
                    case "2": await _transactionsMenu.RunAsync();
                        break;
                    case "3": 
                        break;
                    case "q": _printer.Print("Спасибо за использование приложения \"Мои финансы\" До свидания!", MessageType.Accent); 
                        return;
                    default:

                        _printer.Print("Некорректный ввод.Пожалуйста, выберите одну из предложенных опций.", MessageType.Error);
                        Thread.Sleep(1500);
                        break;


                }
                Console.Clear();

            }   

        }
    }
}
