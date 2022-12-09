using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2022.Day4
{
	public class Day4
	{
		public readonly IEnumerable<string> _lines;


		public Day4()
		{
			this._lines = Utilities.ReadLines(@"./Day4/inputs/input.txt");
		}


		public void Solve()
		{
			SolvePartTwo();
		}

		private void SolvePartOne()
		{
			var fullyContainedSets = _lines.Select(l =>
			{
				var parts = l.Split(",");
				var rangeA = parts.First().Split("-");
				var startA = int.Parse(rangeA.First());
				var endA = int.Parse(rangeA.Last());
				var rangeB = parts.Last().Split("-");
				var startB = int.Parse(rangeB.First());
				var endB = int.Parse(rangeB.Last());

				var setA = new HashSet<int>(Enumerable.Range(startA, (endA - startA) + 1));
				var setB = new HashSet<int>(Enumerable.Range(startB, (endB - startB) + 1));

				var intersect = setA.Intersect(setB);
				return (intersect.SequenceEqual(setA) || intersect.SequenceEqual(setB)) ? 1 : 0;
			});
			Console.WriteLine($"Sum is {fullyContainedSets.Sum()}");
		}

		private void SolvePartTwo()
		{
			var fullyContainedSets = _lines.Select(l =>
			{
				var parts = l.Split(",");
				var rangeA = parts.First().Split("-");
				var startA = int.Parse(rangeA.First());
				var endA = int.Parse(rangeA.Last());
				var rangeB = parts.Last().Split("-");
				var startB = int.Parse(rangeB.First());
				var endB = int.Parse(rangeB.Last());

				var setA = new HashSet<int>(Enumerable.Range(startA, (endA - startA) + 1));
				var setB = new HashSet<int>(Enumerable.Range(startB, (endB - startB) + 1));

				var intersect = setA.Intersect(setB);
				return (intersect.Count() > 0) ? 1 : 0;
			});
			Console.WriteLine($"Sum is {fullyContainedSets.Sum()}");

		}
	}
}

