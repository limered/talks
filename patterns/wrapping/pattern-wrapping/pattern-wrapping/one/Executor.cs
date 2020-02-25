using System;
using System.Threading;
using System.Threading.Tasks;

namespace pattern_wrapping.one
{
    public class Executor
    {
        private readonly IRemoteBackupJson _remoteBackupJson;

        public Executor(IRemoteBackupJson remoteBackupJson)
        {
            _remoteBackupJson = remoteBackupJson;
        }

        public async Task Execute(CancellationToken ct)
        {
            await FinalizeBackup(ct);
        }

        private async Task FinalizeBackup(CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();
            await _remoteBackupJson.SetFinishDate(DateTime.Now);
        }
    }
}