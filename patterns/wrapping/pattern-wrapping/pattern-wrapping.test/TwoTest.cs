﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using pattern_wrapping.two;
using pattern_wrapping.two.GreetingsProvider;

namespace pattern_wrapping.test
{
    public class TwoTest
    {
        private TravelingCart _cut;
        private GreetingServiceFake _greetingServiceFake;

        private const string PrisonerName = "Prisoner";

        [SetUp]
        public void SetUp()
        {
            _greetingServiceFake = new GreetingServiceFake();

            _cut = new TravelingCart();
        }

        [Test]
        public async Task UsesGreetingsFromApi()
        {
            await _cut.FadeFromBlack(PrisonerName);

            Assert.AreEqual(PrisonerName, _greetingServiceFake.LastPrisonerCalled.Name);
        }
    }

    public class GreetingServiceFake
    {
        public Prisoner LastPrisonerCalled { get; set; }
    }
}
