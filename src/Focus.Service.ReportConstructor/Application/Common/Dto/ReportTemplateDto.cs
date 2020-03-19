using System.Collections.Generic;
using Focus.Service.ReportConstructor.Domain.Entities.Questionnaires;
using Focus.Service.ReportConstructor.Domain.Entities.Table;

namespace Focus.Service.ReportConstructor.Application.Common.Dto
{
    public class ReportTemplateDto
    {
        // properties
        public string Id { get; set; }
        public string Title { get; set; }
        public ICollection<QuestionnaireModuleTemplate> Questionnaires { get; set; }
        public ICollection<TableModuleTemplate> Tables { get; set; }
    }
}