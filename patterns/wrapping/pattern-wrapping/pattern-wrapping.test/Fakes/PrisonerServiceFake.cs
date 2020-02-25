using System;
using System.Threading.Tasks;
using pattern_wrapping.one;

namespace pattern_wrapping.test.Fakes
{
    public class PrisonerServiceFake : IPrisonerService
    {
        public DateTime LastCallToMethod { get; set; }

        public Task SetFinishDate(DateTime now)
        {
            LastCallToMethod = now;

            return Task.FromResult(0);
        }
    }
}