using Backups.Exceptions;
using Backups.Services;

namespace Backups.Models;

public class BackupTask : IBackupTask
{
    private Backup _backup;
    private IAlgo _algorithm;
    private IRepository _repository;
    public BackupTask(string name, IAlgo algorithm, Backup backup, IRepository repository)
    {
        if (algorithm is null)
        {
            throw new ArgumentNullException(nameof(algorithm));
        }

        if (backup is null)
        {
            throw new ArgumentNullException(nameof(backup));
        }

        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentNullException(nameof(name));
        }

        Name = name;
        _algorithm = algorithm;
        _backup = backup;
        _repository = repository;
    }

    public string Name { get; }
    public Backup MyBackup => _backup;
    public IRepository MyRepository => _repository;

    public void AddBackupObjects(List<IBackupObject> backupObjects, DateTime dateOfBackup)
    {
        if (backupObjects.Count == 0)
        {
            throw new InvalidCountElementsInListException("Your List of backupObjects is empty");
        }

        var newRestorePoint = new RestorePoint(dateOfBackup, backupObjects);

        _backup.AddRestorePoint(newRestorePoint);
    }

    public void DeleteBackupObjects(List<IBackupObject> backupObjects, DateTime dateOfBackup)
    {
        if (backupObjects.Count == 0)
        {
            throw new InvalidCountElementsInListException("Your List of backupObjects is empty");
        }

        var newBackupObjectsList = new List<IBackupObject>();

        IReadOnlyList<IBackupObject> listOfBackupObjectsInLastRestorePoint = _backup.RestorePoints.Last().BackupObjects;

        foreach (IBackupObject backupObjectInLastRestorePoint in listOfBackupObjectsInLastRestorePoint)
        {
            foreach (IBackupObject deleteBackupObject in backupObjects)
            {
                if (!deleteBackupObject.Equals(backupObjectInLastRestorePoint))
                {
                    newBackupObjectsList.Add(backupObjectInLastRestorePoint);
                }
            }
        }

        var newRestorePoint = new RestorePoint(dateOfBackup, newBackupObjectsList);

        _backup.AddRestorePoint(newRestorePoint);
    }

    public void StartBackupTask()
    {
        _repository.CreateRepository(_repository.RepositoryName, Name);

        _repository.Repositories.Last().CreateRepository(Path.Combine(_repository.RepositoryName, Name), "RestorePoint" + _backup.RestorePoints.Count.ToString());

        _algorithm.InizializeStorage(_backup.RestorePoints.Last().BackupObjects, _repository);

        _algorithm.StartAlgorithm();
    }
}
