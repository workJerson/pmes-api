
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using pmes.core;
using pmes.core.Helpers;
using pmes.core.Middlewares;
using pmes.data;
using pmes.entity;
using Serilog;
using System.Reflection;
using System.Text.Json;


//$env:ASPNETCORE_ENVIRONMENT='local'
//Scaffold-dbContext "server=localhost; port=3306; database=pmes; user=root; password=root@2024;Allow User Variables=True;SslMode=Required" Pomelo.EntityFrameworkCore.MySql -OutputDir Entities -ContextDir Context -Context PmesContext -f

StaticLogger.EnsureInitialized();
Log.Information("Server Booting Up...");
try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((_, config) =>
    {
        config.WriteTo.Console()
        .ReadFrom.Configuration(builder.Configuration);
    });

    var config = new ConfigurationBuilder()
        .SetBasePath(builder.Environment.ContentRootPath)
        .AddJsonFile($"appsettings.json", optional: true, reloadOnChange: true)
        .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
        .AddEnvironmentVariables();

    var configuration = config.Build();
    string connectionString = configuration["Database:ConnectionString"]!;
    // Add services to the container.

    builder.Services.AddControllers(options =>
    {
        options
        .Conventions
        .Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));

    }).AddNewtonsoftJson(options => 
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        options.SerializerSettings.ContractResolver  = new CamelCasePropertyNamesContractResolver();
    });

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "1.0",
            Title = "PMES API",
            Description = "Property Management Enterprise Solutions API.",
            Contact = new OpenApiContact
            {
                Name = "PMES",
                Url = new Uri("https://localhost:44347/swagger/index.html")
            },
        });
        options.AddServer(new OpenApiServer
        {
            Url = "https://localhost:44347",
            Description = "PMES Api"
        });

        options.EnableAnnotations();

        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        options.IncludeXmlComments(xmlPath);
    });

    #region Services
    builder.Services.AddCore();
    builder.Services.AddData();
    builder.Services.AddEntity(connectionString);
    #endregion

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (builder.Environment.EnvironmentName == "local")
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.UseCors("allowAll");

    #region Middlewares
    app.UseMiddleware<ExceptionMiddleware>();
    app.UseMiddleware<RequestLoggingMiddleware>();
    app.UseMiddleware<ResponseLoggingMiddleware>();
    #endregion

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    StaticLogger.EnsureInitialized();
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    StaticLogger.EnsureInitialized();
    Log.Information("Server Shutting down...");
    Log.CloseAndFlush();
}
