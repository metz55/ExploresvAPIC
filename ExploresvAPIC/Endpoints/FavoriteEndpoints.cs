using ExploresvAPIC.Data;
using ExploresvAPIC.Dto;
using ExploresvAPIC.Models;
using Microsoft.EntityFrameworkCore;

namespace ExploresvAPIC.Endpoints
{
    public static class FavoriteEndpoints
    {
        public static void Add(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/favorities").WithTags("Favorite");

            //Create favorite
            group.MapPost("/", async (ExploreDb db, CreateFavoriteDto dto) =>
            {
                var errores = new Dictionary<string, string[]>();

                var user = await db.Users.FindAsync(dto.UserId);
                if (user is null)
                    return Results.BadRequest(new { error = "El usuario especificado no existe." });

                var touristDestination = await db.TouristDestinations.FindAsync(dto.TouristDestinationId);
                if (touristDestination is null)
                    return Results.BadRequest(new { error = "El destino turistico especificado no existe." });

                var entity = new Favorite
                {
                    UserId = dto.UserId,
                    TouristDestinationId = dto.TouristDestinationId
                };

                db.Favorities.Add(entity);
                await db.SaveChangesAsync();

                var dtoSalida = new FavoriteDto(
                    entity.Id,
                    entity.UserId,
                    entity.TouristDestinationId
                );

                return Results.Created($"/api/favorite/{entity.Id}", dtoSalida);
            });

            //Obtener todos los favorite
            group.MapGet("/", async (ExploreDb db) =>
            {
                var consulta = await db.Favorities
                    .Include(u => u.User)
                    .Include(u => u.TouristDestination)
                    .ToListAsync();

                var favorities = consulta.Select(l => new FavoriteDto(
                    l.Id,
                    l.UserId,
                    l.TouristDestinationId
                ))
                .OrderBy(l => l.Id)
                .ToList();

                return Results.Ok(favorities);
            });
        }
    }
}