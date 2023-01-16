using Infrastructure.Configuration.Configuration;
using Infrastructure.IOC.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options => { options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true; });
builder.Services.AddCors();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHealthChecks();
builder.Services.AddBaseServices(builder.Configuration);
builder.Services.AddCustomServices(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.ConfigureRequestLocation();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureSwagger();

var logger = LoggingConfiguration.CreateLogger(builder.Configuration);
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);
builder.Services.AddSingleton(logger);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
