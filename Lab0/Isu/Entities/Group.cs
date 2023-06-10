using System.Text.RegularExpressions;
using Isu.Exceptions;
using Isu.Models;

namespace Isu.Entities;

public class Group
{
    public const int MaxNumberOfStudents = 30;
    private static readonly Regex MyRegex = new Regex(@"^[BDHKLMNOPRTUVWZ][0-9]+$", RegexOptions.Singleline | RegexOptions.Compiled);
    public Group(string groupName)
    {
        if (TryParse(groupName).Equals(false))
        {
            throw new InvalidGroupNameException(groupName);
        }

        GroupName = groupName;
        Students = new List<Student>();

        switch (groupName[2])
        {
            case '1':
                CourseNumber = CourseNumber.First;
                break;
            case '2':
                CourseNumber = CourseNumber.Second;
                break;
            case '3':
                CourseNumber = CourseNumber.Third;
                break;
            case '4':
                CourseNumber = CourseNumber.Fourth;
                break;
            default:
                throw new InvalidGroupNameException(groupName);
        }
    }

    public string GroupName { get; }

    public CourseNumber CourseNumber { get; }

    public List<Student> Students { get; }

    public static bool TryParse(string groupName)
    {
        return MyRegex.IsMatch(groupName);
    }
}