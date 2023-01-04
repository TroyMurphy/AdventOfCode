using Grids;

namespace _2022.Day24
{
	public class Day24
	{
		public readonly IEnumerable<string> _lines;
		public Dictionary<int, HashSet<Point<Direction>>> allStorms = new();
		// public Dictionary<(int x, int y, int minute), List<Direction>> forbidden = new();
		public int width;
		public int height;
		public int stormCycle;
		(int x, int y) destination;

		public Day24(bool test = false)
		{
			this._lines = GetLines(test);
			this.height = this._lines.Count() - 2;
			this.width = this._lines.First().Length - 2;
			this.stormCycle = this.width * this.height;
			this.destination = (width - 1, height);

			int y = 0;
			int x = 0;
			var storms = new HashSet<Point<Direction>>();
			foreach (var line in _lines.Skip(1).Take(this._lines.Count() - 2))
			{
				foreach (var c in line.ToCharArray())
				{
					Direction d = c switch
					{
						'.' => Direction.None,
						'>' => Direction.Right,
						'v' => Direction.Down,
						'<' => Direction.Left,
						'^' => Direction.Up,
						'#' => Direction.Wall
					};
					if (d == Direction.Wall)
					{
						continue;
					}
					else if (d == Direction.None)
					{
						x++;
						continue;
					}
					storms.Add(new Point<Direction>(x, y, d));
					x++;
				}
				y++;
				x = 0;
			}
			allStorms[0] = storms;
		}

		private IEnumerable<string> GetLines(bool test)
		{
			return test
				? Utilities.ReadLines(@"./Day24/inputs/demo2.txt")
				: Utilities.ReadLines(@"./Day24/inputs/input.txt");
		}

		public void Solve()
		{
			SolvePartOne();
		}

		private void SolvePartOne()
		{
			Explore();
		}

		public void Explore()
		{
			var minute = 0;
			(int x, int y) position = (0, -1);

			List<Direction>? bestPath = null;


			List<Direction> path = new();
			PriorityQueue<(int x, int y, List<Direction> path, Direction d, int minute), int> toExplore = new();
			toExplore.Enqueue((position.x, position.y, path, Direction.None, minute), 1);

			while (toExplore.Count > 0)
			{
				var explore = toExplore.Dequeue();
				position = (explore.x, explore.y);
				minute = explore.minute;
				path = explore.path;
				path.Add(explore.d);
				if (bestPath is not null && path.Count > bestPath.Count)
				{
					continue;
				}
				var canVisit = GetSpaces(position.x, position.y, minute + 1);

				// DrawStorm(minute, position);

				// var forbiddenKey = (position.x, position.y, minute);
				// if (!forbidden.ContainsKey(forbiddenKey)) {
				// 	forbidden[forbiddenKey] = new List<Direction>();
				// }

				// cannot wait or move without getting hit

				if (canVisit.Any(x => x.x == destination.x && x.y == destination.y))
				{
					path.Add(Direction.Down);
					position = destination;
					var solution = path.Skip(1).ToList();

					if (bestPath == null || solution.Count < bestPath.Count)
					{
						bestPath = solution;
					}
					continue;
				}


				for (int i = 0; i < canVisit.Count; i++)
				{
					toExplore.Enqueue((canVisit[i].x, canVisit[i].y, new List<Direction>(path), canVisit[i].d, minute + 1), i + (1000 / (minute + 1)));
				}
			}
			Console.WriteLine("The path out is: ");
			Console.WriteLine(string.Join(",", bestPath));
			Console.WriteLine($"The length is {bestPath.Count()}");

		}

		private void SolvePartTwo()
		{
		}

		public List<(Direction d, int x, int y)> GetSpaces(int x, int y, int minute)
		{
			var result = new List<(Direction, int, int)>();
			var storm = GetStorm(minute);
			var rightStorm = storm.FirstOrDefault(p => p.X == x + 1 && p.Y == y);
			var downStorm = storm.FirstOrDefault(p => p.X == x && p.Y == y + 1);
			var upStorm = storm.FirstOrDefault(p => p.X == x && p.Y == y - 1);
			var leftStorm = storm.FirstOrDefault(p => p.X == x - 1 && p.Y == y);
			var hereStorm = storm.FirstOrDefault(p => p.X == x && p.Y == y);

			if (rightStorm is null && x + 1 < width && y >= 0)
			{
				result.Add((Direction.Right, x + 1, y));
			}
			if (downStorm is null && (y + 1 < height) || y + 1 == height && x == destination.x)
			{
				result.Add((Direction.Down, x, y + 1));
			}
			if (hereStorm is null)
			{
				result.Add((Direction.None, x, y));
			}
			if (upStorm is null && y >= 1)
			{
				result.Add((Direction.Up, x, y - 1));
			}
			if (leftStorm is null && x >= 1 && y >= 0)
			{
				result.Add((Direction.Left, x - 1, y));
			}
			return result;
		}

		public void CalculateStorms(int minute)
		{
			if (allStorms.ContainsKey(minute))
			{
				return;
			}
			var storm = allStorms[minute - 1];
			var newStorm = new HashSet<Point<Direction>>();

			foreach (var s in storm)
			{
				int newX = s.X;
				int newY = s.Y;
				switch (s.Value)
				{
					case Direction.Right:
						newX++;
						break;
					case Direction.Down:
						newY++;
						break;
					case Direction.Left:
						newX--;
						break;
					case Direction.Up:
						newY--;
						break;
				}
				newX = (newX + width) % width;
				newY = (newY + height) % height;
				newStorm.Add(new Point<Direction>(newX, newY, s.Value));
			}
			allStorms[minute] = newStorm;
		}

		public HashSet<Point<Direction>> GetStorm(int minute)
		{
			CalculateStorms(minute % (width * height));
			return allStorms[minute];
		}
		public enum Direction
		{
			None = 0,
			Right = 1,
			Down = 2,
			Left = 3,
			Up = 4,
			Wall = -1
		}

		public void DrawStorm(int minute, (int x, int y) position)
		{
			Console.WriteLine($"Minute {minute}:");
			var storm = allStorms[minute];

			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					if (position == (x, y))
					{
						Console.Write("E");
						continue;
					}
					var s = storm.Where(p => p.X == x && p.Y == y);
					string output;
					if (!s.Any())
					{
						output = ".";
					}
					else if (s.Count() > 1)
					{
						output = s.Count().ToString();
					}
					else
					{
						output = s.First().Value switch
						{
							Direction.Right => ">",
							Direction.Down => "v",
							Direction.Left => "<",
							Direction.Up => "^",
							_ => throw new Exception()
						};
					}
					Console.Write(output);
				}
				Console.WriteLine();
			}
		}
	}
}
