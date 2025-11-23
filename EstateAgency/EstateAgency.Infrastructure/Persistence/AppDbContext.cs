using EstateAgency.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EstateAgency.Infrastructure.Persistence;

/// <summary>
/// Контекст базы данных приложения, представляющий реляционную базу данных с использованием Entity Framework Core.
/// Определяет DbSet для сущностей RealEstate, Counterparty и Application.
/// Настраивает маппинг сущностей и связи через в методе OnModelCreating.
/// </summary>
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    /// <summary>
    /// Представление таблицы объектов недвижимости.
    /// </summary>
    public DbSet<RealEstate> RealEstates { get; set; }

    /// <summary>
    /// Представление таблицы контрагентов.
    /// </summary>
    public DbSet<Counterparty> Counterparties { get; set; }

    /// <summary>
    /// Представление таблицы заявок.
    /// </summary>
    public DbSet<Application> Applications { get; set; }

    /// <summary>
    /// Настройка маппинга сущностей, первичных ключей, ограничений свойств и связей.
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Counterparty>(entity =>
        {
            entity.ToTable("counterparties");

            entity.HasKey(c => c.Id);

            entity.Property(c => c.Id)
                .HasColumnName("id")
                .IsRequired()
                .ValueGeneratedOnAdd();

            entity.Property(c => c.FullName)
                .HasColumnName("full_name")
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(c => c.PassportNumber)
                .HasColumnName("passport_number")
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(c => c.Phone)
                .HasColumnName("phone")
                .IsRequired()
                .HasMaxLength(20);
        });

        modelBuilder.Entity<RealEstate>(entity =>
        {
            entity.ToTable("real_estates");

            entity.HasKey(r => r.Id);

            entity.Property(r => r.Id)
                .HasColumnName("id")
                .IsRequired()
                .ValueGeneratedOnAdd();

            entity.Property(r => r.Type)
                .HasColumnName("type")
                .HasConversion<string>()
                .IsRequired();

            entity.Property(r => r.Purpose)
                .HasColumnName("purpose")
                .HasConversion<string>()
                .IsRequired();

            entity.Property(r => r.CadastralNumber)
                .HasColumnName("cadastral_number")
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(r => r.Address)
                .HasColumnName("address")
                .IsRequired()
                .HasMaxLength(300);

            entity.Property(r => r.FloorsTotal)
                .HasColumnName("floors_total")
                .IsRequired();

            entity.Property(r => r.AreaTotal)
                .HasColumnName("area_total")
                .IsRequired();

            entity.Property(r => r.Rooms)
                .HasColumnName("rooms");

            entity.Property(r => r.CeilingHeight)
                .HasColumnName("ceiling_height");

            entity.Property(r => r.FloorNumber)
                .HasColumnName("floor_number");

            entity.Property(r => r.HasEncumbrances)
                .HasColumnName("has_encumbrances")
                .IsRequired();
        });

        modelBuilder.Entity<Application>(entity =>
        {
            entity.ToTable("applications");

            entity.HasKey(a => a.Id);

            entity.Property(a => a.Id)
                .HasColumnName("id")
                .IsRequired()
                .ValueGeneratedOnAdd();

            entity.Property(a => a.CounterpartyId)
                .HasColumnName("counterparty_id")
                .IsRequired();

            entity.Property(a => a.RealEstateId)
                .HasColumnName("real_estate_id")
                .IsRequired();

            entity.Property(a => a.Type)
                .HasColumnName("type")
                .HasConversion<string>()
                .IsRequired();

            entity.Property(a => a.Amount)
                .HasColumnName("amount")
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            entity.Property(a => a.Date)
                .HasColumnName("date")
                .IsRequired()
                .HasColumnType("date");

            entity.HasOne(a => a.Counterparty)
                .WithMany()
                .HasForeignKey(a => a.CounterpartyId)
                .HasConstraintName("fk_applications_counterparty")
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(a => a.RealEstate)
                .WithMany()
                .HasForeignKey(a => a.RealEstateId)
                .HasConstraintName("fk_applications_real_estate")
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}