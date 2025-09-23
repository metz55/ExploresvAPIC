//using ExploresvAPIC.Data;
//using ExploresvAPIC.Dto;
//using ExploresvAPIC.Models;

//namespace ExploresvAPIC.Endpoints
//{
//    public static class TouristDestinationEndpoints
//    {
//        public static void Add(this IEndpointRouteBuilder routes)
//        {
//            var group = routes.MapGroup("/api/touristDestinations").WithTags("TouristDestination");

//            group.MapPost("/", async (ExploreDb db, CreateTouristDestinationDto dto) =>
//            {
//                var errores = new Dictionary<string, string[]>();

//                if (string.IsNullOrWhiteSpace(dto.Title))
//                    errores["title"] = ["El titulo es requerido."];

//                if (string.IsNullOrWhiteSpace(dto.Description))
//                    errores["description"] = ["La Descripcion es requerida."];

//                if (string.IsNullOrWhiteSpace(dto.Location))
//                    errores["location"] = ["La ubicacion es requerida."];

//                if (string.IsNullOrWhiteSpace(dto.Hours))
//                    errores["hours"] = ["El horario es requerida."];

//                if (errores.Count > 0) return Results.ValidationProblem(errores);

//                var entity = new TouristDestination
//                {
//                    Title = dto.Title,
//                    Description = dto.Description,
//                    Location = dto.Location,
//                    Hours = dto.Hours,
//                };

//                db.TouristDestinations.Add(entity);
//                await db.SaveChangesAsync();

//                var dtoSalida = new TouristDestinationDto(
//                    entity.Id,
//                    entity.Title,
//                    entity.Description,
//                    entity.Location,
//                    entity.Hours);

//                return Results.Created($"/touristDestination/{entity.Id}", dtoSalida);
//            });
//        }
//    }
//}