namespace FinancialTracker.Application;

/// <summary>
/// Интерфейс базового репозитория.
/// </summary>
public interface IRepository<T> where T : class
{
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
    Task<List<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
}
