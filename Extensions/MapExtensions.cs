using MapGenerator.Enums;
using MapGenerator.Generators;
using MapGenerator.Models;
using MapGenerator.Constants;

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

    public static Map GenerateBiomeSeeds(this Map map, int seedCount, out List<BiomeSeed> biomeSeeds)
    {
        var biomeGenerator = new BiomeGenerator(map);

        biomeSeeds = biomeGenerator.GenerateBiomeSeeds(seedCount);

        return map;
    }

    public static Map GenerateBiomes(this Map map, List<BiomeSeed> biomeSeeds, double jitter)
    {
        var biomeGenerator = new BiomeGenerator(map);

        return biomeGenerator.GenerateBiomes(biomeSeeds, jitter);
    }

    public static void Print(this Map map)
    {
        for (int i = 0; i < map.Height; i++)
        {
            for (int j = 0; j < map.Width; j++)
            {
                Console.BackgroundColor = PrintConstants.cellBiomeToColor[map.Biomes[i, j]];
                if (map.Objects[i, j] != CellObject.None)
                {
                    Console.ForegroundColor = PrintConstants.cellObjectToColor[map.Objects[i, j]];
                }
                else if (map.Marks[i, j] != CellMark.None)
                {
                    Console.ForegroundColor = PrintConstants.cellMarkToColor[map.Marks[i, j]];
                }
                else if (map.Field[i, j] == CellType.Wall)
                {
                    Console.ForegroundColor = PrintConstants.cellBiomeWallToColor[map.Biomes[i, j]];
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                }

                if (map.Objects[i, j] != CellObject.None)
                {
                    Console.Write(PrintConstants.cellObjectToSymbol[map.Objects[i, j]]);
                }
                else if (map.Marks[i, j] != CellMark.None)
                {
                    Console.Write(PrintConstants.cellMarkToSymbol[map.Marks[i, j]]);
                }
                else
                {
                    Console.Write(PrintConstants.cellTypeToSymbol[map.Field[i, j]]);
                }
                Console.ResetColor();
            }
            Console.WriteLine();
        }
    }
}
