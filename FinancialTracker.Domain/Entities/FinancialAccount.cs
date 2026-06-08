namespace FinancialTracker.Domain;

/// <summary>
/// Финансовый счет пользователя.
/// </summary>
public class FinancialAccount
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public int UserId { get; set; }
    public CurrencyType Currency { get; set; }
    public AccountType AccountType { get; set; }

    public User User { get; set; } = null!;
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    public ICollection<Budget> Budgets { get; set; } = new List<Budget>();
}
