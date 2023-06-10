namespace Backups.Services;

public interface IAlgo
{
    public void InizializeStorage(IReadOnlyList<IBackupObject> backupObjects, IRepository repository);
    public void StartAlgorithm();
}
