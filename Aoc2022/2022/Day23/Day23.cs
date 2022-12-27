namespace _2022.Day23
{
	public class Day23
	{
		public readonly IEnumerable<string> _lines;
		public List<Elf> ElfSet = new();
		public HashSet<(int x, int y)> ElfPositions;
		public DefaultDictionary<(int x, int y), int> AllProposed;
		public Queue<Direction> Directions = new();

		public Day23(bool test = false)
		{
			this._lines = GetLines(test);
			var y = 0;
			foreach (var line in _lines)
			{
				var x = 0;
				foreach (var c in line.ToCharArray())
				{
					if (c == '#')
					{
						ElfSet.Add(new Elf(x, y));
					}
					x++;
				}
				y++;
			}

			this.ElfSet.Select(x => x.Position).ToHashSet();

			this.Directions.Enqueue(Direction.N);
			this.Directions.Enqueue(Direction.S);
			this.Directions.Enqueue(Direction.W);
			this.Directions.Enqueue(Direction.E);
		}

		private IEnumerable<string> GetLines(bool test)
		{
			return test
				? Utilities.ReadLines(@"./Day23/inputs/demo1.txt")
				: Utilities.ReadLines(@"./Day23/inputs/input.txt");
		}

		public void Solve()
		{
			SolvePartOne();
		}

		private void SolvePartOne()
		{
			var rounds = 10;
			for (int i = 0; i < rounds; i++)
			{
				var success = DoRound();
				if (success == false)
				{
					break;
				}
			}

			var minX = this.ElfSet.Select(x => x.Position.x).Min();
			var maxX = this.ElfSet.Select(x => x.Position.x).Max();
			var minY = this.ElfSet.Select(x => x.Position.y).Min();
			var maxY = this.ElfSet.Select(x => x.Position.y).Max();

			var width = (maxX - minX) + 1;
			var height = (maxY - minY) + 1;

			var clear = (width * height) - ElfSet.Count;
			Console.WriteLine($"There are {clear} grass spots");
		}

		private void SolvePartTwo()
		{
		}

		public bool DoRound()
		{
			this.ElfPositions = ElfSet.Select(x => x.Position).ToHashSet();
			this.AllProposed = new(0);
			this.ElfSet.ForEach(x => x.Proposed = null);

			foreach (var elf in ElfSet)
			{
				var neighbors = AllNeighbors(elf.Position);
				neighbors.IntersectWith(ElfPositions);
				if (neighbors.Count == 0)
				{
					elf.Proposed = elf.Position;
				}
			}
			var willPropose = ElfSet.Where(x => x.Proposed is null);
			if (willPropose.Count() == 0)
			{
				return false;
			}

			foreach (var elf in willPropose)
			{
				foreach (var direction in this.Directions)
				{
					var neighbors = Neighbors(elf.Position, direction);
					neighbors.IntersectWith(ElfPositions);
					if (neighbors.Count == 0)
					{
						elf.SetProposed(direction);
						this.AllProposed[elf.Proposed.Value] += 1;
						continue;
					}
				}
			}

			foreach (var elf in ElfSet.Where(x => x.Proposed is not null))
			{
				if (AllProposed[elf.Proposed.Value] == 1)
				{
					elf.Position = elf.Proposed.Value;
				}
			}

			CycleDirection();
			return true;
		}

		public enum Direction
		{
			N, S, W, E
		}

		internal void CycleDirection()
		{
			this.Directions.Enqueue(this.Directions.Dequeue());
		}

		public HashSet<(int x, int y)> AllNeighbors((int x, int y) point)
		{
			var results = new HashSet<(int x, int y)>();
			for (int xoff = -1; xoff <= 1; xoff++)
			{
				for (int yoff = -1; yoff <= 1; yoff++)
				{
					if (xoff == 0 && yoff == 0)
					{
						continue;
					}
					results.Add((point.x + xoff, point.y + yoff));
				}
			}
			return results;
		}

		public HashSet<(int x, int y)> Neighbors((int x, int y) p, Direction d)
		{
			var results = new HashSet<(int x, int y)>();
			switch (d)
			{
				case Direction.N:
					results.Add((p.x - 1, p.y - 1));
					results.Add((p.x, p.y - 1));
					results.Add((p.x + 1, p.y - 1));
					break;

				case Direction.S:
					results.Add((p.x - 1, p.y + 1));
					results.Add((p.x, p.y + 1));
					results.Add((p.x + 1, p.y + 1));
					break;

				case Direction.W:
					results.Add((p.x - 1, p.y - 1));
					results.Add((p.x - 1, p.y));
					results.Add((p.x - 1, p.y + 1));
					break;

				case Direction.E:
					results.Add((p.x + 1, p.y - 1));
					results.Add((p.x + 1, p.y));
					results.Add((p.x + 1, p.y + 1));
					break;
			}
			return results;
		}

		public class Elf
		{
			public (int x, int y) Position;

			public (int x, int y)? Proposed;

			public Elf(int x, int y)
			{
				this.Position = (x, y);
			}

			public void SetProposed(Direction d)
			{
				this.Proposed = d switch
				{
					Direction.N => (this.Position.x, this.Position.y - 1),
					Direction.E => (this.Position.x + 1, this.Position.y),
					Direction.S => (this.Position.x, this.Position.y + 1),
					Direction.W => (this.Position.x - 1, this.Position.y)
				};
			}
		}
	}
}