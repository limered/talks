using System;
using System.Threading.Tasks;

namespace pattern_wrapping.done.one
{
    public interface IPrisonerService
    {
        Task SetFinishDate(DateTime date);
    }

    public class PrisonerService : IPrisonerService
    {
        public Task SetFinishDate(DateTime date)
        {
            return Task.FromResult(0);
        }
    }
}