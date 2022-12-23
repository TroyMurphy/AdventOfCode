using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2022.Day20
{
	public class Day20
	{
		public readonly IEnumerable<string> _lines;
		public long decryptionKey = 811589153;

		public Day20(bool test = false)
		{
			this._lines = GetLines(test);
		}

		private IEnumerable<string> GetLines(bool test)
		{
			return test
				? Utilities.ReadLines(@"./Day20/inputs/demo1.txt")
				: Utilities.ReadLines(@"./Day20/inputs/input.txt");
		}

		public void Solve()
		{
			//SolvePartOne();
			SolvePartTwo();
		}

		private void SolvePartOne()
		{
			var input = new CycledLinkedList();
			var moveOrder = 0;
			var listLength = this._lines.Count();
			foreach (var line in _lines)
			{
				var item = int.Parse(line);
				input.AddLast(new GroveNum() { Value = item % listLength, TrueValue = item, MoveOrder = moveOrder++ });
			}
			//Console.WriteLine("Initial:");
			//Console.WriteLine(string.Join(",", input.Select(x => x.Value).ToList()));

			Encrypt2(input);
			//Console.WriteLine("Encrypted:");
			//Console.WriteLine(string.Join(",", input.Select(x => x.Value).ToList()));

			var offset = input.IndexOfZero();

			var item1 = input.GetAtIndex(1000 + offset).TrueValue;
			var item2 = input.GetAtIndex(2000 + offset).TrueValue;
			var item3 = input.GetAtIndex(3000 + offset).TrueValue;

			Console.WriteLine($"Result: {item1 + item2 + item3}");
		}

		private void SolvePartTwo()
		{
			var input = new CycledLinkedList();
			var moveOrder = 0;
			var listLength = this._lines.Count();
			foreach (var line in _lines)
			{
				var item = long.Parse(line) * decryptionKey;
				input.AddLast(new GroveNum() { Value = (int)(item % (listLength - 1)), TrueValue = item, MoveOrder = moveOrder++ });
			}
			Console.WriteLine("Initial:");
			WriteList(input);
			//Console.WriteLine(string.Join(",", input.Select(x => x.Value).ToList()));
			for (int i = 0; i < 10; i++)
			{
				Encrypt2(input);
			}

			//Console.WriteLine("Encrypted:");
			//Console.WriteLine(string.Join(",", input.Select(x => x.Value).ToList()));

			var offset = input.IndexOfZero();

			var item1 = input.GetAtIndex(1000 + offset).TrueValue;
			var item2 = input.GetAtIndex(2000 + offset).TrueValue;
			var item3 = input.GetAtIndex(3000 + offset).TrueValue;

			Console.WriteLine($"Result: {item1 + item2 + item3}");
		}

		public class GroveNum
		{
			public int Value { get; set; } = 0;
			public long TrueValue { get; set; } = 0;
			public int MoveOrder { get; set; } = 0;
			public bool Moved { get; private set; } = false;

			public void MarkMoved()
			{
				this.Moved = true;
			}
		}

		public static void Encrypt(CycledLinkedList input)
		{
			if (input.First is null)
			{
				throw new Exception();
			}
			// next will return null, get next returns cycle

			var item = input.First;
			while (true)
			{
				if (item is null)
				{
					return;
				}
				LinkedListNode<GroveNum>? returnTo = item.Next;
				while (returnTo is not null && returnTo.Value.Moved == true)
				{
					returnTo = returnTo.Next;
				}
				input.MoveNode(item, item.Value.Value);
				//WriteList(input);
				item = returnTo;
			}
		}

		public static void WriteList(CycledLinkedList input)
		{
			Console.WriteLine(string.Join(",", input.Select(x => $"{x.Value}({x.TrueValue})")));
		}

		public static void Encrypt2(CycledLinkedList input)
		{
			if (input.First is null)
			{
				throw new Exception();
			}
			// next will return null, get next returns cycle

			var moveNum = 0;
			while (moveNum < input.Count)
			{
				var item = input.GetAtMoveNum(moveNum);
				input.MoveNode(item, item.Value.Value);
				//WriteList(input);
				moveNum++;
			}
		}
	}
}