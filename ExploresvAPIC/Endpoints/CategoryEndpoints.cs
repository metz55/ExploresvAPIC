using ExploresvAPIC.Data;
using ExploresvAPIC.Dto;
using ExploresvAPIC.Dto.DepartmentDto;
using ExploresvAPIC.Models;
using Microsoft.EntityFrameworkCore;

namespace ExploresvAPIC.Endpoints
{
    public static class CategoryEndpoint
    {
        public static void Add(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/departments").WithTags("Departments");

            group.MapPost("/", async (ExploreDb db, CrearDepartmentDto dto) => {
                var errores = new Dictionary<string, string[]>();

                if (string.IsNullOrWhiteSpace(dto.Name))
                    errores["name"] = ["El nombre es requerido"];

                if (errores.Count > 0) return Results.ValidationProblem(errores);

                var entity = new Department
                {
                    Name = dto.Name
                };

                db.Departaments.Add(entity);
                await db.SaveChangesAsync();

                var dtoSalida = new DepartmentsDto(
                    entity.Id,
                    entity.Name);

                return Results.Created($"/api/departments/{entity.Id}", dtoSalida);
            });

            group.MapGet("/", async (ExploreDb db) => {

                var consulta = await db.Departaments.ToListAsync();
                var departamets = consulta.Select(l => new DepartmentsDto(
                    l.Id,
                    l.Name
                ))
                .OrderBy(l => l.Name)
                .ToList();


                return Results.Ok(departamets);

            });
        }
    }
}
