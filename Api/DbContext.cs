using Microsoft.EntityFrameworkCore;
using System.Text.Json;

public class Measure
{
    public int Id { get; set; }
    public string Name { get; set; }=string.Empty;
    public float Value { get; set; }
    // Other properties...
}

public class DatabaseContext : DbContext
{
    public DatabaseContext(string dbName) : base()
    {
        DatabaseName = dbName;
    }    public DbSet<Measure> Measurements { get; set; }
    public string DatabaseName { get; set; } = "../data/Measurement.sqlite";

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if(!File.Exists(DatabaseName))
        {
            Console.WriteLine("Database not found:{0}", Path.GetFullPath(DatabaseName));
            Environment.Exit(1);            
        }
        optionsBuilder.UseSqlite($"Data Source={DatabaseName}");
    }
}