using System.Collections.Generic;
using Focus.Core.Common.Abstract;
using Focus.Service.ReportConstructor.Core.Entities.Table;
using Focus.Service.ReportConstructor.Core.Enums;

namespace Focus.Service.ReportConstructor.Application.Dto
{
    public class CellTemplateDto : ValueObject
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public int RowSpan { get; set; }
        public int ColumnSpan { get; set; }
        public string DefaultValue { get; set; }
        public InputType InputType { get; set; }
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Row;
            yield return Column;
            yield return RowSpan;
            yield return ColumnSpan;
            yield return DefaultValue;
            yield return InputType;
        }
    }

    public static class CellTemplateDtoExtensions
    {
        public static CellTemplate AsEntity(this CellTemplateDto dto)
        {
            return new CellTemplate(
                row: dto.Row,
                column: dto.Column,
                rowSpan: dto.RowSpan,
                columnSpan: dto.ColumnSpan,
                defaultValue: dto.DefaultValue,
                inputType: dto.InputType);
        }

        public static CellTemplateDto AsDto(this CellTemplate entity)
        {
            return new CellTemplateDto()
            {
                Row = entity.Row,
                Column = entity.Column,
                RowSpan = entity.RowSpan,
                ColumnSpan = entity.ColumnSpan,
                DefaultValue = entity.DefaultValue,
                InputType = entity.InputType
            };
        }
    }
}