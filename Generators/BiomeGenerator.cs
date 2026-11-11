using MapGenerator.Enums;
using MapGenerator.Extensions;
using MapGenerator.Models;
using Prism.Core.Tests.Helpers;

namespace MapGenerator.Generators;

public class BiomeGenerator
{
    private readonly Map _map;
    private readonly Random _random;

    public BiomeGenerator(Map map)
    {
        _map = map;
        _random = new Random();
    }

    public Map GenerateBiomes(int seedCount, double jitter)
    {
        var jitterMap = new double[_map.Height, _map.Width];

        var cellsQueue = new PriorityQueue<(Cell, CellBiome), double>();

        for (int i = 0; i < _map.Height; i++)
        {
            for (int j = 0; j < _map.Width; j++)
            {
                jitterMap[i, j] = (_random.NextDouble() - 0.5) * jitter;
            }
        }

        for (int i = 0; i < seedCount; i++)
        {
            var cell = RandomGenerator.GenerateRandomPoint(_random, 1, _map.Width - 2, 1, _map.Height - 2);
            CellBiome biome;
            do
            {
                biome = RandomGenerator.GenerateEnumValue<CellBiome>(_random).Value;
            } while (biome == CellBiome.None);
            _map.Biomes[cell.Y, cell.X] = biome;
            PushNeighbors(cell, biome, jitterMap, cellsQueue, 0.0);
        }

        while (cellsQueue.TryDequeue(out var item, out double cost))
        {
            var (cell, biome) = item;
            if (_map.Biomes[cell.Y, cell.X] != CellBiome.None)
                continue;
            _map.Biomes[cell.Y, cell.X] = biome;
            PushNeighbors(cell, biome, jitterMap, cellsQueue, cost);
        }

        return _map;
    }

    private void PushNeighbors(Cell cell, CellBiome biome, double[,] jitterMap, PriorityQueue<(Cell, CellBiome), double> cellsQueue, double cost)
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
            if (!adjacentCell.IsInsideBounds(_map))
                continue;
            if (_map.Biomes[adjacentCell.Y, adjacentCell.X] != CellBiome.None)
                continue;
            //var dist = Math.Sqrt((adjacentCell.X - startCell.X) * (adjacentCell.X - startCell.X) + (adjacentCell.Y - startCell.Y) * (adjacentCell.Y - startCell.Y))
            //            + jitterMap[adjacentCell.Y, adjacentCell.X];
            //cellsQueue.Enqueue((adjacentCell, startCell, biome), dist);
            double enterCost = 1.0 + jitterMap[adjacentCell.Y, adjacentCell.X];
            cellsQueue.Enqueue((adjacentCell, biome), cost + enterCost);
        }
    }
}
