using Microsoft.EntityFrameworkCore;
using MinimalApi.ApiModels;
using MinimalApi.Models;
using MyHomeAutomation.WebApi;
using MyHomeAutomation.WebApi.ApiModels;
using MyHomeAutomation.WebApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(c =>
    {
        c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
    }
);
builder.Services.AddDbContext<MyHomeAutomationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("MyHomeAutomationConn"));
    options.UseLazyLoadingProxies().UseNpgsql();
});

builder.Services.AddScoped<IRelayService, RelayService>();
builder.Services.AddHostedService<RegulatoryService>();
builder.Services.AddHostedService<InterrogationService>();
builder.Services.AddHttpClient();
builder.Services.AddLogging();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(option => option.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

app.MapGet("/temperatures",
    async (CancellationToken cancellationToken, MyHomeAutomationDbContext dbContext) =>
    {
        var result = await dbContext.Temperatures
            .Join(
                dbContext.SensorLocations,
                temperature => temperature.SensorId,
                sensorLocation => sensorLocation.SensorId,
                ((temperature, location) => new TemperatureResponse
                {
                    SensorName = temperature.Sensor.Name,
                    Value = temperature.Value,
                    LocationName = location.Location.Name,
                    Created = temperature.Created
                })
            ).OrderByDescending(t => t.Created).ToListAsync(cancellationToken).ConfigureAwait(false);
        return Results.Ok(result);
    });

app.MapGet("/sensors",
    async (CancellationToken cancellationToken, MyHomeAutomationDbContext dbContext) =>
    {
        var res = new List<SensorResponse>();
        var sensors = await dbContext.Sensors.ToListAsync(cancellationToken);
        foreach (var sensor in sensors)
        {
            var t = await dbContext.Temperatures.Where(t => t.SensorId == sensor.Id).ToListAsync();
            res.Add(new SensorResponse()
            {
                Id = sensor.Id,
                Name = sensor.Name,
                Value = t.MaxBy(t => t.Created).Value,
                Created = t.MaxBy(t => t.Created).Created
            });
        }

        return Results.Ok(res);
    });

app.MapGet("/sensors/{id:int}",
    async (int id, MyHomeAutomationDbContext dbContext) =>
    {
        var sensor = await dbContext.Sensors.FindAsync(id).ConfigureAwait(false);
        return sensor is null ? Results.NotFound() : Results.Ok(sensor);
    });

app.MapGet("/temperatures/{id:int}",
    async (int id, MyHomeAutomationDbContext dbContext) =>
    {
        var location = await dbContext.Locations.FindAsync(id).ConfigureAwait(false);
        return location is null ? Results.NotFound() : Results.Ok(location);
    });

app.MapPost("/temperature",
        async (
            TemperatureRequest requestTemperature,
            MyHomeAutomationDbContext dbContext,
            CancellationToken cancellationToken, ILogger<Program> logger) =>
        {
            //Console.WriteLine($"Input: SensorId:{requestTemperature.SensorId}, Value:{requestTemperature.Value}.");
            logger.LogInformation($"Input: SensorId:{requestTemperature.SensorId}, Value:{requestTemperature.Value}.");

            dbContext.Temperatures.RemoveRange(await dbContext.Temperatures
                .Where(t => t.SensorId.Equals(requestTemperature.SensorId)).ToListAsync().ConfigureAwait(false));

            await dbContext.Temperatures.AddAsync(new Temperature()
            {
                Created = DateTime.UtcNow,
                SensorId = requestTemperature.SensorId,
                Value = decimal.Parse(requestTemperature.Value)
            }, cancellationToken).ConfigureAwait(false);

            await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return Results.NoContent();
        })
    .WithName("PostTemperature");

app.MapGet("/relays",
        async (CancellationToken cancellationToken, MyHomeAutomationDbContext dbContext) =>
            Results.Ok(
                    await dbContext.Relays
                        .OrderBy(r => r.Id)
                        .ToListAsync(cancellationToken)
                        .ConfigureAwait(false)
                ))
    .WithName("GetRelays");

app.MapGet("/relays/{id:int}",
        async (int id, MyHomeAutomationDbContext dbContext) =>
        {
            var result = await dbContext.Relays.FindAsync(id).ConfigureAwait(false);

            return result is null ? Results.NotFound() : Results.Ok(result);
        })
    .WithName("GetRelay");

app.MapPost("/relays",
        async (
            RelayRequest relayRequest,
            MyHomeAutomationDbContext dbContext,
            IRelayService relayService) =>
        {
            var relay = await dbContext.Relays.FindAsync(relayRequest.Id);
            if (relay is null)
            {
                relay = new Relay() {Id = relayRequest.Id, Name = relayRequest.Name};
                await dbContext.Relays.AddAsync(relay).ConfigureAwait(false);
            }

            relay.Active = relayRequest.Active;
            relay.LastUpdate = DateTime.UtcNow;

            await dbContext.SaveChangesAsync();

            await relayService.SetValue(relay.Ip, relay.Active, relay.Type);

            return Results.NoContent();
        })
    .WithName("PostRelays");

app.Run();

public class MyHomeAutomationDbContext : DbContext
{
    public MyHomeAutomationDbContext(DbContextOptions<MyHomeAutomationDbContext> options) : base(options)
    {
    }

    public DbSet<Location> Locations => Set<Location>();
    public DbSet<Temperature> Temperatures => Set<Temperature>();
    public DbSet<Sensor> Sensors => Set<Sensor>();
    public DbSet<SensorLocation> SensorLocations => Set<SensorLocation>();

    public DbSet<Relay> Relays => Set<Relay>();
}