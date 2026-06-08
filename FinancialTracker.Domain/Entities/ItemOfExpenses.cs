namespace FinancialTracker.Domain;

/// <summary>
/// Категория расходов.
/// </summary>
public class ItemOfExpenses
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public TransactionType Type { get; set; }

    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    public ICollection<Budget> Budgets { get; set; } = new List<Budget>();
}
