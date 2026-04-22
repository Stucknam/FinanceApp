using FinanceApp.Application.Services;
using FinanceApp.ConsoleUI.DI;
using FinanceApp.ConsoleUI.Menu;
using FinanceApp.Domain.Interfaces.Srevices;
using FinanceApp.Domain.Models;
using Microsoft.Extensions.DependencyInjection;

var host = ConsoleBootstrapper.Build();

await LoadAsync(host.Services);

var menu = host.Services.GetRequiredService<MainMenu>();

try
{
    menu.RunAsync().Wait();

} catch (Exception ex)
{
    Console.WriteLine("Произошла ошибка:");
    Console.WriteLine(ex.Message);
}


// Загрузка пользовательских настроек
static async Task LoadAsync(IServiceProvider services)
{
    using var scope = services.CreateScope();
    var settings = scope.ServiceProvider.GetRequiredService<ISettingsService>();
    await settings.LoadAsync();

    var accounts = scope.ServiceProvider.GetRequiredService<IAccountService>();
    await accounts.SeedDefaultAccountAsync();
}