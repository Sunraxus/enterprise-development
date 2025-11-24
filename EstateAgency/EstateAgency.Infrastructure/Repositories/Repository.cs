using EstateAgency.Domain.Interface;
using EstateAgency.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EstateAgency.Infrastructure.Repositories;

/// <summary>
/// Универсальная реализация репозитория для управления сущностями через Entity Framework Core.
/// Предоставляет асинхронные методы для выполнения CRUD-операций над любой сущностью.
/// </summary>
/// <typeparam name="T">Тип сущности</typeparam>
public class Repository<T>(AppDbContext context) : IRepository<T> where T : class
{
    private readonly DbSet<T> _dbSet = context.Set<T>();

    /// <summary>
    /// Получение всех сущностей из базы данных.
    /// </summary>
    /// <returns>Коллекция всех сущностей</returns>
    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    /// <summary>
    /// Получение сущности по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор сущности</param>
    /// <returns>Сущность или null, если не найдена</returns>
    public async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    /// <summary>
    /// Добавление новой сущности в базу данных.
    /// </summary>
    /// <param name="entity">Новая сущность</param>
    /// <returns>Добавленная сущность с присвоенным Id</returns>
    public async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    /// <summary>
    /// Обновление существующей сущности в базе данных.
    /// </summary>
    /// <param name="entity">Обновленная сущность</param>
    /// <returns>Обновленная сущность</returns>
    public async Task<T> UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    /// <summary>
    /// Удаление сущности по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор сущности</param>
    /// <returns>True, если удаление успешно; False, если сущность не найдена</returns>
    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity == null)
        {
            return false;
        }

        _dbSet.Remove(entity);
        await context.SaveChangesAsync();
        return true;
    }
}