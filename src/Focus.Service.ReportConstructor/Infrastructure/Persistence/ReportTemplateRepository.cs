using System.Linq;
using System.Threading.Tasks;
using Focus.Infrastructure.Common.MongoDB;
using Focus.Service.ReportConstructor.Application.Dto;
using Focus.Service.ReportConstructor.Application.Services;
using Focus.Service.ReportConstructor.Core.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Focus.Service.ReportConstructor.Infrastructure.Persistence
{
    public class ReportTemplateRepository : IReportTemplateRepository
    {
        private readonly IMongoDatabase _database;
        private readonly IMongoConfiguration _configuration;

        public ReportTemplateRepository(IMongoConfiguration configuration)
        {
            _configuration = configuration;

            var client = new MongoClient(_configuration.ConnectionString);
            _database = client.GetDatabase(_configuration.Database);
        }

        private IMongoCollection<ReportTemplateDto> ReportTemplates
            => _database.GetCollection<ReportTemplateDto>("report_templates");

        public async Task<string> CreateReportTemplateAsync(ReportTemplate reportTemplate)
        {
            var document = reportTemplate.AsDto();

            var id = ObjectId.GenerateNewId().ToString();

            document.Id = id;

            await ReportTemplates.InsertOneAsync(document);

            return id;
        }

        public async Task<ReportTemplate> GetReportTemplateAsync(string reportId)
        {
            var document = await ReportTemplates
                .Find(x => x.Id == reportId)
                .SingleAsync();

            return document.AsEntity();
        }

        public async Task<IQueryable<ReportTemplate>> GetReportTemplatesAsync()
        {
            var documents = await ReportTemplates
                .Find(_ => true)
                .ToListAsync();

            return documents
                .Select(x => x.AsEntity())
                .AsQueryable();
        }
    }
}