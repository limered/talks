using System.Threading.Tasks;
using NativeFramework;
using NUnit.Framework;
using pattern_wrapping.done.two;

namespace pattern_wrapping.done.tests
{
    public class TwoTests
    {
        private TravelingCart _cut;
        private GreetingServiceFakeDone _greetingServiceFake;

        private const string PrisonerName = "Prisoner";

        [SetUp]
        public void SetUp()
        {
            _greetingServiceFake = new GreetingServiceFakeDone();

            _cut = new TravelingCart(_greetingServiceFake);
        }

        [Test]
        public async Task UsesGreetingsFromApi()
        {
            await _cut.FadeFromBlack(PrisonerName);

            Assert.AreEqual(PrisonerName, _greetingServiceFake.LastPrisonerCalled.Name);
        }
    }

    public class GreetingServiceFakeDone : IGreetingServiceApi
    {
        public Prisoner LastPrisonerCalled { get; set; }

        public void Say(Prisoner prisoner, string message)
        {
            LastPrisonerCalled = prisoner;
        }
    }
}
