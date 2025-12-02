using RLib.Tsp;
using RLib.Tsp.Enums;

namespace aoc_csharp.helper.algorithms;

public static class TSP
{
    public sealed record TSPResult(bool Success, string[] Path, int Distance)
    {
        public override string ToString()
        {
            return $"{{ Success = {Success}, Path = {Path.ToListString("->")}, Distance = {Distance} }}";
        }
    };
    public sealed record Edge(string Start, string End, int Cost);

    /// <summary>
    /// Solve a TSP given by an amount of edges with their costs
    /// </summary>
    /// <param name="edges">Possible paths with their cost</param>
    /// <param name="findLongestInstead"></param>
    /// <returns></returns>
    public static TSPResult SolveTSP(List<Edge> edges, bool findLongestInstead = false, bool isSymmetrical = true)
    {
        // Create a node-to-index map
        var nodes = edges.Select(e => e.Start)
                         .Union(edges.Select(e => e.End))
                         .Distinct()
                         .ToList();

        int numNodes = nodes.Count;
        var maxEdgeCost = edges.Max(e => e.Cost);

        // Create the distance matrix
        var nodeIndexMap = nodes.Select((node, index) => new { Node = node, Index = index })
                                 .ToDictionary(x => x.Node, x => x.Index);
        int[,] distanceMatrix = new int[numNodes, numNodes];
        for (int i = 0; i < numNodes; i++)
        {
            for (int j = 0; j < numNodes; j++)
            {
                if (i == j) distanceMatrix[i, j] = 0; // Zero cost for same cities
                else distanceMatrix[i, j] = int.MaxValue; // Large number for uninitialized
            }
        }
        // Populate the distance matrix from edges
        foreach (var edge in edges)
        {
            var cost = findLongestInstead ? maxEdgeCost - edge.Cost : edge.Cost;
            int fromIndex = nodeIndexMap[edge.Start];
            int toIndex = nodeIndexMap[edge.End];
            distanceMatrix[fromIndex, toIndex] = cost;
            if (isSymmetrical) distanceMatrix[toIndex, fromIndex] = cost; // Symmetrical TSP
        }

        // Create the distance callback
        float CalculateCallback(int fromIndex, int toIndex)
        {
            return distanceMatrix[fromIndex, toIndex];
        }

        // Create TSP instance using the distance matrix
        var solverConfiguration = new Solver.SolverConfiguration
        {
            FirstSolutionStrategy = eFirstSolutionStrategy.ConnectCheapestArcs
        };
        var solver = new Solver(CalculateCallback, numNodes, solverConfiguration);
        int[] visited = solver.FindSolution();

        if (visited.Length == 0)
        {
            return new TSPResult(false, [], 0);
        }

        return new TSPResult(
            true,
            [.. visited.Select(v => nodes[v])],
            (int)visited
                .PairWithNext()
                .Select(pair => CalculateCallback(pair.From, pair.To))
                .Select(cost => findLongestInstead ? maxEdgeCost - cost : cost)
                .Sum()
        );
    }

    public static TSPResult SolveSeatingArrangement(List<Edge> edges)
    {
        // Create a node-to-index map
        var nodes = edges.Select(e => e.Start)
                         .Union(edges.Select(e => e.End))
                         .Distinct()
                         .ToList();

        int numNodes = nodes.Count;

        // Create the distance matrix
        var nodeIndexMap = nodes.Select((node, index) => new { Node = node, Index = index })
                                 .ToDictionary(x => x.Node, x => x.Index);
        int[,] distanceMatrix = new int[numNodes, numNodes];
        for (int i = 0; i < numNodes; i++)
        {
            for (int j = 0; j < numNodes; j++)
            {
                if (i == j) distanceMatrix[i, j] = 0; // Zero cost for same cities
                else distanceMatrix[i, j] = int.MaxValue; // Large number for uninitialized
            }
        }
        // Populate the distance matrix from edges
        foreach (var edge in edges)
        {
            var cost = edge.Cost * (-1); // reverse to be able to solve for smallest in TSP
            int fromIndex = nodeIndexMap[edge.Start];
            int toIndex = nodeIndexMap[edge.End];
            distanceMatrix[fromIndex, toIndex] = cost;
        }

        // Create the distance callback
        float CalculateCallback(int fromIndex, int toIndex)
        {
            return distanceMatrix[fromIndex, toIndex];
        }

        // Create the distance callback
        float EvaluateSolution(int[] path)
        {
            var sum = 0f;
            for (int i = 0; i < path.Length - 1; i++)
            {
                sum += CalculateCallback(path[i], path[i + 1]) + CalculateCallback(path[i + 1], path[i]);
            }
            sum += CalculateCallback(path[^1], path[0]);
            sum += CalculateCallback(path[0], path[^1]);
            return sum;
        }

        // Create TSP instance using the distance matrix
        var solverConfiguration = new Solver.SolverConfiguration
        {
            FirstSolutionStrategy = eFirstSolutionStrategy.NearestNeighbor,
        };
        var solver = new Solver(CalculateCallback, numNodes, solverConfiguration);

        // Solve the TSP
        solver.UseFullSolutionCostValidation(EvaluateSolution);
        int[] visited = solver.FindSolution();

        if (visited.Length == 0)
        {
            return new TSPResult(false, [], 0);
        }

        return new TSPResult(
            true,
            [.. visited.Select(v => nodes[v])],
            (int)EvaluateSolution(visited) * (-1)
        );
    }
}