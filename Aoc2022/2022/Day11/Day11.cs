using System.Text.RegularExpressions;

namespace _2022.Day11
{
	public class Day11
	{
		public readonly IEnumerable<string> _lines;
		public List<Monkey> Monkeys;

		public Day11()
		{
			this._lines = Utilities.ReadLines(@"./Day11/inputs/input.txt");
			this.Monkeys = new();

			var lineIndex = 0;
			while (lineIndex < this._lines.Count())
			{
				var idRegex = new Regex(@"Monkey (\d+):");
				var monkeyId = idRegex.Match(GetLine(lineIndex++)).Groups[1].Captures[0];
				var itemsRegex = new Regex(@"Starting items: (.*)");
				var itemsString = itemsRegex.Match(GetLine(lineIndex++)).Groups[1].Captures[0];

				var monkey = new Monkey(
					int.Parse(monkeyId.Value),
					itemsString.Value.Split(new string[] { ",", " " }, StringSplitOptions.RemoveEmptyEntries)
						.Select(x => long.Parse(x)).ToArray()
				);

				var operationRegex = new Regex(@"Operation: new = old ([*+]) (.*)");
				var operationMatch = operationRegex.Match(GetLine(lineIndex++));
				monkey.SetOperation(operationMatch.Groups[1].Captures[0].Value, operationMatch.Groups[2].Captures[0].Value);

				var testRegex = new Regex(@"Test: divisible by (\d+)");
				monkey.SetDivisibleTest(int.Parse(testRegex.Match(GetLine(lineIndex++)).Groups[1].Captures[0].Value));

				var trueRegex = new Regex(@"If true: .*(\d+)");
				monkey.TrueMonkey = int.Parse(trueRegex.Match(GetLine(lineIndex++)).Groups[1].Captures[0].Value);

				var falseRegex = new Regex(@"If false: .*(\d+)");
				monkey.FalseMonkey = int.Parse(falseRegex.Match(GetLine(lineIndex++)).Groups[1].Captures[0].Value);

				this.Monkeys.Add(monkey);
				lineIndex++;
			}
		}

		public string GetLine(int i)
		{
			return this._lines.Skip(i).Take(1).First();
		}

		public void Solve()
		{
			SolvePartTwo();
		}

		private void SolvePartOne()
		{
			Dictionary<int, int> investigations = new();
			foreach (var monkey in this.Monkeys)
			{
				investigations[monkey.Id] = 0;
			}
			int rounds = 20;

			for (int round = 0; round < rounds; round++)
			{
				foreach (var monkey in this.Monkeys)
				{
					foreach (var thrownItem in monkey.GetThrowToMonkeys())
					{
						investigations[monkey.Id] += 1;
						var throwToMonkey = this.Monkeys.First(x => x.Id == thrownItem.monkey);
						throwToMonkey.CatchItem(thrownItem.worryLevel);
					}
				}
			}
			foreach (var k in investigations.Keys)
			{
				Console.WriteLine($"Monkey {k} inspected items {investigations[k]} times");
			}
			var monkeyBusiness = investigations.Values.OrderByDescending(x => x).Take(2).Aggregate((x, a) => a * x);
			Console.WriteLine($"Monkey business: {monkeyBusiness}");
		}

		private void SolvePartTwo()
		{
			Dictionary<int, long> investigations = new();
			foreach (var monkey in this.Monkeys)
			{
				investigations[monkey.Id] = 0;
			}
			int rounds = 10000;
			var magicReducer = this.Monkeys.Select(x => x.Divisor).Aggregate((a, x) => a * x);

			foreach (var monkey in Monkeys)
			{
				monkey.Reducer = magicReducer;
			}

			for (int round = 0; round < rounds; round++)
			{
				foreach (var monkey in this.Monkeys)
				{
					foreach (var thrownItem in monkey.GetThrowToMonkeys(false))
					{
						investigations[monkey.Id] += 1;
						var throwToMonkey = this.Monkeys.First(x => x.Id == thrownItem.monkey);
						throwToMonkey.CatchItem(thrownItem.worryLevel);
					};
				}
			}
			foreach (var k in investigations.Keys)
			{
				Console.WriteLine($"Monkey {k} inspected items {investigations[k]} times");
			}
			var monkeyBusiness = investigations.Values.OrderByDescending(x => x).Take(2).Aggregate((x, a) => a * x);
			Console.WriteLine($"Monkey business: {monkeyBusiness}");
		}
	}
}