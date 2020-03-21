using System;
using System.Collections.Generic;
using Focus.Service.ReportConstructor.Domain.Common.Abstract;
using Focus.Service.ReportConstructor.Domain.Entities;
using Focus.Service.ReportConstructor.Domain.Entities.Questionnaires;
using Focus.Service.ReportConstructor.Domain.Entities.Table;
using Focus.Service.ReportConstructor.Infrastructure.Repository.Documents.Extensions;
using Xunit;

namespace Focus.Service.ReportConstructor.Tests.Infrastructure.Repository.Document.Extensions
{
    public class ReportTemplateDocumentExtensionsTests
    {
        [Fact]
        public void To_Document_From_Entity_Without_Questionnaires()
        {
            var entity = new ReportTemplate()
            {
                Id = Guid.NewGuid().ToString(),
                Title = "title",
                Modules = new List<ModuleTemplate>()
                {
                    new TableModuleTemplate(){
                        Cells = new List<CellTemplate>()
                        {
                            new CellTemplate(),
                            new CellTemplate()
                        }
                    }
                }
            };

            var document = entity.AsDocument();

            Assert.Empty(document.Questionnaires);
            Assert.NotNull(document.Questionnaires);
        }

        [Fact]
        public void To_Document_From_Entity_Without_Tables()
        {
            var entity = new ReportTemplate()
            {
                Id = Guid.NewGuid().ToString(),
                Title = "title",
                Modules = new List<ModuleTemplate>()
                {
                    new QuestionnaireModuleTemplate()
                    {
                        Sections = new List<SectionTemplate>()
                        {
                            new SectionTemplate()
                            {
                                Questions = new List<QuestionTemplate>()
                                {
                                    new QuestionTemplate(),
                                    new QuestionTemplate()
                                }
                            }
                        }
                    }
                }
            };

            var document = entity.AsDocument();

            Assert.Empty(document.Tables);
            Assert.NotNull(document.Tables);
        }

        [Fact]
        public void To_Document_From_Entity_Without_Modules()
        {
            var entity = new ReportTemplate()
            {
                Id = Guid.NewGuid().ToString(),
            };

            var document = entity.AsDocument();

            Assert.Empty(document.Tables);
            Assert.NotNull(document.Tables);

            Assert.Empty(document.Questionnaires);
            Assert.NotNull(document.Questionnaires);
        }
    }
}