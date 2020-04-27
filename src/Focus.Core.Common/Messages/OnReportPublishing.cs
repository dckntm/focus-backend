using System;
using System.Collections.Generic;
using MediatR;

namespace Focus.Core.Common.Messages
{
    public class OnReportPublishing : INotification
    {
        public IList<ReportTemplateSeed> Reports { get; set; }
    }

    public class ReportTemplateSeed
    {
        public string ReportTemplateId { get; set; }
        public IList<string> AssignedOrganizationIds { get; set; }
        public DateTime Deadline { get; set; }
        public IList<QuestionnaireModuleSeed> Questionnaires { get; set; }
        public IList<TableModuleSeed> Tables { get; set; }
    }

    public class QuestionnaireModuleSeed
    {
        public int Order { get; set; }
        public IList<SectionSeed> Sections { get; set; }
    }

    public class TableModuleSeed
    {
        public int Order { get; set; }
        public IList<CellSeed> Cells { get; set; }
    }

    public class SectionSeed
    {
        public int Order { get; set; }
        public bool Repeatable { get; set; }
        public IList<QuestionSeed> Questions { get; set; }
    }

    public class QuestionSeed
    {
        public int Order { get; set; }
        public string AnswerType { get; set; }
    }

    public class CellSeed
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public string AnswerType { get; set; }
    }
}