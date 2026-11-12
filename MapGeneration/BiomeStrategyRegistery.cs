using MapGenerator.Enums;
using MapGenerator.MapGeneration.Strategies.Abstractions;

namespace MapGenerator.MapGeneration;

public sealed class BiomeStrategyRegistry
{
    //private readonly Dictionary<CellBiome, IGenerationStrategy> _byBiome;

    //public BiomeStrategyRegistry(IEnumerable<IGenerationStrategy> strategies)
    //{
    //    _byBiome = strategies.ToDictionary(s => s.SupportedBiome);
    //}

    //public IGenerationStrategy For(CellBiome biome) =>
    //    _byBiome.TryGetValue(biome, out var s)
    //        ? s
    //        : throw new InvalidOperationException($"No strategy for {biome}");
}