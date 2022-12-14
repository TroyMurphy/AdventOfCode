using Grids;
using System.Drawing;
using System.Security.Cryptography;

namespace _2022.Day14
{
	public class Day14
	{
		public readonly IEnumerable<string> _lines;
		public HashSet<(int X, int Y)> map;
		public int rockCount;
		public HashSet<(int, int)> rocks;
		public int floor;

		public Day14()
		{
			this._lines = Utilities.ReadLines(@"./Day14/inputs/demo1.txt");
			//this._lines = Utilities.ReadLines(@"./Day14/inputs/input.txt");

			this.map = new HashSet<(int, int)>();

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
					map.UnionWith(pointCoords.Select(p => (p.Item1, p.Item2)));
				}
			}
			rockCount = map.Count();
			rocks = new HashSet<(int, int)>(map);
			floor = map.Max(p => p.Y);
		}

		public void Solve()
		{
			//SolvePartOne();
			SolvePartTwo();
			//DrawPrettyGrid();
		}

		public void DrawPrettyGrid()
		{
			var saltColor = ConsoleColor.Yellow;
			var rockColor = ConsoleColor.DarkGray;
			var backColor = ConsoleColor.Black;
			var sw = map.Select(x => x.X).Min();
			var h = map.Select(x => x.Y).Max();
			var w = map.Select(x => x.X).Max();

			for (int y = 0; y <= h; y++)
			{
				for (int x = sw; x < w; x++)
				{
					if (rocks.Contains((x, y)))
					{
						Console.BackgroundColor = rockColor;
						Console.Write(" ");
						Console.BackgroundColor = backColor;
						continue;
					}
					if (map.Contains((x, y)))
					{
						Console.BackgroundColor = saltColor;
						Console.Write(" ");
						Console.BackgroundColor = backColor;
						continue;
					}
					Console.BackgroundColor = backColor;
					Console.Write(" ");
				}
				Console.ForegroundColor = backColor;
				Console.WriteLine();
			}
			Console.BackgroundColor = rockColor;
			Console.WriteLine();
			Console.BackgroundColor = backColor;
			Console.WriteLine();
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
			var it = 0;
			while (true)
			{
				it++;
				var response = DropGrainWithFloor();
				if (response == -1)
				{
					break;
				}
			}
			var grains = map.Count() - rockCount;
			Console.WriteLine($"Grains dropped was {grains}");
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
					map.Add((ix, iy));
					break;
				}
				var oldy = iy;
				peek = (peek.x, peek.y + 1);
				if (!map.Contains((peek.x, peek.y)))
				{
					(ix, iy) = peek;
					continue;
				}
				(int x, int y) peekSide = (peek.x - 1, peek.y);
				if (!map.Contains((peekSide.x, peekSide.y)))
				{
					peek = peekSide;
					(ix, iy) = peek;
					continue;
				}
				peekSide = (peek.x + 1, peek.y);
				if (!map.Contains((peekSide.x, peekSide.y)))
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

			map.Add((ix, iy));
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
				var closesRockBelow = map.Where(p => p.X == ix).Select(p => p.Y).Min(y => y);
				(ix, iy) = (peek.x, closesRockBelow - 1);

				if (!map.Contains((peek.x, peek.y)))
				{
					(ix, iy) = peek;
					continue;
				}
				(int x, int y) peekLeft = (peek.x - 1, peek.y);
				if (!map.Contains((peekLeft.x, peekLeft.y)))
				{
					peek = peekLeft;
					(ix, iy) = peek;
					continue;
				}
				(int x, int y) peekRight = (peek.x + 1, peek.y);
				if (!map.Contains((peekRight.x, peekRight.y)))
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
			map.Add((ix, iy));
			if (stopped == true)
			{
				return 1;
			}
			return -1;
		}
	}
}