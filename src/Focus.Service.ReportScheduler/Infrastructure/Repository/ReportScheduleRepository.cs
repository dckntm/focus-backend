using System;
using System.Linq;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using Focus.Service.ReportScheduler.Application.Common.Interface;
using Focus.Service.ReportScheduler.Domain.Entities;
using Focus.Service.ReportScheduler.Infrastructure.Repository.Documents;
using Focus.Service.ReportScheduler.Infrastructure.Repository.Documents.Extensions;

namespace Focus.Service.ReportScheduler.Infrastructure.Repository
{
    public class ReportScheduleRepository : IReportScheduleRepository
    {
        private readonly IMongoRepository<ReportScheduleDocument, Guid> _repository;

        public ReportScheduleRepository(IMongoRepository<ReportScheduleDocument, Guid> repository)
            => _repository = repository;
        public async Task<string> CreateReportScheduleAsync(ReportSchedule schedule)
        {
            var id = Guid.NewGuid().ToString();

            schedule.Id = id;

            await _repository.AddAsync(schedule.AsDocument());

            return id;
        }
        public async Task<ReportSchedule> GetReportScheduleAsync(string id)
        {
            var document = await _repository.GetAsync(new Guid(id));

            return document.AsEntity();
        }

        public async Task<IQueryable<ReportSchedule>> GetReportSchedulesAsync()
        {
            var documents = await _repository.FindAsync(_ => true);

            return documents
                .Select(d => d.AsEntity())
                .AsQueryable();
        }
    }
}