using System;
using System.Collections.Generic;
using Focus.Service.ReportConstructor.Core.Abstract;
using Focus.Service.ReportConstructor.Core.Exceptions;

namespace Focus.Service.ReportConstructor.Core.Entities.Table
{
    public class TableModuleTemplate : IModuleTemplate, ITitled, IOrderable
    {
        private int _rows;
        private int _columns;
        private string _title;
        private int _order;
        private ICollection<CellTemplate> _cells;

        public string Title
        {
            get => _title;
            set
            {
                if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException(
                        "DOMAIN EXCEPTION: Can't assign null, empty or whitespace Table Module Template Title");

                _title = value;
            }
        }
        public int Order
        {
            get => _order;
            set
            {
                if (value < 0)
                    throw new ArgumentException(
                        $"DOMAIN EXCEPTION: Can't assign {value} to Table Module Template Order");

                _order = value;
            }
        }

        // TODO: auto calculation of rows & cols of table module in ctor 
        public TableModuleTemplate(
            string title,
            ICollection<CellTemplate> cells,
            int rows,
            int columns,
            int order = 0)
        {
            if (cells is null || cells.Count < 1)
                throw new InvalidStructureException("Can't create table module without columns");

            // TODO: fix validation of title & order without property assignment

            _title = title;
            _order = order;
            _cells = cells;
            _columns = columns;
            _rows = rows;
        }

        public int Rows { get => _rows; set => _rows = value; }
        public ICollection<CellTemplate> Cells { get => _cells; set => _cells = value; }
        public int Columns { get => _columns; set => _columns = value; }
    }
}