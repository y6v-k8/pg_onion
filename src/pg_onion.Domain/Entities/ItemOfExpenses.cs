// Domain Entity - ItemOfExpenses
namespace pg_onion.Domain.Entities
{
    public enum TransactionType
    {
        Income = 0,
        Expense = 1,
        Transfer = 2
    }

    public class ItemOfExpenses
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TransactionType Type { get; set; }
        public int CategoryId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation properties
        public ICollection<Transaction> Transactions { get; set; }
        public ICollection<Budget> Budgets { get; set; }
    }
}
