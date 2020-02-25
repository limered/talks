using System.Threading;
using System.Threading.Tasks;
using pattern_wrapping.one;

namespace pattern_wrapping.one_done
{
    public class Executor
    {
        private readonly IPrisonerService _prisonerService;
        private readonly IDateTime _dateTimeWrapper;

        public Executor(IPrisonerService prisonerService, 
            IDateTime dateTimeWrapper)
        {
            _prisonerService = prisonerService;
            _dateTimeWrapper = dateTimeWrapper;
        }

        public async Task Execute(CancellationToken ct)
        {
            await FinalizeBackup(ct);
        }

        private async Task FinalizeBackup(CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();
            await _prisonerService.SetFinishDate(_dateTimeWrapper.Now());
        }
    }
}