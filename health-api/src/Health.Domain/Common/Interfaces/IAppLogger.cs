namespace Health.Infrastructure.Logging;

public interface IAppLogger
{
    void LogExpurgo(string message);
    void LogInfo(string message);
    void LogError(string message, Exception? ex = null);
}