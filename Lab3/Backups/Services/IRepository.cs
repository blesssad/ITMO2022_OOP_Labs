using Ionic.Zip;

namespace Backups.Services;

public interface IRepository
{
    public string RepositoryName { get; }
    public IReadOnlyList<IRepository> Repositories { get; }
    public void CreateRepository(string repositoryPath, string repositoryName);
    public ZipFile CreateZipFile(IReadOnlyList<IBackupObject> backupObjects);
    public void SaveZipFiles(IReadOnlyList<ZipFile> zipFiles);
    public void DeleteOldRepository();
}
