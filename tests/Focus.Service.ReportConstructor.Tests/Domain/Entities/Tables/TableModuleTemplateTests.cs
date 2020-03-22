using System.Collections.Generic;
using Focus.Service.ReportConstructor.Domain.Entities.Table;
using Focus.Service.ReportConstructor.Domain.Exceptions;
using Xunit;

namespace Focus.Service.ReportConstructor.Tests.Domain.Entities.Tables
{
    public class TableModuleTemplateTests
    {
        [Fact]
        public void Table_Row_Set_Throws_Exception_For_Non_Positive_Value()
        {
            var table = new TableModuleTemplate();

            Assert.Throws<InvalidStructureException>(() => table.Rows = 0);
            Assert.Throws<InvalidStructureException>(() => table.Rows = -10);
        }

        [Fact]
        public void Table_Column_Set_Throws_Exception_For_Non_Positive_Value()
        {
            var table = new TableModuleTemplate();

            Assert.Throws<InvalidStructureException>(() => table.Columns = 0);
            Assert.Throws<InvalidStructureException>(() => table.Columns = -10);
        }

        [Fact]
        public void Table_Cells_Set_Throws_Exception_For_Null_Or_Empty_Value()
        {
            var table = new TableModuleTemplate();
            var emptyCellList = new List<CellTemplate>();

            Assert.Throws<InvalidStructureException>(() => table.Cells = null);
            Assert.Throws<InvalidStructureException>(() => table.Cells = emptyCellList);
        }
    }
}