using Backups.Exceptions;
using Backups.Services;
using Ionic.Zip;

namespace Backups.Entities;

public class VirtualRepository : IRepository
{
    private List<VirtualRepository> _repositories;
    private List<ZipFile> _storages;
    public VirtualRepository(string repositoryPath)
    {
        if (string.IsNullOrEmpty(repositoryPath))
        {
            throw new ArgumentNullException(nameof(repositoryPath));
        }

        RepositoryName = repositoryPath;
        _repositories = new List<VirtualRepository>();
        _storages = new List<ZipFile>();
    }

    public IReadOnlyList<IRepository> Repositories => _repositories;
    public IReadOnlyList<ZipFile> Storages => _storages;
    public string RepositoryName { get; }

    public void CreateRepository(string repositoryPath, string repositoryName)
    {
        var newRepository = new VirtualRepository(Path.Combine(repositoryPath, repositoryName));
        _repositories.Add(newRepository);
    }

    public ZipFile CreateZipFile(IReadOnlyList<IBackupObject> backupObjects)
    {
        if (backupObjects.Count == 0)
        {
            throw new InvalidCountElementsInListException("Your list is empty!");
        }

        using (var zipFile = new ZipFile())
        {
            return zipFile;
        }
    }

    public void SaveZipFiles(IReadOnlyList<ZipFile> zipFiles)
    {
        if (zipFiles.Count == 0)
        {
            throw new InvalidCountElementsInListException("Your list is empty!");
        }

        foreach (ZipFile zip in zipFiles)
        {
            _storages.Add(zip);
        }
    }

    public void DeleteOldRepository()
    {
        _repositories.Remove(_repositories.First());
    }
}
