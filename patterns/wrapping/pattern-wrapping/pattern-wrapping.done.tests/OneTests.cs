using System;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using pattern_wrapping.done.one;
using pattern_wrapping.done.tests.Fakes;

namespace pattern_wrapping.done.tests
{
    [TestFixture]
    public class OneTestDone
    {
        private Executor _cut;

        private PrisonerServiceFake _prisonerServiceFake;
        private DateTimeFake _dateTimeFake;

        [SetUp]
        public void SetUp()
        {
            _prisonerServiceFake = new PrisonerServiceFake();
            _dateTimeFake = new DateTimeFake();

            _cut = new Executor(_prisonerServiceFake, _dateTimeFake);
        }

        [Test]
        public async Task SetsCurrentTimeStamp()
        {
            await _cut.Execute(new CancellationToken());

            Assert.AreEqual(new DateTime(2020, 1, 16), _prisonerServiceFake.LastCallToMethod);
        }
    }

    internal class DateTimeFake : IDateTime
    {
        public DateTime Now => new DateTime(2020, 1, 16);
    }
}