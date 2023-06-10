using Isu.Extra.Entities;
using Isu.Extra.Exceptions;
using Isu.Extra.Models;
using Isu.Extra.Services;
using Xunit;

namespace Isu.Extra.Test;

public class IsuExtraServiceTests
{
    private readonly IsuExtraService service;

    public IsuExtraServiceTests()
    {
        service = new IsuExtraService();
    }

    [Fact]
    public void AddStudentToOGNP_StudentAddedToOGNP()
    {
        var tintMegaFaculty = new MegaFaculty("TiNT");
        var ktiuMegaFaculty = new MegaFaculty("KTiU");
        var newGroup = new AdvancedGroup("M32091", tintMegaFaculty);
        var newStudent = new AdvancedStudent(316944, "Vadim Pavlovets", newGroup);

        OGNPCourse newCourse = service.CreateOGNPCourse("CyberSecurity", ktiuMegaFaculty);
        newCourse.AddNewGroup("КИБ_4.1");

        service.AddStudentToOGNP(newStudent, newCourse);

        Assert.Contains(newStudent, newCourse.Groups[0].Students);
    }

    [Fact]
    public void RemoveStudentFromOGNP_StudentRemovedToOGNP()
    {
        var tintMegaFaculty = new MegaFaculty("TiNT");
        var ktiuMegaFaculty = new MegaFaculty("KTiU");
        var newGroup = new AdvancedGroup("M32091", tintMegaFaculty);
        var newStudent = new AdvancedStudent(316944, "Vadim Pavlovets", newGroup);

        OGNPCourse newCourse = service.CreateOGNPCourse("CyberSecurity", ktiuMegaFaculty);
        newCourse.AddNewGroup("КИБ_4.1");

        service.AddStudentToOGNP(newStudent, newCourse);

        service.RemoveStudentFromOGNP(newStudent, newCourse);

        Assert.Equal(0, newCourse.Groups[0].Students.Count);
    }

    [Fact]
    public void StudentContainInOgnpGroup_StudentContainsInOgnpGroup()
    {
        var tintMegaFaculty = new MegaFaculty("TiNT");
        var ktiuMegaFaculty = new MegaFaculty("KTiU");
        var newGroup = new AdvancedGroup("M32091", tintMegaFaculty);
        var newStudent = new AdvancedStudent(316944, "Vadim Pavlovets", newGroup);

        OGNPCourse newCourse = service.CreateOGNPCourse("CyberSecurity", ktiuMegaFaculty);
        newCourse.AddNewGroup("КИБ_4.1");

        service.AddStudentToOGNP(newStudent, newCourse);
        IReadOnlyList<AdvancedStudent>? students = service.ReciveListOfStudentFromOgnpGroup("КИБ_4.1");

        Assert.Contains(newStudent, students);
    }

    [Fact]
    public void TakeStudentsWhithoutOGNP_ListContaisStudentWhithoutOGNP()
    {
        var tintMegaFaculty = new MegaFaculty("TiNT");
        var ktiuMegaFaculty = new MegaFaculty("KTiU");

        var newGroup = new AdvancedGroup("M32091", tintMegaFaculty);
        var newStudent1 = new AdvancedStudent(316944, "Vadim Pavlovets", newGroup);
        var newStudent2 = new AdvancedStudent(316945, "Maxim Ganihin", newGroup);
        var newStudent3 = new AdvancedStudent(316946, "Nikita Fisenko", newGroup);

        service.AddAdvancedStudentToGroup(newStudent1, newGroup);
        service.AddAdvancedStudentToGroup(newStudent2, newGroup);
        service.AddAdvancedStudentToGroup(newStudent3, newGroup);

        OGNPCourse newCourse = service.CreateOGNPCourse("CyberSecurity", ktiuMegaFaculty);
        newCourse.AddNewGroup("КИБ_4.1");

        service.AddStudentToOGNP(newStudent1, newCourse);
        IReadOnlyList<AdvancedStudent>? students = service.ReciveListOfStudentWithoutOgnp(newGroup);

        Assert.DoesNotContain(newStudent1, students);
        Assert.Contains(newStudent2, students);
        Assert.Contains(newStudent3, students);
    }

    [Fact]
    public void TakeGroupsFromOgnpCourse_ListContainsAddedGroups()
    {
        var tintMegaFaculty = new MegaFaculty("TiNT");

        OGNPCourse newCourse = service.CreateOGNPCourse("CyberSecurity", tintMegaFaculty);
        newCourse.AddNewGroup("КИБ_4.1");
        newCourse.AddNewGroup("КИБ_4.2");

        IReadOnlyList<OGNPGroup>? groups = service.ReciveGroupsInCourse(newCourse);

        Assert.Equal(2, groups!.Count);
    }

    [Fact]
    public void LessonsCollide_ThrowCollideLessonsException()
    {
        var tintMegaFaculty = new MegaFaculty("TiNT");
        var ktiuMegaFaculty = new MegaFaculty("KTiU");

        var newGroup = new AdvancedGroup("M32091", tintMegaFaculty);
        var newStudent = new AdvancedStudent(316944, "Vadim Pavlovets", newGroup);

        var lesson1 = new Lesson("Alexander Mayatin", Time.Second, 2334);
        var lesson2 = new Lesson("Vlasov Vladislav", Time.Second, 228);

        newGroup.AddLesson(lesson1, Days.Friday);

        OGNPCourse newCourse = service.CreateOGNPCourse("CyberSecurity", ktiuMegaFaculty);
        OGNPGroup newOgnpGroup = newCourse.AddNewGroup("КИБ_4.1");
        newOgnpGroup.AddLesson(lesson2, Days.Friday);

        Assert.Throws<CollideLessonsException>(() => service.AddStudentToOGNP(newStudent, newCourse));
    }

    [Fact]
    public void AddStudentToHisMegaFaculty_ThrowInvalidAddStudentToOgnpException()
    {
        var tintMegaFaculty = new MegaFaculty("TiNT");
        var ktiuMegaFaculty = new MegaFaculty("KTiU");

        var newGroup = new AdvancedGroup("M32091", tintMegaFaculty);
        var newStudent = new AdvancedStudent(316944, "Vadim Pavlovets", newGroup);

        OGNPCourse newCourse = service.CreateOGNPCourse("CyberSecurity", tintMegaFaculty);
        newCourse.AddNewGroup("КИБ_4.1");

        Assert.Throws<InvalidAddStudentToOgnpException>(() => service.AddStudentToOGNP(newStudent, newCourse));
    }

    [Fact]
    public void AddStudentToThirdOGNPThrowInvalidChangeCountOfOgnpException()
    {
        var tintMegaFaculty = new MegaFaculty("TiNT");
        var ktiuMegaFaculty = new MegaFaculty("KTiU");
        var ftmiMegaFaculty = new MegaFaculty("FTMI");
        var nozhMegaFaculty = new MegaFaculty("NOZH");

        var newGroup = new AdvancedGroup("M32091", tintMegaFaculty);
        var newStudent = new AdvancedStudent(316944, "Vadim Pavlovets", newGroup);

        OGNPCourse newCourse1 = service.CreateOGNPCourse("CyberSecurity", ktiuMegaFaculty);
        newCourse1.AddNewGroup("КИБ_4.1");

        OGNPCourse newCourse2 = service.CreateOGNPCourse("Economy", nozhMegaFaculty);
        newCourse2.AddNewGroup("ЕКН_4.1");

        OGNPCourse newCourse3 = service.CreateOGNPCourse("AdvancedPhysics", ftmiMegaFaculty);
        newCourse3.AddNewGroup("ФИЗ_4.1");

        service.AddStudentToOGNP(newStudent, newCourse1);
        service.AddStudentToOGNP(newStudent, newCourse2);

        Assert.Throws<InvalidChangeCountOfOgnpException>(() => service.AddStudentToOGNP(newStudent, newCourse3));
    }
}
