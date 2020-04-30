namespace Focus.Application.Common.Services.Logging
{
    public interface ILog
    {
        void LogApplication(string message);
        void LogInfrastructure(string message);
        void LogApi(string message);
        void LogCore(string message);
    }
}