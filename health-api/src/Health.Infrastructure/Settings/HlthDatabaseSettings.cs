namespace Health.Infrastructure.Settings;

public class HlthDatabaseSettings
{
    public string Host { get; init; } = string.Empty;
    public string Port { get; init; } = "5432";
    public string Name { get; init; } = string.Empty;
    public string User { get; init; } = string.Empty;
    public string Pass { get; init; } = string.Empty;

    public string ConnectionString =>
        $"Host={Host};Port={Port};Database={Name};Username={User};Password={Pass};";
}