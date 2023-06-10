using Banks.Exceptions;
using Banks.Models;
using Banks.Services;

namespace Banks.BankAccounts;

public class CreditAccount : IBankAccount
{
    public CreditAccount(Client owner, long id, decimal balance, decimal limitForSuspiciousAccount, decimal negativeLimit, decimal creditCommission)
    {
        if (owner is null)
        {
            throw new ArgumentNullException(nameof(owner));
        }

        Owner = owner;
        Id = id;
        Balance = balance;
        LimitForSuspiciousAccount = limitForSuspiciousAccount;
        NegativeLimit = negativeLimit;
        CreditСommission = creditCommission;
        Cashback = 0;
    }

    public Client Owner { get; private set; }
    public long Id { get; private set; }
    public decimal Balance { get; private set; }
    public decimal Cashback { get; private set; }
    public decimal LimitForSuspiciousAccount { get; private set; }
    public decimal NegativeLimit { get; private set; }
    public decimal CreditСommission { get; private set; }

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

        if (Balance - money < NegativeLimit)
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
        if (Balance < 0)
        {
            decimal dayPercent = CreditСommission / 100 / 365;
            Cashback += Balance * dayPercent;
        }

        if (DateTime.DaysInMonth(date.Year, date.Month) == date.Day)
        {
            Balance += Cashback;
            Cashback = 0;
        }
    }
}
