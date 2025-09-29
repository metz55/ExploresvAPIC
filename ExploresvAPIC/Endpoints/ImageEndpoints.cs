using ExploresvAPIC.Data;
using ExploresvAPIC.Dto;
using ExploresvAPIC.Models;
using Microsoft.EntityFrameworkCore;

namespace ExploresvAPIC.Endpoints
{
    public static class ImageEndpoints
    {
        public static void Add(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/images").WithTags("Images");


            group.MapPost("/", async (ExploreDb db, CreateImageDto dto) =>
            {
                var errores = new Dictionary<string, string[]>();

                // Validación: debe venir exactamente 1 ID
                if ((dto.EventId.HasValue && dto.TouristDestinationId.HasValue) ||
                    (!dto.EventId.HasValue && !dto.TouristDestinationId.HasValue))
                {
                    return Results.BadRequest(new { error = "Debes especificar únicamente EventId o TouristDestinationId, pero no ambos." });
                }

                // Validar si es Event
                if (dto.EventId.HasValue)
                {
                    var ev = await db.Events.FindAsync(dto.EventId.Value);
                    if (ev is null)
                        return Results.BadRequest(new { error = "El evento especificado no existe" });
                }

                // Validar si es TouristDestination
                if (dto.TouristDestinationId.HasValue)
                {
                    var td = await db.TouristDestinations.FindAsync(dto.TouristDestinationId.Value);
                    if (td is null)
                        return Results.BadRequest(new { error = "El destino turístico especificado no existe" });
                }

                var entity = new Image
                {
                    Datos = dto.Datos,
                    EventId = dto.EventId,
                    TouristDestinationId = dto.TouristDestinationId
                };

                db.Images.Add(entity);
                await db.SaveChangesAsync();

                var dtoSalida = new ImageDto(
                    entity.Id,
                    entity.Datos,
                    entity.EventId,
                    entity.TouristDestinationId
                );

                return Results.Created($"/api/images/{entity.Id}", dtoSalida);
            });
        }

    }
}