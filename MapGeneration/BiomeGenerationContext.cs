using MapGenerator.Configs;
using MapGenerator.Enums;
using MapGenerator.Models;

namespace MapGenerator.MapGeneration;

public sealed class BiomeGenerationContext
{
    public Map Map { get; }
    public CellBiome Biome { get; }
    public BiomeConfig Config { get; }
    public List<Cell> BiomeSeeds { get; }
    public List<Cell> BiomeCells { get; }

    public BiomeGenerationContext(Map map, CellBiome biome, BiomeConfig config, List<Cell> biomeSeeds, List<Cell> biomeCells)
    {
        Map = map;
        Biome = biome;
        Config = config;
        BiomeSeeds = biomeSeeds;
        BiomeCells = biomeCells;
    }
}