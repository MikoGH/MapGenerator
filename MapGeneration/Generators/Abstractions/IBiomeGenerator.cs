using MapGenerator.Enums;
using MapGenerator.Models;

namespace MapGenerator.MapGeneration.Generators.Abstractions;

public interface IBiomeGenerator
{
    public List<BiomeSeed> GenerateBiomeSeeds(Map map, int seedCount);

    public Map GenerateBiomes(Map map, List<BiomeSeed> biomeSeeds, double jitter);

    public Dictionary<CellBiome, List<Cell>> GetCellsByBiome(Map map);
}
