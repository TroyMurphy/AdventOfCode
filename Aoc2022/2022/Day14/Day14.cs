using Grids;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace _2022.Day14
{
	public class Day14
	{
		public readonly IEnumerable<string> _lines;
		public Day14Grid<char?> grid;
		public int floor;

		public Day14()
		{
			//this._lines = Utilities.ReadLines(@"./Day14/inputs/demo1.txt");
			this._lines = Utilities.ReadLines(@"./Day14/inputs/input.txt");

			var points = new List<Point<char?>>();
			foreach (var line in this._lines)
			{
				var coords = line.Split(new string[] { ",", "->" }, StringSplitOptions.RemoveEmptyEntries);
				var pointMap = coords.Select(x => int.Parse(x)).Chunk(2).Select(a => (a.First(), a.Last()));
				for (int i = 0; i < pointMap.Count() - 1; i++)
				{
					var (x1, y1) = pointMap.Skip(i).First();
					var (x2, y2) = pointMap.Skip(i + 1).First();
					List<(int, int)> pointCoords = new();

					if (x1 == x2)
					{
						pointCoords.AddRange(
							Enumerable.Range(
								Math.Min(y1, y2),
								Math.Abs(y1 - y2) + 1
							).Select(yValue => (x1, yValue))
						);
					}
					// y is equal
					else
					{
						pointCoords.AddRange(
							Enumerable.Range(
								Math.Min(x1, x2),
								Math.Abs(x1 - x2) + 1
							).Select(xValue => (xValue, y1))
						);
					}
					points.AddRange(pointCoords.Select(p => new Point<char?>(p.Item1, p.Item2, '#')));
				}
			}
			points.Add(new Point<char?>(1000, floor + 1, null));
			this.grid = new Day14Grid<char?>(points, false);
			floor = grid.Points.Max(p => p.Y);
		}

		public void Solve()
		{
			//SolvePartOne();
			SolvePartTwo();
		}

		private void SolvePartOne()
		{
			var grains = 0;
			while (true)
			{
				var response = DropGrain();
				if (response == 1)
				{
					grains++;
				}
				else
				{
					break;
				}
			}
			Console.WriteLine($"Grains dropped was {grains}");
		}

		private void SolvePartTwo()
		{
			while (true)
			{
				var response = DropGrainWithFloor();
				if (response == -1)
				{
					break;
				}
			}
			var grains = grid.Points.Where(p => p.Value == 'O').Count();
			Console.WriteLine($"Grains dropped was {grains}");

			var newGrid = new Day14Grid<char?>(grid.Points, true);
			PrintFromColumn(newGrid, 485);
		}

		public int DropGrainWithFloor(int x = 500, int y = 0)
		{
			bool stopped = false;
			var ix = x;
			var iy = y;
			(int x, int y) peek = (x, y);

			while (!stopped)
			{
				if (iy == floor + 1)
				{
					grid.SetValueAt(ix, iy, 'O');
					break;
				}
				var oldy = iy;
				peek = (peek.x, peek.y + 1);
				var rocksBelow = grid.Points.Where(p => p.X == ix && p.Y > iy).Select(p => p.Y);
				if (rocksBelow.Any())
				{
					peek = (peek.x, rocksBelow.Min());
					(ix, iy) = (peek.x, peek.y - 1);
				}
				else
				{
					grid.SetValueAt(ix, floor + 1, 'O');
					continue;
				}

				//if (grid.GetValueAt(peek.x, peek.y) == null)
				//{
				//	(ix, iy) = peek;
				//	continue;
				//}
				(int x, int y) peekSide = (peek.x - 1, peek.y);
				if (grid.GetValueAt(peekSide.x, peekSide.y) == null)
				{
					peek = peekSide;
					(ix, iy) = peek;
					continue;
				}
				peekSide = (peek.x + 1, peek.y);
				if (grid.GetValueAt(peekSide.x, peekSide.y) == null)
				{
					peek = peekSide;
					(ix, iy) = peek;
					continue;
				}
				if (oldy == iy)
				{
					stopped = true;
				}
			}
			grid.SetValueAt(ix, iy, 'O');
			if (ix == 500 && iy == 0)
			{
				return -1;
			}
			return 1;
		}

		/// <summary>
		/// return 1 if stopped, or -1 if fell out of bounds
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public int DropGrain(int x = 500, int y = 0)
		{
			bool stopped = false;
			var ix = x;
			var iy = y;
			(int x, int y) peek = (x, y + 1);

			while (peek.y < floor && !stopped)
			{
				var oldy = iy;

				peek = (peek.x, peek.y + 1);
				var closesRockBelow = grid.Points.Where(p => p.X == ix).Select(p => p.Y).Min(y => y);
				(ix, iy) = (peek.x, closesRockBelow - 1);

				if (grid.GetValueAt(peek.x, peek.y) == null)
				{
					(ix, iy) = peek;
					continue;
				}
				(int x, int y) peekLeft = (peek.x - 1, peek.y);
				if (grid.GetValueAt(peekLeft.x, peekLeft.y) == null)
				{
					peek = peekLeft;
					(ix, iy) = peek;
					continue;
				}
				(int x, int y) peekRight = (peek.x + 1, peek.y);
				if (grid.GetValueAt(peekRight.x, peekRight.y) == null)
				{
					peek = peekRight;
					(ix, iy) = peek;
					continue;
				}
				if (oldy == iy)
				{
					stopped = true;
				}
			}
			grid.SetValueAt(ix, iy, 'O');
			if (stopped == true)
			{
				return 1;
			}
			return -1;
		}

		public void PrintFromColumn(Day14Grid<char?> input, int startJ, int endJ = 520)
		{
			for (int i = 0; i < input.GetHeight(); i++)
			{
				for (int j = startJ; j < input.GetWidth() && j < endJ; j++)
				{
					Console.Write(input.Matrix[i, j] is null ? "." : $"{input.Matrix[i, j]}");
				}
				Console.WriteLine();
			}
		}
	}
}