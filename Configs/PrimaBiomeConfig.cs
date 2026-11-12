namespace MapGenerator.Configs;

public sealed record PrimaBiomeConfig(int RemoveEndNodeIterations, int CellularAutomatonIterations) : BiomeConfig;
