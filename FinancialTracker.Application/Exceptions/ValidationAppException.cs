namespace FinancialTracker.Application;

/// <summary>
/// Исключение валидации входных данных или нарушения бизнес-правил.
/// </summary>
public class ValidationAppException : AppException
{
    public ValidationAppException(string message) : base(message, 400)
    {
    }
}
