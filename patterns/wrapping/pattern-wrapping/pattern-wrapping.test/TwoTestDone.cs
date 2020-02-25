using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using pattern_wrapping.two;
using pattern_wrapping.two.GreetingsProvider;
using pattern_wrapping.two_done;
using TravelingCart = pattern_wrapping.two_done.TravelingCart;

namespace pattern_wrapping.test
{
    public class TwoTestDone
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
