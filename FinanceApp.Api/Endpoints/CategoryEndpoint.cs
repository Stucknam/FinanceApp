using FinanceApp.Domain.DTO;
using FinanceApp.Domain.Enums;
using FinanceApp.Domain.Interfaces.Srevices;
using FinanceApp.Domain.Mappers;

namespace FinanceApp.Api.Endpoints
{
    public static class CategoryEndpoint
    {
        public static void MapCategoryEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/category").WithTags("Category");

            // GET /category
            group.MapGet("/", async (ICategoryService service) =>
            {
                var categories = await service.GetAllAsync();
                return Results.Ok(categories.Select(c => c.ToDto()));
            });

            // GET /category/{id}
            group.MapGet("/{id:guid}", async (Guid id, ICategoryService service) =>
            {
                var category = await service.GetByIdAsync(id);
                return category is null
                    ? Results.NotFound()
                    : Results.Ok(category.ToDto());
            });

            // GET /category/type/{type}
            group.MapGet("/type/{type}", async (CategoryType type, ICategoryService service) =>
            {
                var categories = await service.GetByTypeAsync(type);
                return categories is null
                    ? Results.NotFound()
                    : Results.Ok(categories.Select(c => c.ToDto()));
            });

            // POST /category
            group.MapPost("/", async (CreateCategoryDto dto, ICategoryService service) =>
            {
                var category = dto.ToModel();
                var created = await service.CreateAsync(category);
                return Results.Created($"/category/{created.Id}", created.ToDto());
            });


            // PUT /category/{id}
            group.MapPut("/{id:guid}", async (Guid id, CategoryDto dto, ICategoryService service) =>
            {
                var existing = await service.GetByIdAsync(id);
                if (existing is null)
                    return Results.NotFound();

                var updated = dto.ToModel();
                updated.Id = id;

                await service.UpdateAsync(updated);
                return Results.NoContent();
            });


            // DELETE /category/{id}
            group.MapDelete("/{id:guid}", async (Guid id, ICategoryService service) =>
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
