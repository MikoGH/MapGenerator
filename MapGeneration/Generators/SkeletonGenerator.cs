using MapGenerator.Configs;
using MapGenerator.Enums;
using MapGenerator.Extensions;
using MapGenerator.MapGeneration.Generators.Abstractions;
using MapGenerator.Models;
using Microsoft.Extensions.Options;

namespace MapGenerator.MapGeneration.Generators;

public class SkeletonGenerator : ISkeletonGenerator
{
    private readonly Random _random;

    public SkeletonGenerator(IOptions<MapGenerationConfig> config)
    {
        _random = new Random(config.Value.Seed);
    }

    public Map GenerateSkeleton(Map map, List<BiomeSeed> biomeSeeds)
    {
        if (biomeSeeds.Count < 2) return map;

        var edges = BuildMST(map, biomeSeeds);

        foreach (var (fromBiome, toBiome) in edges)
            CarveCorridor(map, fromBiome.Cell, toBiome.Cell);

        return map;
    }

    private List<(BiomeSeed From, BiomeSeed To)> BuildMST(Map map, List<BiomeSeed> biomeSeeds)
    {
        var edges = new List<(BiomeSeed, BiomeSeed)>();
        var visited = new HashSet<BiomeSeed>();
        var frontier = new PriorityQueue<(BiomeSeed From, BiomeSeed To), double>();

        var start = biomeSeeds.First();
        visited.Add(start);

        foreach (var other in biomeSeeds)
        {
            if (other == start)
                continue;
            frontier.Enqueue((start, other), start.Cell.Distance(other.Cell));
        }

        while (frontier.Count > 0 && visited.Count < biomeSeeds.Count)
        {
            var (from, to) = frontier.Dequeue();
            if (visited.Contains(to)) continue;

            visited.Add(to);
            edges.Add((from, to));

            foreach (var other in biomeSeeds)
            {
                if (visited.Contains(other))
                    continue;
                frontier.Enqueue((to, other), to.Cell.Distance(other.Cell));
            }
        }

        return edges;
    }

    private void CarveCorridor(Map map, Cell from, Cell to)
    {
        int x = from.X, y = from.Y;

        while (x != to.X || y != to.Y)
        {
            CarveCell(map, x, y);

            bool moveX = x != to.X && (y == to.Y || _random.NextDouble() < 0.5);
            if (moveX) x += Math.Sign(to.X - x);
            else y += Math.Sign(to.Y - y);
        }
        CarveCell(map, to.X, to.Y);
    }

    private void CarveCell(Map map, int x, int y)
    {
        if (x < 1 || y < 1 || x >= map.Width - 1 || y >= map.Height - 1)
            return;
        map.Field[y, x] = CellType.Empty;
    }
}
