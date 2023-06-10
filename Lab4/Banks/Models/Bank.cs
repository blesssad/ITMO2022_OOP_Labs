using Banks.BankAccounts;
using Banks.Exceptions;

namespace Banks.Models;

public class Bank
{
    private CentralBank _centralBank;

    private List<CreditAccount> _creditAccounts;
    private List<DebitAccount> _debitAccounts;
    private List<DepositAccount> _depositAccounts;
    public Bank(CentralBank bank, decimal limitForSuspictiousAccount, decimal percentForDebitAccount, decimal depositPercentLow, decimal depositPercentMiddle, decimal depositPercentHigh, decimal depositMoneyLow, decimal depositMoneyMiddle, decimal depositMoneyHigh, decimal negativeLimit, decimal creditCommition)
    {
        _creditAccounts = new List<CreditAccount>();
        _debitAccounts = new List<DebitAccount>();
        _depositAccounts = new List<DepositAccount>();

        LimitForSuspictiousAccount = limitForSuspictiousAccount;
        PercentForDebitAccount = percentForDebitAccount;

        DepositPercentLow = depositPercentLow;
        DepositPercentMiddle = depositPercentMiddle;
        DepositPercentHigh = depositPercentHigh;
        DepositMoneyLow = depositMoneyLow;
        DepositMoneyMiddle = depositMoneyMiddle;
        DepositMoneyHigh = depositMoneyHigh;

        NegativeLimit = negativeLimit;
        CreditCommition = creditCommition;

        _centralBank = bank;
    }

    public IReadOnlyCollection<CreditAccount> CreditAccounts => _creditAccounts;
    public IReadOnlyCollection<DebitAccount> DebitAccounts => _debitAccounts;
    public IReadOnlyCollection<DepositAccount> DepositAccounts => _depositAccounts;

    public decimal LimitForSuspictiousAccount { get; private set; }
    public decimal PercentForDebitAccount { get; private set; }

    public decimal DepositPercentLow { get; private set; }
    public decimal DepositMoneyLow { get; private set; }
    public decimal DepositPercentMiddle { get; private set; }
    public decimal DepositMoneyMiddle { get; private set; }
    public decimal DepositPercentHigh { get; private set; }
    public decimal DepositMoneyHigh { get; private set; }

    public decimal NegativeLimit { get; private set; }
    public decimal CreditCommition { get; private set; }

    public CreditAccount CreateCreditAccount(Client owner)
    {
        if (owner is null)
        {
            throw new ArgumentNullException(nameof(owner));
        }

        var newCreditAccount = new CreditAccount(owner, _centralBank.GetNewId(), 0, LimitForSuspictiousAccount, NegativeLimit, CreditCommition);
        _creditAccounts.Add(newCreditAccount);

        return newCreditAccount;
    }

    public DebitAccount CreateDebitAccount(Client owner)
    {
        if (owner is null)
        {
            throw new ArgumentNullException(nameof(owner));
        }

        var newDebitAccount = new DebitAccount(owner, _centralBank.GetNewId(), 0, PercentForDebitAccount, LimitForSuspictiousAccount);
        _debitAccounts.Add(newDebitAccount);

        return newDebitAccount;
    }

    public DepositAccount CreateDepositAccount(Client owner, DateTime dateOfDepositEnd, DateTime dateOfDepositStart, decimal startMoney)
    {
        if (owner is null)
        {
            throw new ArgumentNullException(nameof(owner));
        }

        if (startMoney <= 0)
        {
            throw new InvalidMoneyCountException("Money count is 0 or less");
        }

        decimal percent = 0;

        if (startMoney <= DepositMoneyLow && startMoney > 0)
        {
            percent = DepositPercentLow;
        }
        else if (startMoney <= DepositMoneyMiddle && startMoney > DepositMoneyLow)
        {
            percent = DepositPercentMiddle;
        }
        else if (startMoney >= DepositMoneyHigh)
        {
            percent = DepositPercentHigh;
        }

        var newDepositAccount = new DepositAccount(owner, _centralBank.GetNewId(), startMoney, percent, dateOfDepositEnd, dateOfDepositStart, LimitForSuspictiousAccount);
        _depositAccounts.Add(newDepositAccount);

        return newDepositAccount;
    }

    public void BankChangeDay(DateTime date)
    {
        foreach (DepositAccount account in _depositAccounts)
        {
            account.ChangeDay(date);
        }

        foreach (DebitAccount account in _debitAccounts)
        {
            account.ChangeDay(date);
        }

        foreach (CreditAccount account in _creditAccounts)
        {
            account.ChangeDay(date);
        }
    }
}
