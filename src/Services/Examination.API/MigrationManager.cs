using Examination.Infrastructure.MongoDb.SeedWorks;
using Examination.Infrastructure.MongoDb;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Microsoft.Extensions.Hosting;
using Serilog;
using Microsoft.AspNetCore.Hosting;
using System.Reflection;

namespace Examination.API
{
    public static class MigrationManager
    {
        public static WebApplication MigrateDatabase(this WebApplication app)
        {
            string appName = typeof(MigrationManager).Namespace;
            var configuration = GetConfiguration();

            Log.Information("Apply configuration web host ({ApplicationContext})...", appName);
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<ExamMongoDbSeeding>>();
                var settings = services.GetRequiredService<IOptions<ExamSettings>>();
                var mongoClient = services.GetRequiredService<IMongoClient>();
                new ExamMongoDbSeeding()
                    .SeedAsync(mongoClient, settings, logger)
                    .Wait();
            }

            Log.Information("Started web host ({ApplicationContext})...", appName);
            return app;
        }

        static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddUserSecrets(Assembly.GetAssembly(typeof(MigrationManager)));

            var config = builder.Build();
            return builder.Build();
        }

        static Serilog.ILogger CreateSerilogLogger(IConfiguration configuration, string appName)
        {
            return new LoggerConfiguration()
                .MinimumLevel.Information()
                .Enrich.WithProperty("ApplicationContext", appName)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day, shared: true)
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }


    }
}
