using FinanceApp.Domain.Interfaces.Srevices;

namespace FinanceApp.Api.Endpoints
{
    public static class StatisticsEndpoint
    {
        public static void MapStatisticsEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/statistics").WithTags("Statistics");

            // GET /statistics/balance
            group.MapGet("/balance", async (IStatisticsService service) =>
            {
                var total = await service.GetTotalBalanceAsync();
                return Results.Ok(new { totalBalance = total });
            });

            // GET /statistics/profit?from=2026-01-01&to=2026-02-01
            group.MapGet("/profit", async (DateTime from, DateTime to, IStatisticsService service) =>
            {
                if (from > to)
                    return Results.BadRequest("Parameter 'from' must be earlier than 'to'");

                from = DateTime.SpecifyKind(from, DateTimeKind.Utc);
                to = DateTime.SpecifyKind(to, DateTimeKind.Utc);

                var profit = await service.GetTotalProfitAsync(from, to);
                return Results.Ok(new { totalProfit = profit });
            });
        }
    }
}
