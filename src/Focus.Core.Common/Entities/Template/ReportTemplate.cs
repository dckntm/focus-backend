using System.Collections.Generic;

namespace Focus.Core.Common.Entities.Template
{
    // TODO rename by adding 'dto' suffix (no type name interference)
    public class ReportTemplate
    {
        public string Title { get; set; }
        public IList<QuestionnaireModuleTemplate> Questionnaires { get; set; }
        public IList<TableModuleTemplate> Tables { get; set; }
    }

    public class TableModuleTemplate
    {
        public string Title { get; set; }
        public int Order { get; set; }
        public IList<CellTemplate> Cells { get; set; }
    }

    public class CellTemplate
    {
        // if input type can be converted to input type, that we will build input cell
        // if it's not than we will build static header cell with static info
        public string InputType { get; set; }
        public int Row { get; set; }
        public string Column { get; set; }
        public string RowSpan { get; set; }
        public string ColumnSpan { get; set; }
    }

    public class QuestionnaireModuleTemplate
    {
        public string Title { get; set; }
        public int Order { get; set; }
        public IList<SectionTemplate> Sections { get; set; }

    }

    public class SectionTemplate
    {
        public string Title { get; set; }
        public int Order { get; set; }
        public IList<QuestionTemplate> Questions { get; set; }
    }

    public class QuestionTemplate
    {
        public string Title { get; set; }
        public int Order { get; set; }
        public string InputType { get; set; }
    }
}