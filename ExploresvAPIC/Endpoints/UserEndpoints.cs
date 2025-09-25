using ExploresvAPIC.Data;
using ExploresvAPIC.Dto;
using ExploresvAPIC.Models;
using Microsoft.EntityFrameworkCore;

namespace ExploresvAPIC.Endpoints
{
    public static class UserEndpoints
    {
        public static void Add(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/users").WithTags("Users");

            group.MapPost("/", async (ExploreDb db, CreateUserDto dto) =>
            {
                var errores = new Dictionary<string, string[]>();

                if (string.IsNullOrWhiteSpace(dto.Name))
                    errores["name"] = ["Su nombre es requerido."];

                if (string.IsNullOrWhiteSpace(dto.Apellido))
                    errores["apellido"] = ["Su apellido es requerido."];

                if (string.IsNullOrWhiteSpace(dto.Email))
                    errores["email"] = ["El correo es requerido."];

                if (string.IsNullOrWhiteSpace(dto.Clave))
                    errores["clave"] = ["LA clave es requerida."];

                //Validar si el role existe
                var role = await db.Roles.FindAsync(dto.RoleId);
                if (role is null)
                    return Results.BadRequest(new { error = "El rol especificado no existe." });

                var entity = new User
                {
                    Name = dto.Name,
                    Apellido = dto.Apellido,
                    Email = dto.Email,
                    Clave = dto.Clave,
                    RoleId = dto.RoleId
                };

                db.Users.Add(entity);
                await db.SaveChangesAsync();

                var dtoSalida = new UserDto(
                    entity.Id,
                    entity.Name,
                    entity.Apellido,
                    entity.Email,
                    entity.Clave,
                    entity.RoleId,
                    role.Name);

                return Results.Created($"/users/{entity.Id}", dtoSalida);
            });

            //Obtener todos los Usuarios
            group.MapGet("/", async (ExploreDb db) => {

                var consulta = await db.Users
                    .Include(u => u.Role)
                    .ToListAsync();

                var users = consulta.Select(l => new UserDto(
                    l.Id,
                    l.Name,
                    l.Apellido,
                    l.Email,
                    l.Clave,
                    l.RoleId,
                    l.Role != null ? l.Role.Name : ""
                ))
                .OrderBy(l => l.Name)
                .ToList();

                return Results.Ok(users);
            });

            //Obtener role por ID
            group.MapGet("/{id}", async (int id, ExploreDb db) =>
            {
                var user = await db.Users
                    .Include(u => u.Role)
                    .Where(l => l.Id == id)
                    .Select(l => new UserDto(
                        l.Id,
                        l.Name,
                        l.Apellido,
                        l.Email,
                        l.Clave,
                        l.RoleId,
                        l.Role != null ? l.Role.Name : ""
                    ))
                    .FirstOrDefaultAsync();

                return user is not null ? Results.Ok(user) : Results.NotFound();
            });

            //Actualizar role
            group.MapPut("/{id}", async (int id, ModifyUserDto dto, ExploreDb db) => {
                var user = await db.Users.FindAsync(id);
                if (user is null)
                    return Results.NotFound();

                //Validar si role existe
                var role = await db.Roles.FindAsync(dto.RoleId);
                if (role is null)
                    return Results.BadRequest(new { error = "El rol especificado no existe." });

                user.Name = dto.Name;
                user.Apellido = dto.Apellido;
                user.Email = dto.Email;
                user.Clave = dto.Clave;
                user.RoleId = dto.RoleId;

                await db.SaveChangesAsync();

                return Results.NoContent();
            });
        }
    }
}