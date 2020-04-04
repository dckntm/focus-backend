using System;

namespace Focus.Application.Common.Abstract
{
    public class RequestResult<T>
    {
        public T Result { get; set; }
        public bool IsSuccessfull { get; set; }
        public string ErrorMessage { get; set; }
        public Exception ThrownException { get; set; }
        public RequestResult() { }

        public static RequestResult<T> Successfull(T result)
            => new RequestResult<T>()
            {
                Result = result,
                IsSuccessfull = true
            };

        public static RequestResult<T> Failed(Exception exception = null, string errorMessage = "")
            => new RequestResult<T>()
            {
                IsSuccessfull = false,
                ErrorMessage = exception is null ? errorMessage : exception.Message,
                ThrownException = exception
            };
    }

    public class RequestResult
    {
        public static RequestResult<T> Successfull<T>(T Result)
            => new RequestResult<T>()
            {
                Result = Result,
                IsSuccessfull = true
            };
    }

    public static class RequestResultExtensions
    {
        public static RequestResult<T> WithMessage<T>(this RequestResult<T> result, string message)
        {
            result.ErrorMessage = message;

            return result;
        }

        public static RequestResult<T> WithException<T>(this RequestResult<T> result, Exception e)
        {
            result.ErrorMessage = e.Message;
            result.ThrownException = e;

            return result;
        }
    }
}