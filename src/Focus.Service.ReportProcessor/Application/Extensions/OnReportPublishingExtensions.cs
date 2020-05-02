using System;
using System.Collections.Generic;
using System.Linq;
using Focus.Core.Common.Messages;
using Focus.Service.ReportProcessor.Entities;
using Focus.Service.ReportProcessor.Entities.Questionnaire;
using Focus.Service.ReportProcessor.Entities.Table;
using Focus.Service.ReportProcessor.Enums;

namespace Focus.Service.ReportProcessor.Application.Extensions
{
    public static class OnReportPublishingExtensions
    {
        public static Report AsEntity(this ReportTemplateSeed seed, string organizationId)
        {
            return new Report()
            {
                Id = "",
                Title = seed.Title,
                ReportTemplateId = seed.ReportTemplateId,
                AssignedOrganizationId = organizationId,
                Status = ReportStatus.InProgress,
                Deadline = seed.Deadline,
                QuestionnaireAnswers = seed.Questionnaires
                    .Select(q => q.AsEntity()).ToList(),
                TableAnswers = new List<TableModuleAnswer>()
            };
        }

        public static QuestionnaireModuleAnswer AsEntity(this QuestionnaireModuleSeed seed)
        {
            return new QuestionnaireModuleAnswer()
            {
                Title = seed.Title,
                Order = seed.Order,
                SectionAnswers = seed.Sections
                    .Select(s => s.AsEntity())
                    .ToList()
            };
        }

        public static SectionAnswer AsEntity(this SectionSeed seed)
        {
            return new SectionAnswer()
            {
                Title = seed.Title,
                Order = seed.Order,
                // Repeatable = s.Repeatable,
                QuestionAnswers = seed.Questions
                    .Select(a => a.AsEntity())
                    .ToList()
            };
        }

        public static QuestionAnswer AsEntity(this QuestionSeed seed)
        {
            return new QuestionAnswer
            {
                Answer = "",
                Order = seed.Order,
                Title = seed.Title,
                AnswerType = seed.AnswerType.AsInputType()
            };
        }

        public static InputType AsInputType(this string type)
        {
            return type switch
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
                _ => throw new Exception($"APPLICATION Can't convert {type} to Input Type enum")
            };
        }
    }
}