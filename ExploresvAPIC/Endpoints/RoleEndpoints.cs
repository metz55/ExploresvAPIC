using ExploresvAPIC.Data;
using ExploresvAPIC.Dto;
using ExploresvAPIC.Models;
using Microsoft.EntityFrameworkCore;

namespace ExploresvAPIC.Endpoints
{
    public static class RoleEndpoints
    {
        public static void Add(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/roles").WithTags("Roles");

            //Crear un nuevo role
            group.MapPost("/", async (ExploreDb db, CreateRoleDto dto) =>
            {
                var errores = new Dictionary<string, string[]>();

                if (string.IsNullOrWhiteSpace(dto.Name))
                    errores["name"] = ["El nombre del rol es requerido."];

                var entity = new Role
                {
                    Name = dto.Name
                };

                db.Roles.Add(entity);
                await db.SaveChangesAsync();

                var dtoSalida = new RoleDto(
                    entity.Id,
                    entity.Name);

                return Results.Created($"/roles/{entity.Id}", dtoSalida);
            });

            //Obtener todos los roles
            group.MapGet("/", async (ExploreDb db) =>
            {
                var consulta = await db.Roles.ToListAsync();

                var roles = consulta.Select(l => new RoleDto(
                    l.Id,
                    l.Name
                ))
                .OrderBy(l => l.Name)
                .ToList();

                return Results.Ok(roles);
            });

            //Obtener role por ID
            group.MapGet("/{id}", async (int id, ExploreDb db) =>
            {
                var role = await db.Roles
                    .Where(l => l.Id == id)
                        .Select(l => new RoleDto(
                            l.Id,
                            l.Name
                    ))
                    .FirstOrDefaultAsync();
                return Results.Ok(role);
            });
        }
    }
}
