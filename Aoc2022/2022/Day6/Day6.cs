using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2022.Day6
{
	public class Day6
	{
		public readonly IEnumerable<string> _lines;


		public Day6()
		{
			this._lines = Utilities.ReadLines(@"./Day6/inputs/input.txt");
		}


		public void Solve()
		{
			SolvePartOne();
		}

		private void SolvePartOne()
		{
			var chars = this._lines.First().ToCharArray();
			var magicIndex = 0;

			for(int i = 0; i < chars.Length; i++)
			{
				var set = new HashSet<char>(chars.Skip(i).Take(4));
				if (set.Count == 4)
				{
					magicIndex = i + 4;
					break;
				}
			}
			Console.WriteLine($"Magic Index is: {magicIndex}");
		}

		private void SolvePartTwo()
		{
			var chars = this._lines.First().ToCharArray();
			var magicIndex = 0;

			for(int i = 0; i < chars.Length; i++)
			{
				var set = new HashSet<char>(chars.Skip(i).Take(14));
				if (set.Count == 14)
				{
					magicIndex = i + 14;
					break;
				}
			}
			Console.WriteLine($"Magic Index is: {magicIndex}");
		}
	}
}

