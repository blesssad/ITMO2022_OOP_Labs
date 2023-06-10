using Isu.Extra.Entities;
using Isu.Extra.Models;
using Isu.Services;

namespace Isu.Extra.Services;
public interface IIsuExtraService : IIsuService
{
    void AddAdvancedStudentToGroup(AdvancedStudent student, AdvancedGroup group);
    OGNPCourse CreateOGNPCourse(string courseName, MegaFaculty megaFaculty);
    void AddStudentToOGNP(AdvancedStudent student, OGNPCourse ognpCourse);
    void RemoveStudentFromOGNP(AdvancedStudent student, OGNPCourse ognpCourse);

    IReadOnlyList<AdvancedStudent> ReciveListOfStudentFromOgnpGroup(string groupName);
    IReadOnlyList<AdvancedStudent> ReciveListOfStudentWithoutOgnp(AdvancedGroup group);
    IReadOnlyList<OGNPGroup> ReciveGroupsInCourse(OGNPCourse ognpCourse);
}
