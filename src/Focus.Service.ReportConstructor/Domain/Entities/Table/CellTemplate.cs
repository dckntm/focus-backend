using Focus.Service.ReportConstructor.Domain.Enums;
using Focus.Service.ReportConstructor.Domain.Exceptions;

namespace Focus.Service.ReportConstructor.Domain.Entities.Table
{
    public class CellTemplate
    {
        // fields
        private int _row;
        private int _column;
        private int _rowSpan;
        private int _columnSpan;
        private string _defaultValue;

        // properties
        public int Row
        {
            get => _row;
            set
            {
                if (value < 1)
                    throw new InvalidStructureException($"Cell can't be in {value} row");

                _row = value;
            }
        }
        public int Column
        {
            get => _column;
            set
            {
                if (value < 1)
                    throw new InvalidStructureException($"Cell can't be in {value} column");

                _column = value;
            }
        }
        public int RowSpan
        {
            get => _rowSpan;
            set
            {
                if (value < 1)
                    throw new InvalidStructureException($"Cell can't have a row span of {value}");

                _rowSpan = value;
            }
        }
        public int ColumnSpan
        {
            get => _columnSpan;
            set
            {
                if (value < 1)
                    throw new InvalidStructureException($"Cell can't have a column span of {value}");

                _columnSpan = value;
            }
        }
        public InputType InputType { get; set; }
        public string DefaultValue
        {
            get => InputType == InputType.Label ? _defaultValue : "";
            set
            {
                if (InputType != InputType.Label)
                    throw new InvalidDefaultValueException("Can't assign default value to non-label cell");

                // TODO:  add default value validation based on InputType

                _defaultValue = value;
            }
        }
    }
}