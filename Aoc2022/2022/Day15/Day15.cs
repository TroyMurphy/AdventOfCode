using System.ComponentModel;
using System.Drawing;
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

		public HashSet<(int x, int y)> sensors;
		public HashSet<(int x, int y)> beacons;

		public Day15()
		{
			this.sensors = new();
			this.beacons = new();
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
				var beacon = (bx, by);

				this.sensors.Add(sensor);
				this.beacons.Add(beacon);
				var manhattan = GetManhattan(sensor, beacon);
				this.closest[sensor] = ((bx, by), manhattan);
			}
		}

		private int GetManhattan((int, int) a, (int, int) b)
		{
			var (ax, ay) = a;
			var (bx, by) = b;
			return Math.Max(ax, bx) - Math.Min(ax, bx) + Math.Max(ay, by) - Math.Min(ay, by);
		}

		public void Solve()
		{
			SolvePartTwo();
		}

		private void SolvePartTwo()
		{
			int deadX = 0;
			int deadY = 0;

			var found = false;
			for (int x = 0; x <= upperBound; x++)
			{
				for (int y = 0; y <= upperBound; y++)
				{
					var point = (x, y);
					foreach (var sensor in this.sensors)
					{
						if (sensors.Contains(point) || beacons.Contains(point))
						{
							goto NextPoint;
						}
						var signalDelta = GetManhattan(point, sensor);
						var manhattan = closest[sensor].distance;
						if (signalDelta <= manhattan)
						{
							y += manhattan - signalDelta;
							goto NextPoint;
						}
					}
					found = true;
					deadX = x;
					deadY = y;
					break;
				NextPoint:
					continue;
				}
				if (found)
				{
					break;
				}
			}
			Console.WriteLine($"Dead spot is at {deadX}, {deadY}");
			Console.WriteLine($"Answer is: {(double)(deadX * 4000000 + deadY)}");
		}
	}
}