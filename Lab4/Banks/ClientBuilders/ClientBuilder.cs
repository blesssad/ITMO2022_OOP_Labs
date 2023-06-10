using Banks.Models;

namespace Banks.ClientBuilders;

public class ClientBuilder
{
    private string? _name;
    private string? _address;
    private long? _passport;

    public void SetAddress(string address)
    {
        _address = address;
    }

    public void SetName(string name)
    {
        _name = name;
    }

    public void SetPassport(long passport)
    {
        _passport = passport;
    }

    public Client Build()
    {
        var client = new Client(_name);

        client.ChangeAdress(_address);
        client.ChangePassport(_passport);

        return client;
    }
}
