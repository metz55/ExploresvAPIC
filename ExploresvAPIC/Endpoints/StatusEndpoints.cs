using ExploresvAPIC.Data;
using ExploresvAPIC.Dto;
using ExploresvAPIC.Models;
using Microsoft.EntityFrameworkCore;

namespace ExploresvAPIC.Endpoints
{
    public static class StatusEndpoints
    {
        public static void Add(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/statuses").WithTags("Statuses");

            group.MapPost("/", async (ExploreDb db, CreateStatusDto dto) =>
            {
                var errores = new Dictionary<string, string[]>();

                if (string.IsNullOrWhiteSpace(dto.Name))
                    errores["name"] = ["El nombre de estado es requerido."];

                if (errores.Count > 0)
                    return Results.BadRequest(errores);

                var entity = new Status
                {
                    Name = dto.Name
                };

                //Debe ser Statuses y no Status segun BibliotecaDb
                db.Status.Add(entity);
                await db.SaveChangesAsync();

                var dtoSalida = new StatusDto(
                    entity.Id,
                    entity.Name);

                return Results.Created($"/statuses/{entity.Id}", dtoSalida);
            });

            //Obtener todos
            group.MapGet("/", async (ExploreDb db) => {

                //Debe ser Statuses y no Status segun BibliotecaDb
                var consulta = await db.Status.ToListAsync();

                var statuses = consulta.Select(l => new StatusDto(
                    l.Id,
                    l.Name
                ))
                .OrderBy(l => l.Name)
                .ToList();

                return Results.Ok(statuses);
            });

            //Obtener por ID
            group.MapGet("/{id}", async (int id, ExploreDb db) =>
            {
                var status = await db.Status //db.Statuses debe ser
                .Where(l => l.Id == id)
                    .Select(l => new StatusDto(
                        l.Id,
                        l.Name
                    ))
                    .FirstOrDefaultAsync();
                return Results.Ok(status);
            });
        }
    }
}