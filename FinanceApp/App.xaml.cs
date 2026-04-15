using FinanceApp.Application.Services;
using FinanceApp.Data;
using FinanceApp.Data.Repositories;
using FinanceApp.Domain.Interfaces.Repositories;
using FinanceApp.Domain.Interfaces.Srevices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Configuration;
using System.Data;
using System.Windows;

namespace FinanceApp
{

    public partial class App : System.Windows.Application
    {
        public static IHost AppHost { get; private set; } = null!;

        public App()
        {
            AppHost = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration(config =>
                {
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((context, services) =>
                {
                    var connectionString = context.Configuration.GetConnectionString("DefaultConnection");
                    // Подключаем DBContext
                    services.AddDbContext<FinanceDbContext>(options => options.UseNpgsql(connectionString));

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


                    services.AddSingleton<MainWindow>();
                })
                .Build();


        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await AppHost.StartAsync();

            var settings = AppHost.Services.GetRequiredService<ISettingsService>();
            await settings.LoadAsync();



            var mainWindow = AppHost.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await AppHost.StopAsync();
            AppHost.Dispose();
            base.OnExit(e);

        }
    }

}
