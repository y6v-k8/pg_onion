namespace FinancialTracker.Domain;

/// <summary>
/// Бюджет с лимитом на период для категории расходов.
/// </summary>
public class Budget
{
    public int Id { get; set; }
    public decimal Limit { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int AccountId { get; set; }
    public int CategoryId { get; set; }
    public BudgetPeriod Period { get; set; }

    public FinancialAccount Account { get; set; } = null!;
    public ItemOfExpenses Category { get; set; } = null!;
}
