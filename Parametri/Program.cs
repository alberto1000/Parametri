using Microsoft.AspNetCore.HttpLogging;

IConfiguration configuration = new ConfigurationBuilder()
  .AddJsonFile("appsettings.json")
  .Build();
var builder = WebApplication.CreateBuilder(args);
var miaUrl = configuration.GetSection("UrlToUse").GetRequiredSection("Url").Value;
builder.WebHost.UseUrls(miaUrl);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

Console.WriteLine("Modifica Lolli Alberto");

// Configurazione w3c logger
builder.Services.AddW3CLogging(logging =>
{
    // Log all W3C fields
    logging.LoggingFields = W3CLoggingFields.All;

    logging.FileSizeLimit = 5 * 1024 * 1024;
    logging.RetainedFileCountLimit = 2;
    logging.FileName = "PorcherieLogFile";
    logging.LogDirectory = @"D:\tmp\Parametri\Log";
    logging.FlushInterval = TimeSpan.FromSeconds(2);
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
// Permetto di usare il logger

app.MapControllers();
app.UseW3CLogging();
app.UseRouting();
inspect();
app.Run();

void inspect()
{

    Console.WriteLine("-----------------  uso app.Use ---------------");

    app.Use(async (context, next) =>
    {
        var currentEndpoint = context.GetEndpoint();

        if (currentEndpoint is null)
        {
            await next(context);
            return;
        }


        Console.WriteLine($"Endpoint: {currentEndpoint.DisplayName}");

        if (currentEndpoint is RouteEndpoint routeEndpoint)
        {
            Console.WriteLine($"  - Route Pattern: {routeEndpoint.RoutePattern}");
        }

        foreach (var endpointMetadata in currentEndpoint.Metadata)
        {
            Console.WriteLine($"  - Metadata: {endpointMetadata}");
        }

        await next(context);
    });
    app.MapGet("/", () => "Inspect Endpoint.");
}