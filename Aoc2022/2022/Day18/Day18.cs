using System.Security.Cryptography.X509Certificates;

namespace _2022.Day18
{
	public class Day18
	{
		public readonly IEnumerable<string> _lines;
		public HashSet<(int x, int y, int z)> cubes = new();
		public HashSet<(int x, int y, int z)> water = new();
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
				var faces = GenSurrounding(cube).ToHashSet();
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
			minX = cubes.Min(p => p.x) - 1;
			minY = cubes.Min(p => p.y) - 1;
			minZ = cubes.Min(p => p.z) - 1;
			maxX = cubes.Max(p => p.x) + 1;
			maxY = cubes.Max(p => p.y) + 1;
			maxZ = cubes.Max(p => p.z) + 1;
			FillWater((minX, minY, minZ));
			foreach (var cube in cubes)
			{
				var faces = GenSurrounding(cube).ToHashSet();
				if (faces is null)
				{
					throw new Exception();
				}
				faces.ExceptWith(cubes);
				facesCount += faces.Where(x => IsOuterSurface(x)).Count();
			}
			Console.WriteLine($"There are {facesCount} faces outside");
		}

		private void FillWater((int x, int y, int z) point)
		{
			// Only add cubes if they are in bounds, and not already water
			Queue<(int x, int y, int z)> toFill = new();
			toFill.Enqueue(point);

			while (toFill.Count > 0)
			{
				var checkCube = toFill.Dequeue();
				if (cubes.Contains(checkCube))
				{
					continue;
				}
				water.Add(checkCube);
				var surrounding = GenSurrounding(checkCube);

				foreach ((int x, int y, int z) neighbor in surrounding)
				{
					if (toFill.Contains(neighbor) || water.Contains(neighbor) || (neighbor.x < minX || neighbor.x > maxX ||
						neighbor.y < minY || neighbor.y > maxY ||
						neighbor.z < minZ || neighbor.z > maxZ))
					{
						continue;
					}
					toFill.Enqueue(neighbor);
				}
			}
		}

		private bool IsOuterSurface((int x, int y, int z) point) => water.Contains(point);

		public enum Direction
		{
			X, Y, Z
		}

		public List<(int, int, int)> GenSurrounding((int x, int y, int z) point)
		{
			var result = new List<(int, int, int)>();
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