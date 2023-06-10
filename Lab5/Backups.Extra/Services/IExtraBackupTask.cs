using Backups.Models;
using Backups.Services;

namespace Backups.Extra.Services;

public interface IExtraBackupTask : IBackupTask
{
    public void DeleteRestorePointByCount(int count);
    public void DeleteRestorePointByDate(DateTime dateOfBackup);
    public void HybridDeleteRestore(int count, DateTime dateOfBackup);
    public void Recover(RestorePoint restorePoint);
    public void Recover(RestorePoint restorePoint, string path);
    public void Merge(RestorePoint restorePoint1, RestorePoint restorePoint2);
}
