using Backups.Exceptions;
using Backups.Services;
using Ionic.Zip;

namespace Backups.Entities;

public class RealRepository : IRepository
{
    private List<RealRepository> _repositories;
    public RealRepository(string repositoryPath)
    {
        if (string.IsNullOrEmpty(repositoryPath))
        {
            throw new ArgumentNullException(nameof(repositoryPath));
        }

        RepositoryName = repositoryPath;
        _repositories = new List<RealRepository>();
    }

    public IReadOnlyList<IRepository> Repositories => _repositories;
    public string RepositoryName { get; }

    public void CreateRepository(string repositoryPath, string repositoryName)
    {
        Directory.CreateDirectory(Path.Combine(repositoryPath, repositoryName));

        var newRepository = new RealRepository(Path.Combine(repositoryPath, repositoryName));
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
            foreach (IBackupObject backupObject in backupObjects)
            {
                zipFile.AddItem(backupObject.PathToFile, string.Empty);
            }

            return zipFile;
        }
    }

    public void SaveZipFiles(IReadOnlyList<ZipFile> zipFiles)
    {
        if (zipFiles.Count == 0)
        {
            throw new InvalidCountElementsInListException("Your list is empty!");
        }

        int numberOfStorage = 1;

        foreach (ZipFile zip in zipFiles)
        {
            zip.Save(Path.Combine(_repositories.Last().Repositories.Last().RepositoryName, "Storage" + numberOfStorage.ToString() + ".zip"));
            numberOfStorage++;
        }
    }

    public void DeleteOldRepository()
    {
        _repositories.Remove(_repositories.First());
    }
}
