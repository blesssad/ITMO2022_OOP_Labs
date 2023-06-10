using Backups.Models;

namespace Backups.Services;

public interface IBackupTask
{
    public string Name { get; }
    public Backup MyBackup { get; }
    public IRepository MyRepository { get; }

    public void AddBackupObjects(List<IBackupObject> backupObjects, DateTime dateOfBackup);
    public void DeleteBackupObjects(List<IBackupObject> backupObjects, DateTime dateOfBackup);
    public void StartBackupTask();
}
