using Backups.Services;

namespace Backups.Models;

public class RestorePoint
{
    private List<IBackupObject> _backupObjects;
    public RestorePoint(DateTime date, List<IBackupObject> backupObjects)
    {
        Date = date;
        _backupObjects = backupObjects;
    }

    public DateTime Date { get; private set; }

    public IReadOnlyList<IBackupObject> BackupObjects => _backupObjects;
}
