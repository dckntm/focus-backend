using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Focus.Application.Common.Services.Client;
using Focus.Service.ReportConstructor.Core.Entities.Questionnaire;
using Common = Focus.Core.Common.Entities.Template;
using Focus.Core.Common.Messages.Commands;
using Focus.Service.ReportConstructor.Application.Services;
using MediatR;
using Focus.Service.ReportConstructor.Core.Enums;

namespace Focus.Service.ReportConstructor.Application.Events
{
    public class ConstructReportsHandler : IRequestHandler<ConstructReports>
    {
        public readonly IServiceClient _service;
        public readonly IReportTemplateRepository _repository;

        public ConstructReportsHandler(
            IReportTemplateRepository repository,
            IServiceClient service)
        {
            _repository = repository;
            _service = service;
        }

        public async Task<Unit> Handle(ConstructReports request, CancellationToken cancellationToken)
        {
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

                    command.ReportDescriptors.Add(new ReportPublishDescriptor()
                    {
                        ConstructionDescriptor = descriptor,
                        Template = new Common.ReportTemplate()
                        {
                            Title = template.Title,
                            Questionnaires = template.GetArray()
                                .Where(m => m is QuestionnaireModuleTemplate)
                                .Select(m => m as QuestionnaireModuleTemplate)
                                .Select(m => new Common.QuestionnaireModuleTemplate()
                                {
                                    Title = m.Title,
                                    Order = m.Order,
                                    Sections = m.GetArray()
                                        .Select(s => new Common.SectionTemplate()
                                        {
                                            Title = s.Title,
                                            Order = s.Order,
                                            Questions = s.GetArray()
                                                .Select(q => new Common.QuestionTemplate()
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
                            Tables = new List<Common.TableModuleTemplate>()
                        }
                    });
                }

                await _service.CommandAsync(command, "processor", "api/cs/report/publish");

            }
            catch (Exception e)
            {
                throw e;
            }

            return Unit.Value;
        }
    }
}
