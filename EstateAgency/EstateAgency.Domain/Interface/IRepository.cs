namespace EstateAgency.Domain.Interface;

/// <summary>
/// Базовый интерфейс репозитория для выполнения CRUD-операций с сущностями.
/// </summary>
/// <typeparam name="T">Тип сущности</typeparam>
public interface IRepository<T> where T : class
{
    /// <summary>
    /// Получение всех сущностей.
    /// </summary>
    public Task<IEnumerable<T>> GetAllAsync();

    /// <summary>
    /// Получение сущности по идентификатору.
    /// </summary>
    public Task<T?> GetByIdAsync(int id);

    /// <summary>
    /// Добавление новой сущности.
    /// </summary>
    public Task<T> AddAsync(T entity);

    /// <summary>
    /// Обновление существующей сущности.
    /// </summary>
    public Task<T> UpdateAsync(T entity);

    /// <summary>
    /// Удаление сущности по идентификатору.
    /// </summary>
    public Task<bool> DeleteAsync(int id);
}
