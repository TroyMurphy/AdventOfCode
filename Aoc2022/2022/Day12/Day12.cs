using Grids;
using System.Net.Http.Headers;

namespace _2022.Day12
{
	public class Day12
	{
		public readonly IEnumerable<string> _lines;
		public Grid<int> grid;

		public Day12()
		{
			this._lines = Utilities.ReadLines(@"./Day12/inputs/input.txt");
			this.grid = Utilities.ParseAlphaGrid(_lines);
		}

		public void Solve()
		{
			//SolvePartOne();
			SolvePartTwo();
		}

		private void SolvePartOne()
		{
			var startpoint = grid.Points.First(x => x.Value == -1);
			grid.SetValueAt(startpoint.X, startpoint.Y, 0);

			var endpoint = grid.Points.First(x => x.Value == -2);
			grid.SetValueAt(endpoint.X, endpoint.Y, (int)'z' - 96);

			var solver = new Day12Dijkstra(grid);
			var sol = solver.SolveDijkstra(endpoint.GetCoord(), startpoint.GetCoord(), false);
			//Grid<int>.PrintCharPath(this.grid.matrix, new HashSet<(int, int)>(sol.path));
			// remove start from solution

			Console.WriteLine($"Solution has length of {sol.path.Count() - 1}");
		}

		private void SolvePartTwo()
		{
			var startpoint = grid.Points.First(x => x.Value == -1);
			grid.SetValueAt(startpoint.X, startpoint.Y, 0);

			var endpoint = grid.Points.First(x => x.Value == -2);
			grid.SetValueAt(endpoint.X, endpoint.Y, (int)'z' - 96);
			var solver = new Day12Dijkstra(grid);

			var startingPoints = grid.Points.Where(x => x.Value == 0);

			double bestScore = double.MaxValue;
			double[,] bestGrid = new double[1, 1];
			List<(int, int)> bestPath = new();

			foreach (var p in startingPoints)
			{
				var (sol, path) = solver.SolveDijkstra(endpoint.GetCoord(), p.GetCoord(), false);
				if (path.Count() <= 0)
				{
					continue;
				}

				var gridSolution = sol[endpoint.Y, endpoint.X];
				if (gridSolution < bestScore)
				{
					bestScore = gridSolution;
					bestGrid = sol;
					bestPath = path;
				}

				//if (shortestScenic == null || path.Count < shortestScenic.Count())
				//{
				//	Console.WriteLine($"Solution is {p.X},{p.Y} length of {path.Count() - 1}");
				//	shortestScenic = path;
				//	bestP = p.GetCoord();
				//}
			}

			Grid<double>.PrintPath(bestGrid, new HashSet<(int, int)>(bestPath));
			Console.WriteLine($"bestScore: {bestScore}");
		}
	}
}