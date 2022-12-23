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
			//SolvePartOne();
			SolvePartTwo();
		}

		private void SolvePartOne()
		{
			var root = this.Monkeys["root"];
			var yell = root.GetValue();
			Console.WriteLine($"Root yells: {yell}");
		}

		private void SolvePartTwo()
		{
			var root = this.Monkeys["root"];
			//root.PrintEqual();
			root.ForceMonkey(this.Monkeys[root.Monkey1Key], this.Monkeys[root.Monkey2Key].GetValue().Value);
			Console.WriteLine($"Human should yell {this.Monkeys["humn"].Number}");
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

		public long? GetValue()
		{
			if (this.Id == "humn")
			{
				return null;
			}

			if (this.Number.HasValue)
			{
				return this.Number.Value;
			}
			var m1 = MonkeyDict[this.Monkey1Key].GetValue();
			var m2 = MonkeyDict[this.Monkey2Key].GetValue();

			if (m1 is null || m2 is null)
			{
				return null;
			}

			this.Number = this.Operator switch
			{
				"+" => m1 + m2,
				"*" => m1 * m2,
				"-" => m1 - m2,
				"/" => m1 / m2,
				_ => throw new Exception()
			};
			return this.Number.Value;
		}

		public void PrintEqual()
		{
			var m1 = MonkeyDict[this.Monkey1Key].GetValue();
			var m2 = MonkeyDict[this.Monkey2Key].GetValue();

			Console.WriteLine($"Monkey 1 is {m1} and Monkey 2 is {m2}");
		}

		public void ForceMonkey(Monkey unknownMonkey, long forcedValue)
		{
			if (unknownMonkey.Id == "humn")
			{
				unknownMonkey.Number = forcedValue;
				return;
			}
			var m1 = MonkeyDict[unknownMonkey.Monkey1Key];
			var m2 = MonkeyDict[unknownMonkey.Monkey2Key];
			long? m1Val = m1.GetValue();

			if (m1Val is null)
			{
				long m2Val = m2.GetValue() ?? throw new Exception();
				var value = unknownMonkey.Operator switch
				{
					"+" => forcedValue - m2Val,
					"-" => forcedValue + m2Val,
					"*" => forcedValue / m2Val,
					"/" => forcedValue * m2Val
				};
				ForceMonkey(m1, value);
			}
			if (m2.GetValue() is null)
			{
				var value = unknownMonkey.Operator switch
				{
					"+" => forcedValue - m1Val,
					"-" => m1Val - forcedValue,
					"*" => forcedValue / m1Val,
					"/" => m1Val / forcedValue
				};
				ForceMonkey(m2, value.Value);
			}
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