using Microsoft.EntityFrameworkCore;
using MyHomeAutomation.WebApi.Models;

namespace MyHomeAutomation.WebApi;

public class MyHomeAutomationDbContext: DbContext
{
    public MyHomeAutomationDbContext(DbContextOptions<MyHomeAutomationDbContext> options) : base(options)
    {
    }

    public DbSet<Location> Locations => Set<Location>();
    public DbSet<Temperature> Temperatures => Set<Temperature>();
    public DbSet<Humidity> Humidity => Set<Humidity>();
    public DbSet<Sensor> Sensors => Set<Sensor>();
    public DbSet<SensorLocation> SensorLocations => Set<SensorLocation>();

    public DbSet<Relay> Relays => Set<Relay>();

    public DbSet<InputRegister> InputRegisters => Set<InputRegister>();
}