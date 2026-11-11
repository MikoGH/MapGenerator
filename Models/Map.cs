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
}
