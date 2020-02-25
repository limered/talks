using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using pattern_wrapping.three_done;
using pattern_wrapping.two;
using TravelingCart = pattern_wrapping.three_done.TravelingCart;

namespace pattern_wrapping.test
{
    public class UltraTest
    {
        private TravelingCart _cut;

        private GreetingServiceFakeDone _greetingsService;
        private PrisonerFake _prisonerFake;

        [SetUp]
        public void SetUp()
        {
            _greetingsService = new GreetingServiceFakeDone();
            _prisonerFake = new PrisonerFake();

            _cut = new TravelingCart(_greetingsService, _prisonerFake);
        }

        [Test]
        public async Task EveryPrisonerSleepsOneTime()
        {
            await _cut.FadeFromBlack("one", "two");

            Assert.AreEqual(1, _prisonerFake.SleepCounter);
        }
    }

    internal class PrisonerFake:IPrisoner
    {
        public int SleepCounter;

        public Prisoner Original { get; set; }
        public string Name { get; set; }
        public Task SleepAsync()
        {
            SleepCounter++;
            return Task.FromResult(0);
        }
    }
}
