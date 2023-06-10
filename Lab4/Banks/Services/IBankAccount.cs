using Banks.Models;

namespace Banks.Services;

public interface IBankAccount
{
    public Client Owner { get; }
    public long Id { get; }
    public decimal Balance { get; }

    public void WithdrawMoney(decimal money);
    public void AddMoney(decimal money);
}
