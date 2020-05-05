using System;

namespace Focus.Application.Common.Abstract
{
    public abstract class Result
    {
        public static Successfull Success() => new Successfull();
        public static Successfull<T> Success<T>(T value) => new Successfull<T>() { Value = value };
        public static Failed Fail(string message = "", Exception exception = null) 
            => new Failed 
            { 
                Message = exception?.Message ?? message,
                ThrownException = exception  
            };
    }

    public class Successfull<T> : Result
    {
        public T Value { get; set; }
    }

    public class Successfull : Result { }

    public class Failed : Result
    {
        public string Message { get; set; }
        public Exception ThrownException { get; set; }
    }
}