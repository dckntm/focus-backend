using System.Collections.Generic;
using Focus.Service.ReportConstructor.Domain.Common.Abstract;
using Focus.Service.ReportConstructor.Domain.Exceptions;

namespace Focus.Service.ReportConstructor.Domain.Entities
{
    public class ReportTemplate
    {
        // fields
        private ICollection<ModuleTemplate> _modules;

        // properties
        public string Id { get; set; }
        public string Title { get; set; }
        public ICollection<ModuleTemplate> Modules
        {
            get => _modules;
            set
            {
                if (value is null || value.Count < 1)
                    throw new InvalidStructureException("Report Template can't have no modules");

                _modules = value;
            }
        }

        // TODO : add ctors w\ validation
    }
}