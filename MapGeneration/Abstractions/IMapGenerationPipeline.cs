using MapGenerator.Models;

namespace MapGenerator.MapGeneration.Abstractions;

public interface IMapGenerationPipeline
{
    public Map Generate();
}
