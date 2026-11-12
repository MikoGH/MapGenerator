using MapGenerator.Enums;
using MapGenerator.Models;

namespace MapGenerator.Solvers;

public class MapSolverBfs
{
    private Map _map;
    public MapSolverBfs(Map map)
    {
        _map = map;
    }

    public Map Bfs()
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
        var queue = new Queue<Cell>();
        queue.Enqueue(_map.StartCell);
        Cell cell = _map.StartCell;
        while (queue.Count > 0)
        {
            cell = queue.Dequeue();
            visited[cell.Y, cell.X] = true;
            var (_, _, minDistance) = GetMinDistanceAround(distances, cell.X, cell.Y);
            distances[cell.Y, cell.X] = minDistance + 1;
            if (_map.Objects[cell.Y, cell.X] == CellObject.Finish)
                break;
            queue = TryAddToQueue(queue, visited, cell.X, cell.Y, cell.X + 1, cell.Y);
            queue = TryAddToQueue(queue, visited, cell.X, cell.Y, cell.X - 1, cell.Y);
            queue = TryAddToQueue(queue, visited, cell.X, cell.Y, cell.X, cell.Y + 1);
            queue = TryAddToQueue(queue, visited, cell.X, cell.Y, cell.X, cell.Y - 1);
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

    private Queue<Cell> TryAddToQueue(Queue<Cell> queue, bool[,] visited, int prevX, int prevY, int x, int y)
    {
        if (_map.Field[y, x] != CellType.Wall && !visited[y, x])
        {
            queue.Enqueue(new Cell(x, y));
            visited[y, x] = true;
        }

        return queue;
    }

    private List<Cell> RestorePath(int[,] distances, int x, int y)
    {
        var path = new List<Cell>(distances[y, x] + 1) { new Cell(x, y) };
        while (distances[y, x] > 0)
        {
            (x, y, _) = GetMinDistanceAround(distances, x, y);
            path.Add(new Cell(x, y));
        }

        return path;
    }

    private (int X, int Y, int Distance) GetMinDistanceAround(int[,] distances, int x, int y)
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

        if (minDistance == int.MaxValue)
            return (xMinDistance, yMinDistance, -1);

        return (xMinDistance, yMinDistance, minDistance);
    }
}
