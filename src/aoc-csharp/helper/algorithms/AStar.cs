namespace aoc_csharp.helper.algorithms;

/// <summary>
/// Provides a pathfinding A* implementation to find a path on a grid with obstacles 
/// </summary>
public static class AStar
{
    /// <summary>
    /// Finds the shortest path on a grid between 'start' and 'end' points. 
    /// The grid is assumed to be an area between [0,0] to (maxWidth,maxheight) and not endless
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="filter">Function determines if position can be traversed. Can be used for ostacles</param>
    /// <param name="maxHeight"></param>
    /// <param name="maxWidth"></param>
    /// <param name="distanceFunc"></param>
    /// <param name="calcCost"></param>
    /// <param name="includeDiagonals"></param>
    /// <returns></returns>
    public static List<Point>? FindPath(Point start, Point end, Func<Point, Point, bool> filter, int maxHeight, int maxWidth,
        Func<Point, Point, int>? distanceFunc = null, Func<int, int>? calcCost = null, bool includeDiagonals = false)
    {
        return AStarFindPath(start, end, filter, maxHeight, maxWidth, distanceFunc, calcCost, includeDiagonals) is { } path
            ? ReconstructPath(path)
            : null;
    }

    /// <summary>
    /// Calculates just the Cost aka .Distance between 'start' and 'end' points
    /// The grid is assumed to be an area between [0,0] to (maxWidth,maxheight) and not endless
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="filter">Function determines if position can be traversed. Can be used for ostacles</param>
    /// <param name="maxHeight"></param>
    /// <param name="maxWidth"></param>
    /// <param name="distanceFunc"></param>
    /// <param name="calcCost"></param>
    /// <param name="includeDiagonals"></param>
    /// <returns></returns>
    public static int? FindPathDistance(Point start, Point end, Func<Point, Point, bool> filter, int maxHeight, int maxWidth,
        Func<Point, Point, int>? distanceFunc = null, Func<int, int>? calcCost = null, bool includeDiagonals = false)
    {
        return AStarFindPath(start, end, filter, maxHeight, maxWidth, distanceFunc, calcCost, includeDiagonals)?.Cost;
    }


    /** Internal class used during the computation */
    private sealed class Field
    {
        public Point Position { get; init; }
        public int Cost { get; init; }
        public int Distance { get; init; }
        public int CostDistance => Cost + Distance;
        public Field? Parent { get; init; }

        public override string ToString() => $"{Position} = {Distance}";
    }

    /** Iterative implementation of the algorithm */
    private static Field? AStarFindPath(Point start, Point end, Func<Point, Point, bool> filter, int maxHeight, int maxWidth,
        Func<Point, Point, int>? distanceFunc = null, Func<int, int>? calcCost = null, bool includeDiagonals = false)
    {
        // default distance function is manhattan distance
        distanceFunc ??= (from, to) => from.ManhattanDistance(to);
        // default cost function is increment of 1
        calcCost ??= (cost) => cost + 1;

        var startField = new Field
        {
            Position = start,
            Cost = 0,
            Distance = distanceFunc(start, end)
        };
        var visitedFields = new List<Field>();
        var possibleFields = new List<Field>
        {
            startField
        };

        while (possibleFields.Count > 0)
        {
            var current = possibleFields.MinBy(tile => tile.CostDistance)!;
            // Break condition when algorithm actually found an end.
            if (current.Position == end) return current;

            possibleFields.Remove(current);
            visitedFields.Add(current);

            foreach (var neighbor in current.Position.GetNeighborsFiltered(filter, maxHeight, maxWidth, includeDiagonals))
            {
                var neighborField = new Field
                {
                    Position = neighbor,
                    Cost = calcCost(current.Cost),
                    Distance = distanceFunc(neighbor, end),
                    Parent = current
                };

                if (visitedFields.Any(field => field.Position == neighborField.Position)) continue;
                if (possibleFields.Any(field => field.Position == neighborField.Position && field.CostDistance <= neighborField.CostDistance)) continue;

                possibleFields.Add(neighborField);
            }
        }

        return null;
    }

    /** Reconstructs the path from the end to the start through the parent node links */
    private static List<Point> ReconstructPath(Field current)
    {
        var path = new List<Point>();
        while (current.Parent != null)
        {
            path.Add(current.Position);
            current = current.Parent;
        }
        path.Reverse();
        return path;
    }
}