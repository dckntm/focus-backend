using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Focus.Application.Common.Abstract;
using Focus.Application.Common.Messages.Commands;
using Focus.Service.ReportProcessor.Application.Services;
using Focus.Service.ReportProcessor.Entities;
using Focus.Service.ReportProcessor.Entities.Questionnaire;
using Focus.Service.ReportProcessor.Entities.Table;
using Focus.Service.ReportProcessor.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Focus.Service.ReportProcessor.Application.Commands
{
    public class PublishReportsHandler : IRequestHandler<PublishReports, Result>
    {
        private readonly IReportRepository _repository;
        private readonly ILogger<PublishReportsHandler> _logger;
        public PublishReportsHandler(
            IReportRepository repository, ILogger<PublishReportsHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Result> Handle(PublishReports request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Started Publishing Reports");
            try
            {
                var reports = request.ReportDescriptors
                    .SelectMany(d => BuildReports(d));

                _logger.LogInformation($"Publishing total {reports.Count()} reports");

                if (reports != null && reports.Count() > 0)
                    await _repository.CreateReportsAsync(reports);

                return Result.Success();
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return Result.Fail(e);
            }
        }

        // TODO replace with extensions
        private IEnumerable<Report> BuildReports(ReportPublishDescriptor descriptor)
        {
            return descriptor.ConstructionDescriptor.AssignedOrganizationIds
                .Select(org => new Report()
                {
                    Id = "",
                    Title = descriptor.Template.Title,
                    ReportTemplateId = descriptor.ConstructionDescriptor.ReportTemplateId,
                    AssignedOrganizationId = org,
                    Status = ReportStatus.InProgress,
                    Deadline = descriptor.ConstructionDescriptor.DeadlineDate,
                    QuestionnaireAnswers = descriptor.Template.Questionnaires
                        .Select(q => new QuestionnaireModuleAnswer()
                        {
                            Title = q.Title,
                            Order = q.Order,
                            SectionAnswers = q.Sections
                                .Select(s => new SectionAnswer()
                                {
                                    Title = s.Title,
                                    Order = s.Order,
                                    QuestionAnswers = s.Questions
                                        .Select(a => new QuestionAnswer()
                                        {
                                            Answer = "",
                                            Title = a.Title,
                                            Order = a.Order,
                                            AnswerType = a.InputType switch
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
                                                _ => throw new Exception($"APPLICATION Can't convert {a.InputType} to Input Type enum")
                                            }
                                        }).ToList()
                                }).ToList()
                        }).ToList(),
                    TableAnswers = new List<TableModuleAnswer>()
                });
        }
    }
}