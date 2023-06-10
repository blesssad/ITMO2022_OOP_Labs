namespace Isu.Exceptions;

public class MaxNumberOfStudentException : Exception
{
    public MaxNumberOfStudentException(int currentNumberOfStudents)
    {
        CurrentNumberOfStudents = currentNumberOfStudents;
    }

    public int CurrentNumberOfStudents { get; set; }
}