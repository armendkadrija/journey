using System.Text.Json;
using Journey.Domain.Entities;
using Journey.Infrastructure.Persistence;

namespace Journey.Infrastructure.Persistence;

public static class ApplicationDbContextSeeder
{
    public static async Task SeedStops(ApplicationDbContext context)
    {
        // Seed, if necessary
        if (!context.Stops.Any())
        {
            try
            {
                var stops = LoadJson<Stop>("Stops.json");
                context.Stops.AddRange(stops);
                await context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return;
            }
        }
    }


    public static List<T> LoadJson<T>(String file)
    {
        using (StreamReader r = new StreamReader(Path.Combine(Environment.CurrentDirectory, $"wwwroot/SeedData/{file}")))
        {
            string json = r.ReadToEnd();
            if (json == null)
                return new List<T>();
            else
                return JsonSerializer.Deserialize<List<T>>(json);
        }
    }
}