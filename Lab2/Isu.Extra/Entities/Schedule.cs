using Isu.Extra.Models;

namespace Isu.Extra.Entities;

public class Schedule
{
    private List<ScheduleDay> _scheduleWeek;
    public Schedule()
    {
        var schedule = new List<ScheduleDay>();

        foreach (Days day in Enum.GetValues(typeof(Days)))
        {
            var scheduleDay = new ScheduleDay(day);

            schedule.Add(scheduleDay);
        }

        _scheduleWeek = schedule;
    }

    public IReadOnlyList<ScheduleDay> ReadSchedule => _scheduleWeek;

    public void AddLessonInScheduleWeek(Lesson lesson, Days day)
    {
        foreach (ScheduleDay lessonsDay in _scheduleWeek)
        {
            if (lessonsDay.Day == day)
            {
                lessonsDay.AddLessonInScheduleDay(lesson);
            }
        }
    }
}
