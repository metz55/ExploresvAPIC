using ExploresvAPIC.Data;
using ExploresvAPIC.Dto;
using ExploresvAPIC.Models;
using Microsoft.EntityFrameworkCore;

namespace ExploresvAPIC.Endpoints
{
    public static class RoleEndpoints
    {
        public static void Add(IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/roles").WithTags("Roles");

            // Crear un nuevo rol
            group.MapPost("/", async (ExploreDb db, CreateRoleDto dto) =>
            {
                var errores = new Dictionary<string, string[]>();
                if (string.IsNullOrWhiteSpace(dto.Name))
                    errores["name"] = ["El nombre del rol es requerido."];

                if (errores.Count > 0)
                    return Results.BadRequest(errores);

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

            // Obtener todos los roles
            group.MapGet("/", async (ExploreDb db) =>
            {
                var roles = await db.Roles
                    .Select(r => new RoleDto(
                        r.Id,
                        r.Name
                    ))
                    .OrderBy(r => r.Name)
                    .ToListAsync();

                return Results.Ok(roles);
            });

            // Obtener un rol por ID
            group.MapGet("/{id}", async (int id, ExploreDb db) =>
            {
                var role = await db.Roles
                    .Where(r => r.Id == id)
                    .Select(r => new RoleDto(
                        r.Id,
                        r.Name
                    ))
                    .FirstOrDefaultAsync();

                if (role == null)
                    return Results.NotFound();

                return Results.Ok(role);
            });

            // Modificar un rol
            group.MapPut("/{id}", async (int id, ExploreDb db, ModifyRoleDto dto) =>
            {
                var errores = new Dictionary<string, string[]>();
                if (string.IsNullOrWhiteSpace(dto.Name))
                    errores["name"] = ["El nombre del rol es requerido."];

                if (errores.Count > 0)
                    return Results.BadRequest(errores);

                var role = await db.Roles.FindAsync(id);
                if (role == null)
                    return Results.NotFound();

                role.Name = dto.Name;
                await db.SaveChangesAsync();

                var dtoSalida = new RoleDto(
                    role.Id,
                    role.Name);

                return Results.Ok(dtoSalida);
            });

            // Eliminar un rol
            group.MapDelete("/{id}", async (int id, ExploreDb db) =>
            {
                var role = await db.Roles.FindAsync(id);
                if (role == null)
                    return Results.NotFound();

                db.Roles.Remove(role);
                await db.SaveChangesAsync();

                return Results.NoContent();
            });
        }
    }
}
