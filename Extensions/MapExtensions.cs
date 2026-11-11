using MapGenerator.Generators;
using MapGenerator.Models;

namespace MapGenerator.Extensions;

public static class MapExtensions
{
    public static Map GenerateField(this Map map, int removeEndNodesIterations, int cellularAutomatonIterations)
    {
        var fieldGenerator = new FieldGenerator(map);

        return fieldGenerator.GenerateField(removeEndNodesIterations, cellularAutomatonIterations);
    }

    public static Map GenerateStartAndFinish(this Map map)
    {
        var fieldGenerator = new FieldGenerator(map);

        return fieldGenerator.GenerateStartAndFinish();
    }

    public static Map GenerateBiomes(this Map map, int seedCount, double jitter)
    {
        var biomeGenerator = new BiomeGenerator(map);

        return biomeGenerator.GenerateBiomes(seedCount, jitter);
    }
}
