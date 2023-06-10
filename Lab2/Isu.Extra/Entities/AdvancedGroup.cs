using Isu.Entities;
using Isu.Extra.Models;

namespace Isu.Extra.Entities;

public class AdvancedGroup : Group
{
    public AdvancedGroup(string groupName, MegaFaculty megaFaculty)
        : base(groupName)
    {
        if (string.IsNullOrWhiteSpace(groupName))
        {
            throw new ArgumentNullException(groupName);
        }

        if (megaFaculty is null)
        {
            throw new ArgumentNullException(nameof(megaFaculty));
        }

        var groupSchedule = new Schedule();

        GroupSchedule = groupSchedule;
        MegaFaculty = megaFaculty;
    }

    public MegaFaculty MegaFaculty { get; private set; }
    public Schedule GroupSchedule { get; private set; }

    public void AddLesson(Lesson newLesson, Days day)
    {
        if (newLesson is null)
        {
            throw new ArgumentNullException(nameof(newLesson));
        }

        GroupSchedule.AddLessonInScheduleWeek(newLesson, day);
    }
}
