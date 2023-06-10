using System.Text.Json;
using Backups.Models;

namespace Backups.Extra.Models;

public class StatusController
{
    public StatusController(string path)
    {
        Path = path;
    }

    public string Path { get; }

    public void SaveState(BackupTask backupTask)
    {
        if (backupTask == null)
        {
            throw new ArgumentNullException(nameof(backupTask));
        }

        if (string.IsNullOrWhiteSpace(Path))
        {
            throw new ArgumentNullException(nameof(Path));
        }

        string json = JsonSerializer.Serialize(backupTask, typeof(BackupTask));

        File.WriteAllText(Path, json);
    }

    public BackupTask? LoadState()
    {
        if (string.IsNullOrWhiteSpace(Path))
        {
            throw new ArgumentNullException(nameof(Path));
        }

        BackupTask? backupTask = JsonSerializer.Deserialize<BackupTask>(Path);

        if (backupTask == null)
        {
            throw new ArgumentNullException(nameof(backupTask));
        }

        return backupTask;
    }
}
