using FinanceApp.Application.Services;
using FinanceApp.ConsoleUI.DI;
using FinanceApp.ConsoleUI.Menu;
using FinanceApp.ConsoleUI.Services;
using FinanceApp.Domain.Interfaces.Srevices;
using FinanceApp.Domain.Models;
using Microsoft.Extensions.DependencyInjection;

var host = ConsoleBootstrapper.Build();

await LoadSettingsAsync(host.Services);
await host.SeedDataAsync();

var menu = host.Services.GetRequiredService<MainMenu>();

try
{
    await menu.RunAsync();

} catch (Exception ex)
{
    Console.WriteLine("Произошла ошибка:");
    Console.WriteLine(ex.Message);
}


// Загрузка пользовательских настроек
static async Task LoadSettingsAsync(IServiceProvider services)
{
    using var scope = services.CreateScope();

    var settings = scope.ServiceProvider.GetRequiredService<ISettingsService>();
    await settings.LoadAsync();
}