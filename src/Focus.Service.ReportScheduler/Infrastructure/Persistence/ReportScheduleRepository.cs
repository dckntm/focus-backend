using System.Linq;
using System.Threading.Tasks;
using Focus.Infrastructure.Common.MongoDB;
using Focus.Service.ReportScheduler.Application.Services;
using Focus.Service.ReportScheduler.Core.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Focus.Service.ReportScheduler.Infrastructure.Persistence
{
    public class ReportScheduleRepository : IReportScheduleRepository
    {
        private readonly IMongoDatabase _database;
        private readonly IMongoConfiguration _configuration;

        public ReportScheduleRepository(IMongoConfiguration configuration)
        {
            _configuration = configuration;

            var client = new MongoClient(_configuration.ConnectionString);
            _database = client.GetDatabase(_configuration.Database);
        }

        private IMongoCollection<ReportScheduleDocument> ReportSchedules
            => _database.GetCollection<ReportScheduleDocument>("report_schedules");

        public async Task<string> CreateReportScheduleAsync(ReportSchedule schedule)
        {
            // when we convert entity -> document we guarantee the Id property is properly created
            var document = schedule.AsDocument();

            var id = document.Id;

            await ReportSchedules.InsertOneAsync(document);

            return id.ToString();
        }

        public Task DeleteSchedulesAsync(IQueryable<string> outdated)
        {
            return ReportSchedules
                .DeleteManyAsync(s => outdated.Contains(s.Id.ToString()));
        }

        public async Task<ReportSchedule> GetReportScheduleAsync(string scheduleId)
        {
            var document = await ReportSchedules
                .Find(d => d.Id == new ObjectId(scheduleId))
                .SingleAsync();

            return document.AsEntity();
        }

        public async Task<IQueryable<ReportSchedule>> GetReportSchedulesAsync()
        {
            var documents = await ReportSchedules
                .Find(_ => true)
                .ToListAsync();

            return documents
                .Select(x => x.AsEntity())
                .AsQueryable();
        }
    }
}