using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2022.Day5
{
	public class Day5
	{
		public readonly IList<string> _lines;
		public readonly IEnumerable<string> _instructions;

		public readonly IDictionary<int, Stack<char>> _stacks;


		public Day5()
		{
			this._lines = Utilities.ReadLines(@"./Day5/inputs/demo1.txt").ToList();
			this._stacks = new Dictionary<int, Stack<char>>();

			var lineLabelIndex = 0;
			while (!this._lines[lineLabelIndex].StartsWith(" 1 "))
			{
				lineLabelIndex++;
			}
			this._instructions = this._lines.Skip(lineLabelIndex + 2);


			for(int lineIndex = lineLabelIndex - 1; lineIndex>=0; lineIndex--)
			{
				var line = this._lines[lineIndex].ToCharArray();
				var stackIndex = 1;
				for(int boxChar = 1; boxChar < line.Length; boxChar += 4)
				{
					AddBox(stackIndex++, line[boxChar]);
				}
			}
		}

		public void AddBox(int stackIndex, char boxChar)
		{
			if (boxChar == ' ')
			{
				return;
			}

			if (!this._stacks.ContainsKey(stackIndex))
			{
				this._stacks[stackIndex] = new Stack<char>(new char[] { boxChar });
				return;
			}
			this._stacks[stackIndex].Push(boxChar);
		}


		public void Solve()
		{
			SolvePartTwo();
		}

		private void SolvePartOne()
		{
			foreach(var instruction in this._instructions)
			{
				var splitKeywords = new string[] { "move", "from", "to" };
				var parts = instruction.Split(splitKeywords, StringSplitOptions.RemoveEmptyEntries);
				var move = int.Parse(parts.First());
				var from = int.Parse(parts.Skip(1).First());
				var to = int.Parse(parts.Skip(2).First());

				foreach(var m in Enumerable.Range(0, move))
				{
					this._stacks[to].Push(this._stacks[from].Pop());
				}
			}

			Console.WriteLine($"The message is: {GetMessage()}");

		}

		private string GetMessage()
		{
			List<char> message = new();
			foreach(var i in this._stacks.Keys.Order())
			{
				message.Add(this._stacks[i].Peek());
			}
			return new string(message.ToArray());
		}

		private void WriteStacks()
		{
			foreach(var k in this._stacks.Keys)
			{
				Console.WriteLine($"Stack {k}: {string.Join(",", this._stacks[k])}");
			}
		}

		private void SolvePartTwo()
		{
			foreach (var instruction in this._instructions)
			{
				var splitKeywords = new string[] { "move", "from", "to" };
				var parts = instruction.Split(splitKeywords, StringSplitOptions.RemoveEmptyEntries);
				var move = int.Parse(parts.First());
				var from = int.Parse(parts.Skip(1).First());
				var to = int.Parse(parts.Skip(2).First());

				var tmpStack = new Stack<char>();
				foreach (var m in Enumerable.Range(0, move))
				{
					tmpStack.Push(this._stacks[from].Pop());
				}
				while (tmpStack.Count() > 0)
				{
					this._stacks[to].Push(tmpStack.Pop());
				}
			}
			WriteStacks();

			Console.WriteLine($"The message is: {GetMessage()}");
		}

	}
}

