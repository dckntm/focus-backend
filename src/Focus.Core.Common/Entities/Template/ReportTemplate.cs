using System.Collections.Generic;

namespace Focus.Core.Common.Entities.Template
{
    // TODO rename by adding 'dto' suffix (no type name interference)
    public class ReportTemplateDto
    {
        public string Title { get; set; }
        public IList<QuestionnaireModuleTemplateDto> Questionnaires { get; set; }
        public IList<TableModuleTemplateDto> Tables { get; set; }
    }

    public class TableModuleTemplateDto
    {
        public string Title { get; set; }
        public int Order { get; set; }
        public IList<CellTemplateDto> Cells { get; set; }
    }

    public class CellTemplateDto
    {
        // if input type can be converted to input type, that we will build input cell
        // if it's not than we will build static header cell with static info
        public string InputType { get; set; }
        public int Row { get; set; }
        public string Column { get; set; }
        public string RowSpan { get; set; }
        public string ColumnSpan { get; set; }
    }

    public class QuestionnaireModuleTemplateDto
    {
        public string Title { get; set; }
        public int Order { get; set; }
        public IList<SectionTemplateDto> Sections { get; set; }

    }

    public class SectionTemplateDto
    {
        public string Title { get; set; }
        public int Order { get; set; }
        public IList<QuestionTemplateDto> Questions { get; set; }
    }

    public class QuestionTemplateDto
    {
        public string Title { get; set; }
        public int Order { get; set; }
        public string InputType { get; set; }
    }
}