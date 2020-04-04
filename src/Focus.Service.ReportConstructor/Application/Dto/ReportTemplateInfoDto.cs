using System.Collections.Generic;
using Focus.Core.Common.Abstract;
using Focus.Service.ReportConstructor.Core.Entities;

namespace Focus.Service.ReportConstructor.Application.Dto
{
    public class ReportTemplateInfoDto : ValueObject
    {
        public string Id { get; set; }
        public string Title { get; set; }
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Id;
            yield return Title;
        }
    }

    public static class ReportTemplateInfoDtoExtensions
    {
        public static ReportTemplateInfoDto AsInfoDto(this ReportTemplate entity)
            => new ReportTemplateInfoDto()
            {
                Id = entity.Id,
                Title = entity.Title
            };
    }
}