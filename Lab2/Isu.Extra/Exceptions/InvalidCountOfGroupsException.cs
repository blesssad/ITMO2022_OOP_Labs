namespace Isu.Extra.Exceptions;

public class InvalidCountOfGroupsException : Exception
{
    public InvalidCountOfGroupsException(string error)
    {
        Error = error;
    }

    public string Error { get; set; }
}
