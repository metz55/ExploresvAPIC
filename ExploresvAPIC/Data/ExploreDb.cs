using ExploresvAPIC.Models;
using Microsoft.EntityFrameworkCore;

namespace ExploresvAPIC.Data
{
    public class ExploreDb : DbContext
    {
        public ExploreDb(DbContextOptions<ExploreDb> options) : base(options) { }
        public DbSet<Department> Departaments => Set<Department>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Event> Events => Set<Event>();
        public DbSet<Favorite> Favorities => Set<Favorite>();
        public DbSet<Image> Images => Set<Image>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<Status> Status => Set<Status>();
        public DbSet<TouristDestination> TouristDestinations => Set<TouristDestination>();
        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración para Category
            modelBuilder.Entity<Category>(e =>
            {
                e.Property(x => x.Name).IsRequired().HasMaxLength(25);
            });

            // Configuración para Departament
            modelBuilder.Entity<Department>(e =>
            {
                e.Property(x => x.Name).IsRequired().HasMaxLength(30);
            });

            // Configuración para Event
            modelBuilder.Entity<Event>(e =>
            {
                e.Property(x => x.Title).IsRequired().HasMaxLength(100);
                e.Property(x => x.Description).IsRequired().HasMaxLength(2500);
                e.Property(x => x.Date).IsRequired();
                e.HasMany(x => x.Images)
                    .WithOne(x => x.Event)
                    .HasForeignKey(x => x.EventId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuración para Favorite
            modelBuilder.Entity<Favorite>(e =>
            {
                e.HasOne(x => x.User)
                    .WithMany()
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
                e.HasOne(x => x.TouristDestination)
                    .WithMany()
                    .HasForeignKey(x => x.TouristDestinationId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuración para Image
            modelBuilder.Entity<Image>(e =>
            {
                e.Property(x => x.Datos).IsRequired();
                e.HasOne(x => x.Event)
                    .WithMany(x => x.Images)
                    .HasForeignKey(x => x.EventId)
                    .OnDelete(DeleteBehavior.Restrict);
                e.HasOne(x => x.TouristDestination)
                    .WithMany(x => x.Images)
                    .HasForeignKey(x => x.TouristDestinationId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuración para Role
            modelBuilder.Entity<Role>(e =>
            {
                e.Property(x => x.Name).IsRequired();
            });

            // Configuración para Status
            modelBuilder.Entity<Status>(e =>
            {
                e.Property(x => x.Name).IsRequired();
            });

            // Configuración para TouristDestination
            modelBuilder.Entity<TouristDestination>(e =>
            {
                e.Property(x => x.Title).IsRequired().HasMaxLength(100);
                e.Property(x => x.Description).IsRequired().HasMaxLength(2500);
                e.Property(x => x.Location).IsRequired().HasMaxLength(100);
                e.Property(x => x.Hours).IsRequired();
                e.HasOne(x => x.Category)
                    .WithMany()
                    .HasForeignKey(x => x.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
                e.HasOne(x => x.Department)
                    .WithMany()
                    .HasForeignKey(x => x.DepartmentId)
                    .OnDelete(DeleteBehavior.Restrict);
                e.HasOne(x => x.Status)
                    .WithMany()
                    .HasForeignKey(x => x.StatusId)
                    .OnDelete(DeleteBehavior.Restrict);
                e.HasOne(x => x.Event)
                    .WithMany()
                    .HasForeignKey(x => x.EventId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuración para User
            modelBuilder.Entity<User>(e =>
            {
                e.Property(x => x.Name).IsRequired();
                e.Property(x => x.Apellido).IsRequired();
                e.Property(x => x.Email).IsRequired();
                e.Property(x => x.Clave).IsRequired();
                e.HasOne(x => x.Role)
                    .WithMany()
                    .HasForeignKey(x => x.RoleId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            base.OnModelCreating(modelBuilder);
        }

    }
}