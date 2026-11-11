using MapGenerator.Extensions;
using MapGenerator.Generators;
using MapGenerator.Models;

namespace Labirint;

public class Program
{
    public static void Main(string[] args)
    {
        // сгенерировать карту
        int height = 51, width = 201;
        //int height = 31, width = 141;
        var map = new Map(width, height);
        var fieldGenerator = new FieldGenerator(map);
        var removeEndNodesIterations = 4;
        var cellularAutomatonIterations = 5;
        var biomesSeedCount = 15;
        var biomesJitter = 1.5;
        map.GenerateField(removeEndNodesIterations, cellularAutomatonIterations);
        map.GenerateStartAndFinish();
        map.GenerateBiomes(biomesSeedCount, biomesJitter);
        map.PrintMap();

        Console.ReadLine();
        Console.Clear();

        // найти путь с помощью BFS
        var mapBfs = new Map(map);
        var mapSolverBfs = new MapSolverBfs(mapBfs);
        mapBfs = mapSolverBfs.Bfs();
        mapBfs.PrintMap();

        Console.ReadLine();
        Console.Clear();

        // найти путь с помощью A*
        var mapAstar = new Map(map);
        double g = 1, h = 1;
        var mapSolverAstar = new MapSolverAstar(mapAstar, g, h);
        mapAstar = mapSolverAstar.Astar();
        mapAstar.PrintMap();

        Console.ReadLine();
    }
}