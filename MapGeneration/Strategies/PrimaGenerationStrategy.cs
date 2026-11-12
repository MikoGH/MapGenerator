using MapGenerator.Configs;
using MapGenerator.Enums;
using MapGenerator.Extensions;
using MapGenerator.MapGeneration.Strategies.Abstractions;
using MapGenerator.Models;
using Microsoft.Extensions.Options;
using Prism.Core.Tests.Helpers;

namespace MapGenerator.MapGeneration.Strategies;

public class PrimaGenerationStrategy : IGenerationStrategy
{
    private readonly Random _random;
    //public CellBiome SupportedBiome => CellBiome.

    public PrimaGenerationStrategy(IOptions<MapGenerationConfig> config)
    {
        _random = new Random(config.Value.Seed);
    }

    public Map Generate(BiomeGenerationContext context)
    {
        var map = context.Map;
        var config = context.Config as PrimaBiomeConfig;

        map = GeneratePerfectLabirint(map);

        //map.Print();

        //Console.ReadLine();
        //Console.Clear();

        //RemoveEndNodes(removeEndNodesIterations);

        ////map.Print();

        ////Console.ReadLine();
        ////Console.Clear();

        //CellularAutomaton(cellularAutomatonIterations);

        ////map.Print();

        ////Console.ReadLine();
        ////Console.Clear();

        //RemoveEndNodes(removeEndNodesIterations);

        return map;
    }

    private Map GeneratePerfectLabirint(Map map)
    {
        var startCell = RandomGenerator.GenerateRandomPoint(_random, 0, map.Width / 2, 0, map.Height / 2);
        startCell = new Cell(startCell.X * 2 + 1, startCell.Y * 2 + 1);

        var nextCells = new HashSet<Cell>()
        {
            startCell
        };

        while (nextCells.Count > 0)
        {
            var cellIndex = _random.Next(0, nextCells.Count - 1);
            var cell = nextCells.ElementAt(cellIndex);
            nextCells.Remove(cell);

            if (map.Field[cell.Y, cell.X] == CellType.Empty)
                continue;

            map.Field[cell.Y, cell.X] = CellType.Empty;

            var adjacentCells = new Cell[4]
            {
                new(cell.X - 2, cell.Y),
                new(cell.X + 2, cell.Y),
                new(cell.X, cell.Y - 2),
                new(cell.X, cell.Y + 2)
            };

            _random.Shuffle(adjacentCells);

            foreach (var adjacentCell in adjacentCells)
            {
                if (adjacentCell.IsInsideBounds(map) && map.Field[adjacentCell.Y, adjacentCell.X] == CellType.Empty)
                {
                    map.Field[(cell.Y + adjacentCell.Y) / 2, (cell.X + adjacentCell.X) / 2] = CellType.Empty;
                    break;
                }
            }

            foreach (var adjacentCell in adjacentCells)
            {
                if (adjacentCell.IsInsideBounds(map) && map.Field[adjacentCell.Y, adjacentCell.X] == CellType.Wall)
                {
                    nextCells.Add(new Cell(adjacentCell.X, adjacentCell.Y));
                }
            }
        }

        return map;
    }

    private Dictionary<CellBiome, List<Cell>> GetCellsByBiome(Map map)
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

    private Map RemoveEndNodes(Map map, int iterations)
    {
        for (int _ = 0; _ < iterations; _++)
        {
            for (int y = 1; y < map.Height - 1; y++)
            {
                for (int x = 1; x < map.Width - 1; x++)
                {
                    if (CountAdjacentWalls(map, x, y) == 3)
                        map.Field[y, x] = CellType.Wall;
                }
            }
        }

        return map;
    }

    private Map CellularAutomaton(Map map, int iterations)
    {
        for (int _ = 0; _ < iterations; _++)
        {
            var prevField = (CellType[,])map.Field.Clone();
            for (int y = 1; y < map.Height - 1; y++)
            {
                for (int x = 1; x < map.Width - 1; x++)
                {
                    if (prevField[y, x] == CellType.Wall && CountAdjacentEmpty(map, x, y, prevField) >= 4)
                        map.Field[y, x] = CellType.Empty;
                }
            }
        }

        return map;
    }

    private int CountAdjacentEmpty(Map map, int x, int y, CellType[,] field)
    {
        var count = 0;
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (field[y + i, x + j] == CellType.Empty)
                    count++;
            }
        }
        return count;
    }

    private int CountAdjacentWalls(Map map, int x, int y)
    {
        var count = 0;
        count += map.Field[y + 1, x] == CellType.Wall ? 1 : 0;
        count += map.Field[y - 1, x] == CellType.Wall ? 1 : 0;
        count += map.Field[y, x + 1] == CellType.Wall ? 1 : 0;
        count += map.Field[y, x - 1] == CellType.Wall ? 1 : 0;
        return count;
    }

    //private bool IsCellInsideBounds(Map map, Cell cell)
    //{
    //    if (cell.X >= 0 && cell.Y >= 0 && cell.X < map.Width && cell.Y < map.Height)
    //        return true;
    //    return false;
    //}

    //private Cell GenerateRandomPoint(int minX, int maxX, int minY, int maxY)
    //{
    //    var x = _random.Next(minX, maxX);
    //    var y = _random.Next(minY, maxY);

    //    return new Cell(x, y);
    //}
}
