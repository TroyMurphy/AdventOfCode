using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2022.Day24
{
	public class Day24
	{
		public readonly IEnumerable<string> _lines;

		public Day24(bool test = true)
		{
			this._lines = GetLines(test);
		}

		private IEnumerable<string> GetLines(bool test)
		{
			return test
				? Utilities.ReadLines(@"./Day24/inputs/demo1.txt")
				: Utilities.ReadLines(@"./Day24/inputs/input.txt");
		}

		public void Solve()
		{
		}

		private void SolvePartOne()
		{
		}

		private void SolvePartTwo()
		{
		}
	}
}
