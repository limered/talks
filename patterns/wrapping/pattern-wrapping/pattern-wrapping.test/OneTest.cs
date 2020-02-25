using NUnit.Framework;
using pattern_wrapping.one;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace pattern_wrapping.test
{
    [TestFixture]
    public class OneTest
    {
        private Executor _cut;

        private RemoteBackupJsonFake _remoteBackupJsonFake;

        [SetUp]
        public void SetUp()
        {
            _remoteBackupJsonFake = new RemoteBackupJsonFake();

            _cut = new Executor(_remoteBackupJsonFake);
        }

        [Test]
        public async Task SetsCurrentTimeStamp()
        {
            await _cut.Execute(new CancellationToken());

            Assert.AreEqual(DateTime.Now, _remoteBackupJsonFake.LastCallToMethod);
        }
    }

    public class RemoteBackupJsonFake : IRemoteBackupJson
    {
        public DateTime LastCallToMethod { get; set; }

        public Task SetFinishDate(DateTime now)
        {
            LastCallToMethod = now;

            return Task.FromResult(0);
        }
    }
}