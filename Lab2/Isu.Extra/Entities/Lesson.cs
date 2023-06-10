using Isu.Extra.Exceptions;
using Isu.Extra.Models;

namespace Isu.Extra.Entities;

public class Lesson
{
    public Lesson(string teacherName, Time lessonTime, int roomNumber)
    {
        if (string.IsNullOrWhiteSpace(teacherName))
        {
            throw new ArgumentNullException(teacherName);
        }

        if (roomNumber <= 0)
        {
            throw new InvalidRoomNumberException("Room number is negative or zero");
        }

        LessonTime = lessonTime;
        TeacherName = teacherName;
        RoomNumber = roomNumber;
    }

    public Time LessonTime { get; private set; }
    public string TeacherName { get; private set; }

    public int RoomNumber { get; private set; }
}
