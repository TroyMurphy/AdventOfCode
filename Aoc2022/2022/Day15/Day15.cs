using System.Text.RegularExpressions;
using System.Xml.Schema;

namespace _2022.Day15
{
	public class Day15
	{
		public bool test = false;

		public readonly IEnumerable<string> _lines;
		public int targetLine;
		public int upperBound;

		//public HashSet<(int x, int y)> sensors;
		//public HashSet<(int x, int y)> beacons;
		public Dictionary<(int x, int y), ((int x, int y) beacon, int distance)> closest;

		public HashSet<(int x, int y)> noBeacons;

		public Day15()
		{
			//this.sensors = new();
			//this.beacons = new();
			this.noBeacons = new();
			this.closest = new();
			if (test)
			{
				this._lines = Utilities.ReadLines(@"./Day15/inputs/demo1.txt");
				this.targetLine = 10;
				this.upperBound = 20;
			}
			else
			{
				this._lines = Utilities.ReadLines(@"./Day15/inputs/input.txt");
				this.targetLine = 2000000;
				this.upperBound = 4000000;
			}

			var lineRegex = @".*x=([-]?\d+), y=([-]?\d+).*x=([-]?\d+), y=([-]?\d+).*";
			foreach (var line in this._lines)
			{
				Regex r = new Regex(lineRegex);
				Match m = r.Match(line);
				var sx = int.Parse(m.Groups[1].ToString());
				var sy = int.Parse(m.Groups[2].ToString());
				var bx = int.Parse(m.Groups[3].ToString());
				var by = int.Parse(m.Groups[4].ToString());

				var sensor = (sx, sy);

				//this.sensors.Add(sensor);
				//this.beacons.Add((bx, by));
				var manhattan = Math.Max(sx, bx) - Math.Min(sx, bx) + Math.Max(sy, by) - Math.Min(sy, by);
				this.closest[sensor] = ((bx, by), manhattan);
			}
		}

		public void Solve()
		{
			SolvePartOne();
		}

		private void SolvePartOne()
		{
			foreach (var sensor in closest.Keys)
			{
				var (sx, sy) = sensor;
				var (beacon, manhattan) = closest[sensor];
				var (bx, by) = beacon;
				if (!(sy - manhattan < targetLine && sy + manhattan > targetLine))
				{
					continue;
				}

				var yOffset = targetLine - sy;
				for (int xOffset = manhattan - Math.Abs(yOffset); xOffset >= 0; xOffset--)
				{
					noBeacons.Add((sx + xOffset, targetLine));
					noBeacons.Add((sx - xOffset, targetLine));
				}
			}
			noBeacons.ExceptWith(closest.Values.Select(x => x.beacon));

			var eliminated = noBeacons.Where(p => p.y == targetLine);
			Console.WriteLine($"In the row where y={targetLine}, there are {eliminated.Count()} positions");
		}

		private void SolvePartTwo()
		{
		}
	}
}