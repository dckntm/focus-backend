using System.Collections.Generic;
using System.Linq;
using Focus.Core.Common.Abstract;
using Focus.Service.ReportConstructor.Core.Entities.Table;

namespace Focus.Service.ReportConstructor.Application.Dto
{
    public class TableModuleTemplateDto : ValueObject
    {
        public string Title { get; set; }
        public int Order { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        public CellTemplateDto[] Cells { get; set; }
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Title;
            yield return Order;
            yield return Rows;
            yield return Columns;

            foreach (var cell in Cells)
                yield return cell;
        }
    }

    public static class TableModuleTemplateDtoExtensions
    {
        public static TableModuleTemplate AsEntity(this TableModuleTemplateDto dto)
        {
            return new TableModuleTemplate(
                title: dto.Title,
                cells: dto.Cells
                            .Select(x => x.AsEntity())
                            .ToList(),
                rows: dto.Rows,
                columns: dto.Columns,
                order: dto.Order);
        }

        public static TableModuleTemplateDto AsDto(this TableModuleTemplate entity)
        {
            return new TableModuleTemplateDto
            {
                Title = entity.Title,
                Rows = entity.Rows,
                Columns = entity.Columns,
                Order = entity.Order,
                Cells = entity.Cells
                    .Select(x => x.AsDto())
                    .ToArray()
            };
        }
    }
}