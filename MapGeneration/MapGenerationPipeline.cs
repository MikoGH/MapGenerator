using MapGenerator.Configs;
using MapGenerator.MapGeneration.Abstractions;
using MapGenerator.MapGeneration.Generators.Abstractions;
using MapGenerator.Models;
using Microsoft.Extensions.Options;

namespace MapGenerator.MapGeneration;

public sealed class MapGenerationPipeline : IMapGenerationPipeline
{
    private readonly IBiomeGenerator _biomeGenerator;
    private readonly ISkeletonGenerator _skeletonGenerator;
    //private readonly BiomeStrategyRegistry _registry;
    private readonly MapGenerationConfig _config;
    private readonly Random _random;

    public MapGenerationPipeline(
        IBiomeGenerator biomeGenerator,
        ISkeletonGenerator skeletonGenerator,
        //BiomeStrategyRegistry registry,
        IOptions<MapGenerationConfig> config)
    {
        _biomeGenerator = biomeGenerator;
        _skeletonGenerator = skeletonGenerator;
        //_registry = registry;
        _config = config.Value;
        _random = new Random(_config.Seed);
    }

    public Map Generate()
    {
        var map = new Map(_config.Width, _config.Height);

        var biomeSeeds = _biomeGenerator.GenerateBiomeSeeds(map, _config.BiomeGeneration.SeedCount);

        map = _biomeGenerator.GenerateBiomes(map, biomeSeeds, _config.BiomeGeneration.Jitter);

        map = _skeletonGenerator.GenerateSkeleton(map, biomeSeeds);

        //var biomes = map.Biomes.Cast<CellBiome>()
        //    .Distinct()
        //    .Where(b => b != CellBiome.None);

        //foreach (var biome in biomes)
        //{
        //    var strategy = _registry.For(biome);
        //    var ctx = new BiomeGenerationContext(
        //        map, biome, skeleton.Protected,
        //        new Random(_random.Next()),
        //        _configs[biome]);
        //    strategy.Generate(ctx);
        //}

        return map;
    }
}