using MapGenerator.Enums;

namespace MapGenerator.Models;

public class Map
{
    int _height, _width;
    Cell _startCell, _finishCell;
    readonly CellType[,] _field;
    readonly CellMark[,] _marks;
    readonly CellObject[,] _objects;
    readonly CellBiome[,] _biomes;
    public int Height => _height;
    public int Width => _width;
    public Cell StartCell
    {
        get => _startCell;
        set => _startCell = value;
    }
    public Cell FinishCell
    {
        get => _finishCell;
        set => _finishCell = value;
    }
    public CellType[,] Field => _field;
    public CellMark[,] Marks => _marks;
    public CellObject[,] Objects => _objects;
    public CellBiome[,] Biomes => _biomes;
    private readonly Dictionary<CellType, char> cellTypeToSymbol = new()
    {
        { CellType.Wall, '#' },
        { CellType.Empty, ' ' },
    };
    private readonly Dictionary<CellMark, char> cellMarkToSymbol = new()
    {
        { CellMark.Visited, '*' },
        { CellMark.Path, '*' }
    };
    private readonly Dictionary<CellObject, char> cellObjectToSymbol = new()
    {
        { CellObject.Start, 'S' },
        { CellObject.Finish, 'F' },
    };
    //private readonly Dictionary<CellType, ConsoleColor> cellTypeToColor = new()
    //{
    //    { CellType.Wall, ConsoleColor.White },
    //    { CellType.Empty, ConsoleColor.White },
    //};
    //private readonly Dictionary<CellMark, ConsoleColor> cellMarkToColor = new()
    //{
    //    { CellMark.None, ConsoleColor.White },
    //    { CellMark.Visited, ConsoleColor.Blue },
    //    { CellMark.Path, ConsoleColor.Green },
    //};
    //private readonly Dictionary<CellObject, ConsoleColor> cellObjectToColor = new()
    //{
    //    { CellObject.None, ConsoleColor.White },
    //    { CellObject.Start, ConsoleColor.Yellow },
    //    { CellObject.Finish, ConsoleColor.Yellow },
    //};
    private readonly Dictionary<CellBiome, ConsoleColor> cellBiomeToColor = new()
    {
        { CellBiome.None, ConsoleColor.White },
        { CellBiome.Desert, ConsoleColor.Yellow },
        { CellBiome.Dungeon, ConsoleColor.Magenta },
        //{ CellBiome.Forest, ConsoleColor.Green },
        { CellBiome.Ice, ConsoleColor.Cyan },
        { CellBiome.Lava, ConsoleColor.Red },
        { CellBiome.Swamp, ConsoleColor.Green },
    };
    private readonly Dictionary<CellBiome, ConsoleColor> cellBiomeWallToColor = new()
    {
        { CellBiome.None, ConsoleColor.Gray },
        { CellBiome.Desert, ConsoleColor.DarkYellow },
        { CellBiome.Dungeon, ConsoleColor.DarkMagenta },
        //{ CellBiome.Forest, ConsoleColor.DarkGreen },
        { CellBiome.Ice, ConsoleColor.DarkCyan },
        { CellBiome.Lava, ConsoleColor.DarkRed },
        { CellBiome.Swamp, ConsoleColor.DarkGreen },
    };

    public Map(int width, int height)
    {
        _field = new CellType[height, width];
        _marks = new CellMark[height, width];
        _objects = new CellObject[height, width];
        _biomes = new CellBiome[height, width];
        _width = width;
        _height = height;
    }

    public Map(Map map)
    {
        _width = map.Width;
        _height = map.Height;
        _field = (CellType[,])map.Field.Clone();
        _marks = (CellMark[,])map.Marks.Clone();
        _objects = (CellObject[,])map.Objects.Clone();
        _biomes = (CellBiome[,])map.Biomes.Clone();
        _startCell = map.StartCell;
        _finishCell = map.FinishCell;
    }

    public void PrintMap()
    {
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                Console.BackgroundColor = cellBiomeToColor[Biomes[i, j]];
                if (Field[i, j] == CellType.Wall)
                    Console.ForegroundColor = cellBiomeWallToColor[Biomes[i, j]];
                else
                    Console.ForegroundColor = ConsoleColor.Black;
                if (Objects[i, j] != CellObject.None)
                {
                    Console.Write(cellObjectToSymbol[Objects[i, j]]);
                }
                else if (Marks[i, j] != CellMark.None)
                {
                    Console.Write(cellMarkToSymbol[Marks[i, j]]);
                }
                else
                {
                    Console.Write(cellTypeToSymbol[Field[i, j]]);
                }
                Console.ResetColor();
            }
            Console.WriteLine();
        }
    }
}
