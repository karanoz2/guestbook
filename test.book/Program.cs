using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using test.book.BLL.MessagingFeature;
using test.book.dal;
using test.book.dal.health;
using test.book.DAL;
using test.book.DAL.Postgras;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHealthChecks()
        .AddCheck<DbHealthCheck>("database");

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c=> {
    c.CustomSchemaIds(type => type.ToString());
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "guestbook", Version = "v1" });
});

#region DI
builder.Services.AddDAL();
builder.Services.AddScoped<IMessageInsert, MessageMutator>();
builder.Services.AddScoped<IMessageReader, MessageReader>();
#endregion

builder.Services.AddOptions<PostgreSqlOptions>()
    .Bind(builder.Configuration.GetSection("PostgreSql"))
    .ValidateDataAnnotations();

var app = builder.Build();

app.MapHealthChecks("/healthy", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    AllowCachingResponses = false,
    ResultStatusCodes = {
        [HealthStatus.Healthy] = StatusCodes.Status200OK,
        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
    }
});

await app.Services.InitDB();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();
