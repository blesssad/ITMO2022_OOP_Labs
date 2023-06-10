using Backups.Entities;
using Backups.Models;
using Backups.Services;
using Xunit;

namespace Backups.Test;

public class BackupTaskTest
{
    [Fact]
    public void RealRepository_AddBackupObjectsToBackupTask_ListContaisRestorePoint()
    {
        var firstBackupFile = new BackupFile(@"./../../../ForTest/first.txt");
        var secondBackupDirectory = new BackupDirectory(@"./../../../ForTest/Folder");
        var newListOfBackupObjects = new List<IBackupObject>();

        newListOfBackupObjects.Add(firstBackupFile);
        newListOfBackupObjects.Add(secondBackupDirectory);

        var backup = new Backup();
        var repository = new RealRepository(@"./../../../Backups");
        var algorithm = new SplitStorage();

        var backupTask = new BackupTask("FirstTask", algorithm, backup, repository);

        backupTask.AddBackupObjects(newListOfBackupObjects, DateTime.Now);

        backupTask.StartBackupTask();

        var secondListOfBackupObjects = new List<IBackupObject>();
        secondListOfBackupObjects.Add(secondBackupDirectory);

        backupTask.DeleteBackupObjects(secondListOfBackupObjects, DateTime.Now);

        backupTask.StartBackupTask();

        Assert.Equal(2, backup.RestorePoints.Count);
        Assert.Equal(2, backup.RestorePoints.First().BackupObjects.Count);
        Assert.Equal(1, backup.RestorePoints.Last().BackupObjects.Count);
    }

    [Fact]
    public void RealRepository_BackupTaskIsWork_RestorePointWithZipCreated()
    {
        var firstBackupFile = new BackupFile(@"./../../../ForTest/first.txt");
        var secondBackupFile = new BackupFile(@"./../../../ForTest/file.txt");
        var newListOfBackupObjects = new List<IBackupObject>();

        newListOfBackupObjects.Add(firstBackupFile);
        newListOfBackupObjects.Add(secondBackupFile);

        var backup = new Backup();
        var repository = new RealRepository(@"./../../../Backups");
        var algorithm = new SingleStorage();

        var backupTask = new BackupTask("SecondTask", algorithm, backup, repository);

        backupTask.AddBackupObjects(newListOfBackupObjects, DateTime.Now);

        backupTask.StartBackupTask();

        Assert.True(File.Exists(@"./../../../Backups/SecondTask/RestorePoint1/Storage1.zip"));
    }
}
