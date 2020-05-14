using System;

namespace Focus.Application.Common.Abstract
{
    public abstract class Result
    {
        public static Successful Success() => new Successful();
        public static Successful<T> Success<T>(T value) => new Successful<T>() { Value = value };
        public static Failed Fail(Exception exception = null, string message = "")
            => new Failed
            {
                Message = exception?.Message ?? message,
                ThrownException = exception
            };
    }

    public abstract class Result<T> : Result { }

    public class Successful<T> : Result<T>
    {
        public T Value { get; set; }
    }

    public class Successful : Result { }

    public class Failed : Result
    {
        public string Message { get; set; }
        public Exception ThrownException { get; set; }
    }
}