using Isu.Exceptions;

namespace Isu.Entities;

public class Student
{
    private int _id;

    public Student(int id, string name, Group group)
    {
        if (group is null)
        {
            throw new ArgumentNullException(nameof(group));
        }

        foreach (char symbol in name)
        {
            int numberOfSpace = 0;

            if (symbol.Equals(' '))
            {
                numberOfSpace++;
            }

            if (numberOfSpace > 1 || name.Length <= 1)
            {
                throw new InvalidStudentNameException(name);
            }
        }

        string[] partsOfName = name.Split(' ');

        Id = id;
        FirstName = partsOfName[0];
        LastName = partsOfName[1];
        Group = group;
    }

    public int Id
    {
        get
        {
            return _id;
        }
        set
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(Id));
            }

            _id = value;
        }
    }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public Group Group { get; set; }
}