using EKTAtestTask.Models;

namespace EKTAtestTask.Services;

public class TemperatureService
{
    private static readonly Random Random = new();
    public static double Generate() => Math.Round(15 + Random.NextDouble() * 90 + 10, 1);
    
    public static void Randomize (IEnumerable<ModuleModel> modules)
    {
        foreach (var module in modules)
        {
            module.Temperature = Generate();
        }
    }
}