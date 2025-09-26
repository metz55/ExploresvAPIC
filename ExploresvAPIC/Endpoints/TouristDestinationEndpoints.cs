using ExploresvAPIC.Data;
using ExploresvAPIC.Dto;
using ExploresvAPIC.Models;

using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace ExploresvAPIC.Endpoints
{
    public static class TouristDestinationEndpoints
    {
        public static void Add(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/touristDestinations").WithTags("TouristDestination");

            // TouristDestinationEndpoints.cs
            // TouristDestinationEndpoints.cs
            group.MapPost("/", async (ExploreDb db, CreateTouristDestinationDto dto) =>
            {
                var errores = new Dictionary<string, string[]>();
                if (string.IsNullOrWhiteSpace(dto.Title))
                    errores["title"] = ["El título es requerido."];
                if (string.IsNullOrWhiteSpace(dto.Description))
                    errores["description"] = ["La descripción es requerida."];
                if (string.IsNullOrWhiteSpace(dto.Location))
                    errores["location"] = ["La ubicación es requerida."];
                if (string.IsNullOrWhiteSpace(dto.Hours))
                    errores["hours"] = ["El horario es requerido."];

                if (errores.Count > 0)
                    return Results.BadRequest(errores);

                // Validar relaciones
                var department = await db.Departaments.FindAsync(dto.DepartmentId);
                if (department is null)
                    return Results.BadRequest(new { error = "El departamento especificado no existe." });

                var status = await db.Status.FindAsync(dto.StatusId);
                if (status is null)
                    return Results.BadRequest(new { error = "El estado especificado no existe." });

                var category = await db.Categories.FindAsync(dto.CategoryId);
                if (category is null)
                    return Results.BadRequest(new { error = "La categoría especificada no existe." });

                

                // Crear el destino turístico
                var entity = new TouristDestination
                {
                    Title = dto.Title,
                    Description = dto.Description,
                    Location = dto.Location,
                    Hours = dto.Hours,
                    DepartmentId = dto.DepartmentId,
                    StatusId = dto.StatusId,
                    CategoryId = dto.CategoryId
                    
                };

                db.TouristDestinations.Add(entity);
                await db.SaveChangesAsync();

                // Procesar imágenes si existen
                if (dto.Images != null && dto.Images.Any())
                {
                    foreach (var base64Image in dto.Images)
                    {
                        // Convertir Base64 a byte[]
                        byte[] imageBytes = Convert.FromBase64String(base64Image);

                        var image = new Image
                        {
                            Datos = imageBytes,
                            TouristDestinationId = entity.Id
                            
                        };
                        db.Images.Add(image);
                    }
                    await db.SaveChangesAsync();
                }

                // Obtener las imágenes recién agregadas para el DTO de salida
                var imagesDto = await db.Images
                    .Where(img => img.TouristDestinationId == entity.Id)
                    .Select(img => new ImageDto(
                        img.Id,
                        img.Datos,
                        img.EventId,
                        img.TouristDestinationId
                    ))
                    .ToListAsync();

                var dtoSalida = new TouristDestinationDto(
                    entity.Id,
                    entity.Title,
                    entity.Description,
                    entity.Location,
                    entity.Hours,
                    entity.CategoryId,
                    category.Name,
                    entity.DepartmentId,
                    department.Name,
                    entity.StatusId,
                    status.Name,
                    imagesDto
                    
                );

                return Results.Created($"/touristDestination/{entity.Id}", dtoSalida);
            });

            group.MapGet("/{id}", async (int id, ExploreDb db) =>
            {
                var destino = await db.TouristDestinations
                    .Include(t => t.Category)
                    .Include(t => t.Department)
                    .Include(t => t.Status)
                    .Include(t => t.Images)
                    .Where(l => l.Id == id)
                    .Select(l => new TouristDestinationDto(
                        l.Id,
                        l.Title,
                        l.Description,
                        l.Location,
                        l.Hours,
                        l.CategoryId,
                        l.Category != null ? l.Category.Name : string.Empty,
                        l.DepartmentId,
                        l.Department != null ? l.Department.Name : string.Empty,
                        l.StatusId,
                        l.Status != null ? l.Status.Name : string.Empty,
                        l.Images.Select(img => new ImageDto(
                            img.Id,
                            img.Datos,
                            img.EventId,
                            img.TouristDestinationId
                        )).ToList()
                        
                    ))
                    .FirstOrDefaultAsync();

                if (destino == null)
                    return Results.NotFound(new { error = "Destino turístico no encontrado." });

                return Results.Ok(destino);
            });
        }
    }
}
