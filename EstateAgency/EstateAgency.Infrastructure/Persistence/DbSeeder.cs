using EstateAgency.Test;
using Microsoft.EntityFrameworkCore;

namespace EstateAgency.Infrastructure.Persistence;

/// <summary>
/// Заполняет базу тестовыми данными (если таблицы пусты) для RealEstates, Counterparties и Applications.
/// </summary>
public static class DbSeeder
{
    /// <summary>
    /// Заполнение таблиц стартовыми данными.
    /// </summary>
    public static async Task SeedAsync(AppDbContext context)
    {
        var fixture = new TestDataFixture();

        if (!await context.RealEstates.AnyAsync())
        {
            await context.RealEstates.AddRangeAsync(fixture.EstateObjects);
            await context.SaveChangesAsync();
            await context.Database.ExecuteSqlRawAsync(
                "SELECT setval(pg_get_serial_sequence('real_estates','id'), MAX(id)) FROM real_estates");
        }

        if (!await context.Counterparties.AnyAsync())
        {
            await context.Counterparties.AddRangeAsync(fixture.Counterparties);
            await context.SaveChangesAsync();
            await context.Database.ExecuteSqlRawAsync(
                "SELECT setval(pg_get_serial_sequence('counterparties','id'), MAX(id)) FROM counterparties");
        }

        if (!await context.Applications.AnyAsync())
        {
            await context.Applications.AddRangeAsync(fixture.Requests);
            await context.SaveChangesAsync();
            await context.Database.ExecuteSqlRawAsync(
                "SELECT setval(pg_get_serial_sequence('applications','id'), MAX(id)) FROM applications");
        }
    }
}
