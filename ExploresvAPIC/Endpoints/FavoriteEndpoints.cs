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
                    .Include(f => f.User)
                    .Include(f => f.TouristDestination)
                        .ThenInclude(td => td.Category)
                    .Include(f => f.TouristDestination)
                        .ThenInclude(td => td.Department)
                    .Include(f => f.TouristDestination)
                        .ThenInclude(td => td.Status)
                    .Include(f => f.TouristDestination)
                        .ThenInclude(td => td.Images)
                    .ToListAsync();

                var favorities = consulta.Select(l => new FavoriteWithTouristDestinationDto(
                    l.Id,
                    l.UserId,
                    l.TouristDestinationId,
                    new TouristDestinationDto(
                        l.TouristDestination.Id,
                        l.TouristDestination.Title,
                        l.TouristDestination.Description,
                        l.TouristDestination.Location,
                        l.TouristDestination.Hours,
                        l.TouristDestination.CategoryId,
                        l.TouristDestination.Category.Name,
                        l.TouristDestination.DepartmentId,
                        l.TouristDestination.Department.Name,
                        l.TouristDestination.StatusId,
                        l.TouristDestination.Status.Name,
                        l.TouristDestination.Images.Select(img => new ImageDto(
                            img.Id,
                            img.Datos,
                            img.EventId,
                            img.TouristDestinationId
                        )).ToList(),
                        new List<EventDto>()
                    )
                ))
                .OrderBy(l => l.Id)
                .ToList();

                return Results.Ok(favorities);
            });
        }
    }
}