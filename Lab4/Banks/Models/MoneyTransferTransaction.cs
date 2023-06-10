using Banks.Services;

namespace Banks.Models;

public class MoneyTransferTransaction : ITransaction
{
    public MoneyTransferTransaction(IBankAccount fromAccount, IBankAccount toAccount, decimal money)
    {
        FromAccount = fromAccount;
        ToAccount = toAccount;
        Money = money;
        IsTransaction = false;
    }

    public IBankAccount FromAccount { get; private set; }

    public IBankAccount ToAccount { get; private set; }

    public decimal Money { get; private set; }
    public bool IsTransaction { get; private set; }

    public void Execute()
    {
        if (IsTransaction)
            return;

        FromAccount.WithdrawMoney(Money);
        ToAccount.AddMoney(Money);
        IsTransaction = true;
    }

    public void Rollback()
    {
        if (IsTransaction == false)
            return;

        ToAccount.WithdrawMoney(Money);
        FromAccount.AddMoney(Money);

        IsTransaction = false;
    }
}
