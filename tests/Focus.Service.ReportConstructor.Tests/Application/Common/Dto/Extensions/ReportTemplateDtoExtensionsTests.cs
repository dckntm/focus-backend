using System;
using System.Collections.Generic;
using Focus.Service.ReportConstructor.Application.Common.Dto;
using Focus.Service.ReportConstructor.Application.Common.Dto.Extensions;
using Focus.Service.ReportConstructor.Domain.Common.Abstract;
using Focus.Service.ReportConstructor.Domain.Entities;
using Focus.Service.ReportConstructor.Domain.Entities.Questionnaires;
using Focus.Service.ReportConstructor.Domain.Entities.Table;
using Xunit;

namespace Focus.Service.ReportConstructor.Tests.Application.Common.Dto.Extensions
{
    public class ReportTemplateDtoExtensionsTests
    {
        [Fact]
        public void To_Dto_With_Null_Modules()
        {
            var template = new ReportTemplate();

            var dto = template.AsDto();

            Assert.Empty(dto.Questionnaires);
            Assert.NotNull(dto.Questionnaires);

            Assert.Empty(dto.Tables);
            Assert.NotNull(dto.Tables);
        }

        [Fact]
        public void To_Dto_From_Entity_Without_Tables()
        {
            var entity = new ReportTemplate()
            {
                Id = "id",
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

            var dto = entity.AsDto();

            Assert.NotNull(dto.Tables);
            Assert.Empty(dto.Tables);
        }

        [Fact]
        public void To_Dto_From_Entity_Without_Questionnaires()
        {
            var entity = new ReportTemplate()
            {
                Id = "id",
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

            var dto = entity.AsDto();

            Assert.NotNull(dto.Questionnaires);
            Assert.Empty(dto.Questionnaires);
        }

        [Fact]
        public void To_Entity_With_Null_Questionnaires_Tables_Throws_Exception()
        {
            var dto = new ReportTemplateDto()
            {
                Id = "id",
                Title = "title",
                Questionnaires = null,
                Tables = null
            };

            Assert.Throws<ArgumentNullException>(() => dto.AsEntity());
        }
    }
}