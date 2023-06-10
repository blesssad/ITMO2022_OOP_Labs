namespace Isu.Extra.Models;
public class MegaFaculty
{
    public MegaFaculty(string nameOfMegaFaculty)
    {
        if (string.IsNullOrEmpty(nameOfMegaFaculty))
        {
            throw new ArgumentNullException(nameof(nameOfMegaFaculty));
        }

        NameOfMegaFaculty = nameOfMegaFaculty;
    }

    public string NameOfMegaFaculty { get; private set; }
}