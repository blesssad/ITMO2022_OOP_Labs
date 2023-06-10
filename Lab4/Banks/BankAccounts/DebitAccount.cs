using Banks.Exceptions;
using Banks.Models;
using Banks.Services;

namespace Banks.BankAccounts;

public class DebitAccount : IBankAccount
{
    public DebitAccount(Client owner, long id, decimal balance, decimal percent, decimal limit)
    {
        if (owner is null)
        {
            throw new ArgumentNullException(nameof(owner));
        }

        Owner = owner;
        Id = id;
        Balance = balance;
        Cashback = 0;
        Percent = percent;
        LimitForSuspiciousAccount = limit;
    }

    public Client Owner { get; private set; }

    public long Id { get; }

    public decimal Balance { get; private set; }

    public decimal Percent { get; private set; }
    public decimal Cashback { get; private set; }

    public decimal LimitForSuspiciousAccount { get; private set; }

    public void AddMoney(decimal money)
    {
        if (money <= 0)
        {
            throw new InvalidMoneyCountException("Your money count is 0 or less");
        }

        Balance += money;
    }

    public void WithdrawMoney(decimal money)
    {
        if (Owner.Adress is null && money < LimitForSuspiciousAccount)
        {
            throw new SuspicticiousAccountHasLimitException("Try to withdraw more then limit");
        }

        if (Owner.Passport is null && money < LimitForSuspiciousAccount)
        {
            throw new SuspicticiousAccountHasLimitException("Try to withdraw more then limit");
        }

        if (Balance - money < 0)
        {
            throw new BalanceLessThenNeedToWithdrawException("Insufficient Funds");
        }

        if (money <= 0)
        {
            throw new InvalidMoneyCountException("Your money count is 0 or less");
        }

        Balance -= money;
    }

    public void ChangeDay(DateTime date)
    {
        decimal dayPercent = Percent / 100 / 365;
        Cashback += Balance * dayPercent;

        if (DateTime.DaysInMonth(date.Year, date.Month) == date.Day)
        {
            Balance += Cashback;
            Cashback = 0;
        }
    }
}
