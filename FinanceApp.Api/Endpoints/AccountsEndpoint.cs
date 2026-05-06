using FinanceApp.Domain.DTO;
using FinanceApp.Domain.Interfaces.Srevices;
using FinanceApp.Domain.Mappers;

namespace FinanceApp.Api.Endpoints
{
    public static class AccountsEndpoint
    {
        public static void MapAccountsEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/accounts");

            group.MapGet("/", async (IAccountService service) =>
            {
                var accounts = await service.GetAccountsAsync();

                return Results.Ok(accounts.Select(a => a.ToDto()));
            });

            group.MapGet("/{id:guid}", async (Guid id, IAccountService service) =>
            {
                var account = await service.GetByIdAsync(id);
                return account is null ? Results.NotFound() : Results.Ok(account.ToDto());
            });

            //group.MapPost("/", async (CreateAccountRequest req, IAccountService service) =>
            //{
            //    var id = await service.CreateAsync(req);
            //    return Results.Created($"/accounts/{id}", id);
            //});
        }


    }
}
