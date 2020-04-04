using System;
using System.Collections.Generic;
using Focus.Service.ReportConstructor.Core.Abstract;
using Focus.Service.ReportConstructor.Core.Exceptions;

namespace Focus.Service.ReportConstructor.Core.Entities.Table
{
    public class TableModuleTemplate : IModuleTemplate
    {
        private int _rows;
        private int _columns;
        private string _title;
        private int _order;

        public string Title
        {
            get => _title;
            set
            {
                if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException(
                        "DOMAIN EXCEPTION: Can't assign null, empty or whitespace Table Module Template Title");
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
        private readonly ICollection<CellTemplate> _cells;

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
        }
        public int Columns => _columns;
        public ICollection<CellTemplate> Cells => _cells;
    }
}