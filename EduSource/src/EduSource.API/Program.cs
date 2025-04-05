using EduSource.API.DepedencyInjection.Extensions;
using EduSource.Application.DependencyInjection.Extensions;
using EduSource.Infrastructure.DependencyInjection.Extensions;
using EduSource.Infrastructure.Services;
using EduSource.Persistence.DependencyInjection.Extensions;
using EduSource.Persistence;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using EduSource.API.Middleware;
using EduSource.Infrastructure.Dapper.DependencyInjection.Extensions;
using EduSource.Persistence.SeedData;
using EduSource.Persistance.DependencyInjection.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddConfigureMediatR();

builder
    .Services
    .AddControllers()
    .AddApplicationPart(EduSource.Presentation.AssemblyReference.Assembly);

builder.Services.AddTransient<ExceptionHandlingMiddleware>();

builder.Services.AddAuthenticationAndAuthorization(builder.Configuration);

builder.Services.AddHttpClient();

// Configure Options and SQL
builder.Services.ConfigureSqlServerRetryOptions(builder.Configuration.GetSection(nameof(SqlServerRetryOptions)));
builder.Services.AddSqlConfiguration();
builder.Services.AddRepositoryBaseConfiguration();

// Configure Dapper
builder.Services.AddInfrastructureDapper();

// Configure Options and Redis
builder.Services.AddConfigurationRedis(builder.Configuration);

builder.Services.AddConfigurationService();

builder.Services.AddConfigurationAppSetting(builder.Configuration);

builder.Services.AddConfigurationAutoMapper();

builder.Services
        .AddSwaggerGenNewtonsoftSupport()
        .AddFluentValidationRulesToSwagger()
        .AddEndpointsApiExplorer()
        .AddSwagger();

builder.Services
    .AddApiVersioning(options => options.ReportApiVersions = true)
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

// Config CORS
builder.Services.AddCors(options =>
{
    var clientUrl = builder.Configuration["ClientConfiguration:Url"];
    options.AddPolicy("AllowSpecificOrigin",
        option =>
        {
            option.WithOrigins("http://localhost:4040", clientUrl)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
        });
});

var app = builder.Build();

// Seed data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();

    SeedData.Seed(context, builder.Configuration, new PasswordHashService());
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseCors("AllowSpecificOrigin");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

//if (builder.Environment.IsDevelopment() || builder.Environment.IsStaging())
app.ConfigureSwagger();

try
{
    await app.RunAsync();
}
catch (Exception ex)
{
    await app.StopAsync();
}
finally
{
    await app.DisposeAsync();
}

public partial class Program { }