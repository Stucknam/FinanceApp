using FinanceApp.Domain.DTO;
using FinanceApp.Domain.Interfaces.Srevices;
using FinanceApp.Domain.Mappers;

namespace FinanceApp.Api.Endpoints
{
    public static class TransactionEndpoint
    {
        public static void MapTransactionEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/transactions")
                           .WithTags("Transactions");

            // GET /transactions/account/{id}?from=...&to=...
            group.MapGet("/account/{accountId}", async (
                Guid accountId,
                DateTime from,
                DateTime to,
                ITransactionService service) =>
            {
                from = DateTime.SpecifyKind(from, DateTimeKind.Utc);
                to = DateTime.SpecifyKind(to, DateTimeKind.Utc);

                to = to.Date.AddDays(1).AddTicks(-1);

                var list = await service.GetByAccountAsync(accountId, from, to);
                return Results.Ok(list.Select(x => x.ToDto()));
            });

            // GET /transactions?from=...&to=...
            group.MapGet("/", async (
                DateTime from,
                DateTime to,
                ITransactionService service) =>
            {
                from = DateTime.SpecifyKind(from, DateTimeKind.Utc);
                to = DateTime.SpecifyKind(to, DateTimeKind.Utc);

                to = to.Date.AddDays(1).AddTicks(-1);

                var list = await service.GetByPeriodAsync(from, to);
                return Results.Ok(list.Select(x => x.ToDto()));
            });

            // GET /transactions/all
            group.MapGet("/all", async (ITransactionService service) =>
            {
                var list = await service.GetAllAsync();
                return Results.Ok(list.Select(x => x.ToDto()));
            });

            // GET /transactions/last/{count}
            group.MapGet("/last/{count:int}", async (
                int count,
                ITransactionService service) =>
            {
                var list = await service.GetLastNonTransferAsync(count);
                return Results.Ok(list.Select(x => x.ToDto()));
            });

            // POST /transactions
            group.MapPost("/", async (
                CreateTransactionDto dto,
                ITransactionService service) =>
            {
                var model = dto.ToModel();
                var created = await service.CreateAsync(model);
                return Results.Created($"/transactions/{created.Id}", created.ToDto());
            });

            // PUT /transactions/{id}
            group.MapPut("/{id}", async (
                Guid id,
                TransactionDto dto,
                ITransactionService service) =>
            {
                if (id != dto.Id)
                    return Results.BadRequest("ID mismatch");

                var model = dto.ToModel();
                await service.UpdateAsync(model);

                return Results.NoContent();
            });

            // DELETE /transactions/{id}
            group.MapDelete("/{id}", async (
                Guid id,
                ITransactionService service) =>
            {
                await service.DeleteAsync(id);
                return Results.NoContent();
            });
        }
    }
}
