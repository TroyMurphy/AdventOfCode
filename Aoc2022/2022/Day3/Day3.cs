using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace _2022.Day3
{
	public class Day3
	{
		public readonly IEnumerable<string> _lines;


		public Day3()
		{
			this._lines = Utilities.ReadLines(@"./Day3/inputs/input.txt");
		}


		public void Solve()
		{
			SolvePartTwo();
		}

		private void SolvePartOne()
		{
			var rucksacks = _lines
				.Select(x => x.ToCharArray())
				.Select(a => (
					new HashSet<char>(a.Take(a.Length / 2).ToArray()),
					new HashSet<char>(a.Skip(a.Length / 2).ToArray())
				));

			var common = rucksacks.Select((r) =>
			{
				var (a, b) = r;
				return a.Intersect(b);
			});

			// lowercase a has ASCII value of
			var priorities = common.Select(chars =>
			{
				var c = chars.First();
				var s = c.ToString();
				if (s.ToLower() == s)
				{
					return (int)c - (96);
				}
				return (int)c - 65 + 27;
			});

			Console.WriteLine($"All priorities sum to be {priorities.Sum()}");
		}

		private void SolvePartTwo()
		{
			var badges = new List<char>();
			var rucksacks = _lines.ToList();
			for(int i = 0; i < rucksacks.Count(); i+=3)
			{
				badges.Add(rucksacks
					.Skip(i)
					.Take(3)
					.Select(x =>
						new HashSet<char>(x.ToCharArray())
					)
					.Aggregate((a, x) => new HashSet<char>(x.Intersect(a)))
					.First());
			}
			// lowercase a has ASCII value of
			var priorities = badges.Select(c =>
			{
				var s = c.ToString();
				if (s.ToLower() == s)
				{
					return (int)c - (96);
				}
				return (int)c - 65 + 27;
			});

			Console.WriteLine($"All priorities sum to be {priorities.Sum()}");

		}
	}
}

