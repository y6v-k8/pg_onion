namespace FinancialTracker.Domain;

/// <summary>
/// Транзакция (операция доход/расход).
/// </summary>
public class Transaction
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public DateTime DateTime { get; set; }
    public string Description { get; set; } = string.Empty;
    public TransactionType Type { get; set; }
    public int AccountId { get; set; }
    public int? CategoryId { get; set; }

    public FinancialAccount Account { get; set; } = null!;
    public ItemOfExpenses? Category { get; set; }
}
