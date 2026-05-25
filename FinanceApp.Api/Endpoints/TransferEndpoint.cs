using FinanceApp.Domain.DTO;
using FinanceApp.Domain.Interfaces.Srevices;
using FinanceApp.Domain.Models;
using FinanceApp.Domain.Mappers;

namespace FinanceApp.Api.Endpoints
{
    public static class TransferEndpoint
    {
        public static void MapTransferEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/transfers").WithTags("Transfers");

            // GET /transfers/account/{id}?from=...&to=...
            group.MapGet("/account/{accountId}", async (
                Guid accountId,
                DateTime from,
                DateTime to,
                ITransferService service) =>
            {
                from = DateTime.SpecifyKind(from, DateTimeKind.Utc);
                to = DateTime.SpecifyKind(to, DateTimeKind.Utc);
                to = to.Date.AddDays(1).AddTicks(-1);

                var transfers = await service.GetByAccountAsync(accountId, from, to);
                return Results.Ok(transfers.Select(t => t.ToDto()));
            });

            // GET /transfers?from=...&to=...
            group.MapGet("/", async (
                DateTime from,
                DateTime to,
                ITransferService service) =>
            {
                from = DateTime.SpecifyKind(from, DateTimeKind.Utc);
                to = DateTime.SpecifyKind(to, DateTimeKind.Utc);
                to = to.Date.AddDays(1).AddTicks(-1);

                var transfers = await service.GetByPeriodAsync(from, to);
                return Results.Ok(transfers.Select(t => t.ToDto()));
            });

            // GET /transfers/last/{count}
            group.MapGet("/last/{count:int}", async (
                int count,
                ITransferService service) =>
            {
                var transfers = await service.GetLastTransfersAsync(count);
                return Results.Ok(transfers.Select(t => t.ToDto()));
            });

            group.MapPost("/", async (
                CreateTransferDto dto,
                ITransferService service) =>
            {
                var model = dto.ToModel();
                var created = await service.CreateAsync(model);
                return Results.Created($"/transfers/{created.Id}", created.ToDto());
            });


            // POST /transfers/make
            // Создание перевода между счетами (fromId → toId)
            group.MapPost("/make", async (
                Guid fromId,
                Guid toId,
                decimal amount,
                string description,
                ITransferService service) =>
            {
                var created = await service.CreateTransferAsync(fromId, toId, amount, description);
                return Results.Created($"/transfers/{created.Id}", created);
            });

            // PUT /transfers/{id}
            group.MapPut("/{id}", async (
                Guid id,
                TransferDto dto,
                ITransferService service) =>
            {
                if (id != dto.Id)
                    return Results.BadRequest("ID mismatch");

                var model = dto.ToModel();
                await service.UpdateAsync(model);

                return Results.NoContent();
            });


            // DELETE /transfers/{id}
            group.MapDelete("/{id}", async (
                Guid id,
                ITransferService service) =>
            {
                await service.DeleteAsync(id);
                return Results.NoContent();
            });
        }
    }
}
