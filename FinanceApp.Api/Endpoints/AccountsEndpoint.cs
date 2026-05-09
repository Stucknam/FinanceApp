using FinanceApp.Domain.DTO;
using FinanceApp.Domain.Interfaces.Srevices;
using FinanceApp.Domain.Mappers;
using FinanceApp.Domain.Models;

namespace FinanceApp.Api.Endpoints
{
    public static class AccountsEndpoint
    {
        public static void MapAccountsEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/accounts");

            // 1. GET /accounts
            group.MapGet("/", async (IAccountService service) =>
            {
                var accounts = await service.GetAccountsAsync();

                return Results.Ok(accounts.Select(a => a.ToDto()));
            });

            // 2. GET /accounts/{id}
            group.MapGet("/{id:guid}", async (Guid id, IAccountService service) =>
            {
                var account = await service.GetByIdAsync(id);
                return account is null ? Results.NotFound() : Results.Ok(account.ToDto());
            });

            // 3. GET /accounts/{id}/balance
            group.MapGet("/balance/{id:guid}", async (Guid id, IAccountService service) =>
            {
                var account = await service.GetByIdAsync(id);
                if (account is null)
                    return Results.NotFound();

                var balance = await service.GetBalanceAsync(id);
                return Results.Ok(balance);

            });

            // 4. GET /accounts/{id}/profit?from=...&to=...
            group.MapGet("/{id:guid}/profit", async (
                Guid id,
                DateTime from,
                DateTime to,
                IAccountService service) =>
            {
                var account = await service.GetByIdAsync(id);
                if (account is null)
                    return Results.NotFound();

                var profit = await service.GetProfitAsync(id, from, to);
                return Results.Ok(profit);
            });

            // 5. POST /accounts — создать счёт
            group.MapPost("/", async (CreateAccountDto account, IAccountService service) =>
            {
                var newAccount = account.ToModel();

                var created = await service.CreateAsync(newAccount);
                return Results.Created($"/accounts/{created.Id}", created);
            });

            // 6. PUT /accounts/{id} — обновить счёт
            group.MapPut("/{id:guid}", async (Guid id, AccountDto dto, IAccountService service) =>
            {
                var existing = await service.GetByIdAsync(id);
                if (existing is null)
                    return Results.NotFound();

                // гарантируем корректный ID
                dto.Id = id;

                var updated = dto.ToModel();
                await service.UpdateAsync(updated);

                return Results.NoContent();
            });

            // 7. DELETE /accounts/{id} — удалить счёт
            group.MapDelete("/{id:guid}", async (Guid id, IAccountService service) =>
            {
                var existing = await service.GetByIdAsync(id);
                if (existing is null)
                    return Results.NotFound();

                await service.DeleteAsync(id);
                return Results.NoContent();
            });

        }


    }
}
