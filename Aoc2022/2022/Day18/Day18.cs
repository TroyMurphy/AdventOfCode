using System.Security.Cryptography.X509Certificates;

namespace _2022.Day18
{
	public class Day18
	{
		public readonly IEnumerable<string> _lines;
		public HashSet<(int x, int y, int z)> cubes = new();
		public HashSet<(int x, int y, int z)> insideCubes = new();
		public long facesCount = 0;

		public int minX;
		public int minY;
		public int minZ;
		public int maxX;
		public int maxY;
		public int maxZ;

		public Day18(bool test = false)
		{
			this._lines = GetLines(test);
			foreach (var line in _lines)
			{
				var cArray = line.Split(",").Select(x => int.Parse(x)).ToArray();
				if (cArray is null || cArray.Length != 3)
				{
					throw new Exception();
				}
				cubes.Add((cArray[0], cArray[1], cArray[2]));
			}
		}

		private IEnumerable<string> GetLines(bool test)
		{
			return test
				? Utilities.ReadLines(@"./Day18/inputs/demo1.txt")
				: Utilities.ReadLines(@"./Day18/inputs/input.txt");
		}

		public void Solve()
		{
			SolvePartTwo();
		}

		private void SolvePartOne()
		{
			foreach (var cube in cubes)
			{
				var faces = GenSurrounding(cube);
				if (faces is null)
				{
					throw new Exception();
				}
				faces.ExceptWith(cubes);
				facesCount += faces.Count;
			}
			Console.WriteLine($"There are {facesCount} faces");
		}

		private void SolvePartTwo()
		{
			minX = cubes.Min(p => p.x);
			minY = cubes.Min(p => p.y);
			minZ = cubes.Min(p => p.z);
			maxX = cubes.Max(p => p.x);
			maxY = cubes.Max(p => p.y);
			maxZ = cubes.Max(p => p.z);
			foreach (var cube in cubes)
			{
				var faces = GenSurrounding(cube);
				if (faces is null)
				{
					throw new Exception();
				}
				faces.ExceptWith(cubes);
				facesCount += faces.Where(x => !IsInside(x)).Count();
			}
			Console.WriteLine($"There are {facesCount} faces outside");
		}

		private bool IsInside((int x, int y, int z) point)
		{
			if (insideCubes.Contains(point))
			{
				return true;
			}
			var result = DoesHitSurrounding(point, Direction.X) && DoesHitSurrounding(point, Direction.Y) && DoesHitSurrounding(point, Direction.Z);
			if (result)
			{
				insideCubes.Add(point);
			}
			return result;
		}

		public enum Direction
		{
			X, Y, Z
		}

		private bool DoesHitSurrounding((int x, int y, int z) point, Direction d)
		{
			var hitsIncreasing = false;
			var hitsDecreasing = false;
			int checkVal;
			int max;
			int min;
			switch (d)
			{
				case (Direction.X):
					checkVal = point.x;
					max = maxX;
					min = minX;
					break;

				case (Direction.Y):
					checkVal = point.y;
					max = maxY;
					min = minY;
					break;

				case (Direction.Z):
					checkVal = point.z;
					max = maxZ;
					min = minZ;
					break;

				default:
					throw new Exception();
			};

			var reset = checkVal;
			while (checkVal <= max)
			{
				var checkPoint = d switch
				{
					Direction.X => (++checkVal, point.y, point.z),
					Direction.Y => (point.x, ++checkVal, point.z),
					Direction.Z => (point.x, point.y, ++checkVal),
					_ => throw new Exception()
				};
				if (cubes.Contains(checkPoint))
				{
					hitsIncreasing = true;
					break;
				}
			}
			if (!hitsIncreasing)
			{
				return false;
			}
			checkVal = reset;
			while (checkVal >= min)
			{
				var checkPoint = d switch
				{
					Direction.X => (--checkVal, point.y, point.z),
					Direction.Y => (point.x, --checkVal, point.z),
					Direction.Z => (point.x, point.y, --checkVal),
					_ => throw new Exception()
				};
				if (cubes.Contains(checkPoint))
				{
					hitsDecreasing = true;
					break;
				}
			}
			return hitsDecreasing;
		}

		public HashSet<(int, int, int)> GenSurrounding((int x, int y, int z) point)
		{
			var result = new HashSet<(int, int, int)>();
			for (int r = 0; r < 3; r++)
			{
				switch (r)
				{
					case 0:
						result.Add((point.x + 1, point.y, point.z));
						result.Add((point.x - 1, point.y, point.z));
						break;

					case 1:
						result.Add((point.x, point.y + 1, point.z));
						result.Add((point.x, point.y - 1, point.z));
						break;

					case 2:
						result.Add((point.x, point.y, point.z + 1));
						result.Add((point.x, point.y, point.z - 1));
						break;

					default:
						throw new Exception();
				}
			}
			return result;
		}
	}
}