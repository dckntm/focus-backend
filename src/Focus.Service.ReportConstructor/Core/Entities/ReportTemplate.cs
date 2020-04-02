using System.Collections.Generic;
using Focus.Service.ReportConstructor.Core.Abstract;
using Focus.Service.ReportConstructor.Core.Exceptions;

namespace Focus.Service.ReportConstructor.Core.Entities
{
    public class ReportTemplate
    {
        private ICollection<ModuleTemplate> _modules;

        public ReportTemplate() { }

        public ReportTemplate(ICollection<ModuleTemplate> modules, string id, string title)
        {
            if (modules is null || modules.Count < 1)
                throw new InvalidStructureException("Report Template can't have no modules");

            _modules = modules;
            Id = id;
            Title = title;
        }

        public string Id { get; set; }
        public string Title { get; set; }
        public ICollection<ModuleTemplate> Modules { get => _modules; set => _modules = value; }
    }
}