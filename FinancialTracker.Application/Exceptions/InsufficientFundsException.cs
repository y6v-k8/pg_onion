namespace FinancialTracker.Application;

/// <summary>
/// Исключение при попытке потратить больше, чем есть на счете.
/// </summary>
public class InsufficientFundsException : AppException
{
    public InsufficientFundsException(string message) : base(message, 400)
    {
    }
}
