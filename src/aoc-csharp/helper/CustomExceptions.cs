namespace aoc_csharp.helper;

public class UnknownDirectionException : Exception
{
    public UnknownDirectionException(string direction) : base($"Unknown direction {direction}") { }
    public UnknownDirectionException(Direction direction) : base($"Unknown direction {direction}") { }
    public UnknownDirectionException(Point currentPoint, Point target) : base($"Unknown direction towards {target} from {currentPoint}") { }
};