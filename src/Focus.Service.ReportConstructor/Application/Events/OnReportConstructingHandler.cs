using Focus.Service.ReportConstructor.Core.Entities.Questionnaire;
using Focus.Service.ReportConstructor.Application.Services;
using Focus.Service.ReportConstructor.Core.Entities;
using Focus.Application.Common.Services.Messaging;
using Focus.Service.ReportConstructor.Core.Enums;
using System.Collections.Generic;
using Focus.Core.Common.Messages;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using MediatR;
using System;
using Focus.Application.Common.Services.Logging;

namespace Focus.Service.ReportConstructor.Application.Events
{
    public class OnReportConstructingHandler : INotificationHandler<OnReportConstructing>
    {
        public readonly IPublisher _publisher;
        public readonly IReportTemplateRepository _repository;
        public readonly ILog _logger;

        public OnReportConstructingHandler(IPublisher publisher, IReportTemplateRepository repository, ILog logger)
        {
            _publisher = publisher;
            _repository = repository;
            _logger = logger;
        }

        public async Task Handle(OnReportConstructing notification, CancellationToken cancellationToken)
        {
            _logger.LogApplication("Received request for constructing reports");

            try
            {
                var published = new OnReportPublishing()
                {
                    Reports = new List<ReportTemplateSeed>()
                };

                var templates = await _repository.GetReportTemplatesAsync();

                foreach (var templateSeed in notification.NewReports)
                {
                    ReportTemplate template = templates.FirstOrDefault(x => x.Id == templateSeed.ReportTemplateId);

                    if (template is null)
                        throw new Exception(
                            $"APPLICATION Can't find report template with id: {templateSeed.ReportTemplateId}");

                    published.Reports.Add(new ReportTemplateSeed()
                    {
                        ReportTemplateId = templateSeed.ReportTemplateId,
                        AssignedOrganizationIds = templateSeed.AssignedOrganizationIds,
                        Deadline = templateSeed.Deadline,
                        // TODO: place this complex conversion to some extensions methods or separated method in handler class
                        Questionnaires = template.GetArray()
                                                .Where(x => x is QuestionnaireModuleTemplate)
                                                .Select(x => x as QuestionnaireModuleTemplate)
                                                .Select(x => new QuestionnaireModuleSeed()
                                                {
                                                    Order = x.Order,
                                                    Sections = x.GetArray()
                                                        .Select(y => new SectionSeed()
                                                        {
                                                            Order = y.Order,
                                                            Questions = y.GetArray()
                                                                .Select(z => new QuestionSeed()
                                                                {
                                                                    Order = z.Order,
                                                                    AnswerType = z.InputType switch
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
                                                                })
                                                                .ToList()
                                                        })
                                                .ToList()
                                                })
                                                .ToList(),
                        // TODO: replace this mock with real implementation
                        Tables = new List<TableModuleSeed>()
                    });
                }

                _publisher.Publish(
                    message: published,
                    exchangeName: "focus",
                    exchangeType: "topic",
                    routeKey: "focus.event.report.publish"
                );

                _logger.LogApplication("Successfully published new reports from Report Constructor");
            }
            catch (Exception ex)
            {
                _logger.LogApplication($"Failed to construct reports due to: \n {ex.Message}");

                throw ex;
            }
        }
    }
}