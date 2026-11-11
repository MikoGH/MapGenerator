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

    public static double Distance(this Cell cell, Cell otherCell)
        => Math.Sqrt((cell.X - otherCell.X) * (cell.X - otherCell.X) + (cell.Y - otherCell.Y) * (cell.Y - otherCell.Y));
}
