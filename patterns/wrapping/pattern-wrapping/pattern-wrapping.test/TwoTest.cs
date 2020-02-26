using NativeFramework;
using NUnit.Framework;
using pattern_wrapping.two;
using System.Threading.Tasks;

namespace pattern_wrapping.test
{
    public class TwoTest
    {
        private const string PrisonerName = "Prisoner";
        private TravelingCart _cut;
        private GreetingServiceFake _greetingServiceFake;
        [SetUp]
        public void SetUp()
        {
            _greetingServiceFake = new GreetingServiceFake();

            _cut = new TravelingCart();
        }

        [Test]
        public async Task UsesGreetingsFromApi()
        {
            await _cut.StartGame(PrisonerName);

            Assert.AreEqual(PrisonerName, _greetingServiceFake.LastPrisonerCalled.Name);
        }
    }

    public class GreetingServiceFake
    {
        public Prisoner LastPrisonerCalled { get; set; }
    }
}