using System;
using System.Threading;
using System.Threading.Tasks;
using Focus.Application.Common.Abstract;
using Focus.Service.ReportProcessor.Application.Dto;
using Focus.Service.ReportProcessor.Application.Services;
using MediatR;

namespace Focus.Service.ReportProcessor.Application.Commands
{
    public class UpdateReport : IRequest<Result>
    {
        public UpdateReport(ReportUpdateDto report, bool passed)
        {
            Report = report;
            Posted = passed;
        }
        public ReportUpdateDto Report { get; private set; }
        public bool Posted { get; private set; }
    }

    public class UpdateReportHandler : IRequestHandler<UpdateReport, Result>
    {
        private readonly IReportRepository _repository;
        public UpdateReportHandler(IReportRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result> Handle(UpdateReport request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Posted)
                    await _repository.PassReport(request.Report);
                else await _repository.SaveReport(request.Report);

                return Result.Success();
            }
            catch (Exception e)
            {
                return Result.Fail(e);
            }
        }
    }
}