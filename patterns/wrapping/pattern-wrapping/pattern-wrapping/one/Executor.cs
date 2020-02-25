using System;
using System.Threading;
using System.Threading.Tasks;

namespace pattern_wrapping.one
{
    public class Executor
    {
        private readonly IRemoteBackupJson _remoteBackupJson;
        private readonly IDateTime _dateTimeWrapper;

        public Executor(IRemoteBackupJson remoteBackupJson, 
            IDateTime dateTimeWrapper)
        {
            _remoteBackupJson = remoteBackupJson;
            _dateTimeWrapper = dateTimeWrapper;
        }

        public async Task Execute(CancellationToken ct)
        {
            await FinalizeBackup(ct);
        }

        private async Task FinalizeBackup(CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();
            await _remoteBackupJson.SetFinishDate(_dateTimeWrapper.Now());
        }
    }
}