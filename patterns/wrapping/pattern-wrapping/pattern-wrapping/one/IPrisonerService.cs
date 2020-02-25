using System;
using System.Threading.Tasks;

namespace pattern_wrapping.one
{
    public interface IPrisonerService
    {
        Task SetFinishDate(DateTime now);
    }

    public class PrisonerService : IPrisonerService
    {
        public Task SetFinishDate(DateTime now)
        {
            throw new NotImplementedException();
        }
    }
}