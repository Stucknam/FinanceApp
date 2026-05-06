using FinanceApp.Application.Services;
using FinanceApp.ConsoleUI.Menu;
using FinanceApp.ConsoleUI.Output;
using FinanceApp.ConsoleUI.Services;
using FinanceApp.Data;
using FinanceApp.Data.Repositories;
using FinanceApp.Domain.Interfaces.Repositories;
using FinanceApp.Domain.Interfaces.Srevices;
using FinanceApp.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceApp.ConsoleUI.DI
{
    public static class ConsoleBootstrapper
    {
        public static IHost Build()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration(config =>
                {
                    config.AddJsonFile("appsettings.json", optional: false);
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                })

                .ConfigureServices((context, services) =>
                {
                    var connectionString = context.Configuration.GetConnectionString("DefaultConnection");
                    // Подключаем DBContext
                    services.AddDbContext<FinanceDbContext>(options => {
                        options.UseNpgsql(connectionString); 
                        options.EnableSensitiveDataLogging(false);
                        options.EnableDetailedErrors(false);
                        options.LogTo(_ => { }, LogLevel.None); 
                    });

                    services.AddSingleton<AppPaths>();

                    services.AddSingleton<ISettingsService>(sp =>
                    {
                        var paths = sp.GetRequiredService<AppPaths>();
                        return new SettingsService(paths.SettingsPath);
                    });


                    // Регистрация репозиториев
                    services.AddScoped<IAccountRepository, AccountRepository>();
                    services.AddScoped<ICategoryRepository, CategoryRepository>();
                    services.AddScoped<ITransactionRepository, TransactionRepository>();
                    services.AddScoped<ITransferRepository, TransferRepository>();

                    // Регистрация сервисов
                    services.AddScoped<IAccountService, AccountService>();
                    services.AddScoped<ITransactionService, TransactionService>();
                    services.AddScoped<ITransferService, TransferService>();
                    services.AddScoped<ICategoryService, CategoryService>();
                    services.AddScoped<IStatisticsService, StatisticsService>();

                    
                    services.AddSingleton<IConsolePrinter, ConsolePrinter>();
                    services.AddSingleton<IPaginationService, PaginateService>();
                    services.AddSingleton<AccountsMenu>();
                    services.AddSingleton<TransactionsMenu>();
                    services.AddSingleton<MainMenu>();

                    services.AddSingleton<ConsoleHelper>();

                })
                .Build();
        }

        public static async Task SeedDataAsync(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var accountService = scope.ServiceProvider.GetRequiredService<IAccountService>();
            var settingsService = scope.ServiceProvider.GetRequiredService<ISettingsService>();

            var accounts = await accountService.GetAccountsAsync();
            if (accounts.Count == 0)
            {
                var defaultAccount = await accountService.CreateAsync(new Account
                {
                    Name = "Основной счёт",
                    Amount = 0,
                    Description = "Создан автоматически"
                });
                settingsService.SetDefaultAccountId(defaultAccount.Id);
            
            }
        }
    }
}
