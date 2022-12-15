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

		public HashSet<(int x, int y)> pointsToCheck = new();

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
			foreach (var sensor in sensors)
			{
				QueuePerimeter(sensor, closest[sensor].distance);
			}
		}

		private void QueuePerimeter((int x, int y) sensor, int manhattan)
		{
			var perimeterDistance = manhattan + 1;
			for (int xOffset = 0; xOffset <= perimeterDistance; xOffset++)
			{
				var yOffset = perimeterDistance - xOffset;

				pointsToCheck.Add((sensor.x + xOffset, sensor.y + yOffset));
				pointsToCheck.Add((sensor.x + xOffset, sensor.y - yOffset));
				pointsToCheck.Add((sensor.x - xOffset, sensor.y + yOffset));
				pointsToCheck.Add((sensor.x - xOffset, sensor.y - yOffset));
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
			foreach (var (x, y) in pointsToCheck.Where(p => p.x > 0 && p.x < upperBound && p.y > 0 && p.y < upperBound))
			{
				var point = (x, y);
				var hasSignal = false;
				foreach (var sensor in this.sensors)
				{
					var signalDelta = GetManhattan(point, sensor);
					var manhattan = closest[sensor].distance;
					if (signalDelta <= manhattan)
					{
						hasSignal = true;
					}
				}
				if (hasSignal)
				{
					continue;
				}
				deadX = x;
				deadY = y;
				break;
			}
			Console.WriteLine($"Dead spot is at {deadX}, {deadY}");
			long freq = (long)deadX * 4000000 + deadY;
			Console.WriteLine($"Answer is: {freq}");
		}
	}
}