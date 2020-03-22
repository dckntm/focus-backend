using System;
using System.Linq;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using Focus.Service.ReportConstructor.Application.Common.Interface;
using Focus.Service.ReportConstructor.Domain.Entities;
using Focus.Service.ReportConstructor.Infrastructure.Exceptions;
using Focus.Service.ReportConstructor.Infrastructure.Repository.Documents;
using Focus.Service.ReportConstructor.Infrastructure.Repository.Documents.Extensions;

namespace Focus.Service.ReportConstructor.Infrastructure.Repository
{
    public class ReportTemplateRepository : IReportTemplateRepository
    {
        private readonly IMongoRepository<ReportTemplateDocument, Guid> _repository;

        public ReportTemplateRepository(IMongoRepository<ReportTemplateDocument, Guid> repository)
            => _repository = repository;

        public Task<string> CreateReportTemplateAsync(ReportTemplate reportTemplate)
        {
            var document = reportTemplate.AsDocument();

            document.Id = Guid.NewGuid();

            return Task.Run(async () =>
            {
                await _repository.AddAsync(document);

                return document.Id.ToString();
            });
        }

        public async Task<ReportTemplate> GetReportTemplateAsync(string id)
        {
            var document = await _repository.GetAsync(x => x.Id == new Guid(id));

            if (document is null)
                throw new ReportTemplateDocumentNotFoundException($"Not found document with ID: {id}");

            return document.AsEntity();
        }

        public async Task<IQueryable<ReportTemplate>> GetReportTemplatesAsync()
        {
            var documents = await _repository.FindAsync(x => true);

            return documents
                .Select(x => x.AsEntity())
                .AsQueryable();
        }
    }
}