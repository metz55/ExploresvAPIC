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

                var entity = new User
                {
                    Name = dto.Name,
                    Apellido = dto.Apellido,
                    Email = dto.Email,
                    Clave = dto.Clave
                };

                db.Users.Add(entity);
                await db.SaveChangesAsync();

                var dtoSalida = new UserDto(
                    entity.Id,
                    entity.Name,
                    entity.Apellido,
                    entity.Email,
                    entity.Clave,
                    default); //Revisar si esto esta bien

                return Results.Created($"/users/{entity.Id}", dtoSalida);
            });

            //Obtener todos
            group.MapGet("/", async (ExploreDb db) => {

                var consulta = await db.Users.ToListAsync();

                var users = consulta.Select(l => new UserDto(
                    l.Id,
                    l.Name,
                    l.Apellido,
                    l.Email,
                    l.Clave,
                    default
                ))
                .OrderBy(l => l.Name)
                .ToList();

                return Results.Ok(users);
            });

            //Obtener por ID
            group.MapGet("/{id}", async (int id, ExploreDb db) =>
            {
                var user = await db.Users
                .Where(l => l.Id == id)
                    .Select(l => new UserDto(
                        l.Id,
                        l.Name,
                        l.Apellido,
                        l.Email,
                        l.Clave,
                        default
                    ))
                    .FirstOrDefaultAsync();
                return Results.Ok(user);
            });

            //Actualizar o modificar
            group.MapPut("/{id}", async (int id, ModifyUserDto dto, ExploreDb db) => {
                var user = await db.Users.FindAsync(id);
                if (user is null)
                    return Results.NotFound();

                user.Name = dto.Name;
                user.Apellido = dto.Apellido;
                user.Email = dto.Email;
                user.Clave = dto.Clave;

                await db.SaveChangesAsync();

                return Results.NoContent();
            });
        }
    }
}