using System;
using System.Collections.Generic;
using Focus.Service.ReportConstructor.Core.Abstract;
using Focus.Service.ReportConstructor.Core.Exceptions;

namespace Focus.Service.ReportConstructor.Core.Entities
{
    public class ReportTemplate : ListContainer<IModuleTemplate>, ITitled
    {
        private string _title;

        public string Id { get; set; }
        public string Title
        {
            get => _title;
            set
            {
                if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException(
                        "DOMAIN EXCEPTION: Can't assign null, empty or whitespace Section Template Title");

                _title = value;
            }
        }
        public ReportTemplate(
            string id,
            string title,
            IList<IModuleTemplate> modules)
        {
            if (modules is null || modules.Count < 1)
                throw new ArgumentException(
                    "DOMAIN EXCEPTION: Can't instantiate Report Template with null or empty collection of Module Template");

            if (string.IsNullOrEmpty(title) || string.IsNullOrWhiteSpace(title))
                throw new ArgumentException(
                    "DOMAIN EXCEPTION: Can't instantiate Report Template with null, empty or whitespace Title");

            Id = id;
            _title = title;
            _collection = modules;
        }
    }
}