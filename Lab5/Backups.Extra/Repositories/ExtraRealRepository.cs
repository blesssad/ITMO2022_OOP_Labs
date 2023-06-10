using Backups.Services;
using Ionic.Zip;

namespace Backups.Extra.Repositories;

public static class ExtraRealRepository
{
    public static void DeleteOldRestorePoint(this IRepository repository)
    {
        string nameOfRestorePoint = repository.Repositories.First().Repositories.First().RepositoryName;

        Directory.Delete(nameOfRestorePoint, true);

        repository.DeleteOldRepository();
    }

    public static void RecoverFromRestorePoint(this IRepository repository,  int count, string path)
    {
        var directoryInfo = new DirectoryInfo(repository.Repositories.First().Repositories[count].RepositoryName);

        FileSystemInfo[] array = directoryInfo.GetFileSystemInfos();

        foreach (FileSystemInfo file in array)
        {
            using (var zipFile = ZipFile.Read(file.FullName))
            {
                zipFile.ExtractAll(path, ExtractExistingFileAction.OverwriteSilently);
            }
        }
    }

    public static void RecoverFromRestorePoint(this IRepository repository, int count, List<string> allBackupObjectsPath)
    {
        var directoryInfo = new DirectoryInfo(repository.Repositories.First().Repositories[count].RepositoryName);

        FileSystemInfo[] array = directoryInfo.GetFiles();

        foreach (FileSystemInfo file in array)
        {
            using (var zip = ZipFile.Read(file.FullName))
            {
                foreach (string backupObjectPath in allBackupObjectsPath)
                {
                    foreach (ZipEntry e in zip.Where(x => backupObjectPath.Contains(x.FileName)))
                    {
                        e.Extract(backupObjectPath, ExtractExistingFileAction.OverwriteSilently);
                    }
                }
            }
        }
    }

    public static void DeleteRepository(this IRepository repository, int count)
    {
        string nameOfRestorePoint = repository.Repositories[count].Repositories.First().RepositoryName;

        Directory.Delete(nameOfRestorePoint, true);
    }

    public static void Merge(this IRepository repository, int count1, int count2, string path)
    {
        int i = 1;

        var directoryInfo1 = new DirectoryInfo(repository.Repositories[count1].Repositories.First().RepositoryName);

        FileSystemInfo[] array1 = directoryInfo1.GetFiles();

        var directoryInfo2 = new DirectoryInfo(repository.Repositories[count2].Repositories.First().RepositoryName);

        FileSystemInfo[] array2 = directoryInfo2.GetFiles();

        foreach (FileSystemInfo file in array1)
        {
            File.Copy(file.FullName, Path.Combine(path, "Storge" + i + ".zip"), true);
            i++;
        }

        foreach (FileSystemInfo file in array2)
        {
            File.Copy(file.FullName, Path.Combine(path, "Storge" + i + ".zip"), true);
            i++;
        }
    }
}
