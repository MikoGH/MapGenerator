using MapGenerator.Configs;
using MapGenerator.Enums;
using MapGenerator.Extensions;
using MapGenerator.MapGeneration.Generators.Abstractions;
using MapGenerator.Models;
using Microsoft.Extensions.Options;
using Prism.Core.Tests.Helpers;

namespace MapGenerator.MapGeneration.Generators;

public class BiomeGenerator : IBiomeGenerator
{
    private readonly Random _random;

    public BiomeGenerator(IOptions<MapGenerationConfig> config)
    {
        _random = new Random(config.Value.Seed);
    }

    public List<BiomeSeed> GenerateBiomeSeeds(Map map, int seedCount)
    {
        var seeds = new List<BiomeSeed>(seedCount);

        for (int i = 0; i < seedCount; i++)
        {
            var cell = RandomGenerator.GenerateRandomPoint(_random, 1, map.Width - 2, 1, map.Height - 2);
            CellBiome biome;
            do
            {
                biome = RandomGenerator.GenerateEnumValue<CellBiome>(_random).Value;
            } while (biome == CellBiome.None);
            seeds.Add(new(cell, biome));
        }

        return seeds;
    }

    public Map GenerateBiomes(Map map, List<BiomeSeed> biomeSeeds, double jitter)
    {
        var jitterMap = new double[map.Height, map.Width];

        var cellsQueue = new PriorityQueue<(Cell, CellBiome), double>();

        for (int i = 0; i < map.Height; i++)
        {
            for (int j = 0; j < map.Width; j++)
            {
                jitterMap[i, j] = (_random.NextDouble() - 0.5) * jitter;
            }
        }

        foreach (var biomeSeed in biomeSeeds)
        {
            map.Biomes[biomeSeed.Cell.Y, biomeSeed.Cell.X] = biomeSeed.Biome;
            PushNeighbors(map, biomeSeed.Cell, biomeSeed.Biome, jitterMap, cellsQueue, 0.0);
        }

        while (cellsQueue.TryDequeue(out var item, out double cost))
        {
            var (cell, biome) = item;
            if (map.Biomes[cell.Y, cell.X] != CellBiome.None)
                continue;
            map.Biomes[cell.Y, cell.X] = biome;
            PushNeighbors(map, cell, biome, jitterMap, cellsQueue, cost);
        }

        return map;
    }

    public Dictionary<CellBiome, List<Cell>> GetCellsByBiome(Map map)
    {
        var cellsByBiome = new Dictionary<CellBiome, List<Cell>>();

        for (int y = 0; y < map.Height - 1; y++)
        {
            for (int x = 0; x < map.Width - 1; x++)
            {
                var biome = map.Biomes[y, x];
                if (!cellsByBiome.ContainsKey(biome))
                    cellsByBiome[biome] = new List<Cell>();

                cellsByBiome[biome].Add(new Cell(x, y));
            }
        }

        return cellsByBiome;
    }

    private void PushNeighbors(Map map, Cell cell, CellBiome biome, double[,] jitterMap, PriorityQueue<(Cell, CellBiome), double> cellsQueue, double cost)
    {
        var adjacentCells = new Cell[4]
        {
                new(cell.X - 1, cell.Y),
                new(cell.X + 1, cell.Y),
                new(cell.X, cell.Y - 1),
                new(cell.X, cell.Y + 1)
        };

        foreach (var adjacentCell in adjacentCells)
        {
            if (!adjacentCell.IsInsideBounds(map))
                continue;
            if (map.Biomes[adjacentCell.Y, adjacentCell.X] != CellBiome.None)
                continue;

            double enterCost = 1.0 + jitterMap[adjacentCell.Y, adjacentCell.X];
            cellsQueue.Enqueue((adjacentCell, biome), cost + enterCost);
        }
    }
}
