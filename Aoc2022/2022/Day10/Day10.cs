using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2022.Day10
{
	public class Day10
	{
		public readonly IEnumerable<string> _lines;

		public Day10()
		{
			this._lines = Utilities.ReadLines(@"./Day10/inputs/input.txt");
		}

		public void Solve()
		{
			//SolvePartOne();
			SolvePartTwo();
		}

		public Dictionary<int, int> BuildDuringCycleValues()
		{
			var result = new Dictionary<int, int>();
			List<int> inputs = this._lines.Select(x =>
				x switch
				{
					"noop" => default,
					_ => int.Parse(x.Split(" ").Last())
				}
			).ToList();

			var xRegister = 1;
			var cycleIndex = 1;
			var inputIndex = 0;
			result[cycleIndex] = 1;

			while (inputIndex < inputs.Count)
			{
				var v = inputs[inputIndex];
				if (v == 0)
				{
					result[++cycleIndex] = xRegister;
				}
				else
				{
					result[++cycleIndex] = xRegister;
					xRegister += v;
					result[++cycleIndex] = xRegister;
				}
				inputIndex++;
			}

			return result;
		}

		private void SolvePartOne()
		{
			var inputs = BuildDuringCycleValues();
			int signalStrength = 0;
			foreach (var strengthVal in new int[] { 20, 60, 100, 140, 180, 220 })
			{
				signalStrength += strengthVal * inputs[strengthVal];
			}

			Console.WriteLine($"Signal strength is {signalStrength}");
		}

		private void SolvePartTwo()
		{
			var inputs = BuildDuringCycleValues();
			for (int row = 0; row < 6; row++)
			{
				for (int pixel = 0; pixel < 40; pixel++)
				{
					var xRegister = inputs[(40 * row) + pixel + 1];
					var sprite = new int[] { xRegister - 1, xRegister, xRegister + 1 };
					if (sprite.Contains(pixel))
					{
						Console.Write("#");
						continue;
					}
					Console.Write(".");
				}
				Console.WriteLine();
			}
		}
	}
}