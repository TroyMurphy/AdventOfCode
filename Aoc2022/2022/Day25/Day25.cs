using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2022.Day25
{
	public class Day25
	{
		public readonly IEnumerable<string> _lines;

		public Day25(bool test = false)
		{
			this._lines = GetLines(test);
		}

		private IEnumerable<string> GetLines(bool test)
		{
			return test
				? Utilities.ReadLines(@"./Day25/inputs/demo1.txt")
				: Utilities.ReadLines(@"./Day25/inputs/input.txt");
		}

		public void Solve()
		{
			SolvePartOne();
		}

		private void SolvePartOne()
		{
			double sum = 0;
			foreach (var line in _lines)
			{
				sum += SnafuToDecimal(line);
			}
			var output = DecimalToSnafu(sum);
			Console.WriteLine($"Output to bob is: {output}");
		}

		private void SolvePartTwo()
		{
			//double sum = 0;
			//foreach (var line in _lines)
			//{
			//	sum += SnafuToDecimal(line);
			//}
			//var output = DecimalToSnafu(sum);
			//Console.WriteLine($"Output to bob is: {output}");
		}

		public double SnafuToDecimal(string snafu)
		{
			var chars = snafu.ToCharArray().Reverse();
			var power = 0;
			double result = 0;

			foreach (var c in chars)
			{
				if (char.IsDigit(c))
				{
					result += int.Parse(c.ToString()) * Math.Pow(5, power);
				}
				else if (c == '-')
				{
					result -= Math.Pow(5, power);
				}
				else if (c == '=')
				{
					result -= 2 * Math.Pow(5, power);
				}
				power++;
			}
			return result;
		}

		public string DecimalToSnafu(double dec)
		{
			List<char> digits = new();
			var remainder = dec;
			var highestPower = 0;

			var placeValue = Math.Pow(5, highestPower);
			while (remainder >= ((placeValue + (placeValue / 4)) * 2) + 1)
			{
				placeValue = Math.Pow(5, ++highestPower);
			}

			while (highestPower >= 0)
			{
				// this value divided by 4 is very close to the sum of all numbers before it
				var maxReachableWithZero = highestPower switch
				{
					0 => 0,
					1 => 1 * 2,
					2 => 6 * 2,
					3 => 31 * 2,
					4 => 156 * 2,
					_ => ((placeValue / 4) * 2)
				};

				if (remainder >= 2 * placeValue || remainder > placeValue + maxReachableWithZero)
				{
					digits.Add('2');
					remainder -= placeValue * 2;
					placeValue = Math.Pow(5, --highestPower);
				}
				else if (remainder >= placeValue || remainder > maxReachableWithZero)
				{
					digits.Add('1');
					remainder -= placeValue;
					placeValue = Math.Pow(5, --highestPower);
				}
				else if (remainder >= 0 || remainder > (maxReachableWithZero * -1))
				{
					digits.Add('0');
					placeValue = Math.Pow(5, --highestPower);
				}
				else if (remainder < placeValue * -2 || remainder < (placeValue * -1) - maxReachableWithZero)
				{
					digits.Add('=');
					remainder += placeValue * 2;
					placeValue = Math.Pow(5, --highestPower);
				}
				else
				{
					digits.Add('-');
					remainder += placeValue;
					placeValue = Math.Pow(5, --highestPower);
				}
			}
			if (remainder != 0)
			{
				throw new Exception();
			}
			return string.Join("", digits);
		}
	}
}