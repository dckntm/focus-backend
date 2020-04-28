using System.Threading;
using System.Threading.Tasks;
using Focus.Service.ReportProcessor.Application.Dto;
using Focus.Service.ReportProcessor.Application.Services;
using MediatR;

namespace Focus.Service.ReportProcessor.Application.Commands
{
    public class UpdateReport : IRequest
    {
        public UpdateReport(ReportUpdateDto report, bool passed)
        {
            Report = report;
            Posted = passed;
        }
        public ReportUpdateDto Report { get; private set; }
        public bool Posted { get; private set; }
    }

    public class UpdateReportHandler : IRequestHandler<UpdateReport>
    {
        private readonly IReportRepository _repository;

        public UpdateReportHandler(IReportRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(UpdateReport request, CancellationToken cancellationToken)
        {
            if (request.Posted)
                await _repository.PassReport(request.Report);
            else await _repository.SaveReport(request.Report);

            return Unit.Value;
        }
    }
}