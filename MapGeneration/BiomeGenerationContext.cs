using MapGenerator.Configs;
using MapGenerator.Enums;
using MapGenerator.Models;

namespace MapGenerator.MapGeneration;

public sealed class BiomeGenerationContext
{
    public Map Map { get; }
    public CellBiome Biome { get; }
    public bool[,] Protected { get; }
    public Random Random { get; }
    public BiomeConfig Config { get; }

    public BiomeGenerationContext(
        Map map, CellBiome biome, bool[,] protectedCells,
        Random random, BiomeConfig config)
    {
        Map = map; Biome = biome; Protected = protectedCells;
        Random = random; Config = config;
    }
}