using System.Collections.Generic;
using Focus.Service.ReportConstructor.Domain.Common.Abstract;
using Focus.Service.ReportConstructor.Domain.Exceptions;

namespace Focus.Service.ReportConstructor.Domain.Entities.Table
{
    public class TableModuleTemplate : ModuleTemplate
    {
        // fields
        private int _rows;
        private int _columns;
        private ICollection<CellTemplate> _cells;

        // properties
        public int Rows
        {
            get => _rows;
            set
            {
                if (value < 1)
                    throw new InvalidStructureException($"Table cant have {value} rows");

                _rows = value;
            }
        }
        public int Columns
        {
            get => _columns;
            set
            {
                if (value < 1)
                    throw new InvalidStructureException($"Table cant have {value} columns");

                _columns = value;
            }
        }
        public ICollection<CellTemplate> Cells
        {
            get => _cells;
            set
            {
                if (value is null || value.Count < 1)
                    throw new InvalidStructureException("Table can't have no cells");

                _cells = value;

                // TODO add auto calculation for rows & cols
            }
        }
    }
}