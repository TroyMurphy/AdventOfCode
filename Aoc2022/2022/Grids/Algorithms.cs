namespace Grids;

public class Algorithms
{
	private readonly Grid<int> _grid;

	public Algorithms(Grid<int> grid)
	{
		this._grid = grid;
	}

	public (double[,], List<(int, int)>) SolveDijkstra(int startX = 0, int startY = 0, (int, int)? to = null)
	{
		double[,] solution = new double[this._grid.GetHeight(), this._grid.GetWidth()];

		for (int i = 0; i < this._grid.GetHeight(); i++)
		{
			for (int j = 0; j < this._grid.GetWidth(); j++)
			{
				solution[i, j] = double.MaxValue;
			}
		}

		if (to == null)
		{
			to = (solution.GetLength(0) - 1, solution.GetLength(1) - 1);
		}

		var (sol, previous) = ShortestPath((0, 0), solution);
		var path = new List<(int, int)>();

		var node = to;
		while (node.HasValue)
		{
			path.Add(node.Value);
			node = previous[node.Value];
		}
		path.Reverse();

		return (sol, path);
	}

	public (double[,], Dictionary<(int, int), (int, int)?>) ShortestPath((int, int) start, double[,] solution)
	{
		var (xStart, yStart) = start;
		this._grid.matrix[yStart, xStart] = 0;

		var toVisit = new PriorityQueue<((int, int), double), double>(this._grid.GetWidth() * this._grid.GetHeight());
		// dictionary of visited nodes mapped to the best node to visit from
		var previous = new Dictionary<(int, int), (int, int)?>();
		solution[yStart, xStart] = 0;
		//pick a cell to visit first
		toVisit.Enqueue((start, 0), 0);
		previous[start] = null;

		while (toVisit.Count > 0)
		{
			var (vertex, w) = toVisit.Dequeue();
			var (x, y) = vertex;
			var neighbors = GetNeighbors(this._grid, x, y);
			// // get our cell with the lowest neighboring weight
			// var ((x, y), w) = toVisit.Dequeue();
			// // when we visit a cell, mark it as visited by noting its best predecessor
			// var neighbors = GetNeighbors(this._grid, x, y);

			foreach (var n in neighbors)
			{
				var (nx, ny) = n.GetCoord();
				// 	// if the neighbor has been visited, do nothing
				if (previous.ContainsKey((nx, ny)))
					continue;
				// 	// otherwise, update the cell value with the reachable weight from the current cell.
				var weightToNeighbor = w + n.Value;
				var neighborWeight = solution[ny, nx];

				if (weightToNeighbor < neighborWeight)
				{
					solution[ny, nx] = weightToNeighbor;
					previous[n.GetCoord()] = vertex;
					toVisit.Enqueue(((nx, ny), weightToNeighbor), weightToNeighbor);
				}
			}
		}

		return (solution, previous);
	}

	public static List<Point<int>> GetNeighbors(Grid<int> graph, int x, int y, bool includeDiagonals = false)
	{
		List<Point<int>> result = new();

		for (int xOffset = -1; xOffset <= 1; xOffset++)
		{
			for (int yOffset = -1; yOffset <= 1; yOffset++)
			{
				var newX = x + xOffset;
				var newY = y + yOffset;

				if ((xOffset == 0 && yOffset == 0) || newX < 0 || newX >= graph.GetWidth() || newY < 0 || newY >= graph.GetHeight())
				{
					continue;
				}
				if (!includeDiagonals && Math.Abs(xOffset + yOffset) != 1)
				{
					continue;
				}
				result.Add(new Point<int>(newX, newY, graph.matrix[newY, newX]));
			}
		}
		return result;
	}
}