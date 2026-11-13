using MapGenerator.Configs;
using MapGenerator.Enums;
using MapGenerator.MapGeneration.Abstractions;
using MapGenerator.MapGeneration.Generators.Abstractions;
using MapGenerator.Models;
using Microsoft.Extensions.Options;

namespace MapGenerator.MapGeneration;

public sealed class MapGenerationPipeline : IMapGenerationPipeline
{
    private readonly IBiomeGenerator _biomeGenerator;
    private readonly ISkeletonGenerator _skeletonGenerator;
    private readonly IBiomeStrategyRegistry _registry;
    private readonly MapGenerationConfig _config;
    private readonly Random _random;

    public MapGenerationPipeline(
        IBiomeGenerator biomeGenerator,
        ISkeletonGenerator skeletonGenerator,
        IBiomeStrategyRegistry registry,
        IOptions<MapGenerationConfig> config)
    {
        _biomeGenerator = biomeGenerator;
        _skeletonGenerator = skeletonGenerator;
        _registry = registry;
        _config = config.Value;
        _random = new Random(_config.Seed);
    }

    public Map Generate()
    {
        var map = new Map(_config.Width, _config.Height);

        var biomeSeeds = _biomeGenerator.GenerateBiomeSeeds(map, _config.BiomeGeneration.SeedCount);

        map = _biomeGenerator.GenerateBiomes(map, biomeSeeds, _config.BiomeGeneration.Jitter);

        map = _skeletonGenerator.GenerateSkeleton(map, biomeSeeds);

        var cellsByBiome = _biomeGenerator.GetCellsByBiome(map);

        foreach (var biome in cellsByBiome.Keys)
        {
            if (!_config.BiomeConfigs.ContainsKey(biome))
                continue;  // TMP
            var strategy = _registry.For(biome);
            var currentBiomeSeeds = biomeSeeds.Where(x => x.Biome == biome).Select(x => x.Cell).ToList();
            var context = new BiomeGenerationContext(map, biome, _config.BiomeConfigs[biome], currentBiomeSeeds, cellsByBiome[biome]);
            map = strategy.Generate(context);
        }

        return map;
    }
}