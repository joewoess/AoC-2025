namespace aoc_csharp.helper;

/// <summary>
///     Represents an immutable two-dimensional point with integer coordinates.
/// </summary>
/// <param name="X">The X-coordinate of the point.</param>
/// <param name="Y">The Y-coordinate of the point.</param>
public readonly record struct Point(int X, int Y)
{
    public override string ToString() => $"({X},{Y})";
}

/// <summary>
/// Specifies the possible directions for movement or orientation in a two-dimensional space.
/// </summary>
/// <remarks>This enumeration is commonly used to represent directional input, navigation, or positioning in
/// grid-based systems, games, or user interfaces. The values include both cardinal (Up, Down, Left, Right) and diagonal
/// (UpLeft, UpRight, DownLeft, DownRight) directions.</remarks>
public enum Direction
{
    Up,
    Down,
    Left,
    Right,
    UpLeft,
    UpRight,
    DownLeft,
    DownRight
}

/// <summary>
/// Provides extension methods for the Point and Direction types to facilitate movement, direction calculation, distance
/// measurement, and neighbor enumeration in a two-dimensional grid.
/// </summary>
/// <remarks>These methods are intended to simplify common operations when working with grid-based navigation,
/// such as stepping in a direction, determining the direction or distance between points, and enumerating neighboring
/// points. The extensions support both orthogonal and diagonal movement, and include utilities for parsing and
/// representing directions as characters or strings.</remarks>
public static class PointerExtensions
{
    #region Stepping

    /// <summary>
    /// Returns a new point that is one step away from the specified point in the given direction.
    /// </summary>
    /// <param name="currentPoint">The starting point from which to step.</param>
    /// <param name="dir">The direction in which to move from the current point.</param>
    /// <returns>A new <see cref="Point"/> that is one unit away from <paramref name="currentPoint"/> in the specified <paramref
    /// name="dir"/> direction.</returns>
    /// <exception cref="UnknownDirectionException">Thrown if <paramref name="dir"/> is not a recognized direction.</exception>
    public static Point StepInDirection(this Point currentPoint, Direction dir)
    {
        return dir switch
        {
            Direction.Up => new Point(currentPoint.X, currentPoint.Y - 1),
            Direction.Down => new Point(currentPoint.X, currentPoint.Y + 1),
            Direction.Left => new Point(currentPoint.X - 1, currentPoint.Y),
            Direction.Right => new Point(currentPoint.X + 1, currentPoint.Y),
            Direction.UpLeft => new Point(currentPoint.X - 1, currentPoint.Y - 1),
            Direction.UpRight => new Point(currentPoint.X + 1, currentPoint.Y - 1),
            Direction.DownLeft => new Point(currentPoint.X - 1, currentPoint.Y + 1),
            Direction.DownRight => new Point(currentPoint.X + 1, currentPoint.Y + 1),
            _ => throw new UnknownDirectionException(dir)
        };
    }

    /// <summary>
    /// Returns a new point that is one step closer to the specified target point, using the shortest available path
    /// according to the provided movement preferences.
    /// </summary>
    /// <remarks>If the current point is already at the target, the method returns the current point
    /// unchanged. The movement preferences allow for flexible path selection, which can be useful in grid-based
    /// navigation scenarios.</remarks>
    /// <param name="currentPoint">The current point from which to step towards the target.</param>
    /// <param name="target">The destination point to move towards.</param>
    /// <param name="allowDiagonal">true to allow diagonal movement; otherwise, false. If false, only horizontal or vertical steps are taken.</param>
    /// <param name="preferDiagonal">true to prefer diagonal movement when both horizontal and vertical movement are possible; otherwise, false. Only
    /// relevant if allowDiagonal is true.</param>
    /// <param name="preferHorizontalOverVertical">true to prefer horizontal movement over vertical when a diagonal step is not taken; otherwise, vertical movement
    /// is preferred.</param>
    /// <returns>A new Point that is one step closer to the target point, or the original point if it is already at the target.</returns>
    public static Point StepTowards(this Point currentPoint, Point target, bool allowDiagonal = true, bool preferDiagonal = true, bool preferHorizontalOverVertical = true)
    {
        if (currentPoint == target) return currentPoint;
        var (xStep, yStep) = currentPoint.CalculateStepTowards(target);
        return (allowDiagonal, preferDiagonal, preferHorizontalOverVertical) switch
        {
            // move diagonal
            (true, true, _) => new Point(currentPoint.X + xStep, currentPoint.Y + yStep),
            (true, false, _) when Math.Abs(currentPoint.X - target.X) == Math.Abs(currentPoint.Y - target.Y) => new Point(currentPoint.X + xStep, currentPoint.Y + yStep),
            // move horizontal
            (_, _, true) when Math.Abs(currentPoint.X - target.X) != 0 => new Point(currentPoint.X + xStep, currentPoint.Y),
            (_, _, false) when Math.Abs(currentPoint.Y - target.Y) == 0 => new Point(currentPoint.X + xStep, currentPoint.Y),
            // move vertical
            _ => new Point(currentPoint.X, currentPoint.Y + yStep),
        };
    }

    /// <summary>
    ///     Returns a char representation for the next step between points
    /// </summary>
    /// <param name="currentPoint">The starting point from which to determine the direction.</param>
    /// <param name="target">The destination point toward which the direction is calculated.</param>
    /// <returns>A character indicating the direction from the current point to the target. The specific character returned
    /// depends on the direction (for example, 'N' for north, 'E' for east, etc.).</returns>
    public static char GetDirectionCharTowards(this Point currentPoint, Point target)
    {
        return currentPoint.GetDirectionTowards(target).ParseCharFromDirection();
    }

    /// <summary>
    /// Returns the Unicode character that visually represents the specified direction.
    /// </summary>
    /// <param name="direction">The direction to convert to a representative character.</param>
    /// <returns>A Unicode character corresponding to the specified direction. For example, '^' for Up or '↘' for DownRight.</returns>
    /// <exception cref="UnknownDirectionException">Thrown if the specified direction is not a recognized value.</exception>
    public static char ParseCharFromDirection(this Direction direction)
    {
        return direction switch
        {
            Direction.Up => '^',
            Direction.Down => 'v',
            Direction.Left => '<',
            Direction.Right => '>',
            Direction.UpLeft => '↖',
            Direction.UpRight => '↗',
            Direction.DownLeft => '↙',
            Direction.DownRight => '↘',
            _ => throw new UnknownDirectionException(direction)
        };
    }

    /// <summary>
    ///     Returns direction to next step as a point clamped to (-1, 1)
    /// </summary>
    /// <remarks>The returned point indicates the direction and magnitude of the next step along each axis. If
    /// the current point is already at the target, the result will be (0, 0).</remarks>
    /// <param name="currentPoint">The starting point from which to calculate the step.</param>
    /// <param name="target">The destination point toward which to move.</param>
    /// <returns>A new Point representing the direction of movement from the current point toward the target, with each
    /// coordinate clamped to the range [-1, 1].</returns>
    private static Point CalculateStepTowards(this Point currentPoint, Point target)
    {
        var (xDiff, yDiff) = (target.X - currentPoint.X, target.Y - currentPoint.Y);
        return new Point(Math.Clamp(xDiff, -1, 1), Math.Clamp(yDiff, -1, 1));
    }

    /// <summary>
    ///     Returns an enumerable of points between from and to inclusive, stepping directly towards the target point.
    /// </summary>
    /// <remarks>The returned sequence always ends with the target point. This method does not perform any
    /// obstacle or boundary checking; it assumes all intermediate points are valid.</remarks>
    /// <param name="from">The starting point of the path.</param>
    /// <param name="to">The destination point to walk towards.</param>
    /// <param name="allowDiagonal">true to allow diagonal movement between points; otherwise, false. If false, only horizontal and vertical steps
    /// are taken.</param>
    /// <param name="preferDiagonal">true to prefer diagonal steps when both horizontal and vertical movement are possible; otherwise, false.</param>
    /// <param name="preferHorizontal">true to prefer horizontal movement over vertical when a choice must be made; otherwise, vertical movement is
    /// preferred.</param>
    /// <param name="includeStart">true to include the starting point in the returned sequence; otherwise, the sequence begins with the first step
    /// after the starting point.</param>
    /// <returns>An enumerable sequence of points representing each step from the starting point to the target point, in order.
    /// The sequence includes the starting point if includeStart is true.</returns>
    public static IEnumerable<Point> WalkDirectlyTowards(this Point from, Point to, bool allowDiagonal = true, bool preferDiagonal = true,
        bool preferHorizontal = true, bool includeStart = false)
    {
        var currentPos = from;
        if (includeStart) yield return currentPos;
        while (currentPos != to)
        {
            currentPos = currentPos.StepTowards(to, allowDiagonal, preferDiagonal, preferHorizontal);
            yield return currentPos;
        }
    }

    #endregion

    #region Comparisons

    /// <summary>
    /// Determines whether the target point is adjacent to or the same as the current point.
    /// </summary>
    /// <remarks>Adjacency is determined using Chebyshev distance, meaning points are considered adjacent if
    /// they are horizontally, vertically, or diagonally next to each other, or occupy the same position.</remarks>
    /// <param name="currentPoint">The point from which to measure adjacency.</param>
    /// <param name="target">The point to test for adjacency to the current point.</param>
    /// <returns>true if the target point is adjacent to or the same as the current point; otherwise, false.</returns>
    public static bool IsWithinReach(this Point currentPoint, Point target) => currentPoint.ChebyshevDistance(target) <= 1;

    /// <summary>
    ///     Gets Direction to another point. This prefers diagonals.
    ///     If currentPoint == target, Direction.Up is returned.
    /// </summary>
    /// <param name="currentPoint">Current point used as starting Point for calculation</param>
    /// <param name="target">Target point to step towards</param>
    /// <returns>Direction of next step towards the target, or Up if already there</returns>
    /// <exception cref="UnknownDirectionException">Thrown if this was an invalid representation of a direction</exception>
    public static Direction GetDirectionTowards(this Point currentPoint, Point target)
    {
        if (currentPoint == target) return Direction.Up;
        return currentPoint.CalculateStepTowards(target) switch
        {
            (1, 0) => Direction.Right,
            (-1, 0) => Direction.Left,
            (0, 1) => Direction.Down,
            (0, -1) => Direction.Up,
            (1, 1) => Direction.DownRight,
            (1, -1) => Direction.UpRight,
            (-1, 1) => Direction.DownLeft,
            (-1, -1) => Direction.UpLeft,
            _ => throw new UnknownDirectionException(currentPoint, target)
        };
    }

    /// <summary>
    ///     Parse direction from a string/char representation
    /// </summary>
    /// <param name="direction">string representation of a direction</param>
    /// <returns>Direction as <see cref="Direction"/> </returns>
    /// <exception cref="UnknownDirectionException">Thrown if this was an invalid representation of a direction</exception>
    public static Direction ParseDirection(this string direction)
    {
        return direction switch
        {
            "U" or "^" => Direction.Up,
            "D" or "v" => Direction.Down,
            "L" or "<" => Direction.Left,
            "R" or ">" => Direction.Right,
            "UL" or "↖" => Direction.UpLeft,
            "UR" or "↗" => Direction.UpRight,
            "DL" or "↙" => Direction.DownLeft,
            "DR" or "↘" => Direction.DownRight,
            _ => throw new UnknownDirectionException(direction)
        };
    }

    /// <summary>
    ///     Returns the points neighbors of a given point
    /// </summary>
    /// <param name="currentPoint">Current point used as starting Point for calculation</param>
    /// <param name="allowDiagonal">If true, includes diagonal neighbors</param>
    /// <returns>IEnumerable of neighbors</returns>
    public static IEnumerable<Point> GetNeighborPoints(this Point currentPoint, bool allowDiagonal = true)
    {
        var directions = allowDiagonal
            ? [.. CardinalDirections, .. DiagonalDirections]
            : CardinalDirections;
        return directions.Select(dir => currentPoint.StepInDirection(dir));
    }

    public static readonly Direction[] CardinalDirections = [Direction.Up, Direction.Down, Direction.Left, Direction.Right];
    public static readonly Direction[] DiagonalDirections = [Direction.UpLeft, Direction.UpRight, Direction.DownLeft, Direction.DownRight];

    #endregion

    #region Distances

    /// <summary>
    ///     Manhattan Distance = no diagonal movement
    /// </summary>
    /// <param name="currentPoint">Current point used as starting Point for calculation</param>
    /// <param name="target">Target Point to calculate distance to</param>
    /// <returns>Manhattan distance as an integer</returns>
    public static int ManhattanDistance(this Point currentPoint, Point target)
    {
        return Math.Abs(currentPoint.X - target.X) + Math.Abs(currentPoint.Y - target.Y);
    }

    /// <summary>
    ///     Chebyshev Distance = diagonal movement allowed
    /// </summary>
    /// <param name="currentPoint">Current point used as starting Point for calculation</param>
    /// <param name="target">Target Point to calculate distance to</param>
    /// <returns>Chebyshev Distance as an integer</returns>
    public static int ChebyshevDistance(this Point currentPoint, Point target)
    {
        return Math.Max(Math.Abs(currentPoint.X - target.X), Math.Abs(currentPoint.Y - target.Y));
    }

    /// <summary>
    ///     Euclidean Distance = mathematical distance
    /// </summary>
    /// <param name="currentPoint">Current point used as starting Point for calculation</param>
    /// <param name="target">Target Point to calculate distance to</param>
    /// <returns>Euclidean Distance as an integer</returns>
    public static int EuclideanDistance(this Point currentPoint, Point target)
    {
        return (int)Math.Sqrt(Math.Pow(currentPoint.X - target.X, 2) + Math.Pow(currentPoint.Y - target.Y, 2));
    }

    #endregion
}