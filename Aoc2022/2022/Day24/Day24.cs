using Grids;

namespace _2022.Day24
{
	public class Day24
	{
		const bool TEST = false;
		public readonly IEnumerable<string> _lines;
		public Dictionary<int, HashSet<Point<Direction>>> allStorms = new();
		// public Dictionary<(int x, int y, int minute), List<Direction>> forbidden = new();

		public Graph<string> graph;
		public int width;
		public int height;
		public int stormCycle;
		(int x, int y) destination;

		public Day24(bool test = TEST)
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
			BuildBlizzardGraph();
			var shortest = graph.GetDijkstra("START-0", "END");
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine($"Shortest path is length {shortest.Count - 1}");
			Console.ResetColor();
		}

		private void BuildBlizzardGraph()
		{
			for (int i = 0; i < stormCycle; i++)
			{
				CalculateStorms(i);
			}
			var nodesDict = new Dictionary<string, (IList<string>, int)>();

			for (int m = 0; m < stormCycle; m++)
			{
				for (int i = 0; i < width; i++)
				{
					for (int j = 0; j < width; j++)
					{
						if (GetStorm(m).Any(x => x.X == i && x.Y == j))
						{
							continue;
						}
						var nodeName = $"{i}-{j}-{m}";
						// get spaces that may be travelled to in the nth minute
						// REMEMBER TO START MOVING AT MINUTE ONE
						var attached = GetSpaces(i, j, m + 1);
						nodesDict[nodeName] = (attached.Select(p => $"{p.x}-{p.y}-{(m + 1) % stormCycle}").ToList(), 1);
					}
				}
				var startAttached = GetSpaces(0, -1, m + 1);
				nodesDict[$"START-{m}"] = (startAttached.Select(p => p.y == -1 ? $"START-{(m + 1) % stormCycle}" : $"{p.x}-{p.y}-{m + 1}").ToList(), 1);
				// might unnecessarily add an exit from a blizzard but it's directional so we don't care.
				nodesDict[$"{width - 1}-{height - 1}-{m}"] = (new List<string> { "END" }, 1);
			}
			nodesDict[$"END"] = (new List<string> { }, 1);
			graph = new Graph<string>(nodesDict);
		}

		public void Explore()
		{
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
			CalculateStorms(minute % (stormCycle));
			return allStorms[minute % (stormCycle)];
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
