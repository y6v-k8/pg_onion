namespace FinancialTracker.Application;

/// <summary>
/// Результат пагинированного запроса.
/// </summary>
public class PagedResult<T>
{
    public List<T> Items { get; set; } = new List<T>();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}
