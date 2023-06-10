namespace Banks.Exceptions;

public class BalanceLessThenNeedToWithdrawException : Exception
{
    public BalanceLessThenNeedToWithdrawException(string error)
    {
        Error = error;
    }

    public string Error { get; }
}
