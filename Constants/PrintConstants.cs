using MapGenerator.Enums;

namespace MapGenerator.Constants;

public static class PrintConstants
{
    public static readonly Dictionary<CellType, char> cellTypeToSymbol = new()
    {
        { CellType.Wall, '#' },
        { CellType.Empty, ' ' },
    };

    public static readonly Dictionary<CellMark, char> cellMarkToSymbol = new()
    {
        { CellMark.Visited, '*' },
        { CellMark.Path, '*' }
    };

    public static readonly Dictionary<CellObject, char> cellObjectToSymbol = new()
    {
        { CellObject.Start, 'S' },
        { CellObject.Finish, 'F' },
    };

    public static readonly Dictionary<CellMark, ConsoleColor> cellMarkToColor = new()
    {
        { CellMark.None, ConsoleColor.White },
        { CellMark.Visited, ConsoleColor.Gray },
        { CellMark.Path, ConsoleColor.Yellow },
    };

    public static readonly Dictionary<CellObject, ConsoleColor> cellObjectToColor = new()
    {
        { CellObject.None, ConsoleColor.White },
        { CellObject.Start, ConsoleColor.Yellow },
        { CellObject.Finish, ConsoleColor.Yellow },
    };

    public static readonly Dictionary<CellBiome, ConsoleColor> cellBiomeToColor = new()
    {
        { CellBiome.None, ConsoleColor.White },
        { CellBiome.Desert, ConsoleColor.Yellow },
        { CellBiome.Dungeon, ConsoleColor.Magenta },
        { CellBiome.Ice, ConsoleColor.Cyan },
        { CellBiome.Lava, ConsoleColor.Red },
        { CellBiome.Swamp, ConsoleColor.Green },
    };

    public static readonly Dictionary<CellBiome, ConsoleColor> cellBiomeWallToColor = new()
    {
        { CellBiome.None, ConsoleColor.Gray },
        { CellBiome.Desert, ConsoleColor.DarkYellow },
        { CellBiome.Dungeon, ConsoleColor.DarkMagenta },
        { CellBiome.Ice, ConsoleColor.DarkCyan },
        { CellBiome.Lava, ConsoleColor.DarkRed },
        { CellBiome.Swamp, ConsoleColor.DarkGreen },
    };

}
