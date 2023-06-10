namespace Banks.Services;

public interface ITransaction
{
    public IBankAccount FromAccount { get; }
    public IBankAccount ToAccount { get; }
    decimal Money { get; }

    public void Execute();
    public void Rollback();
}
