﻿using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;
using pattern_wrapping.one_done;
using pattern_wrapping.test.Fakes;
using Executor = pattern_wrapping.one.Executor;

namespace pattern_wrapping.test
{
    [TestFixture]
    public class OneTest
    {
        private Executor _cut;

        private PrisonerServiceFake _prisonerServiceFake;

        [SetUp]
        public void SetUp()
        {
            _prisonerServiceFake = new PrisonerServiceFake();

            _cut = new Executor(_prisonerServiceFake);
        }

        [Test]
        public async Task SetsCurrentTimeStamp()
        {
            await _cut.Execute(new CancellationToken());

            Assert.AreEqual(DateTime.Now, _prisonerServiceFake.LastCallToMethod);
        }
    }
}