using Infrastructure.Configuration.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Service.Interface;
using Service.Services;
using System.Globalization;

namespace Infrastructure.IOC.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBaseServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient();

            services.BindConfigurations(configuration);

            services.Configure<IISServerOptions>(options =>
            {
                options.MaxRequestBodySize = long.MaxValue;
                options.MaxRequestBodyBufferSize = int.MaxValue;
            });

            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = long.MaxValue;
                x.MultipartBoundaryLengthLimit = int.MaxValue;
                x.MultipartHeadersCountLimit = int.MaxValue;
                x.MultipartHeadersLengthLimit = int.MaxValue;
                x.BufferBodyLengthLimit = long.MaxValue;
                x.BufferBody = true;
                x.ValueCountLimit = int.MaxValue;
            });

            return services;
        }

        public static IServiceCollection AddCustomServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IFileService, FileService>();

            return services;
        }

        public static IServiceCollection ConfigureRequestLocation(this IServiceCollection services)
        {
            return services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("pt-BR");
                options.SupportedCultures = new List<CultureInfo> { new CultureInfo("pt-BR") };
            });
        }

        public static IServiceCollection ConfigureSwagger(this IServiceCollection service)
        {
            return service.AddSwaggerGen(_ =>
            {
                _.SwaggerDoc(ApplicationConfiguration.Current?.Version, new OpenApiInfo
                {
                    Title = ApplicationConfiguration.Current?.Name,
                    Version = ApplicationConfiguration.Current?.Version
                });
            });
        }

        #region private
        private static IServiceCollection BindConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            var applicationConfiguration = new ApplicationConfiguration();
            configuration.Bind("Application", applicationConfiguration);
            services.AddSingleton(applicationConfiguration);
            return services;
        }
        #endregion
    }
}
