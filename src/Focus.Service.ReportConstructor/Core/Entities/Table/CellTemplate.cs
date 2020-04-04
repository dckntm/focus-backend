using Focus.Service.ReportConstructor.Core.Abstract;
using Focus.Service.ReportConstructor.Core.Enums;
using Focus.Service.ReportConstructor.Core.Exceptions;

namespace Focus.Service.ReportConstructor.Core.Entities.Table
{
    public class CellTemplate
    {
        // fields
        private int _row;
        private int _column;
        private int _rowSpan;
        private int _columnSpan;
        private string _defaultValue;

        public CellTemplate() { }

        public CellTemplate(int row, int column, int rowSpan, int columnSpan, string defaultValue, InputType inputType)
        {
            // TODO: add validation for position & size of cell 

            _row = row;
            _column = column;
            _rowSpan = rowSpan;
            _columnSpan = columnSpan;
            _defaultValue = defaultValue;
            InputType = inputType;
        }

        // properties
        public int Row
        {
            get => _row;
            set => _row = value;
        }
        public int Column
        {
            get => _column;
            set => _column = value;
        }
        public int RowSpan
        {
            get => _rowSpan;
            set => _rowSpan = value;
        }
        public int ColumnSpan
        {
            get => _columnSpan;
            set => _columnSpan = value;
        }
        public InputType InputType { get; set; }
        public string DefaultValue
        {
            get => InputType == InputType.Label ? _defaultValue : "";
            set => _defaultValue = value;
        }
    }
}