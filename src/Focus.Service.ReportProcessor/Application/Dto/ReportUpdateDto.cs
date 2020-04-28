using System.Collections.Generic;
using Focus.Service.ReportProcessor.Entities.Questionnaire;
using Focus.Service.ReportProcessor.Entities.Table;

namespace Focus.Service.ReportProcessor.Application.Dto
{
    public class ReportUpdateDto
    {
        public string Id { get; set; }
        public IList<QuestionnaireModuleAnswer> QuestionnaireAnswers { get; set; }
        public IList<TableModuleAnswer> TableAnswers { get; set; }
    }
}