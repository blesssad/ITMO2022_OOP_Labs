using Backups.Extra.Exceptions;
using Backups.Extra.Repositories;
using Backups.Models;
using Backups.Services;

namespace Backups.Extra.Models;

public static class ExtraBackupTask
{
    public static void DeleteRestorePointByCount(this IBackupTask backupTask, int count)
    {
        if (backupTask == null)
        {
            throw new ArgumentNullException(nameof(backupTask));
        }

        if (count <= 0)
        {
            throw new InvalidCountOfDeleteRestorePoint("Count is zero or less");
        }

        int minRestorePoints = 1;

        for (int i = 0; i < count; i++)
        {
            if (backupTask.MyBackup.RestorePoints.Count == minRestorePoints)
            {
                break;
            }

            backupTask.MyRepository.DeleteOldRestorePoint();
            backupTask.MyBackup.DeleteOldRestorePoint();
        }
    }

    public static void DeleteRestorePointByDate(this IBackupTask backupTask, DateTime date)
    {
        if (backupTask == null)
        {
            throw new ArgumentNullException(nameof(backupTask));
        }

        int minRestorePoints = 1;

        foreach (RestorePoint restorePoint in backupTask.MyBackup.RestorePoints.ToList())
        {
            if (backupTask.MyBackup.RestorePoints.Count == minRestorePoints)
            {
                break;
            }

            if (restorePoint.Date < date)
            {
                backupTask.MyRepository.DeleteOldRestorePoint();
                backupTask.MyBackup.DeleteOldRestorePoint();
            }
        }
    }

    public static void HybridDeleteRestore(this IBackupTask backupTask, int count, DateTime date)
    {
        if (backupTask == null)
        {
            throw new ArgumentNullException(nameof(backupTask));
        }

        if (count <= 0)
        {
            throw new InvalidCountOfDeleteRestorePoint("Count is zero or less");
        }

        backupTask.DeleteRestorePointByCount(count);
        backupTask.DeleteRestorePointByDate(date);
    }

    public static void Recover(this BackupTask backupTask, RestorePoint restorePoint)
    {
        if (!backupTask.MyBackup.RestorePoints.ToList().Contains(restorePoint))
        {
            throw new RestorePointWasDeletedException("Your restore point was deleted");
        }

        int count = 0;
        var allBackupObjectsPath = new List<string>();

        foreach (RestorePoint myRestorePoint in backupTask.MyBackup.RestorePoints.ToList())
        {
            if (myRestorePoint == restorePoint)
            {
                foreach (IBackupObject backupObject in restorePoint.BackupObjects.ToList())
                {
                    allBackupObjectsPath.Add(backupObject.PathToFile);
                }
            }

            count++;
        }

        backupTask.MyRepository.RecoverFromRestorePoint(count, allBackupObjectsPath);
    }

    public static void Recover(this BackupTask backupTask, RestorePoint restorePoint, string path)
    {
        if (!backupTask.MyBackup.RestorePoints.ToList().Contains(restorePoint))
        {
            throw new RestorePointWasDeletedException("Your restore point was deleted");
        }

        int count = 0;

        foreach (RestorePoint myRestorePoint in backupTask.MyBackup.RestorePoints.ToList())
        {
            if (myRestorePoint == restorePoint)
            {
                break;
            }

            count++;
        }

        backupTask.MyRepository.RecoverFromRestorePoint(count, path);
    }

    public static void Merge(this BackupTask backupTask, RestorePoint restorePoint1, RestorePoint restorePoint2)
    {
        if (!backupTask.MyBackup.RestorePoints.ToList().Contains(restorePoint1) || !backupTask.MyBackup.RestorePoints.ToList().Contains(restorePoint2))
        {
            throw new RestorePointWasDeletedException("Your restore point was deleted");
        }

        int count1 = 0;
        int count2 = 0;
        var backupObjects = new List<IBackupObject>();

        foreach (RestorePoint myRestorePoint in backupTask.MyBackup.RestorePoints.ToList())
        {
            if (myRestorePoint == restorePoint1)
            {
                foreach (IBackupObject backupObject in restorePoint1.BackupObjects.ToList())
                {
                    backupObjects.Add(backupObject);
                }

                break;
            }

            count1++;
        }

        foreach (RestorePoint myRestorePoint in backupTask.MyBackup.RestorePoints.ToList())
        {
            if (myRestorePoint == restorePoint2)
            {
                foreach (IBackupObject backupObject in restorePoint2.BackupObjects.ToList())
                {
                    backupObjects.Add(backupObject);
                }

                break;
            }

            count2++;
        }

        backupTask.AddBackupObjects(backupObjects, DateTime.Now);

        backupTask.MyRepository.CreateRepository(backupTask.MyRepository.RepositoryName, backupTask.Name);

        backupTask.MyRepository.Repositories.Last().CreateRepository(Path.Combine(backupTask.MyRepository.RepositoryName, backupTask.Name), "RestorePoint" + backupTask.MyBackup.RestorePoints.Count.ToString());

        backupTask.MyRepository.Merge(count1, count2, backupTask.MyRepository.Repositories.Last().Repositories.Last().RepositoryName);

        backupTask.MyRepository.DeleteRepository(count1);
        backupTask.MyRepository.DeleteRepository(count2);
    }
}
