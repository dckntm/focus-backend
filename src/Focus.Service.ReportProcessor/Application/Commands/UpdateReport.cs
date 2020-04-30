using System.Threading;
using System.Threading.Tasks;
using Focus.Application.Common.Services.Logging;
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
        private readonly ILog _logger;
        public UpdateReportHandler(IReportRepository repository, ILog logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateReport request, CancellationToken cancellationToken)
        {
            if (request.Posted)
                await _repository.PassReport(request.Report);
            else await _repository.SaveReport(request.Report);

            _logger.LogApplication($"Successfully {(request.Posted ? "posted" : "saved")} report {request.Report.Id}");

            return Unit.Value;
        }
    }
}