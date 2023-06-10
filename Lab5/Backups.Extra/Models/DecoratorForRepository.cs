using Backups.Exceptions;
using Backups.Extra.Services;
using Backups.Services;
using Ionic.Zip;

namespace Backups.Extra.Models;

public class DecoratorForRepository : IRepository
{
    private IRepository _inner;
    private ILogger _logger;

    public DecoratorForRepository(IRepository inner, ILogger logger)
    {
        _inner = inner;
        _logger = logger;
    }

    public string RepositoryName => _inner.RepositoryName;

    public IReadOnlyList<IRepository> Repositories => _inner.Repositories;

    public void CreateRepository(string repositoryPath, string repositoryName)
    {
        if (string.IsNullOrWhiteSpace(repositoryPath))
        {
            throw new ArgumentNullException(nameof(repositoryPath));
        }

        if (string.IsNullOrWhiteSpace(repositoryName))
        {
            throw new ArgumentNullException(nameof(repositoryName));
        }

        _inner.CreateRepository(repositoryPath, repositoryName);
        _logger.Log("Create Repository");
    }

    public ZipFile CreateZipFile(IReadOnlyList<IBackupObject> backupObjects)
    {
        if (backupObjects.Count == 0)
        {
            throw new InvalidCountElementsInListException("Your list is empty!");
        }

        ZipFile zipFile = _inner.CreateZipFile(backupObjects);
        _logger.Log("Create ZipFile");

        return zipFile;
    }

    public void DeleteOldRepository()
    {
        _inner.DeleteOldRepository();
        _logger.Log("Delete Repository");
    }

    public void SaveZipFiles(IReadOnlyList<ZipFile> zipFiles)
    {
        if (zipFiles.Count == 0)
        {
            throw new InvalidCountElementsInListException("Your list is empty!");
        }

        _inner.SaveZipFiles(zipFiles);
        _logger.Log("Save ZipFiles");
    }
}
