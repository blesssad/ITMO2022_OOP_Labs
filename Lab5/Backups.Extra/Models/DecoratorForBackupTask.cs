using Backups.Exceptions;
using Backups.Extra.Services;
using Backups.Models;
using Backups.Services;

namespace Backups.Extra.Models;

public class DecoratorForBackupTask : IBackupTask
{
    private IBackupTask _inner;
    private ILogger _logger;

    public DecoratorForBackupTask(IBackupTask inner, ILogger logger)
    {
        _inner = inner;
        _logger = logger;
    }

    public string Name => _inner.Name;

    public Backup MyBackup => _inner.MyBackup;

    public IRepository MyRepository => _inner.MyRepository;

    public void AddBackupObjects(List<IBackupObject> backupObjects, DateTime dateOfBackup)
    {
        if (backupObjects.Count == 0)
        {
            throw new InvalidCountElementsInListException("Your list is Empty");
        }

        _inner.AddBackupObjects(backupObjects, dateOfBackup);
        _logger.Log("Add BackupObjects");
    }

    public void DeleteBackupObjects(List<IBackupObject> backupObjects, DateTime dateOfBackup)
    {
        if (backupObjects.Count == 0)
        {
            throw new InvalidCountElementsInListException("Your list is Empty");
        }

        _inner.DeleteBackupObjects(backupObjects, dateOfBackup);
        _logger.Log("Delete BackupObjects");
    }

    public void StartBackupTask()
    {
        _inner.StartBackupTask();
        _logger.Log("Start BackupTask");
    }
}
