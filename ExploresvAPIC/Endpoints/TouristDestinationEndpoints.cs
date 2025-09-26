//using ExploresvAPIC.Data;
//using ExploresvAPIC.Dto;
//using ExploresvAPIC.Models;
////using ExploresvAPIC.Utilities;
//using Microsoft.EntityFrameworkCore;
//using System.Text.Json;

//namespace ExploresvAPIC.Endpoints
//{
//    public static class TouristDestinationEndpoints
//    {
//        public static void Add(this IEndpointRouteBuilder routes)
//        {
//            var group = routes.MapGroup("/api/touristDestinations").WithTags("TouristDestination");

//            // Endpoint para crear un destino turístico con imágenes
//            group.MapPost("/", async (ExploreDb db, HttpRequest request) =>
//            {
//                //var formData = await RequestFormParser.ParseMultipartFormData(request);
//                var jsonData = formData.Fields.FirstOrDefault(f => f.Key == "data").Value;
//                var dto = JsonSerializer.Deserialize<CreateTouristDestinationDto>(jsonData);

//                // Validar relaciones
//                var department = await db.Departaments.FindAsync(dto.DepartmentId);
//                if (department is null)
//                    return Results.BadRequest(new { error = "El departamento especificado no existe." });

//                var status = await db.Status.FindAsync(dto.StatusId);
//                if (status is null)
//                    return Results.BadRequest(new { error = "El estado especificado no existe." });

//                var category = await db.Categories.FindAsync(dto.CategoryId);
//                if (category is null)
//                    return Results.BadRequest(new { error = "La categoría especificada no existe." });

//                // Crear el destino turístico
//                var entity = new TouristDestination
//                {
//                    Title = dto.Title,
//                    Description = dto.Description,
//                    Location = dto.Location,
//                    Hours = dto.Hours,
//                    DepartmentId = dto.DepartmentId,
//                    StatusId = dto.StatusId,
//                    CategoryId = dto.CategoryId,
//                    EventId = dto.EventId // Opcional
//                };

//                db.TouristDestinations.Add(entity);
//                await db.SaveChangesAsync();

//                // Procesar imágenes si existen
//                if (dto.Images != null && dto.Images.Any())
//                {
//                    foreach (var imageFile in dto.Images)
//                    {
//                        using (var memoryStream = new MemoryStream())
//                        {
//                            await imageFile.CopyToAsync(memoryStream);
//                            var fileBytes = memoryStream.ToArray();
//                            var image = new Image
//                            {
//                                Datos = fileBytes,
//                                TouristDestinationId = entity.Id,
//                                EventId = dto.EventId // Opcional
//                            };
//                            db.Images.Add(image);
//                        }
//                    }
//                    await db.SaveChangesAsync();
//                }

//                // Obtener las imágenes recién agregadas para el DTO de salida
//                var imagesDto = await db.Images
//                    .Where(img => img.TouristDestinationId == entity.Id)
//                    .Select(img => new ImageDto(
//                        img.Id,
//                        img.Datos,
//                        img.EventId,
//                        img.TouristDestinationId
//                    ))
//                    .ToListAsync();

//                var dtoSalida = new TouristDestinationDto(
//                    entity.Id,
//                    entity.Title,
//                    entity.Description,
//                    entity.Location,
//                    entity.Hours,
//                    entity.CategoryId,
//                    category.Name,
//                    entity.DepartmentId,
//                    department.Name,
//                    entity.StatusId,
//                    status.Name,
//                    imagesDto,
//                    entity.EventId
//                );

//                return Results.Created($"/touristDestination/{entity.Id}", dtoSalida);
//            });

//            // Obtener todos los destinos
//            group.MapGet("/", async (ExploreDb db) =>
//            {
//                var consulta = await db.TouristDestinations
//                    .Include(t => t.Category)
//                    .Include(t => t.Department)
//                    .Include(t => t.Status)
//                    .Include(t => t.Images)
//                    .ToListAsync();

//                var destinos = consulta.Select(l => new TouristDestinationDto(
//                    l.Id,
//                    l.Title,
//                    l.Description,
//                    l.Location,
//                    l.Hours,
//                    l.CategoryId,
//                    l.Category != null ? l.Category.Name : string.Empty,
//                    l.DepartmentId,
//                    l.Department != null ? l.Department.Name : string.Empty,
//                    l.StatusId,
//                    l.Status != null ? l.Status.Name : string.Empty,
//                    l.Images.Select(img => new ImageDto(
//                        img.Id,
//                        img.Datos,
//                        img.EventId,
//                        img.TouristDestinationId
//                    )).ToList(),
//                    l.EventId
//                ))
//                .OrderBy(l => l.Title)
//                .ToList();

//                return Results.Ok(destinos);
//            });
//        }
//    }
//}
