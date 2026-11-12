using MapGenerator.Enums;
using MapGenerator.Models;

namespace MapGenerator.MapGeneration.Strategies.Abstractions;

public interface IGenerationStrategy
{
    //public CellBiome SupportedBiome { get; }

    public Map Generate(BiomeGenerationContext context);
}
