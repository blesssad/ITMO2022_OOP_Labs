using Banks.Exceptions;

namespace Banks.Models;

public class Client
{
    public Client(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentNullException(nameof(name));
        }

        Name = name;
    }

    public string Name { get; private set; }
    public string? Adress { get; private set; }
    public long? Passport { get; private set; }

    public void ChangeAdress(string? adress)
    {
        Adress = adress;
    }

    public void ChangePassport(long? passport)
    {
        if (passport < 0)
        {
            throw new InvalidPassportException("Invalid Passport number");
        }

        Passport = passport;
    }
}
