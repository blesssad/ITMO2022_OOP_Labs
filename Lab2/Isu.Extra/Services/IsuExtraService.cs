using Isu.Extra.Entities;
using Isu.Extra.Exceptions;
using Isu.Extra.Models;
using Isu.Services;

namespace Isu.Extra.Services;

public class IsuExtraService : IsuService, IIsuExtraService
{
    private const int MinOgnpForStudent = 0;
    private const int MaxOgnpForStudent = 2;
    private readonly Dictionary<string, AdvancedGroup> _advancedGroups;
    private readonly Dictionary<MegaFaculty, OGNPCourse> _ognpCourses;
    public IsuExtraService()
    {
        _advancedGroups = new Dictionary<string, AdvancedGroup>();
        _ognpCourses = new Dictionary<MegaFaculty, OGNPCourse>();
    }

    public void AddAdvancedStudentToGroup(AdvancedStudent student, AdvancedGroup group)
    {
        if (student is null)
        {
            throw new ArgumentNullException(nameof(student));
        }

        if (group is null)
        {
            throw new ArgumentNullException(nameof(group));
        }

        student.Group = group;

        group.Students.Add(student);
    }

    public OGNPCourse CreateOGNPCourse(string courseName, MegaFaculty megaFaculty)
    {
        var newOGNPCOurse = new OGNPCourse(courseName, megaFaculty);

        _ognpCourses.Add(megaFaculty, newOGNPCOurse);

        return newOGNPCOurse;
    }

    public void AddStudentToOGNP(AdvancedStudent student, OGNPCourse ognpCourse)
    {
        if (student.AdvancedGroup.MegaFaculty == ognpCourse.MegaFaculty)
        {
            throw new InvalidAddStudentToOgnpException("Student MegaFaculty equals MegaFaculty of ognp");
        }

        if (student.CurrentNumberOfOGNP == MaxOgnpForStudent)
        {
            throw new InvalidChangeCountOfOgnpException("Student already has 2 ognp Course");
        }

        if (student is null)
        {
            throw new ArgumentNullException(nameof(student));
        }

        if (ognpCourse is null)
        {
            throw new ArgumentNullException(nameof(ognpCourse));
        }

        if (ognpCourse.Groups.Count == 0)
        {
            throw new InvalidCountOfGroupsException("OGNP doesn't have groups");
        }

        ognpCourse.Groups
            .Where(group => group.Students.Count < OGNPGroup.MaxNumberOfStudents)
            .First().AddStudent(student);

        student.AddOGNP();
    }

    public void RemoveStudentFromOGNP(AdvancedStudent student, OGNPCourse ognpCourse)
    {
        if (student.CurrentNumberOfOGNP == MinOgnpForStudent)
        {
            throw new InvalidChangeCountOfOgnpException("Student has 0 ognp Course");
        }

        if (student is null)
        {
            throw new ArgumentNullException(nameof(student));
        }

        if (ognpCourse is null)
        {
            throw new ArgumentNullException(nameof(ognpCourse));
        }

        ognpCourse.Groups
            .Where(group => group.FindStudent(student).Equals(true))
            .Single().RemoveStudent(student);
    }

    public IReadOnlyList<AdvancedStudent> ReciveListOfStudentFromOgnpGroup(string groupName)
    {
        if (string.IsNullOrWhiteSpace(groupName))
        {
            throw new ArgumentNullException(nameof(groupName));
        }

        return _ognpCourses.Values
           .SelectMany(course => course.Groups
           .Where(group => group.GroupName == groupName)
           .Single().Students)
           .ToList();
    }

    public IReadOnlyList<AdvancedStudent> ReciveListOfStudentWithoutOgnp(AdvancedGroup group)
    {
        if (group is null)
        {
            throw new ArgumentNullException(nameof(group));
        }

        var newList = new List<AdvancedStudent>();

        foreach (AdvancedStudent student in group.Students)
        {
            if (student.CurrentNumberOfOGNP == MinOgnpForStudent)
            {
                newList.Add(student);
            }
        }

        return newList;
    }

    public IReadOnlyList<OGNPGroup> ReciveGroupsInCourse(OGNPCourse ognpCourse)
    {
        if (ognpCourse is null)
        {
            throw new ArgumentNullException(nameof(ognpCourse));
        }

        return ognpCourse.Groups.ToList();
    }
}
