using System;

namespace Focus.Service.ReportConstructor.Domain.Exceptions
{
    public class InvalidDefaultValueException : Exception
    {
        public InvalidDefaultValueException(string message) : base(message) { }
    }
}