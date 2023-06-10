using Backups.Exceptions;
using Backups.Services;

namespace Backups.Models;

public class BackupDirectory : IBackupObject
{
    public BackupDirectory(string path)
    {
        if (Path.HasExtension(path))
        {
            throw new InvalidTypeOfBackupObjectException("Your Directory has an extension");
        }

        if (!Directory.Exists(path))
        {
            throw new InvalidPathToFileOrDirectoryException("Your Directory doesn't exist");
        }

        PathToFile = path;
    }

    public string PathToFile { get; }
}
