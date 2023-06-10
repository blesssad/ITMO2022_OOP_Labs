using Backups.Entities;
using Backups.Extra.Loggers;
using Backups.Extra.Models;
using Backups.Models;
using Backups.Services;
using Xunit;

namespace Backups.Extra.Test;

public class ExtraBackupTaskTest
{
    [Fact]
    public void DeleteByCount_ExistsOnlyOneRepository()
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

        backupTask.DeleteRestorePointByCount(1);

        Assert.False(Directory.Exists(@"./../../../Backups/FirstTask/RestorePoint1"));
    }

    [Fact]
    public void DeleteByDate_ExistsOnlyOneRepository()
    {
        var firstBackupFile = new BackupFile(@"./../../../ForTest/first.txt");
        var secondBackupDirectory = new BackupDirectory(@"./../../../ForTest/Folder");
        var newListOfBackupObjects = new List<IBackupObject>();

        newListOfBackupObjects.Add(firstBackupFile);
        newListOfBackupObjects.Add(secondBackupDirectory);

        var backup = new Backup();
        var repository = new RealRepository(@"./../../../Backups");
        var algorithm = new SplitStorage();

        var backupTask = new BackupTask("SecondTask", algorithm, backup, repository);

        backupTask.AddBackupObjects(newListOfBackupObjects, DateTime.Today.AddDays(-5));

        backupTask.StartBackupTask();

        var secondListOfBackupObjects = new List<IBackupObject>();
        secondListOfBackupObjects.Add(secondBackupDirectory);

        backupTask.DeleteBackupObjects(secondListOfBackupObjects, DateTime.Today.AddDays(-2));

        backupTask.StartBackupTask();

        backupTask.DeleteRestorePointByDate(DateTime.Today.AddDays(-3));

        Assert.False(Directory.Exists(@"./../../../Backups/SecondTask/RestorePoint1"));
    }

    [Fact]
    public void LogExistsAndWorkCorrect()
    {
        var firstBackupFile = new BackupFile(@"./../../../ForTest/first.txt");
        var secondBackupDirectory = new BackupDirectory(@"./../../../ForTest/Folder");
        var newListOfBackupObjects = new List<IBackupObject>();

        newListOfBackupObjects.Add(firstBackupFile);
        newListOfBackupObjects.Add(secondBackupDirectory);

        var backup = new Backup();
        var repository = new RealRepository(@"./../../../Backups");
        var algorithm = new SplitStorage();
        var logger = new FileLogger(@"./../../../ForTest/log.txt");
        var decorator = new DecoratorForRepository(repository, logger);

        var backupTask = new BackupTask("ThirdTask", algorithm, backup, decorator);

        var decorator2 = new DecoratorForBackupTask(backupTask, logger);

        decorator2.AddBackupObjects(newListOfBackupObjects, DateTime.Today.AddDays(-5));

        decorator2.StartBackupTask();

        var secondListOfBackupObjects = new List<IBackupObject>();
        secondListOfBackupObjects.Add(secondBackupDirectory);

        decorator2.DeleteBackupObjects(secondListOfBackupObjects, DateTime.Today.AddDays(-2));

        decorator2.StartBackupTask();

        decorator2.DeleteRestorePointByDate(DateTime.Today.AddDays(-3));

        Assert.True(File.Exists(@"./../../../ForTest/log.txt"));
    }

    [Fact]
    public void RecoverWorkCorrect()
    {
        var firstBackupFile = new BackupFile(@"./../../../ForTest/first.txt");
        var secondBackupDirectory = new BackupFile(@"./../../../ForTest/Folder/hello.txt");
        var newListOfBackupObjects = new List<IBackupObject>();

        newListOfBackupObjects.Add(firstBackupFile);
        newListOfBackupObjects.Add(secondBackupDirectory);

        var backup = new Backup();
        var repository = new RealRepository(@"./../../../Backups");
        var algorithm = new SplitStorage();

        var backupTask = new BackupTask("ForthTask", algorithm, backup, repository);

        backupTask.AddBackupObjects(newListOfBackupObjects, DateTime.Today.AddDays(-5));

        backupTask.StartBackupTask();

        var secondListOfBackupObjects = new List<IBackupObject>();
        secondListOfBackupObjects.Add(secondBackupDirectory);

        backupTask.DeleteBackupObjects(secondListOfBackupObjects, DateTime.Today.AddDays(-2));

        backupTask.StartBackupTask();

        RestorePoint firstRestorePoint = backupTask.MyBackup.RestorePoints[0];

        backupTask.Recover(firstRestorePoint, @"./../../../Recover");

        Assert.True(File.Exists(@"./../../../Recover/hello.txt"));
        Assert.True(File.Exists(@"./../../../Recover/first.txt"));
    }

    [Fact]
    public void MergeWorkCorrect_DirectoryWasCreated()
    {
        var firstBackupFile = new BackupFile(@"./../../../ForTest/first.txt");
        var secondBackupDirectory = new BackupFile(@"./../../../ForTest/file.txt");
        var newListOfBackupObjects = new List<IBackupObject>();

        newListOfBackupObjects.Add(firstBackupFile);
        newListOfBackupObjects.Add(secondBackupDirectory);

        var backup = new Backup();
        var repository = new RealRepository(@"./../../../Backups");
        var algorithm = new SplitStorage();

        var backupTask = new BackupTask("FifthTask", algorithm, backup, repository);

        backupTask.AddBackupObjects(newListOfBackupObjects, DateTime.Today.AddDays(-5));

        backupTask.StartBackupTask();

        var secondListOfBackupObjects = new List<IBackupObject>();
        secondListOfBackupObjects.Add(secondBackupDirectory);

        backupTask.AddBackupObjects(secondListOfBackupObjects, DateTime.Today.AddDays(-2));

        backupTask.StartBackupTask();

        RestorePoint firstRestorePoint = backupTask.MyBackup.RestorePoints[0];
        RestorePoint secondRestorePoint = backupTask.MyBackup.RestorePoints[1];

        backupTask.Merge(firstRestorePoint, secondRestorePoint);

        Assert.True(Directory.Exists(@"./../../../Backups/FifthTask/RestorePoint3"));
    }
}
