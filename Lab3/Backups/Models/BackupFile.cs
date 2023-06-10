using Backups.Exceptions;
using Backups.Services;

namespace Backups.Models;

public class BackupFile : IBackupObject
{
    public BackupFile(string path)
    {
        if (!Path.HasExtension(path))
        {
            throw new InvalidTypeOfBackupObjectException("Your file hasn't an extension");
        }

        if (!File.Exists(path))
        {
            throw new InvalidPathToFileOrDirectoryException("Your file doesn't exist");
        }

        PathToFile = path;
    }

    public string PathToFile { get; }
}
