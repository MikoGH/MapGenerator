using MapGenerator.Configs;
using MapGenerator.Enums;
using MapGenerator.Extensions;
using MapGenerator.MapGeneration.Strategies.Abstractions;
using MapGenerator.Models;
using Microsoft.Extensions.Options;

namespace MapGenerator.MapGeneration.Strategies;

public class PrimaGenerationStrategy : IGenerationStrategy
{
    private readonly Random _random;

    public PrimaGenerationStrategy(IOptions<MapGenerationConfig> config)
    {
        _random = new Random(config.Value.Seed);
    }

    public Map Generate(BiomeGenerationContext context)
    {
        var map = context.Map;
        var config = context.Config as PrimaBiomeConfig;
        var biome = context.Biome;
        var biomeSeeds = context.BiomeSeeds;
        var biomeCells = context.BiomeCells;
        var removeEndNodeIterations = config.RemoveEndNodeIterations;
        var cellularAutomatonIterations = config.CellularAutomatonIterations;

        var components = GetComponentSeeds(map, biome, biomeCells, biomeSeeds);
        var seeds = components.Select(x => x.Seeds.First()).ToList();

        map = GeneratePerfectLabirint(map, biome, seeds);

        //map.Print();
        //Console.ReadLine();
        //Console.Clear();

        map = RemoveEndNodes(map, biomeCells, removeEndNodeIterations);

        //map.Print();
        //Console.ReadLine();
        //Console.Clear();

        map = CellularAutomaton(map, biomeCells, cellularAutomatonIterations);

        ////map.Print();
        ////Console.ReadLine();
        ////Console.Clear();

        //RemoveEndNodes(removeEndNodesIterations);

        return map;
    }

    private Map GeneratePerfectLabirint(Map map, CellBiome biome, List<Cell> biomeSeeds)
    {
        var nextCells = new HashSet<Cell>(biomeSeeds);
        var biomeSeedsHash = new HashSet<Cell>(biomeSeeds);

        while (nextCells.Count > 0)
        {
            var cellIndex = _random.Next(0, nextCells.Count);
            var cell = nextCells.ElementAt(cellIndex);
            nextCells.Remove(cell);

            if (map.Field[cell.Y, cell.X] == CellType.Empty && !biomeSeedsHash.Contains(cell))
                continue;

            if (map.Biomes[cell.Y, cell.X] != biome)
                continue;

            if (cell.IsNearBounds(map))
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
                if (adjacentCell.IsInsideBounds(map)
                    && map.Biomes[adjacentCell.Y, adjacentCell.X] == biome
                    && map.Field[adjacentCell.Y, adjacentCell.X] == CellType.Empty)
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

    private Map RemoveEndNodes(Map map, List<Cell> cells, int iterations)
    {
        for (int _ = 0; _ < iterations; _++)
        {
            foreach (var cell in cells)
            {
                if (cell.IsNearBounds(map))
                    continue;

                if (cell.CountAdjacentWalls(map) == 3)
                    map.Field[cell.Y, cell.X] = CellType.Wall;
            }
        }

        return map;
    }

    private Map CellularAutomaton(Map map, List<Cell> cells, int iterations)
    {
        for (int _ = 0; _ < iterations; _++)
        {
            var prevField = (CellType[,])map.Field.Clone();
            foreach (var cell in cells)
            {
                if (cell.IsNearBounds(map))
                    continue;

                if (prevField[cell.Y, cell.X] == CellType.Wall && cell.CountAdjacentEmpty(prevField) >= 3)
                    map.Field[cell.Y, cell.X] = CellType.Empty;
            }
        }

        return map;
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

    // TODO: Вынести в отдельный класс
    private static List<BiomeComponent> GetComponentSeeds(Map map, CellBiome biome, List<Cell> biomeCells, IReadOnlyList<Cell> seeds)
    {
        var label = new int[map.Height, map.Width];
        for (int y = 0; y < map.Height; y++)
            for (int x = 0; x < map.Width; x++)
                label[y, x] = -1;

        var components = new List<BiomeComponent>();
        var seedsByComponent = new Dictionary<int, List<Cell>>();

        foreach (var seed in seeds)
        {
            if (map.Biomes[seed.Y, seed.X] != biome)
                continue;

            int id = label[seed.Y, seed.X] >= 0
                ? label[seed.Y, seed.X]
                : BfsLabel(map, biome, seed, label, components);

            if (!seedsByComponent.TryGetValue(id, out var list))
                seedsByComponent[id] = new List<Cell>();
            seedsByComponent[id].Add(seed);
        }

        for (int i = 0; i < components.Count; i++)
            if (seedsByComponent.TryGetValue(components[i].Id, out var componentSeeds))
                components[i] = components[i] with { Seeds = componentSeeds };

        return components;
    }

    public sealed record BiomeComponent(int Id, List<Cell> Cells, List<Cell> Seeds);

    private static int BfsLabel(Map map, CellBiome biome, Cell start, int[,] label, List<BiomeComponent> components)
    {
        int id = components.Count;
        var cells = new List<Cell>();
        var queue = new Queue<Cell>();

        queue.Enqueue(start);
        label[start.Y, start.X] = id;

        while (queue.Count > 0)
        {
            var cell = queue.Dequeue();
            cells.Add(cell);

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
                if (label[adjacentCell.Y, adjacentCell.X] >= 0)
                    continue;
                if (map.Biomes[adjacentCell.Y, adjacentCell.X] != biome)
                    continue;

                label[adjacentCell.Y, adjacentCell.X] = id;
                queue.Enqueue(adjacentCell);
            }
        }

        components.Add(new BiomeComponent(id, cells, new List<Cell>()));
        return id;
    }
}
