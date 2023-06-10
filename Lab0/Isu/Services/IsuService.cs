using Isu.Entities;
using Isu.Exceptions;
using Isu.Models;

namespace Isu.Services
{
    public class IsuService : IIsuService
    {
        private readonly Dictionary<string, Group> _groups;
        private int _totalStudents;

        public IsuService()
        {
            _groups = new Dictionary<string, Group>();
            _totalStudents = 1;
        }

        public Group AddGroup(string name)
        {
            var newGroup = new Group(name);

            _groups.Add(name, newGroup);

            return newGroup;
        }

        public Student AddStudent(Group group, string name)
        {
            if (group is null)
            {
                throw new ArgumentNullException(nameof(group));
            }

            if (group.Students.Count.Equals(Group.MaxNumberOfStudents))
            {
                throw new MaxNumberOfStudentException(group.Students.Count);
            }

            var newStudent = new Student(_totalStudents, name, group);
            _totalStudents += 1;

            group.Students.Add(newStudent);

            return newStudent;
        }

        public void ChangeStudentGroup(Student student, Group newGroup)
        {
            if (student is null)
            {
                throw new ArgumentNullException(nameof(student));
            }

            if (newGroup is null)
            {
                throw new ArgumentNullException(nameof(newGroup));
            }

            if (newGroup.Students.Count.Equals(Group.MaxNumberOfStudents))
            {
                throw new MaxNumberOfStudentException(newGroup.Students.Count);
            }

            string oldStudentGroupName = student.Group.GroupName;

            _groups[oldStudentGroupName].Students.Remove(student);
            student.Group = newGroup;
            newGroup.Students.Add(student);
        }

        public Group? FindGroup(string groupName)
        {
            if (_groups.ContainsKey(groupName))
            {
                return _groups[groupName];
            }

            return null;
        }

        public List<Group> FindGroups(CourseNumber courseNumber)
        {
            var allGroupsFromCourse = new List<Group>();

            foreach (Group group in _groups.Values)
            {
                if (group.CourseNumber.Equals(courseNumber))
                {
                    allGroupsFromCourse.Add(group);
                }
            }

            return allGroupsFromCourse;
        }

        public Student? FindStudent(int id)
        {
            foreach (Group group in _groups.Values)
            {
                foreach (Student student in group.Students)
                {
                    if (student.Id.Equals(id))
                        return student;
                }
            }

            return null;
        }

        public List<Student> FindStudents(string groupName)
        {
            var students = new List<Student>();

            if (FindGroup(groupName) != null)
            {
                students = _groups[groupName].Students;
            }

            return students;
        }

        public List<Student> FindStudents(CourseNumber courseNumber)
        {
            var allStudentsFromCourse = new List<Student>();

            foreach (Group group in _groups.Values)
            {
                if (group.CourseNumber == courseNumber)
                {
                    foreach (Student student in group.Students)
                    {
                        allStudentsFromCourse.Add(student);
                    }
                }
            }

            return allStudentsFromCourse;
        }

        public Student GetStudent(int id)
        {
            if (FindStudent(id) == null)
                throw new InvalidGetStudentException(id);

            return FindStudent(id) !;
        }
    }
}
