using Isu.Extra.Entities;
using Isu.Extra.Exceptions;

namespace Isu.Extra.Models;

public class ScheduleDay
{
    public const int MaxCountOfLessons = 8;
    private List<Lesson?> _dayLessons;
    public ScheduleDay(Days day)
    {
        _dayLessons = new List<Lesson?>();

        for (int i = 0; i < MaxCountOfLessons; i++)
        {
            _dayLessons.Add(null);
        }

        Day = day;
    }

    public Days Day { get; }
    public IReadOnlyList<Lesson?> Lessons => _dayLessons;

    public void AddLessonInScheduleDay(Lesson lesson)
    {
        if (lesson is null)
        {
            throw new ArgumentNullException(nameof(lesson));
        }

        if (_dayLessons[(int)lesson.LessonTime] != null)
        {
            throw new CollideLessonsException("In that day alredy exsist lesson");
        }

        _dayLessons[(int)lesson.LessonTime] = lesson;
    }
}
