using System;
using System.Threading;
using System.Threading.Tasks;

namespace pattern_wrapping.one
{
    public class Executor
    {
        private readonly IPrisonerService _prisonerService;

        public Executor(IPrisonerService prisonerService)
        {
            _prisonerService = prisonerService;
        }

        public async Task Execute(CancellationToken ct)
        {
            await ChopOffHead(ct);
        }

        private async Task ChopOffHead(CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();
            await _prisonerService.SetFinishDate(DateTime.Now);
        }
    }
}