using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Focus.Application.Common.Services.Logging;
using Focus.Core.Common.Messages;
using Focus.Service.ReportProcessor.Application.Services;
using Focus.Service.ReportProcessor.Entities;
using Focus.Service.ReportProcessor.Entities.Questionnaire;
using Focus.Service.ReportProcessor.Entities.Table;
using Focus.Service.ReportProcessor.Enums;
using MediatR;

namespace Focus.Service.ReportProcessor.Application.Events
{
    public class OnReportPublishingHandler : INotificationHandler<OnReportPublishing>
    {
        private readonly IReportRepository _repository;
        private readonly ILog _logger;

        public OnReportPublishingHandler(IReportRepository repository, ILog logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task Handle(OnReportPublishing notification, CancellationToken cancellationToken)
        {
            // code responsible for pushing to database new reports

            // build reports 
            var reports = notification.Reports
                            .SelectMany(x => BuildReportsFrom(x));

            // push reports to database
            await _repository.CreateReportsAsync(reports);

            _logger.LogApplication($"Published {reports.Count()} of reports");
        }

        private IEnumerable<Report> BuildReportsFrom(ReportTemplateSeed seed)
        {
            return seed.AssignedOrganizationIds
                .Select(x => new Report()
                {
                    Id = "",
                    Title = seed.Title,
                    ReportTemplateId = seed.ReportTemplateId,
                    AssignedOrganizationId = x,
                    Status = ReportStatus.InProgress,
                    Deadline = seed.Deadline,
                    QuestionnaireAnswers = seed.Questionnaires
                        .Select(q => new QuestionnaireModuleAnswer()
                        {
                            Order = q.Order,
                            SectionAnswers = q.Sections
                                .Select(s => new SectionAnswer()
                                {
                                    Order = s.Order,
                                    // Repeatable = s.Repeatable,
                                    QuestionAnswers = s.Questions
                                        .Select(a => new QuestionAnswer()
                                        {
                                            Answer = "",
                                            Order = a.Order,
                                            AnswerType = a.AnswerType switch
                                            {
                                                "ShortText" => InputType.ShortText,
                                                "LongText" => InputType.LongText,
                                                "Email" => InputType.Email,
                                                "PhoneNumber" => InputType.PhoneNumber,
                                                "Label" => InputType.Label,
                                                "Integer" => InputType.Integer,
                                                "Decimal" => InputType.Decimal,
                                                "Financial" => InputType.Financial,
                                                "MultipleChoiceOptionList" => InputType.MultipleChoiceOptionList,
                                                "SingOptionSelect" => InputType.SingleOptionSelect,
                                                "Boolean" => InputType.Boolean,
                                                _ => throw new Exception($"APPLICATION Can't convert {a.AnswerType} to Input Type enum")
                                            }
                                        }).ToList()
                                }).ToList()
                        }).ToList(),
                    TableAnswers = new List<TableModuleAnswer>()
                });
        }
    }
}