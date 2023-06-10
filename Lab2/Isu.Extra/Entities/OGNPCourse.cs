using Isu.Extra.Models;

namespace Isu.Extra.Entities;

public class OGNPCourse
{
    private List<OGNPGroup> _ognpGroups;
    public OGNPCourse(string courseName, MegaFaculty megaFaculty)
    {
        if (string.IsNullOrWhiteSpace(courseName))
        {
            throw new ArgumentNullException(courseName);
        }

        MegaFaculty = megaFaculty;
        CourseName = courseName;
        _ognpGroups = new List<OGNPGroup>();
    }

    public MegaFaculty MegaFaculty { get; private set; }
    public string CourseName { get; private set; }

    public IReadOnlyList<OGNPGroup> Groups => _ognpGroups;

    public OGNPGroup AddNewGroup(string groupName)
    {
        if (string.IsNullOrWhiteSpace(groupName))
        {
            throw new ArgumentNullException(nameof(groupName));
        }

        var newGroup = new OGNPGroup(groupName, MegaFaculty);

        _ognpGroups.Add(newGroup);

        return newGroup;
    }
}
