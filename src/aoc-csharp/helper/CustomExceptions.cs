namespace aoc_csharp.helper;

/// <summary>
///     Exception thrown when an unknown direction is encountered
/// </summary>
public class UnknownDirectionException : Exception
{
    public UnknownDirectionException(string direction) : base($"Unknown direction {direction}") { }
    public UnknownDirectionException(Direction direction) : base($"Unknown direction {direction}") { }
    public UnknownDirectionException(Point currentPoint, Point target) : base($"Unknown direction towards {target} from {currentPoint}") { }
};

public class InvalidLiveDisplayOperationException() : InvalidOperationException("Tried to perform an invalid operation on the live display context.");
public class IsNotAWholeNumberException() : ArithmeticException("Tried to perform an integer operation on a real number");
