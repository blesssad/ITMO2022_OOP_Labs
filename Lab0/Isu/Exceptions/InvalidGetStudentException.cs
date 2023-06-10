namespace Isu.Exceptions;

public class InvalidGetStudentException : Exception
{
    public InvalidGetStudentException(int id)
    {
        Id = id;
    }

    public int Id { get; set; }
}