using Isu.Entities;
using Isu.Exceptions;
using Isu.Services;
using Xunit;

namespace Isu.Test;

public class IsuServiceTests
{
    private readonly IsuService service;

    public IsuServiceTests()
    {
        service = new IsuService();
    }

    [Fact]
    public void AddStudentToGroup_StudentHasGroupAndGroupContainsStudent()
    {
        Group group = service.AddGroup("M32091");

        Student student = service.AddStudent(group, "Vadim Pavlovets");

        Assert.Contains(student, group.Students);
        Assert.Equal(student.Group, group);
    }

    [Fact]
    public void ReachMaxStudentPerGroup_ThrowException()
    {
        Group group = service.AddGroup("M3109");

        for (int counter = 0; counter < Group.MaxNumberOfStudents; counter++)
        {
            service.AddStudent(group, "Maxim Ganihin");
        }

        Assert.Throws<MaxNumberOfStudentException>(() => service.AddStudent(group, "Nikita Fisenko"));
    }

    [Theory]
    [InlineData("M310a9")]
    [InlineData("I3109")]
    [InlineData("MM372041")]
    public void CreateGroupWithInvalidName_ThrowException(string invalidName)
    {
        Assert.Throws<InvalidGroupNameException>(() => service.AddGroup(invalidName));
    }

    [Fact]
    public void TransferStudentToAnotherGroup_GroupChanged()
    {
        Group oldGroup = service.AddGroup("M32091");
        Group newGroup = service.AddGroup("M32061");
        Student student = service.AddStudent(oldGroup, "Vadim Pavlovets");

        service.ChangeStudentGroup(student, newGroup);

        Assert.Contains(student, newGroup.Students);
        Assert.DoesNotContain(student, oldGroup.Students);
    }
}