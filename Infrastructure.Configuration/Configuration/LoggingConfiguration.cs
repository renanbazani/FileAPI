using Serilog;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Configuration.Configuration
{
    public static class LoggingConfiguration
    {
        public static ILogger CreateLogger(IConfiguration configuration)
        {
            if(configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            string _logPath = configuration["Serilog:filePath"];

            return new LoggerConfiguration()
                .Enrich.WithProperty("solution", configuration["Application:Solution"])
                .Enrich.WithProperty("application", configuration["Application:Name"])
                .ReadFrom.Configuration(configuration)
                .WriteTo.Console()                
                .WriteTo.File(_logPath, rollingInterval: RollingInterval.Day, outputTemplate: configuration["Serilog:outputTemplate"])
                .CreateLogger();
        }
    }
}
