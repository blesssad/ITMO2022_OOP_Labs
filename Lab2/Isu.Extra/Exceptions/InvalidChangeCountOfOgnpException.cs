namespace Isu.Extra.Exceptions;

public class InvalidChangeCountOfOgnpException : Exception
{
    public InvalidChangeCountOfOgnpException(string error)
    {
        Error = error;
    }

    public string Error { get; set; }
}
