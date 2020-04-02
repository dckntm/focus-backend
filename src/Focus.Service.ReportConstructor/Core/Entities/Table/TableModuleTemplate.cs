using System.Collections.Generic;
using Focus.Service.ReportConstructor.Core.Abstract;
using Focus.Service.ReportConstructor.Core.Exceptions;

namespace Focus.Service.ReportConstructor.Core.Entities.Table
{
    public class TableModuleTemplate : ModuleTemplate
    {
        private int _rows;
        private int _columns;
        private readonly ICollection<CellTemplate> _cells;

        // TODO: auto calculation of rows & cols of table module in ctor 
        public TableModuleTemplate(
            string title,
            int order,
            ICollection<CellTemplate> cells,
            int rows,
            int columns)
        {
            if (cells is null || cells.Count < 1)
                throw new InvalidStructureException("Can't create table module without columns");

            Title = title;
            Order = order;
            _cells = cells;
            _columns = columns;
            _rows = rows;
        }

        public TableModuleTemplate() { }

        // TODO: add domain rules for changin rows & cols

        public int Rows
        {
            get => _rows;
            // set
            // {
            //     if (value < 1)
            //         throw new InvalidStructureException($"Table can't have {value} rows");

            //     _rows = value;
            // }
        }
        public int Columns
        {
            get => _columns;
            // set
            // {
            //     if (value < 1)
            //         throw new InvalidStructureException($"Table can't have {value} cols");

            //     _columns = value;
            // }

        }
        public ICollection<CellTemplate> Cells => _cells;
    }
}