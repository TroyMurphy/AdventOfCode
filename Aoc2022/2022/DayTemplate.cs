using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2022.DayTemplate
{
	public class DayTemplate
	{
		public readonly IEnumerable<string> _lines;

		public DayTemplate(bool test = true)
		{
			this._lines = GetLines(test);
		}

		private IEnumerable<string> GetLines(bool test)
		{
			return test
				? Utilities.ReadLines(@"./DayTemplate/inputs/demo1.txt")
				: Utilities.ReadLines(@"./DayTemplate/inputs/input.txt");
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