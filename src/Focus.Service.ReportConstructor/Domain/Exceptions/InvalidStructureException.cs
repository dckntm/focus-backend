using System;

namespace Focus.Service.ReportConstructor.Domain.Exceptions
{
    /// <summary>
    /// Informs that the structure of the element is wrong
    /// </summary>
    public class InvalidStructureException : Exception
    {
        public InvalidStructureException(string message) : base(message) { }
    }
}