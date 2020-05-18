using Focus.Service.ReportProcessor.Application.Services;
using Focus.Service.ReportProcessor.Application.Dto;
using Focus.Service.ReportProcessor.Entities;
using Focus.Infrastructure.Common.MongoDB;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using System.Linq;
using MongoDB.Bson;
using System;
using Focus.Service.ReportProcessor.Enums;

namespace Focus.Service.ReportProcessor.Infrastructure.Persistence
{
    public class ReportRepository : IReportRepository
    {
        private readonly IMongoDatabase _database;
        private readonly IMongoConfiguration _configuration;

        public ReportRepository(IMongoConfiguration configuration)
        {
            _configuration = configuration;

            var client = new MongoClient(_configuration.ConnectionString);
            _database = client.GetDatabase(_configuration.Database);
        }

        public IMongoCollection<ReportDocument> Reports => _database.GetCollection<ReportDocument>("reports");

        public async Task CreateReportsAsync(IEnumerable<Report> reports)
        {
            await Reports.InsertManyAsync(reports.Select(x => x.AsDocument()));
        }

        public async Task<IEnumerable<Report>> GetOrganizationReportsAsync(string organizationId)
        {
            return await Reports
                .Find(x => x.AssignedOrganizationId == organizationId)
                .Project(x => x.AsEntity())
                .ToListAsync();
        }

        public async Task<Report> GetReportAsync(string reportId)
        {
            return await Reports
                .Find(x => x.Id == new ObjectId(reportId))
                .Project(x => x.AsEntity())
                .FirstOrDefaultAsync();
        }

        public async Task PassReport(ReportUpdateDto report)
        {
            var doc = await Reports
                .Find(x => x.Id == new ObjectId(report.Id))
                .FirstOrDefaultAsync();

            if (doc is null)
                throw new Exception($"INFRASTRUCTURE: Can't post report with {report.Id} id");

            doc.Status = ReportStatus.Passed;
            doc.QuestionnaireAnswers = report.QuestionnaireAnswers;
            doc.TableAnswers = report.TableAnswers;

            await Reports.FindOneAndReplaceAsync(
                filter: Builders<ReportDocument>.Filter.Eq(x => x.Id, doc.Id),
                replacement: doc);
        }

        public async Task SaveReport(ReportUpdateDto report)
        {
            var doc = await Reports
                .Find(x => x.Id == new ObjectId(report.Id))
                .FirstOrDefaultAsync();

            if (doc is null)
                throw new Exception($"INFRASTRUCTURE: Can't save report with {report.Id} id");

            doc.QuestionnaireAnswers = report.QuestionnaireAnswers;
            doc.TableAnswers = report.TableAnswers;

            await Reports.FindOneAndReplaceAsync(
                filter: Builders<ReportDocument>.Filter.Eq(x => x.Id, doc.Id),
                replacement: doc);
        }

        public async Task<IEnumerable<Report>> GetReportsAsync()
        {
            return await Reports
                .Find(_ => true)
                .Project(x => x.AsEntity())
                .ToListAsync();
        }

        public Task ChangeReportsStatusAsync(IEnumerable<string> overdueReports, ReportStatus overdue)
        {
            var overdueReportIds = overdueReports.Select(r => new ObjectId(r));

            return Reports
                .UpdateManyAsync(
                    Builders<ReportDocument>.Filter.In(r => r.Id, overdueReportIds), 
                    Builders<ReportDocument>.Update.Set(r => r.Status, overdue));
        }
    }
}