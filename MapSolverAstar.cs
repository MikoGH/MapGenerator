using MapGenerator.Enums;
using MapGenerator.Models;

namespace Labirint;

public class MapSolverAstar
{
    private Map _map;
    private double _g;
    private double _h;
    public MapSolverAstar(Map map, double g, double h)
    {
        _map = map;
        _g = g;
        _h = h;
    }

    public Map Astar()
    {
        var visited = new bool[_map.Height, _map.Width];
        var distances = new int[_map.Height, _map.Width];
        for (int i = 0; i < _map.Height; i++)
        {
            for (int j = 0; j < _map.Width; j++)
            {
                distances[i, j] = int.MaxValue;
            }
        }

        var nextCells = new PriorityQueue<Cell, int>();
        nextCells.Enqueue(_map.StartCell, GetDistanceSum(distances, _map.StartCell));
        distances[_map.StartCell.Y, _map.StartCell.X] = 0;
        visited[_map.StartCell.Y, _map.StartCell.X] = true;
        Cell cell = _map.StartCell;
        while (nextCells.Count > 0)
        {
            cell = nextCells.Dequeue();

            if (_map.Objects[cell.Y, cell.X] == CellObject.Finish)
                break;

            nextCells = TryAddCell(nextCells, visited, cell, new Cell(cell.X + 1, cell.Y), ref distances);
            nextCells = TryAddCell(nextCells, visited, cell, new Cell(cell.X - 1, cell.Y), ref distances);
            nextCells = TryAddCell(nextCells, visited, cell, new Cell(cell.X, cell.Y + 1), ref distances);
            nextCells = TryAddCell(nextCells, visited, cell, new Cell(cell.X, cell.Y - 1), ref distances);
        }

        var path = RestorePath(distances, cell.X, cell.Y);

        for (var i = 0; i < _map.Height; i++)
            for (int j = 0; j < _map.Width; j++)
                if (visited[i, j] && _map.Field[i, j] == CellType.Empty && _map.Objects[i, j] == CellObject.None)
                    _map.Marks[i, j] = CellMark.Visited;

        foreach (var pathCell in path)
            if (_map.Marks[pathCell.Y, pathCell.X] == CellMark.Visited)
                _map.Marks[pathCell.Y, pathCell.X] = CellMark.Path;

        return _map;
    }

    private PriorityQueue<Cell, int> TryAddCell(PriorityQueue<Cell, int> nextCells, bool[,] visited, Cell prevCell, Cell cell, ref int[,] distances)
    {
        if (_map.Field[cell.Y, cell.X] != CellType.Wall && distances[prevCell.Y, prevCell.X] + 1 < distances[cell.Y, cell.X])
        {
            distances[cell.Y, cell.X] = distances[prevCell.Y, prevCell.X] + 1;
            visited[cell.Y, cell.X] = true;
            nextCells.Enqueue(cell, GetDistanceSum(distances, cell));
        }

        return nextCells;
    }

    private List<Cell> RestorePath(int[,] distances, int x, int y)
    {
        var path = new List<Cell>(distances[y, x] + 1) { new Cell(x, y) };
        while (distances[y, x] > 0)
        {
            (x, y) = GetMinDistanceCellAround(distances, x, y);
            path.Add(new Cell(x, y));
        }

        return path;
    }

    private (int X, int Y) GetMinDistanceCellAround(int[,] distances, int x, int y)
    {
        var (xMinDistance, yMinDistance, minDistance) = (0, 0, int.MaxValue);

        if (distances[y + 1, x] < minDistance)
            (xMinDistance, yMinDistance, minDistance) = (x, y + 1, distances[y + 1, x]);
        if (distances[y - 1, x] < minDistance)
            (xMinDistance, yMinDistance, minDistance) = (x, y - 1, distances[y - 1, x]);
        if (distances[y, x + 1] < minDistance)
            (xMinDistance, yMinDistance, minDistance) = (x + 1, y, distances[y, x + 1]);
        if (distances[y, x - 1] < minDistance)
            (xMinDistance, yMinDistance, minDistance) = (x - 1, y, distances[y, x - 1]);

        return (xMinDistance, yMinDistance);
    }

    private int GetDistanceSum(int[,] distances, Cell cell)
    {
        return distances[cell.Y, cell.X]
            + Math.Abs(_map.FinishCell.X - cell.X)
            + Math.Abs(_map.FinishCell.Y - cell.Y);
    }
}


//public Map Astar()
//{
//    var visited = new bool[_map.Height, _map.Width];
//    var distances = new int[_map.Height, _map.Width];
//    for (int i = 0; i < _map.Height; i++)
//    {
//        for (int j = 0; j < _map.Width; j++)
//        {
//            distances[i, j] = int.MaxValue;
//        }
//    }
//    //var nextCells = new List<Cell>();
//    var nextCells = new PriorityQueue<Cell, int>();
//    //nextCells.Add(_map.StartCell);
//    nextCells.Enqueue(_map.StartCell, GetF(distances, _map.StartCell));
//    Cell cell = _map.StartCell;
//    distances[cell.Y, cell.X] = 0;
//    while (nextCells.Count > 0)
//    {
//        visited[cell.Y, cell.X] = true;
//        //nextCells.Remove(cell);
//        cell = nextCells.Dequeue();

//        var (_, _, minDistance) = GetMinDistanceAround(distances, cell.X, cell.Y);
//        distances[cell.Y, cell.X] = Math.Min(distances[cell.Y, cell.X], minDistance + 1);

//        if (_map.Field[cell.Y, cell.X] == CellType.Finish)
//            break;

//        nextCells = TryAddToList(nextCells, visited, cell.X, cell.Y, cell.X + 1, cell.Y, ref distances);
//        nextCells = TryAddToList(nextCells, visited, cell.X, cell.Y, cell.X - 1, cell.Y, ref distances);
//        nextCells = TryAddToList(nextCells, visited, cell.X, cell.Y, cell.X, cell.Y + 1, ref distances);
//        nextCells = TryAddToList(nextCells, visited, cell.X, cell.Y, cell.X, cell.Y - 1, ref distances);

//        //cell = nextCells[0];
//        //foreach (var nextCell in nextCells)
//        //    if (GetF(distances, nextCell) <= GetF(distances, cell))
//        //        cell = nextCell;
//    }

//    var path = RestorePath(distances, cell.X, cell.Y);

//    for (var i = 0; i < _map.Height; i++)
//        for (int j = 0; j < _map.Width; j++)
//            if (visited[i, j] && _map.Field[i, j] == CellType.Empty)
//                _map.Field[i, j] = CellType.Visited;

//    foreach (var pathCell in path)
//        if (_map.Field[pathCell.Y, pathCell.X] == CellType.Visited)
//            _map.Field[pathCell.Y, pathCell.X] = CellType.Path;


//    return _map;
//}


//private (int X, int Y, int Distance) GetMinDistanceAround(int[,] distances, int x, int y)
//{
//    var (xMinDistance, yMinDistance, minDistance) = (0, 0, int.MaxValue);

//    if (distances[y + 1, x] < minDistance)
//        (xMinDistance, yMinDistance, minDistance) = (x, y + 1, distances[y + 1, x]);
//    if (distances[y - 1, x] < minDistance)
//        (xMinDistance, yMinDistance, minDistance) = (x, y - 1, distances[y - 1, x]);
//    if (distances[y, x + 1] < minDistance)
//        (xMinDistance, yMinDistance, minDistance) = (x + 1, y, distances[y, x + 1]);
//    if (distances[y, x - 1] < minDistance)
//        (xMinDistance, yMinDistance, minDistance) = (x - 1, y, distances[y, x - 1]);

//    if (minDistance == int.MaxValue)
//        return (xMinDistance, yMinDistance, -1);

//    return (xMinDistance, yMinDistance, minDistance);
//}


//private List<Cell> TryAddToList(List<Cell> nextCells, bool[,] visited, int prevX, int prevY, int x, int y, ref int[,] distances)
//{
//    if (_map.Field[y, x] != CellType.Wall && !visited[y, x])
//    {
//        nextCells.Add(new Cell(x, y));
//        visited[y, x] = true;
//        distances[y, x] = distances[prevY, prevX] + 1;
//    }

//    return nextCells;
//}