using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2022.Day1
{
	public class Day1
	{
		public readonly IEnumerable<string> _lines;


		public Day1()
		{
			this._lines = Utilities.ReadLines(@"./Day1/inputs/input.txt");
		}


		public void Solve()
		{
			SolvePartTwo();
		}

		private void SolvePartOne()
		{
			List<List<int>> calorieGroups = new();
			var calorieList = new List<int>();
			foreach (var line in _lines)
			{
				if (line == String.Empty)
				{
					calorieList = new List<int>();
					calorieGroups.Add(calorieList);
					continue;
				}
				calorieList.Add(int.Parse(line));
			}
			calorieGroups.Add(calorieList);

			var max = calorieGroups.OrderByDescending(x => x).First();
			Console.WriteLine($"Max sum is {max}");
		}
		private void SolvePartTwo()
		{
			List<List<int>> calorieGroups = new();
			var calorieList = new List<int>();
			foreach (var line in _lines)
			{
				if (line == String.Empty)
				{
					calorieList = new List<int>();
					calorieGroups.Add(calorieList);
					continue;
				}
				calorieList.Add(int.Parse(line));
			}
			calorieGroups.Add(calorieList);

			var max = calorieGroups.Select(x => x.Sum()).OrderByDescending(x => x).Take(3).Sum();

			Console.WriteLine($"Max sum is {max}");
		}
	}
}

