using Microsoft.EntityFrameworkCore;
using Lab12;

namespace Lab12
{
    public class AppDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Настройка модели Book
            modelBuilder.Entity<Book>(entity =>
            {
                // Настройка Title
                entity.Property(b => b.Title)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnType("varchar(100)"); // Явно указываем тип столбца

                // Настройка Author
                entity.Property(b => b.Author)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnType("varchar(50)");

                // Настройка Genre
                entity.Property(b => b.Genre)
                    .HasMaxLength(30)
                    .HasColumnType("varchar(30)")
                    .HasDefaultValue("Не указан"); // Значение по умолчанию

                // Настройка IsAvailable
                entity.Property(b => b.IsAvailable)
                    .HasDefaultValue(true)
                    .HasColumnType("tinyint(1)");

                // Настройка Year
                entity.Property(b => b.Year)
                    .HasColumnType("int");
            });
        }
    }
}