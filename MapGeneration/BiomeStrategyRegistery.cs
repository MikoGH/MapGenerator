using MapGenerator.Enums;
using MapGenerator.MapGeneration.Abstractions;
using MapGenerator.MapGeneration.Strategies.Abstractions;

namespace MapGenerator.MapGeneration;

public sealed class BiomeStrategyRegistry : IBiomeStrategyRegistry
{
    private readonly Dictionary<CellBiome, IGenerationStrategy> _byBiome;

    public BiomeStrategyRegistry()
    {
        _byBiome = new();
    }

    public BiomeStrategyRegistry Register(CellBiome biome, IGenerationStrategy strategy)
    {
        if (_byBiome.ContainsKey(biome))
            throw new InvalidOperationException($"Strategy for biome '{biome}' is already registered.");
        _byBiome[biome] = strategy;
        return this;
    }

    public IGenerationStrategy For(CellBiome biome) =>
        _byBiome.TryGetValue(biome, out var s)
            ? s
            : throw new InvalidOperationException($"No strategy for {biome}");
}