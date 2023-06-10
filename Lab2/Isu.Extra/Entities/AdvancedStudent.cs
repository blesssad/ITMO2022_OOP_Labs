using Isu.Entities;
using Isu.Extra.Exceptions;

namespace Isu.Extra.Entities;

public class AdvancedStudent : Student
{
    public AdvancedStudent(int id, string studentName, AdvancedGroup advancedGroup)
        : base(id, studentName, advancedGroup)
    {
        if (advancedGroup is null)
        {
            throw new ArgumentNullException(nameof(advancedGroup));
        }

        AdvancedGroup = advancedGroup;
        CurrentNumberOfOGNP = 0;
    }

    public AdvancedGroup AdvancedGroup { get; private set; }

    public int CurrentNumberOfOGNP { get; private set; }

    public void AddOGNP()
    {
        if (CurrentNumberOfOGNP >= 2)
        {
            throw new InvalidChangeCountOfOgnpException("ognp count more then 2");
        }

        CurrentNumberOfOGNP++;
    }

    public void RemoveOGNP()
    {
        if (CurrentNumberOfOGNP < 0)
        {
            throw new InvalidChangeCountOfOgnpException("ognp count is negative");
        }

        CurrentNumberOfOGNP--;
    }
}
