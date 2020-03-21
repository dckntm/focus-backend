using Focus.Service.ReportConstructor.Domain.Entities.Table;
using Focus.Service.ReportConstructor.Domain.Exceptions;
using Xunit;

namespace Focus.Service.ReportConstructor.Tests.Domain.Entities.Tables
{
    public class CellTemplateTests
    {

        [Fact]
        public void Cell_Row_Set_Throws_Exception_For_Non_Positive_Value()
        {
            var cell = new CellTemplate();

            Assert.Throws<InvalidStructureException>(() => cell.Row = -10);
            Assert.Throws<InvalidStructureException>(() => cell.Row = 0);
        }

        [Fact]
        public void Cell_Column_Set_Throws_Exception_For_Non_Positive_Value()
        {
            var cell = new CellTemplate();

            Assert.Throws<InvalidStructureException>(() => cell.Column = -10);
            Assert.Throws<InvalidStructureException>(() => cell.Column = 0);
        }

        [Fact]
        public void Cell_RowSpan_Set_Throws_Exception_For_Non_Positive_Value()
        {
            var cell = new CellTemplate();

            Assert.Throws<InvalidStructureException>(() => cell.RowSpan = -10);
            Assert.Throws<InvalidStructureException>(() => cell.RowSpan = 0);
        }

        [Fact]
        public void Cell_ColumnSpan_Set_Throws_Exception_For_Non_Positive_Value()
        {
            var cell = new CellTemplate();

            Assert.Throws<InvalidStructureException>(() => cell.ColumnSpan = -10);
            Assert.Throws<InvalidStructureException>(() => cell.ColumnSpan = 0);
        }

        [Fact]
        public void Cell_Default_Value_Set_Throws_Error_For_Non_Label_Cell_Input_Type()
        {
            var cell = new CellTemplate();

            Assert.Throws<InvalidDefaultValueException>(() => cell.DefaultValue = "value");
        }
    }
}