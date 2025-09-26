using ExploresvAPIC.Data;
using ExploresvAPIC.Dto;
using ExploresvAPIC.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ExploresvAPIC.Endpoints
{
    public static class EventEndpoints
    {
        public static void Add(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/events").WithTags("Events");

            group.MapPost("/", async (ExploreDb db, CreateEventDto dto) =>
            {
                var errores = new Dictionary<string, string[]>();

                if (string.IsNullOrWhiteSpace(dto.Title))
                    errores["title"] = ["El titulo del evento es requerido."];

                if (string.IsNullOrWhiteSpace(dto.Description))
                    errores["description"] = ["La descripcion es requerida."];

                //
                if (dto.Date == default)
                    errores["date"] = ["La fecha es requerida."];

                if (dto.Images == null || !dto.Images.Any())
                    errores["images"] = ["Debe agregar al menos una imagen."];

                if (errores.Any())
                    return Results.BadRequest(errores);

                var entity = new Event
                {
                    Title = dto.Title,
                    Description = dto.Description,
                    Date = dto.Date,
                    Images = dto.Images.Select(img => new Image { Datos = img }).ToList()
                };

                db.Events.Add(entity);
                await db.SaveChangesAsync();

                var dtoSalida = new EventDto(
                    entity.Id,
                    entity.Title,
                    entity.Description,
                    entity.Date,
                    entity.Images.Select(i => new ImageDto(
                        i.Id, 
                        i.Datos, 
                        i.EventId, 
                        i.TouristDestinationId
                    )).ToList()
                );

                return Results.Created($"/api/events/{entity.Id}", dtoSalida);
            });

            //Obtener todos
            group.MapGet("/", async (ExploreDb db) => {

                var consulta = await db.Events
                    .Include(e => e.Images) //
                    .ToListAsync();

                var events = consulta.Select(l => new EventDto(
                    l.Id,
                    l.Title,
                    l.Description,
                    l.Date,
                    l.Images.Select(i => new ImageDto(
                        i.Id,
                        i.Datos,
                        i.EventId,
                        i.TouristDestinationId
                    )).ToList()
                ))
                .OrderBy(l => l.Title)
                .ToList();

                return Results.Ok(events);
            });

            //Obtener por ID
            group.MapGet("/{id}", async (int id, ExploreDb db) =>
            {
                var evento = await db.Events
                    .Where(l => l.Id == id)
                    .Select(l => new EventDto(
                        l.Id,
                        l.Title,
                        l.Description,
                        l.Date,
                        l.Images.Select(i => new ImageDto(
                            i.Id,
                            i.Datos,
                            i.EventId,
                            i.TouristDestinationId
                        )).ToList()
                    ))
                    .FirstOrDefaultAsync();

                if (evento is null)
                    return Results.NotFound();

                return Results.Ok(evento);
            });

            //Actualizar Event
            group.MapPut("/{id}", async (int id, ModifyEventDto dto, ExploreDb db) =>
            {
                var evento = await db.Events.FindAsync(id);

                if (evento is null)
                    return Results.NotFound();

                // Validaciones simples
                var errores = new Dictionary<string, string[]>();

                if (string.IsNullOrWhiteSpace(dto.Title))
                    errores["title"] = ["El título es requerido."];

                if (string.IsNullOrWhiteSpace(dto.Description))
                    errores["description"] = ["La descripción es requerida."];

                if (dto.Date == default)
                    errores["date"] = ["La fecha es requerida."];

                if (errores.Any())
                    return Results.BadRequest(errores);

                // Actualizamos propiedades
                evento.Title = dto.Title;
                evento.Description = dto.Description;
                evento.Date = dto.Date;

                await db.SaveChangesAsync();

                return Results.NoContent();
            });
        }
    }
}