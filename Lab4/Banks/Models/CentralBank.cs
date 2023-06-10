using Banks.Services;

namespace Banks.Models;
public class CentralBank
{
    private long _usersId;
    private List<Bank> _banks;
    private ITransaction? _transaction;
    public CentralBank()
    {
        _banks = new List<Bank>();
        _usersId = 1000000000000000;
        _transaction = null;
    }

    public IReadOnlyList<Bank> Banks => _banks;

    public void AddBank(Bank newBank)
    {
        if (newBank is null)
        {
            throw new ArgumentNullException(nameof(newBank));
        }

        _banks.Add(newBank);
    }

    public long GetNewId()
    {
        long newId = _usersId;

        _usersId++;

        return newId;
    }

    public void SetTransaction(ITransaction transaction)
    {
        _transaction = transaction;
    }

    public void TransferMoney()
    {
        if (_transaction is null)
        {
            throw new ArgumentNullException(nameof(_transaction));
        }

        _transaction.Execute();
    }

    public void RollbackMoney()
    {
        if (_transaction is null)
        {
            throw new ArgumentNullException(nameof(_transaction));
        }

        _transaction.Rollback();
    }
}
