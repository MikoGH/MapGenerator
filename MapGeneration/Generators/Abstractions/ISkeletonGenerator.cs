using MapGenerator.Models;

namespace MapGenerator.MapGeneration.Generators.Abstractions;

public interface ISkeletonGenerator
{
    public Map GenerateSkeleton(Map map, List<BiomeSeed> biomeSeeds);
}
