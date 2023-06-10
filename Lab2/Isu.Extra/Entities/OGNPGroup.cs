using Isu.Extra.Exceptions;
using Isu.Extra.Models;

namespace Isu.Extra.Entities;
public class OGNPGroup
{
    public const int MaxNumberOfStudents = 30;
    private List<AdvancedStudent> _studentsInGroup;
    public OGNPGroup(string groupName, MegaFaculty megaFaculty)
    {
        GroupName = groupName;
        MegaFaculty = megaFaculty;

        OGNPGroupSchedule = new Schedule();
        _studentsInGroup = new List<AdvancedStudent>();
    }

    public string GroupName { get; }
    public MegaFaculty MegaFaculty { get; }
    public Schedule OGNPGroupSchedule { get; private set; }
    public IReadOnlyCollection<AdvancedStudent> Students => _studentsInGroup;
    public void AddStudent(AdvancedStudent student)
    {
        if (student is null)
        {
            throw new ArgumentNullException(nameof(student));
        }

        Lesson? studentLesson = OGNPGroupSchedule.ReadSchedule
            .SelectMany(scheduleDay => scheduleDay.Lessons
            .Where(lesson => lesson != null && student.AdvancedGroup.GroupSchedule.ReadSchedule[(int)scheduleDay.Day].Lessons[(int)lesson.LessonTime] != null))
            .FirstOrDefault();

        if (studentLesson != null)
        {
            throw new CollideLessonsException("Lessons collide");
        }

        _studentsInGroup.Add(student);
    }

    public bool FindStudent(AdvancedStudent student)
    {
        bool answer = _studentsInGroup.Contains(student);

        return answer;
    }

    public void RemoveStudent(AdvancedStudent student)
    {
        if (student is null)
        {
            throw new ArgumentNullException(nameof(student));
        }

        _studentsInGroup.Remove(student);
    }

    public void AddLesson(Lesson newLesson, Days day)
    {
        OGNPGroupSchedule.AddLessonInScheduleWeek(newLesson, day);
    }
}
