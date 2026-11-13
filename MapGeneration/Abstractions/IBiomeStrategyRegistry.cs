using MapGenerator.Enums;
using MapGenerator.MapGeneration.Strategies.Abstractions;

namespace MapGenerator.MapGeneration.Abstractions;

public interface IBiomeStrategyRegistry
{
    public BiomeStrategyRegistry Register(CellBiome biome, IGenerationStrategy strategy);

    public IGenerationStrategy For(CellBiome biome);
}
