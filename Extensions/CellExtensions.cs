using MapGenerator.Enums;
using MapGenerator.Models;

namespace MapGenerator.Extensions;

public static class CellExtensions
{
    public static bool IsInsideBounds(this Cell cell, Map map)
    {
        if (cell.X >= 0 && cell.Y >= 0 && cell.X < map.Width && cell.Y < map.Height)
            return true;
        return false;
    }

    public static bool IsNearBounds(this Cell cell, Map map)
    {
        if (cell.X == 0 || cell.Y == 0 || cell.X == map.Width - 1 || cell.Y == map.Height - 1)
            return true;
        return false;
    }

    public static double Distance(this Cell cell, Cell otherCell)
        => Math.Sqrt((cell.X - otherCell.X) * (cell.X - otherCell.X) + (cell.Y - otherCell.Y) * (cell.Y - otherCell.Y));

    public static int CountAdjacentWalls(this Cell cell, Map map)
    {
        var count = 0;
        count += map.Field[cell.Y + 1, cell.X] == CellType.Wall ? 1 : 0;
        count += map.Field[cell.Y - 1, cell.X] == CellType.Wall ? 1 : 0;
        count += map.Field[cell.Y, cell.X + 1] == CellType.Wall ? 1 : 0;
        count += map.Field[cell.Y, cell.X - 1] == CellType.Wall ? 1 : 0;
        return count;
    }

    public static int CountAdjacentEmpty(this Cell cell, CellType[,] field)
    {
        var count = 0;
        count += field[cell.Y + 1, cell.X] == CellType.Empty ? 1 : 0;
        count += field[cell.Y - 1, cell.X] == CellType.Empty ? 1 : 0;
        count += field[cell.Y, cell.X + 1] == CellType.Empty ? 1 : 0;
        count += field[cell.Y, cell.X - 1] == CellType.Empty ? 1 : 0;
        //for (int i = -1; i < 2; i++)
        //{
        //    for (int j = -1; j < 2; j++)
        //    {
        //        if (field[cell.Y + i, cell.X + j] == CellType.Empty)
        //            count++;
        //    }
        //}
        return count;
    }
}
