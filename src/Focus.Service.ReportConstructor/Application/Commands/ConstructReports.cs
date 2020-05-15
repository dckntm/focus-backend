using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Focus.Application.Common.Services.Client;
using Focus.Service.ReportConstructor.Application.Services;
using MediatR;
using Focus.Service.ReportConstructor.Core.Enums;
using Focus.Application.Common.Abstract;
using Focus.Application.Common.Messages.Commands;
using Focus.Core.Common.Entities.Template;
using Focus.Service.ReportConstructor.Core.Entities.Questionnaire;
using Microsoft.Extensions.Logging;

namespace Focus.Service.ReportConstructor.Application.Commands
{
    public class ConstructReportsHandler : IRequestHandler<ConstructReports, Result>
    {
        public readonly IServiceClient _service;
        public readonly IReportTemplateRepository _repository;
        private readonly ILogger<ConstructReportsHandler> _logger;

        public ConstructReportsHandler(
            IReportTemplateRepository repository,
            IServiceClient service, ILogger<ConstructReportsHandler> logger)
        {
            _repository = repository;
            _service = service;
            _logger = logger;
        }

        public async Task<Result> Handle(ConstructReports request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Started constructing reports");
            try
            {
                var command = new PublishReports()
                {
                    ReportDescriptors = new List<ReportPublishDescriptor>()
                };

                var templates = await _repository.GetReportTemplatesAsync();

                foreach (var descriptor in request.ReportDescriptors)
                {
                    var template = templates.FirstOrDefault(x => x.Id == descriptor.ReportTemplateId);

                    if (template is null)
                        throw new Exception(
                                $"APPLICATION Can't find report template with id: {descriptor.ReportTemplateId}");

                    _logger.LogInformation($"Constructing report for template: {template.Title}");

                    command.ReportDescriptors.Add(new ReportPublishDescriptor()
                    {
                        ConstructionDescriptor = descriptor,
                        Template = new ReportTemplateDto()
                        {
                            Title = template.Title,
                            Questionnaires = template.GetArray()
                                .Where(m => m is QuestionnaireModuleTemplate)
                                .Select(m => m as QuestionnaireModuleTemplate)
                                .Select(m => new QuestionnaireModuleTemplateDto()
                                {
                                    Title = m.Title,
                                    Order = m.Order,
                                    Sections = m.GetArray()
                                        .Select(s => new SectionTemplateDto()
                                        {
                                            Title = s.Title,
                                            Order = s.Order,
                                            Questions = s.GetArray()
                                                .Select(q => new QuestionTemplateDto()
                                                {
                                                    Title = q.QuestionText,
                                                    Order = q.Order,
                                                    InputType = q.InputType switch
                                                    {
                                                        InputType.ShortText => "ShortText",
                                                        InputType.LongText => "LongText",
                                                        InputType.Email => "Email",
                                                        InputType.PhoneNumber => "PhoneNumber",
                                                        InputType.Label => "Label",
                                                        InputType.Integer => "Integer",
                                                        InputType.Decimal => "Decimal",
                                                        InputType.Financial => "Financial",
                                                        InputType.MultipleChoiceOptionList => "MultipleChoiceOptionList",
                                                        InputType.SingleOptionSelect => "SingOptionSelect",
                                                        InputType.Boolean => "Boolean",
                                                        _ => ""
                                                    }
                                                }).ToList()
                                        }).ToList()
                                }).ToList(),
                            Tables = new List<TableModuleTemplateDto>()
                        }
                    });
                }

                await _service.CommandAsync(command, "processor", "api/cs/report/publish");
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return Result.Fail(e);
            }

            return Result.Success();
        }
    }
}
