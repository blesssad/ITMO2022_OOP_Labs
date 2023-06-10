using Banks.Exceptions;
using Banks.Models;
using Banks.Services;

namespace Banks.BankAccounts;

public class DepositAccount : IBankAccount
{
    private bool _restrictToWithdrawAndTransfer;
    public DepositAccount(Client owner, long id, decimal balance, decimal percent, DateTime dateOfDepositEnd, DateTime currentDate, decimal limitForSuspiciousAccount)
    {
        if (owner is null)
        {
            throw new ArgumentNullException(nameof(owner));
        }

        Owner = owner;
        Id = id;
        Balance = balance;
        Percent = percent;
        Cashback = 0;
        DateOfDepositEnd = dateOfDepositEnd;
        IntervalOfDeposit = dateOfDepositEnd.Subtract(currentDate).Days;
        _restrictToWithdrawAndTransfer = true;
        LimitForSuspiciousAccount = limitForSuspiciousAccount;
    }

    public Client Owner { get; private set; }

    public long Id { get; private set; }

    public decimal Balance { get; private set; }

    public decimal Percent { get; private set; }
    public decimal Cashback { get; private set; }
    public DateTime DateOfDepositEnd { get; private set; }
    public int IntervalOfDeposit { get; private set; }
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
        if (_restrictToWithdrawAndTransfer == true)
        {
            throw new RestrictToWithdrawAndTransferException("You can't withdraw money");
        }

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
        decimal dayPercent = Percent / 100 / IntervalOfDeposit;
        Cashback += Balance * dayPercent;

        if (DateTime.DaysInMonth(date.Year, date.Month) == date.Day)
        {
            Balance += Cashback;
            Cashback = 0;
        }

        if (DateTime.Compare(date, DateOfDepositEnd) == 0)
        {
            Percent = 0;
            _restrictToWithdrawAndTransfer = false;
        }
    }
}
