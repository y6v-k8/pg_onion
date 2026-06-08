namespace FinancialTracker.Domain;

/// <summary>
/// Пользователь финансового трекера.
/// </summary>
public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<FinancialAccount> Accounts { get; set; } = new List<FinancialAccount>();
}
