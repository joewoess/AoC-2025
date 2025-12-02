using System.Text;

namespace aoc_csharp.helper;

/// <summary>
/// Provides extension methods for working with two-dimensional and jagged grid data structures, including conversion,
/// transformation, and formatting utilities.
/// </summary>
/// <remarks>The Grids class offers a variety of static methods to facilitate common operations on grid-like data,
/// such as converting between dictionaries and arrays, transposing grids, extracting diagonals, and generating
/// printable string representations. These methods are designed to simplify manipulation and visualization of
/// grid-based data in applications such as games, simulations, or data analysis. All methods are implemented as
/// extension methods for convenient usage with standard .NET types.</remarks>
public static class Grids
{
    /// <summary>
    ///     Maps a generic dictionary to a multidimensional grid with a given mapper function
    /// </summary>
    /// <remarks>The resulting grid covers the minimal bounding rectangle that contains all points in the
    /// dictionary. If a point within this rectangle is not present in the dictionary, the mapping function is called
    /// with null for that position.</remarks>
    /// <typeparam name="TGrid">The type of the elements in the resulting grid.</typeparam>
    /// <typeparam name="TMap">The type of the values stored in the input dictionary.</typeparam>
    /// <param name="map">The dictionary containing point keys and associated values to be mapped into a grid. Each key represents a
    /// position in the grid.</param>
    /// <param name="mapper">A function that maps a value from the dictionary (or null if the point is not present) to a grid element.</param>
    /// <returns>A two-dimensional array representing the grid, where each element is produced by applying the mapping function
    /// to the corresponding point's value in the dictionary.</returns>
    public static TGrid[,] AsGrid<TGrid, TMap>(this Dictionary<Point, TMap> map, Func<TMap?, TGrid> mapper)
    {
        var minX = map.Keys.Min(p => p.X);
        var maxX = map.Keys.Max(p => p.X);
        var minY = map.Keys.Min(p => p.Y);
        var maxY = map.Keys.Max(p => p.Y);
        var width = maxX - minX + 1;
        var height = maxY - minY + 1;
        var grid = new TGrid[height, width];

        for (var lineIdx = 0; lineIdx < height; lineIdx++)
        {
            for (var posIdx = 0; posIdx < width; posIdx++)
            {
                grid[lineIdx, posIdx] = mapper(map.GetValueOrDefault(new Point(minX + posIdx, minY + lineIdx)));
            }
        }

        return grid;
    }

    /// <summary>
    ///     Converts the specified map to a printable string representation using the provided mapping function.
    /// </summary>
    /// <typeparam name="TGrid">The type of the grid cell values to be used in the printable representation.</typeparam>
    /// <typeparam name="TMap">The type of the values stored in the map.</typeparam>
    /// <param name="map">A dictionary mapping points to values to be converted into a printable grid.</param>
    /// <param name="mapper">A function that maps each value from the map to a grid cell value. The input may be null if a point is not
    /// present in the map.</param>
    /// <returns>A string representing the map as a printable grid.</returns>
    public static string AsPrintable<TGrid, TMap>(this Dictionary<Point, TMap> map, Func<TMap?, TGrid> mapper) => AsPrintable(AsGrid(map, mapper));

    /// <summary>
    ///     Converts a two-dimensional array to a printable string representation, with optional formatting and
    ///     customization.
    /// </summary>
    /// <remarks>This method is useful for debugging or displaying grid-like data structures in a
    /// human-readable format. Customizing the mapper, separators, and padding allows for flexible output suitable for
    /// various display scenarios.</remarks>
    /// <typeparam name="TGrid">The type of elements contained in the grid.</typeparam>
    /// <param name="grid">The two-dimensional array to convert to a printable string.</param>
    /// <param name="mapper">An optional function that maps each grid element to its string representation. If null, the default string
    /// conversion is used.</param>
    /// <param name="separator">An optional string to use as the separator between elements in a row. If null, a default separator is used.</param>
    /// <param name="padLength">An optional value specifying the minimum width for each element's string representation. If specified, each
    /// element is padded to this length.</param>
    /// <param name="defaultWithoutMapper">The string to use for elements when no mapper is provided and the element is null.</param>
    /// <param name="lineSeparator">An optional string to use as the separator between rows. If null, a newline character ("\n") is used.</param>
    /// <returns>A string representing the contents of the grid, formatted according to the specified options.</returns>
    public static string AsPrintable<TGrid>(this TGrid[,] grid, Func<TGrid, string>? mapper = null, string? separator = null, int? padLength = null,
        string defaultWithoutMapper = "", string? lineSeparator = "\n")
    {
        return AsPrintable(grid.AsJaggedArray(), mapper, separator, padLength, defaultWithoutMapper, lineSeparator);
    }

    /// <summary>
    ///     Converts a two-dimensional array into a printable string representation, with optional mapping, padding, and
    ///     custom separators.
    /// </summary>
    /// <remarks>If the grid or any of its rows are empty, the resulting string will reflect the structure
    /// accordingly. This method is useful for displaying grids or matrices in a human-readable format, such as for
    /// debugging or logging purposes.</remarks>
    /// <typeparam name="TGrid">The type of the elements contained in the grid.</typeparam>
    /// <param name="grid">The two-dimensional array to convert to a string. Each inner array represents a row.</param>
    /// <param name="mapper">A function that maps each grid element to its string representation. If null, the element's ToString() method is
    /// used, or the value of defaultWithoutMapper if the element is null.</param>
    /// <param name="separator">The string to insert between elements in a row. If null, no separator is used.</param>
    /// <param name="padLength">The minimum width for each mapped element. If specified, each element is left-padded to this length.</param>
    /// <param name="defaultWithoutMapper">The string to use for null elements when no mapper is provided. Defaults to an empty string.</param>
    /// <param name="lineSeparator">The string to insert between rows. If null, no line separator is used. Defaults to a newline character ("\n").</param>
    /// <returns>A string representing the contents of the grid, with each element mapped, padded, and separated according to the
    /// specified parameters.</returns>
    public static string AsPrintable<TGrid>(this TGrid[][] grid, Func<TGrid, string>? mapper = null, string? separator = null, int? padLength = null,
        string defaultWithoutMapper = "", string? lineSeparator = "\n")
    {
        var result = new StringBuilder();
        var line = new StringBuilder();

        mapper ??= t => t?.ToString() ?? defaultWithoutMapper;

        for (var lineIdx = 0; lineIdx < grid.Length; lineIdx++)
        {
            line.Clear();
            for (var posIdx = 0; posIdx < grid[lineIdx].Length; posIdx++)
            {
                var val = grid[lineIdx][posIdx];
                var mapping = mapper?.Invoke(val) ?? defaultWithoutMapper;
                line.Append(padLength is not null
                    ? mapping.PadLeft(padLength.Value)
                    : mapping);

                if (separator is not null) line.Append(separator);
            }

            result.Append(line);
            if (lineSeparator is not null) result.Append(lineSeparator);
        }

        return result.ToString();
    }

    /// <summary>
    ///     Converts a two-dimensional array to a jagged array with the same element values and dimensions.
    /// </summary>
    /// <remarks>The resulting jagged array preserves the order and values of the original two-dimensional
    /// array. The length of the outer array corresponds to the number of rows, and each inner array corresponds to a
    /// row of the original array. The method supports arrays with non-zero lower bounds.</remarks>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    /// <param name="twoDimensionalArray">The two-dimensional array to convert. Must not be null.</param>
    /// <returns>A jagged array containing the same elements as the input two-dimensional array, with each inner array
    /// representing a row.</returns>
    public static T[][] AsJaggedArray<T>(this T[,] twoDimensionalArray)
    {
        var rowsFirstIndex = twoDimensionalArray.GetLowerBound(0);
        var rowsLastIndex = twoDimensionalArray.GetUpperBound(0);
        var numberOfRows = rowsLastIndex - rowsFirstIndex + 1;

        var columnsFirstIndex = twoDimensionalArray.GetLowerBound(1);
        var columnsLastIndex = twoDimensionalArray.GetUpperBound(1);
        var numberOfColumns = columnsLastIndex - columnsFirstIndex + 1;

        var jaggedArray = new T[numberOfRows][];
        for (var lineIdx = 0; lineIdx < numberOfRows; lineIdx++)
        {
            jaggedArray[lineIdx] = new T[numberOfColumns];

            for (var posIdx = 0; posIdx < numberOfColumns; posIdx++)
            {
                jaggedArray[lineIdx][posIdx] = twoDimensionalArray[lineIdx + rowsFirstIndex, posIdx + columnsFirstIndex];
            }
        }

        return jaggedArray;
    }

    /// <summary>
    ///     Converts a jagged array to a two-dimensional array, filling missing elements with a specified default value.
    /// </summary>
    /// <remarks>The resulting array has a number of rows equal to the length of the jagged array and a number
    /// of columns equal to the length of the longest inner array. If the jagged array contains inner arrays of
    /// different lengths, shorter rows are padded with the default value.</remarks>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    /// <param name="jaggedArray">The jagged array to convert. Each inner array represents a row in the resulting two-dimensional array. Cannot be
    /// null.</param>
    /// <param name="defaultValue">The value to use for elements in the resulting array where the corresponding position does not exist in the
    /// source jagged array. If not specified, the default value for type T is used.</param>
    /// <returns>A two-dimensional array containing the elements of the jagged array. Positions that do not exist in the source
    /// are set to the specified default value.</returns>
    public static T?[,] As2dArray<T>(this T[][] jaggedArray, T? defaultValue = default)
    {
        var rows = jaggedArray.Length;
        var maxColumns = jaggedArray.Max(row => row.Length);

        var grid2d = new T?[rows, maxColumns];
        for (var lineIdx = 0; lineIdx < rows; lineIdx++)
        {
            for (var posIdx = 0; posIdx < maxColumns; posIdx++)
            {
                if (posIdx >= jaggedArray[lineIdx].Length) grid2d[lineIdx, posIdx] = defaultValue;
                else grid2d[lineIdx, posIdx] = jaggedArray[lineIdx][posIdx];
            }
        }
        return grid2d;
    }

    /// <summary>
    ///     Converts a dictionary mapping points to characters into a two-dimensional character array representing a grid.
    /// </summary>
    /// <param name="map">A dictionary where each key is a point specifying a grid location and each value is the character to place at
    /// that location.</param>
    /// <param name="emptySpace">The character to use for grid positions that are not present in the dictionary. The default is '.'.</param>
    /// <returns>A two-dimensional character array containing the grid representation. Positions not present in the dictionary
    /// are filled with the specified empty space character.</returns>
    public static char[,] AsCharGrid(this Dictionary<Point, char> map, char emptySpace = '.') => AsGrid(map, c => c == 0 ? emptySpace : c);


    /// <summary>
    ///     Applies a mapping function to each element within the specified rectangular area of a two-dimensional grid.
    /// </summary>
    /// <remarks>If the specified area extends beyond the bounds of the grid, only the overlapping region is
    /// updated. The method modifies the input array in place and also returns it for convenience.</remarks>
    /// <typeparam name="TGrid">The type of the elements in the grid.</typeparam>
    /// <param name="grid">The two-dimensional array whose elements will be updated within the specified area.</param>
    /// <param name="from">One corner of the rectangular area to update. Coordinates are clamped to the bounds of the grid.</param>
    /// <param name="to">The opposite corner of the rectangular area to update. Coordinates are clamped to the bounds of the grid.</param>
    /// <param name="mapper">A function that takes the current element and returns the new value to assign to that element.</param>
    /// <returns>The original grid array with the specified area updated using the mapping function.</returns>
    public static TGrid[,] SetArea<TGrid>(this TGrid[,] grid, Point from, Point to, Func<TGrid, TGrid> mapper)
    {
        var minX = Math.Max(Math.Min(from.X, to.X), 0);
        var maxX = Math.Min(Math.Max(from.X, to.X), grid.GetLength(0) - 1);
        var minY = Math.Max(Math.Min(from.Y, to.Y), 0);
        var maxY = Math.Min(Math.Max(from.Y, to.Y), grid.GetLength(1) - 1);

        for (var lineIdx = minY; lineIdx <= maxY; lineIdx++)
        {
            for (var posIdx = minX; posIdx <= maxX; posIdx++)
            {
                grid[lineIdx, posIdx] = mapper(grid[lineIdx, posIdx]);
            }
        }

        return grid;
    }

    /// <summary>
    ///     Returns a new two-dimensional array with the rows and columns of the specified array exchanged.
    /// </summary>
    /// <remarks>The returned array has dimensions swapped compared to the input array: the number of rows
    /// becomes the number of columns and vice versa. The original array is not modified.</remarks>
    /// <typeparam name="TGrid">The type of the elements in the array.</typeparam>
    /// <param name="grid">The two-dimensional array to transpose. Cannot be null.</param>
    /// <returns>A new two-dimensional array whose rows and columns are transposed from the input array.</returns>
    public static TGrid[,] Transpose<TGrid>(this TGrid[,] grid)
    {
        var rows = grid.GetLength(0);
        var columns = grid.GetLength(1);

        var result = new TGrid[columns, rows];
        for (var c = 0; c < columns; c++)
        {
            for (var r = 0; r < rows; r++)
            {
                result[c, r] = grid[r, c];
            }
        }
        return result;
    }
    /// <summary>
    ///     Transposes a two-dimensional jagged array, swapping its rows and columns.
    /// </summary>
    /// <remarks>If the input array is not rectangular (i.e., some rows are shorter than others), the
    /// resulting transposed array will use <paramref name="defaultValueIfJagged"/> for positions where the original row
    /// does not have a value. The length of the resulting array is equal to the length of the longest row in the input
    /// array.</remarks>
    /// <typeparam name="TGrid">The type of the elements in the input and output arrays.</typeparam>
    /// <param name="grid">The two-dimensional jagged array to transpose. Each inner array represents a row.</param>
    /// <param name="defaultValueIfJagged">The value to use for missing elements if the input array is jagged (i.e., rows have different lengths). If not
    /// specified, the default value of <typeparamref name="TGrid"/> is used.</param>
    /// <returns>A new jagged array in which the rows and columns of the input array are transposed. If the input array is
    /// jagged, missing elements are filled with <paramref name="defaultValueIfJagged"/>.</returns>
    public static TGrid?[][] Transpose<TGrid>(this TGrid[][] grid, TGrid? defaultValueIfJagged = default)
    {
        var rows = grid.Length;
        var maxColumns = grid.Max(line => line.Length);

        var result = new TGrid?[maxColumns][];
        for (var c = 0; c < maxColumns; c++)
        {
            result[c] = new TGrid?[rows];
            for (var r = 0; r < rows; r++)
            {
                if (c > grid[r].Length) result[c][r] = defaultValueIfJagged;
                else result[c][r] = grid[r][c];
            }
        }
        return result;
    }
    /// <summary>
    ///     Returns all downward (top-left to bottom-right) diagonal lines in the specified two-dimensional grid.
    /// </summary>
    /// <remarks>Each diagonal line includes elements where the row and column indices increase together,
    /// starting from the first row and each column, and from each row in the first column. The method does not return
    /// empty diagonals. The number of diagonals returned is equal to the sum of the number of rows and columns minus
    /// one.</remarks>
    /// <typeparam name="TGrid">The type of elements contained in the grid.</typeparam>
    /// <param name="grid">The two-dimensional array from which to extract diagonal lines. Must not be null.</param>
    /// <returns>A list of lists, where each inner list contains the elements of a downward diagonal in the grid. The order of
    /// diagonals starts from the top-left corner and proceeds to the bottom-right.</returns>
    public static List<List<TGrid>> GetDiagonalLinesDown<TGrid>(this TGrid[,] grid)
    {
        var lines = new List<List<TGrid>>();
        var normalRowLength = grid.GetLength(0);
        var normalColLength = grid.GetLength(1);

        // First half left of center diagonal
        for (int row = 0; row < normalRowLength; row++)
        {
            var diagonal = new List<TGrid>();
            for (int r = row, c = 0; r < normalRowLength && c < normalColLength; r++, c++)
            {
                diagonal.Add(grid[r, c]);
            }
            lines.Add(diagonal);
        }
        // Second half right of center diagonal
        for (int startCol = 1; startCol < normalColLength; startCol++)
        {
            var diagonal = new List<TGrid>();
            for (int r = 0, c = startCol; r < normalRowLength && c < normalColLength; r++, c++)
            {
                diagonal.Add(grid[r, c]);
            }
            lines.Add(diagonal);
        }

        return lines;
    }
    /// <summary>
    ///     Returns all upward-sloping diagonal lines from a two-dimensional grid, where each line consists of elements from
    ///     bottom-left to top-right.
    /// </summary>
    /// <remarks>Each diagonal line starts either from the bottom row or the rightmost column and proceeds
    /// upward and to the right. The method does not skip empty diagonals; all possible upward-sloping diagonals are
    /// included. The number of returned diagonals equals the sum of the grid's row and column counts minus
    /// one.</remarks>
    /// <typeparam name="TGrid">The type of elements contained in the grid.</typeparam>
    /// <param name="grid">The two-dimensional array from which to extract upward-sloping diagonal lines. Cannot be null.</param>
    /// <returns>A list of lists, where each inner list contains the elements of a single upward-sloping diagonal line from the
    /// grid. The order of the lists corresponds to diagonals starting from the leftmost column and then from the bottom
    /// row.</returns>
    public static List<List<TGrid>> GetDiagonalLinesUp<TGrid>(this TGrid[,] grid)
    {
        var lines = new List<List<TGrid>>();
        var normalRowLength = grid.GetLength(0);
        var normalColLength = grid.GetLength(1);

        // First half left of center diagonal
        for (int row = 0; row < normalRowLength; row++)
        {
            var diagonal = new List<TGrid>();
            for (int r = row, c = normalColLength - 1; r < normalRowLength && c >= 0; r++, c--)
            {
                diagonal.Add(grid[r, c]);
            }
            lines.Add(diagonal);
        }
        // Second half right of center diagonal
        for (int col = normalColLength - 2; col >= 0; col--)
        {
            var diagonal = new List<TGrid>();
            for (int r = 0, c = col; r < normalRowLength && c >= 0; r++, c--)
            {
                diagonal.Add(grid[r, c]);
            }
            lines.Add(diagonal);
        }

        return lines;
    }

}