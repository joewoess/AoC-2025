namespace aoc_csharp.helper;

public static class PathFinding
{
    public static List<TGrid> MapNeighbors<TGrid>(this Point currentPos, Func<Point, Point, TGrid> mapper, Func<Point, Point, bool> filter,
        int maxHeight, int maxWidth, bool includeDiagonals = false)
    {
        return currentPos
            .GetNeighborsFiltered(filter, maxHeight, maxWidth, includeDiagonals)
            .Select(pos => mapper(currentPos, pos))
            .ToList();
    }

    public static List<Point> GetNeighborsFiltered(this Point currentPos, Func<Point, Point, bool> filter, int maxHeight, int maxWidth, bool includeDiagonals = false)
    {
        return currentPos.GetNeighborPoints(includeDiagonals)
            .Where(pos => pos.X >= 0 && pos.X < maxWidth)
            .Where(pos => pos.Y >= 0 && pos.Y < maxHeight)
            .Where(pos => filter(currentPos, pos))
            .ToList();
    }

    public static IEnumerable<TGrid> GetNeighbors<TGrid>(TGrid[][] grid, int currentY, int currentX, bool includeDiagonals = false)
    {
        return new Point(currentX, currentY)
            .GetNeighborPoints(includeDiagonals)
            .Where(pos => pos.X >= 0 && pos.X < grid[0].Length)
            .Where(pos => pos.Y >= 0 && pos.Y < grid.Length)
            .Select(pos => grid[pos.Y][pos.X]);
    }
}