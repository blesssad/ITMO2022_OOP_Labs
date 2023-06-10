namespace Isu.Extra.Exceptions;

public class CollideLessonsException : Exception
{
    public CollideLessonsException(string error)
    {
        Error = error;
    }

    public string Error { get; set; }
}
