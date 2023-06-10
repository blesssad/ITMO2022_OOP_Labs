namespace Isu.Extra.Exceptions;

public class InvalidAddStudentToOgnpException : Exception
{
    public InvalidAddStudentToOgnpException(string error)
    {
        Error = error;
    }

    public string Error { get; set; }
}
