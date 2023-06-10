using Banks.BankAccounts;
using Banks.ClientBuilders;
using Banks.Models;
using Xunit;

namespace Banks.Test;

public class BanksTest
{
    [Fact]
    public void DebitAccountTest_BalanceIsCorrect()
    {
        var clientBuilder = new ClientBuilder();

        clientBuilder.SetName("Vadim Pavlovets");
        clientBuilder.SetAddress("Saint-Peterburg");
        clientBuilder.SetPassport(401723232323);

        var today = new DateTime(2022, 11, 24);

        var centralBank = new CentralBank();
        var newBank = new Bank(centralBank, 1000, 1, 2, 3, 5, 10000, 50000, 100000, -100000, 4);

        centralBank.AddBank(newBank);

        DebitAccount newDebitAccount = newBank.CreateDebitAccount(clientBuilder.Build());

        newDebitAccount.AddMoney(100000);

        for (int iter = 0; iter <= 30; iter++)
        {
            today = today.AddDays(1);
            newBank.BankChangeDay(today);
        }

        Assert.Equal(100016, (int)newDebitAccount.Balance);
    }

    [Fact]
    public void CreditAccountTest_BalanceIsCorrect()
    {
        var clientBuilder = new ClientBuilder();

        clientBuilder.SetName("Vadim Pavlovets");
        clientBuilder.SetAddress("Saint-Peterburg");
        clientBuilder.SetPassport(401723232323);

        var today = new DateTime(2022, 11, 24);

        var centralBank = new CentralBank();
        var newBank = new Bank(centralBank, 1000, 1, 2, 3, 5, 10000, 50000, 100000, -100000, 4);

        centralBank.AddBank(newBank);

        CreditAccount newCreditAccount = newBank.CreateCreditAccount(clientBuilder.Build());

        newCreditAccount.WithdrawMoney(100000);

        for (int iter = 0; iter <= 30; iter++)
        {
            today = today.AddDays(1);
            newBank.BankChangeDay(today);
        }

        Assert.Equal(-100065, (int)newCreditAccount.Balance);
    }

    [Fact]
    public void DepositAccountTest_BalanceIsCorrect()
    {
        var clientBuilder = new ClientBuilder();

        clientBuilder.SetName("Vadim Pavlovets");
        clientBuilder.SetAddress("Saint-Peterburg");
        clientBuilder.SetPassport(401723232323);

        var startDate = new DateTime(2022, 11, 24);
        var endDate = new DateTime(2023, 5, 24);

        var centralBank = new CentralBank();
        var newBank = new Bank(centralBank, 1000, 1, 2, 3, 5, 10000, 50000, 100000, -100000, 4);

        centralBank.AddBank(newBank);

        DepositAccount newDepositAccount = newBank.CreateDepositAccount(clientBuilder.Build(), endDate, startDate, 50000);

        for (int iter = 0; iter <= 70; iter++)
        {
            startDate = startDate.AddDays(1);
            newBank.BankChangeDay(startDate);
        }

        Assert.Equal(50565, (int)newDepositAccount.Balance);
    }

    [Fact]
    public void TransferIsCorrect_MoneyWithdrawFromAccountAndMoneyAddToAccount()
    {
        var clientBuilder = new ClientBuilder();

        clientBuilder.SetName("Vadim Pavlovets");
        clientBuilder.SetAddress("Saint-Peterburg");
        clientBuilder.SetPassport(401723232323);

        var clientBuilder2 = new ClientBuilder();

        clientBuilder2.SetName("Arseniy Khimane");
        clientBuilder2.SetAddress("Saint-Peterburg");
        clientBuilder2.SetPassport(401745454545);

        var centralBank = new CentralBank();
        var newBank = new Bank(centralBank, 1000, 1, 2, 3, 5, 10000, 50000, 100000, -100000, 4);

        centralBank.AddBank(newBank);

        DebitAccount firstDebitAccount = newBank.CreateDebitAccount(clientBuilder.Build());

        firstDebitAccount.AddMoney(1000);

        DebitAccount secondDebitAccount = newBank.CreateDebitAccount(clientBuilder.Build());

        var moneyTransaction = new MoneyTransferTransaction(firstDebitAccount, secondDebitAccount, 500);

        centralBank.SetTransaction(moneyTransaction);
        centralBank.TransferMoney();

        Assert.Equal(500, (int)firstDebitAccount.Balance);
        Assert.Equal(500, (int)secondDebitAccount.Balance);
    }

    [Fact]
    public void RollbackIsCorrect_MoneyStays()
    {
        var clientBuilder = new ClientBuilder();

        clientBuilder.SetName("Vadim Pavlovets");
        clientBuilder.SetAddress("Saint-Peterburg");
        clientBuilder.SetPassport(401723232323);

        var centralBank = new CentralBank();
        var newBank = new Bank(centralBank, 1000, 1, 2, 3, 5, 10000, 50000, 100000, -100000, 4);

        centralBank.AddBank(newBank);

        DebitAccount firstDebitAccount = newBank.CreateDebitAccount(clientBuilder.Build());

        firstDebitAccount.AddMoney(1000);

        DebitAccount secondDebitAccount = newBank.CreateDebitAccount(clientBuilder.Build());

        var moneyTransaction = new MoneyTransferTransaction(firstDebitAccount, secondDebitAccount, 500);

        centralBank.SetTransaction(moneyTransaction);
        centralBank.TransferMoney();
        centralBank.RollbackMoney();

        Assert.Equal(1000, (int)firstDebitAccount.Balance);
        Assert.Equal(0, (int)secondDebitAccount.Balance);
    }
}
