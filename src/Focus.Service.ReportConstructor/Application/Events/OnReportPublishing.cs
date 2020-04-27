using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Focus.Application.Common.Services.Messaging;
using Focus.Core.Common.Messages;
using Focus.Service.ReportConstructor.Application.Services;
using Focus.Service.ReportConstructor.Core.Entities;
using Focus.Service.ReportConstructor.Core.Entities.Questionnaire;
using Focus.Service.ReportConstructor.Core.Enums;
using MediatR;

namespace Focus.Service.ReportConstructor.Application.Events
{
    public class OnReportPublishingHandler : INotificationHandler<OnReportPublishing>
    {
        public readonly IPublisher _publisher;
        public readonly IReportTemplateRepository _repository;

        public OnReportPublishingHandler(IPublisher publisher, IReportTemplateRepository repository)
        {
            _publisher = publisher;
            _repository = repository;
        }

        public async Task Handle(OnReportPublishing notification, CancellationToken cancellationToken)
        {
            try
            {
                var published = new OnReportPublishing()
                {
                    Reports = new List<ReportTemplateSeed>()
                };

                var templates = await _repository.GetReportTemplatesAsync();

                foreach (var templateSeed in notification.Reports)
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
                    routeKey: "focus.report.publishing"
                );
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}