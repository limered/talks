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
        private DateTimeFake _dateTimeFake;

        [SetUp]
        public void SetUp()
        {
            _remoteBackupJsonFake = new RemoteBackupJsonFake();
            _dateTimeFake = new DateTimeFake();

            _cut = new Executor(_remoteBackupJsonFake, _dateTimeFake);
        }

        [Test]
        public async Task SetsCurrentTimeStamp()
        {
            await _cut.Execute(new CancellationToken());

            Assert.AreEqual(new DateTime(2020, 1, 16), _remoteBackupJsonFake.LastCallToMethod);
        }
    }

    internal class DateTimeFake : IDateTime
    {
        public DateTime Now()
        {
            return new DateTime(2020, 1, 16);
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