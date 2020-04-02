using System.Linq;
using System.Threading.Tasks;
using Focus.Infrastructure.Common.Interfaces;
using Focus.Service.ReportConstructor.Application.Services;
using Focus.Service.ReportConstructor.Core.Entities;
using Focus.Service.ReportConstructor.Core.Persistence;
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

        private IMongoCollection<ReportTemplateDocument> ReportTemplates
            => _database.GetCollection<ReportTemplateDocument>("report_templates");

        public async Task<string> CreateReportTemplateAsync(ReportTemplate reportTemplate)
        {
            // to document mapping gaurantess that we generated some id 
            var document = reportTemplate.AsDocument();

            await ReportTemplates.InsertOneAsync(document);

            return document.Id.ToString();
        }

        public async Task<ReportTemplate> GetReportTemplateAsync(string reportId)
        {
            var document = await ReportTemplates
                .Find(x => x.Id == new ObjectId(reportId))
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