using System;

namespace Focus.Service.ReportConstructor.Infrastructure.Exceptions
{
    public class ReportTemplateDocumentNotFoundException : Exception
    {
        public ReportTemplateDocumentNotFoundException(string message) : base(message) { }
    }
}