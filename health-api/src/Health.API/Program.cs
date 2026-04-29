using System.Text.Json;
using System.Text.Json.Serialization;
using Health.API.Extensions;
using Health.API.Middlewares;
using Health.Application.UseCases.Beneficiarios.CreateBeneficiario;
using Health.Domain.Shared.Abstractions;
using Health.Infrastructure.Adapters.Postgres.Extensions;


var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddKeyPerFile(directoryPath: "/run/secrets", optional: true);
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseLower));
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });
builder.Services.AddSwaggerConfig();
builder.Services.AddPostgresAdapter(builder.Configuration);
var assembly = typeof(CreateBeneficiarioUseCase).Assembly;
var useCaseInterface = typeof(IUseCase<,>);

var registrations = assembly.GetTypes()
    .SelectMany(t => t.GetInterfaces(), (implementation, @interface) => new { implementation, @interface })
    .Where(x => x.@interface.IsGenericType && x.@interface.GetGenericTypeDefinition() == useCaseInterface);

foreach (var reg in registrations)
{
    builder.Services.AddScoped(reg.@interface, reg.implementation);
}

var app = builder.Build();

app.UseSwaggerConfig();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();