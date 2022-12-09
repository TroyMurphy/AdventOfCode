using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2022.Day2
{
	public class Day2
	{
		public readonly IEnumerable<string> _lines;


		public Day2()
		{
			//this._lines = Utilities.ReadLines(@"./Day2/inputs/demo1.txt");
			this._lines = Utilities.ReadLines(@"./Day2/inputs/input.txt");
		}


		public void Solve()
		{
			SolvePartTwo();

		}

		private void SolvePartOne()
		{
			var tuples = this._lines
				.Select(x => x.Split(" "))
				.Select(x => (x[0], x[1]))
				.Select(x =>
				{
					var (a, b) = x;
					var elf = a switch { "A" => 1, "B" => 2, "C" => 3, _ => 0 };
					var me = b switch { "X" => 1, "Y" => 2, "Z" => 3, _ => 0 };
					return (elf, me);
				});

			var scoreTotals = tuples.Select(x =>
			{
				var (elf, me) = x;
				var score = me;
				var outcome = (me - elf);
				score += outcome switch
				{
					1 => 6,
					0 => 3,
					-2 => 6,
					_ => 0
				};
				return score;
			});


			Console.WriteLine($"Score total is {scoreTotals.Sum()}");
		}

		private void SolvePartTwo()
		{
			var tuples = this._lines
				.Select(x => x.Split(" "))
				.Select(x => (x[0], x[1]))
				.Select(x =>
				{
					var (a, b) = x;
					var elf = a switch { "A" => 1, "B" => 2, "C" => 3, _ => 0 };
					//var me = b switch { "X" => 1, "Y" => 2, "Z" => 3, _ => 0 };
					//win
					// 1 => 2
					// 2 => 3
					// 3 => 1
					// lose
					// 1 => 3
					// 2 => 1
					// 3 => 2

					var me = b switch
					{
						"X" => elf == 1 ? 3 : elf - 1,
						"Y" => elf,
						"Z" => elf == 3 ? 1 : elf + 1,
						_ => throw new Exception()
					};

					return (elf, me);
				});

			var scoreTotals = tuples.Select(x =>
			{
				var (elf, me) = x;
				var score = me;
				var outcome = (me - elf);
				score += outcome switch
				{
					1 => 6,
					0 => 3,
					-2 => 6,
					_ => 0
				};
				return score;
			});


			Console.WriteLine($"Score total is {scoreTotals.Sum()}");
		}
	}
}

