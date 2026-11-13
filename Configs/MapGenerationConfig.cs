using MapGenerator.Enums;

namespace MapGenerator.Configs;

public class MapGenerationConfig
{
    public int Seed { get; set; }

    public int Width { get; set; }

    public int Height { get; set; }

    public BiomeGenerationConfig BiomeGeneration { get; set; }

    public BiomeConfigRaw BiomeConfigsRaw { get; set; }

    public Dictionary<CellBiome, BiomeConfig> BiomeConfigs { get; set; }
}
