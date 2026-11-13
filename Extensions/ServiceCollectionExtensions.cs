using MapGenerator.Configs;
using MapGenerator.Enums;
using MapGenerator.MapGeneration;
using MapGenerator.MapGeneration.Abstractions;
using MapGenerator.MapGeneration.Generators;
using MapGenerator.MapGeneration.Generators.Abstractions;
using MapGenerator.MapGeneration.Strategies;
using MapGenerator.MapGeneration.Strategies.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace MapGenerator.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<IBiomeGenerator, BiomeGenerator>();
        services.AddSingleton<ISkeletonGenerator, SkeletonGenerator>();

        services.AddSingleton<IGenerationStrategy, PrimaGenerationStrategy>();

        services.AddSingleton<IBiomeStrategyRegistry, BiomeStrategyRegistry>(sp =>
        {
            var config = sp.GetRequiredService<IOptions<MapGenerationConfig>>();

            var primaStartegy = new PrimaGenerationStrategy(config);

            return new BiomeStrategyRegistry()
                .Register(CellBiome.Lava, primaStartegy)
                .Register(CellBiome.Ice, primaStartegy);
        });

        services.AddTransient<IMapGenerationPipeline, MapGenerationPipeline>();

        return services;
    }

    public static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration config)
    {
        services.AddOptions<MapGenerationConfig>()
            .Bind(config.GetSection("MapGeneration"))
            .PostConfigure(config =>
            {
                config.BiomeConfigs = new Dictionary<CellBiome, BiomeConfig>
                {
                    [CellBiome.Lava] = config.BiomeConfigsRaw.Lava,
                    [CellBiome.Ice] = config.BiomeConfigsRaw.Ice,
                };
            });

        return services;
    }
}
