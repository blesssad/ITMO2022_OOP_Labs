using Backups.Services;

namespace Backups.Extra.Services;

public interface IExtraRepository : IRepository
{
    public void DeleteOldRestorePoint();
    public void RecoverFromRestorePoint(int count, string path);
    public void RecoverFromRestorePoint(int count, List<string> allBackupObjectsPath);
    public void Merge(int count1, int count2, string path);
}
