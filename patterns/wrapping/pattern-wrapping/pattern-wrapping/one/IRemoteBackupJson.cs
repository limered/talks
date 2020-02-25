using System;
using System.Threading.Tasks;

namespace pattern_wrapping.one
{
    public interface IRemoteBackupJson
    {
        Task SetFinishDate(DateTime now);
    }

    public class RemoteBackupJson : IRemoteBackupJson
    {
        public Task SetFinishDate(DateTime now)
        {
            throw new NotImplementedException();
        }
    }
}