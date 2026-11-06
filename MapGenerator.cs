using MapGenerator.Enums;
using MapGenerator.Models;

namespace Labirint;

public class MapGenerator
{
    private Map _map;
    private Random _random;

    public MapGenerator(int width, int height)
    {
        _map = new Map(width, height);
        _random = new Random();
    }

    public Map GenerateMap(int removeEndNodesIterations, int cellularAutomatonIterations)
    {
        GeneratePerfectLabirint();

        _map.PrintMap();

        Console.ReadLine();
        Console.Clear();

        RemoveEndNodes(removeEndNodesIterations);

        _map.PrintMap();

        Console.ReadLine();
        Console.Clear();

        CellularAutomaton(cellularAutomatonIterations);

        _map.PrintMap();

        Console.ReadLine();
        Console.Clear();

        RemoveEndNodes(removeEndNodesIterations);

        return _map;
    }

    public Map GenerateStartAndFinish()
    {
        var startCell = GenerateRandomPoint(1, _map.Width - 2, 1, _map.Height - 2);
        while (_map.Field[startCell.Y, startCell.X] != CellType.Empty)
            startCell = GenerateRandomPoint(1, _map.Width - 2, 1, _map.Height - 2);
        _map.StartCell = startCell;

        var finishCell = GenerateRandomPoint(1, _map.Width - 2, 1, _map.Height - 2);
        while (_map.Field[finishCell.Y, finishCell.X] != CellType.Empty)
            finishCell = GenerateRandomPoint(1, _map.Width - 2, 1, _map.Height - 2);
        _map.FinishCell = finishCell;

        _map.Objects[_map.StartCell.Y, _map.StartCell.X] = CellObject.Start;
        _map.Objects[_map.FinishCell.Y, _map.FinishCell.X] = CellObject.Finish;

        return _map;
    }

    private Map GeneratePerfectLabirint()
    {
        var startCell = GenerateRandomPoint(0, _map.Width / 2, 0, _map.Height / 2);
        startCell = new Cell(startCell.X * 2 + 1, startCell.Y * 2 + 1);

        var nextCells = new HashSet<Cell>()
        {
            startCell
        };

        while (nextCells.Count > 0)
        {
            var cellIndex = _random.Next(0, nextCells.Count - 1);
            var cell = nextCells.ElementAt(cellIndex);
            nextCells.Remove(cell);

            if (_map.Field[cell.Y, cell.X] == CellType.Empty)
                continue;

            _map.Field[cell.Y, cell.X] = CellType.Empty;

            var adjacentCells = new Cell[4]
            {
                new(cell.X - 2, cell.Y),
                new(cell.X + 2, cell.Y),
                new(cell.X, cell.Y - 2),
                new(cell.X, cell.Y + 2)
            };

            _random.Shuffle(adjacentCells);

            foreach (var adjacentCell in adjacentCells)
            {
                if (IsCellInsideBounds(adjacentCell) && _map.Field[adjacentCell.Y, adjacentCell.X] == CellType.Empty)
                {
                    _map.Field[(cell.Y + adjacentCell.Y) / 2, (cell.X + adjacentCell.X) / 2] = CellType.Empty;
                    break;
                }
            }

            foreach (var adjacentCell in adjacentCells)
            {
                if (IsCellInsideBounds(adjacentCell) && _map.Field[adjacentCell.Y, adjacentCell.X] == CellType.Wall)
                {
                    nextCells.Add(new Cell(adjacentCell.X, adjacentCell.Y));
                }
            }
        }

        return _map;
    }

    private Map RemoveEndNodes(int iterations)
    {
        for (int _ = 0; _ < iterations; _++)
        {
            for (int y = 1; y < _map.Height - 1; y++)
            {
                for (int x = 1; x < _map.Width - 1; x++)
                {
                    if (CountAdjacentWalls(x, y) == 3)
                        _map.Field[y, x] = CellType.Wall;
                }
            }
        }

        return _map;
    }

    private Map CellularAutomaton(int iterations)
    {
        for (int _ = 0; _ < iterations; _++)
        {
            var prevField = (CellType[,])_map.Field.Clone();
            for (int y = 1; y < _map.Height - 1; y++)
            {
                for (int x = 1; x < _map.Width - 1; x++)
                {
                    if (prevField[y, x] == CellType.Wall && CountAdjacentEmpty(x, y, prevField) >= 4)
                        _map.Field[y, x] = CellType.Empty;
                }
            }
        }

        return _map;
    }

    private int CountAdjacentEmpty(int x, int y, CellType[,] field)
    {
        var count = 0;
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (field[y + i, x + j] == CellType.Empty)
                    count++;
            }
        }
        return count;
    }

    private int CountAdjacentWalls(int x, int y)
    {
        var count = 0;
        count += _map.Field[y + 1, x] == CellType.Wall ? 1 : 0;
        count += _map.Field[y - 1, x] == CellType.Wall ? 1 : 0;
        count += _map.Field[y, x + 1] == CellType.Wall ? 1 : 0;
        count += _map.Field[y, x - 1] == CellType.Wall ? 1 : 0;
        return count;
    }

    private bool IsCellInsideBounds(Cell cell)
    {
        if (cell.X >= 0 && cell.Y >= 0 && cell.X < _map.Width && cell.Y < _map.Height)
            return true;
        return false;
    }

    private Cell GenerateRandomPoint(int minX, int maxX, int minY, int maxY)
    {
        var x = _random.Next(minX, maxX);
        var y = _random.Next(minY, maxY);

        return new Cell(x, y);
    }
}
