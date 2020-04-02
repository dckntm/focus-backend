using System;

namespace Focus.Service.ReportConstructor.Core.Exceptions
{
    public class InvalidStructureException : Exception
    {
        public InvalidStructureException(string message) : base(message)
        {
        }
    }
}
