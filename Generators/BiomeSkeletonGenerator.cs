using MapGenerator.Enums;
using MapGenerator.Extensions;
using MapGenerator.Models;

namespace MapGenerator.Generators;

public class BiomeSkeletonGenerator
{
    private readonly Map _map;
    private readonly Random _random;
    private readonly bool[,] _protected;

    public bool[,] Protected => _protected;

    public BiomeSkeletonGenerator(Map map, int? seed = null)
    {
        _map = map;
        _random = seed.HasValue ? new Random(seed.Value) : new Random();
        _protected = new bool[map.Height, map.Width];
    }

    public Map GenerateSkeleton(List<BiomeSeed> biomeSeeds)
    {
        if (biomeSeeds.Count < 2) return _map;

        var edges = BuildMST(biomeSeeds);

        foreach (var (fromBiome, toBiome) in edges)
            CarveCorridor(fromBiome.Cell, toBiome.Cell);

        return _map;
    }

    private List<(BiomeSeed From, BiomeSeed To)> BuildMST(List<BiomeSeed> biomeSeeds)
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

    private void CarveCorridor(Cell from, Cell to)
    {
        int x = from.X, y = from.Y;

        while (x != to.X || y != to.Y)
        {
            CarveCell(x, y);

            bool moveX = x != to.X && (y == to.Y || _random.NextDouble() < 0.5);
            if (moveX) x += Math.Sign(to.X - x);
            else y += Math.Sign(to.Y - y);
        }
        CarveCell(to.X, to.Y);
    }

    private void CarveCell(int x, int y)
    {
        if (x < 1 || y < 1 || x >= _map.Width - 1 || y >= _map.Height - 1) return;
        _map.Field[y, x] = CellType.Empty;
        _protected[y, x] = true;
        //_map.Marks[y, x] = CellMark.Corridor; // добавьте значение в enum
    }

}
