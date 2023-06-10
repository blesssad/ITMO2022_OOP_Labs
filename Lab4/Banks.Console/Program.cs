using Banks.ClientBuilders;
using Banks.Models;
using Banks.Services;

internal class Program
{
    private static void Main()
    {
        var centralBank = new CentralBank();

        var sberbank = new Bank(centralBank, 1000, 1, 2, 3, 5, 10000, 50000, 100000, -100000, 4);
        var tinkoff = new Bank(centralBank, 500, 1, 3, 4, 5, 10000, 75000, 150000, -200000, 6);

        centralBank.AddBank(tinkoff);
        centralBank.AddBank(sberbank);

        Console.WriteLine("Регистрация!");
        Console.WriteLine("Enter your name");

        string? clientName = Console.ReadLine();

        if (clientName == null)
        {
            System.Environment.Exit(1);
        }

        string? clientAdress = null;
        long? clientPassport = null;

        Console.WriteLine("Do you want to enter your adress? Yes/No");

        string? str = Console.ReadLine();

        if (str == null)
        {
            System.Environment.Exit(1);
        }

        if (str!.Equals("Yes"))
        {
            clientAdress = Console.ReadLine();
        }

        Console.WriteLine("Do you want to enter your passport? Yes/No");
        str = Console.ReadLine();

        if (str == null)
        {
            System.Environment.Exit(1);
        }

        if (str!.Equals("Yes"))
        {
            clientPassport = Convert.ToInt64(Console.ReadLine());
        }

        var builder = new ClientBuilder();
        builder.SetName(clientName);

        if (clientAdress == null && clientPassport != null)
        {
            builder.SetPassport((long)clientPassport);
        }
        else if (clientAdress != null && clientPassport == null)
        {
            builder.SetAddress(clientAdress);
        }
        else if (clientAdress != null && clientPassport != null)
        {
            builder.SetPassport((long)clientPassport);
            builder.SetAddress(clientAdress);
        }

        Client newClient = builder.Build();

        IBankAccount? bankAccount = null;

        Console.WriteLine("Choose Bank (Tinkoff/Sberbank)");

        string? bank = Console.ReadLine();

        if (bank == "Tinkoff")
        {
            Console.WriteLine("Choose Account (Debit/Deposit/Credit)");
            str = Console.ReadLine();

            if (str == "Debit")
            {
                bankAccount = tinkoff.CreateDebitAccount(newClient);
            }
            else if (str == "Deposit")
            {
                bankAccount = tinkoff.CreateDepositAccount(newClient, Convert.ToDateTime(Console.ReadLine()), Convert.ToDateTime(Console.ReadLine()), Convert.ToDecimal(Console.ReadLine()));
            }
            else if (str == "Credit")
            {
                bankAccount = tinkoff.CreateCreditAccount(newClient);
            }
            else
            {
                System.Environment.Exit(1);
            }
        }
        else if (bank == "Sberbank")
        {
            Console.WriteLine("Choose Account (Debit/Deposit/Credit)");
            str = Console.ReadLine();

            if (str == "Debit")
            {
                bankAccount = sberbank.CreateDebitAccount(newClient);
            }
            else if (str == "Deposit")
            {
                bankAccount = sberbank.CreateDepositAccount(newClient, Convert.ToDateTime(Console.ReadLine()), Convert.ToDateTime(Console.ReadLine()), Convert.ToDecimal(Console.ReadLine()));
            }
            else if (str == "Credit")
            {
                bankAccount = sberbank.CreateCreditAccount(newClient);
            }
            else
            {
                System.Environment.Exit(1);
            }
        }
        else
        {
            System.Environment.Exit(1);
        }

        while (true)
        {
            Console.WriteLine("Operations: AddMoney, Withdraw, CheckBalance, Exit");
            str = Console.ReadLine();

            switch (str)
            {
                case "AddMoney":
                    Console.WriteLine("Count of money to add");
                    bankAccount.AddMoney(Convert.ToDecimal(Console.ReadLine()));
                    break;
                case "Withdraw":
                    Console.WriteLine("Count of money to withdraw");
                    bankAccount.WithdrawMoney(Convert.ToDecimal(Console.ReadLine()));
                    break;
                case "CheckBalance":
                    Console.WriteLine(bankAccount.Balance);
                    break;
                case "Exit":
                    System.Environment.Exit(0);
                    break;
                default:
                    break;
            }
        }
    }
}