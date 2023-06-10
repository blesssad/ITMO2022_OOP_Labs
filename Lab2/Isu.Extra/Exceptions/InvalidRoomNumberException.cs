namespace Isu.Extra.Exceptions;

public class InvalidRoomNumberException : Exception
{
    public InvalidRoomNumberException(string error)
    {
        Error = error;
    }

    public string Error { get; set; }
}
