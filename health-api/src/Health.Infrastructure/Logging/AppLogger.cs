using Microsoft.Extensions.Logging;

namespace Health.Infrastructure.Logging;

public partial class AppLogger(ILogger<AppLogger> _logger) : IAppLogger
{
    private string Timestamp => DateTime.Now.ToString("dd-MM-yyyy HH:mm");

    public void LogExpurgo(string message)
    {
        // O formato exato solicitado: 25-04-2025 10:56 ------ -------> Mensagem
        LogExpurgoInternal(_logger, Timestamp, message);
    }

    public void LogInfo(string message) => _logger.LogInformation(message);

    public void LogError(string message, Exception? ex = null)
        => _logger.LogError(ex, message);

    // Source Generator para performance extrema
    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "{Timestamp} ------ -------> {Message}")]
    static partial void LogExpurgoInternal(ILogger logger, string timestamp, string message);
}