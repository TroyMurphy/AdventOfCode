using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace _2022.Day21
{
	public class Day21
	{
		public readonly IEnumerable<string> _lines;
		public Dictionary<string, Monkey> Monkeys { get; set; } = new();

		public Day21(bool test = false)
		{
			this._lines = GetLines(test);
			foreach (var line in this._lines)
			{
				var monkey = new Monkey(line, Monkeys);
				this.Monkeys[monkey.Id] = monkey;
			}
		}

		private IEnumerable<string> GetLines(bool test)
		{
			return test
				? Utilities.ReadLines(@"./Day21/inputs/demo1.txt")
				: Utilities.ReadLines(@"./Day21/inputs/input.txt");
		}

		public void Solve()
		{
			SolvePartOne();
		}

		private void SolvePartOne()
		{
			var root = this.Monkeys["root"];
			var yell = root.GetValue();
			Console.WriteLine($"Root yells: {yell}");
		}

		private void SolvePartTwo()
		{
		}
	}

	public class Monkey
	{
		public Dictionary<string, Monkey> MonkeyDict { get; init; }
		public string Id { get; init; }
		public long? Number { get; set; }

		public string Monkey1Key { get; set; }
		public string Monkey2Key { get; set; }
		public string Operator { get; set; }

		public long GetValue()
		{
			if (this.Number.HasValue)
			{
				return this.Number.Value;
			}
			var m1 = MonkeyDict[this.Monkey1Key];
			var m2 = MonkeyDict[this.Monkey2Key];

			this.Number = this.Operator switch
			{
				"+" => m1.GetValue() + m2.GetValue(),
				"*" => m1.GetValue() * m2.GetValue(),
				"-" => m1.GetValue() - m2.GetValue(),
				"/" => m1.GetValue() / m2.GetValue(),
				_ => throw new Exception()
			};
			return this.Number.Value;
		}

		public Monkey(string input, Dictionary<string, Monkey> monkeyDict)
		{
			var dependentPattern = @"(\w+): (\w+) ([+*-/]) (\w+)";
			var constRegex = @"(\w+): (\d+)";

			var simple = Regex.Match(input, constRegex);
			if (simple.Success)
			{
				this.Id = simple.Groups[1].Captures[0].Value;
				this.Number = long.Parse(simple.Groups[2].Captures[0].Value);
			}
			else
			{
				var complex = Regex.Match(input, dependentPattern);
				this.Id = complex.Groups[1].Captures[0].Value;
				this.Number = null;
				this.Monkey1Key = complex.Groups[2].Captures[0].Value;
				this.Operator = complex.Groups[3].Captures[0].Value;
				this.Monkey2Key = complex.Groups[4].Captures[0].Value;
			}
			this.MonkeyDict = monkeyDict;
			this.MonkeyDict[this.Id] = this;
		}
	}
}