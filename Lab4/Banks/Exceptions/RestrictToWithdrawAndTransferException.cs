namespace Banks.Exceptions;

public class RestrictToWithdrawAndTransferException : Exception
{
    public RestrictToWithdrawAndTransferException(string error)
    {
        Error = error;
    }

    public string Error { get; }
}
