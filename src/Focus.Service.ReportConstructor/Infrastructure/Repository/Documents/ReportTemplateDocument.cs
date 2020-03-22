using System;
using System.Collections.Generic;
using Convey.Types;
using Focus.Service.ReportConstructor.Domain.Entities.Questionnaires;
using Focus.Service.ReportConstructor.Domain.Entities.Table;

namespace Focus.Service.ReportConstructor.Infrastructure.Repository.Documents
{
    public class ReportTemplateDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public ICollection<QuestionnaireModuleTemplate> Questionnaires { get; set; }
        public ICollection<TableModuleTemplate> Tables { get; set; }
    }
}