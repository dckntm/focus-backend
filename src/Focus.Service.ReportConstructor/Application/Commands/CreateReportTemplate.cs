using System;
using System.Threading;
using System.Threading.Tasks;
using Focus.Application.Common.Abstract;
using Focus.Service.ReportConstructor.Application.Dto;
using Focus.Service.ReportConstructor.Application.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Focus.Service.ReportConstructor.Application.Commands
{
    public class CreateReportTemplate : IRequest<Result>
    {
        public CreateReportTemplate(ReportTemplateDto reportTemplate)
            => ReportTemplate = reportTemplate;
        public ReportTemplateDto ReportTemplate { get; private set; }
    }

    public class CreateReportTemplateHandler : IRequestHandler<CreateReportTemplate, Result>
    {
        private readonly ILogger<CreateReportTemplateHandler> _logger;
        private readonly IReportTemplateRepository _repository;
        public CreateReportTemplateHandler(IReportTemplateRepository repository, ILogger<CreateReportTemplateHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Result> Handle(CreateReportTemplate request, CancellationToken cancellationToken)
        {
            // _logger.LogInformation($"{JsonConvert.SerializeObject(request.)}")

            _logger.LogInformation($"template: {request.ReportTemplate is null}");
            _logger.LogInformation($"title: {request.ReportTemplate.Title is null}");
            _logger.LogInformation($"questionnaires: {request.ReportTemplate.Questionnaires is null}");

            // foreach (var q in request.ReportTemplate?.Questionnaires)
            // {
            //     _logger.LogInformation($"{q.Order}. {q.Title} : {q.Sections}");

            //     foreach (var s in q.Sections)
            //     {
            //         _logger.LogInformation($"{s.Order}. {s.Title} : {s.Questions}");

            //         foreach (var qq in s.Questions)
            //             _logger.LogInformation($"{qq.Order}. {qq.QuestionText} : {qq.InputType}");
            //     }
            // }

            _logger.LogInformation($"tables: {request.ReportTemplate.Tables is null}");

            try
            {
                var id = await _repository.CreateReportTemplateAsync(request.ReportTemplate.AsEntity());

                return Result.Success(id);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return Result.Fail(e);
            }
        }
    }
}