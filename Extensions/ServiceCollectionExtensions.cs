using MapGenerator.Configs;
using MapGenerator.MapGeneration;
using MapGenerator.MapGeneration.Abstractions;
using MapGenerator.MapGeneration.Generators;
using MapGenerator.MapGeneration.Generators.Abstractions;
using MapGenerator.MapGeneration.Strategies;
using MapGenerator.MapGeneration.Strategies.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MapGenerator.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<IBiomeGenerator, BiomeGenerator>();
        services.AddSingleton<ISkeletonGenerator, SkeletonGenerator>();

        services.AddSingleton<IGenerationStrategy, PrimaGenerationStrategy>();

        services.AddSingleton<BiomeStrategyRegistry>();

        services.AddTransient<IMapGenerationPipeline, MapGenerationPipeline>();

        return services;
    }

    public static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration config)
    {
        services.AddOptions<MapGenerationConfig>()
            .Bind(config.GetSection("MapGeneration"));

        return services;
    }
}
