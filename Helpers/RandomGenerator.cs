using LuaEngine.Scripts.Tests.Helpers;
using MapGenerator.Models;

namespace Prism.Core.Tests.Helpers;

public static class RandomGenerator
{
    public static Result<T> GenerateEnumValue<T>(Random sporadic) where T : struct, Enum
    {
        var values = Enum.GetValues(typeof(T));
        if (values.Length <= 0)
            return Result<T>.Empty;

        T randomValue = (T)values.GetValue(sporadic.Next(values.Length))!;
        return randomValue;
    }

    public static Cell GenerateRandomPoint(Random sporadic, int minX, int maxX, int minY, int maxY)
    {
        var x = sporadic.Next(minX, maxX);
        var y = sporadic.Next(minY, maxY);

        return new Cell(x, y);
    }

    public static bool GenerateBool(Random sporadic)
    {
        return sporadic.Next(2) == 1;
    }

    public static Result<T> PickOne<T>(Random sporadic, IEnumerable<T> values) =>
        PickOne(sporadic, values.ToArray());

    public static Result<T> PickOne<T>(Random sporadic, params T[] values)
    {
        var count = values.Length;
        if (count <= 0)
            return Result<T>.Empty;

        return values.ElementAt(sporadic.Next(0, count));
    }
}
