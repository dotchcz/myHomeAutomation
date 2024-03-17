using Microsoft.EntityFrameworkCore;
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
builder.Services.AddScoped<IInputRegisterService, InputRegisterService>();
builder.Services.AddHostedService<RegulatoryService>();
builder.Services.AddHostedService<InterrogationService>();
builder.Services.AddHostedService<BoilerControlService>();
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

app.MapGet("/sensors",
    async (CancellationToken cancellationToken, MyHomeAutomationDbContext dbContext) =>
    {
        var res = new List<SensorResponse>();
        var sensors = await dbContext
            .Sensors
            .OrderBy(s => s.Id)
            .ToListAsync(cancellationToken);

        foreach (var sensor in sensors)
        {
            var t = await dbContext.Temperatures.Where(t => t.SensorId == sensor.Id).OrderBy(tt => tt.Created)
                .FirstOrDefaultAsync();
            var h = await dbContext.Humidity.Where(h => h.SensorId == sensor.Id).OrderBy(hh => hh.Created)
                .FirstOrDefaultAsync();
            res.Add(new SensorResponse()
            {
                Id = sensor.Id,
                Name = sensor.Name,
                ValueTemp = t?.Value ?? 0,
                CreatedTemp = t?.Created,
                ValueHumidity = h?.Value ?? 0,
                CreatedHumidity = h?.Created
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
            SensorRequest requestTemperature,
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

app.MapPost("/humidity",
        async (
            SensorRequest sensorRequest,
            MyHomeAutomationDbContext dbContext,
            CancellationToken cancellationToken, ILogger<Program> logger) =>
        {
            logger.LogInformation($"Input: SensorId:{sensorRequest.SensorId}, Value:{sensorRequest.Value}.");

            dbContext.Humidity.RemoveRange(await dbContext.Humidity
                .Where(t => t.SensorId.Equals(sensorRequest.SensorId)).ToListAsync().ConfigureAwait(false));

            await dbContext.Humidity.AddAsync(new Humidity
            {
                Created = DateTime.UtcNow,
                SensorId = sensorRequest.SensorId,
                Value = decimal.Parse(sensorRequest.Value)
            }, cancellationToken).ConfigureAwait(false);

            await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return Results.NoContent();
        })
    .WithName("PostHumidity");

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

app.MapPost("/inputRegister",
        async (
            ReadInputRegisterRequest readInputRegister,
            IInputRegisterService inputRegisterService) =>
        {
            Console.WriteLine(
                $"Přijato {readInputRegister.Values.Length} hodnot registrů, Offset {readInputRegister.Offset}");
            await inputRegisterService.Update(readInputRegister.Offset, readInputRegister);

            return Results.NoContent();
        })
    .WithName("InputRegister");

app.MapGet("/photovoltaics", async (IInputRegisterService inputRegisterService) =>
{
    var photovoltaics = await inputRegisterService.GetPhotovoltaics();
    return Results.Ok(photovoltaics);
}).WithName("Photovoltaics");


app.Run();