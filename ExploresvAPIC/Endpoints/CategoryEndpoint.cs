using ExploresvAPIC.Data;
using ExploresvAPIC.Dto;
using ExploresvAPIC.Models;
using Microsoft.EntityFrameworkCore;

namespace ExploresvAPIC.Endpoints
{
    public static class CategoryEndpoint
    {
        public static void Add(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/categories").WithTags("Categories");

            // Crear Category
            group.MapPost("/", async (ExploreDb db, CreateCategoryDto dto) =>
            {
                var errores = new Dictionary<string, string[]>();

                if (string.IsNullOrWhiteSpace(dto.Name))
                    errores["name"] = ["El nombre es requerido"];

                if (errores.Count > 0)
                    return Results.ValidationProblem(errores);

                var entity = new Category
                {
                    Name = dto.Name
                };

                db.Categories.Add(entity);
                await db.SaveChangesAsync();

                var dtoSalida = new CategoryDto(
                    entity.Id,
                    entity.Name
                );

                return Results.Created($"/api/categories/{entity.Id}", dtoSalida);
            });

            // Obtener todas las categorías
            group.MapGet("/", async (ExploreDb db) =>
            {
                var consulta = await db.Categories.ToListAsync();

                var categories = consulta.Select(l => new CategoryDto(
                    l.Id,
                    l.Name
                ))
                .OrderBy(l => l.Name)
                .ToList();

                return Results.Ok(categories);
            });
        }
    }
}
