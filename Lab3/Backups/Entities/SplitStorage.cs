using Backups.Exceptions;
using Backups.Services;
using Ionic.Zip;

namespace Backups.Entities;

public class SplitStorage : IAlgo
{
    private IReadOnlyList<IBackupObject> _backupObjects;
    private IRepository? _repository;
    public SplitStorage()
    {
        _backupObjects = new List<IBackupObject>();
        _repository = null;
    }

    public void InizializeStorage(IReadOnlyList<IBackupObject> backupObjects, IRepository repository)
    {
        if (repository is null)
        {
            throw new ArgumentNullException(nameof(repository));
        }

        if (backupObjects.Count == 0)
        {
            throw new InvalidCountElementsInListException("Your list is empty!");
        }

        _backupObjects = backupObjects;
        _repository = repository;
    }

    public void StartAlgorithm()
    {
        if (_repository is null)
        {
            throw new ArgumentNullException(nameof(_repository));
        }

        if (_backupObjects.Count == 0)
        {
            throw new InvalidCountElementsInListException("Your list is empty!");
        }

        var newListOfZipFiles = new List<ZipFile>();
        var newListOfBackupObjects = new List<IBackupObject>();

        foreach (IBackupObject backupObject in _backupObjects)
        {
            newListOfBackupObjects.Add(backupObject);
            newListOfZipFiles.Add(_repository.CreateZipFile(newListOfBackupObjects));
            newListOfBackupObjects.Remove(backupObject);
        }

        _repository.SaveZipFiles(newListOfZipFiles);
    }
}
