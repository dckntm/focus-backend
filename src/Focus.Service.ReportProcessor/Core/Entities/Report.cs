using System;
using System.Collections.Generic;
using Focus.Core.Common.Abstract;
using Focus.Service.ReportProcessor.Entities.Questionnaire;
using Focus.Service.ReportProcessor.Entities.Table;
using Focus.Service.ReportProcessor.Enums;

namespace Focus.Service.ReportProcessor.Entities
{
    public class Report : ValueObject
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string ReportTemplateId { get; set; }
        public string AssignedOrganizationId { get; set; }
        public ReportStatus Status { get; set; }
        public DateTime Deadline { get; set; }
        public IList<QuestionnaireModuleAnswer> QuestionnaireAnswers { get; set; }
        public IList<TableModuleAnswer> TableAnswers { get; set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Id;
            foreach (var q in QuestionnaireAnswers)
                yield return q;

            foreach (var t in TableAnswers)
                yield return t;
        }
    }
}