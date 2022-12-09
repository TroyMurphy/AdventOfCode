using System.Xml;

namespace _2022.Day9
{
	public class Day9
	{
		public readonly IEnumerable<string> _lines;
		public IEnumerable<(string direction, int times)> instructions;

		public Day9()
		{
			this._lines = Utilities.ReadLines(@"./Day9/inputs/input.txt");
			this.instructions = this._lines.Select(l =>
			{
				var els = l.Split(" ");

				return (els.First(), int.Parse(els.Last()));
			});
		}

		public void Solve()
		{
			SolvePartTwo();
		}

		public (int x, int y) Move((int, int) startHead, (int, int) endHead, (int, int) tail)
		{
			var (sx, sy) = startHead;
			var (ex, ey) = endHead;
			var (tx, ty) = tail;

			// within a square grid
			if (Math.Abs(ex - tx) <= 1 && Math.Abs(ey - ty) <= 1)
			{
				return tail;
			}

			// if in line vertically, just move it the distance head moved
			if (ex == tx)
			{
				return (tx, ty + (ey - sy));
			}
			// same for horizontal
			if (ey == ty)
			{
				return (tx + (ex - sx), ty);
			}
			//therefore move tail in a diagonal to keep up
			//return (tx + (sx - tx), ty + (sy - ty));
			var moveHorizontal = ex > tx ? 1 : -1;
			var moveVertical = ey > ty ? 1 : -1;
			return (tx + moveHorizontal, ty + moveVertical);
		}

		public ((int, int) head, (int, int) tail) MoveHeadTail((int x, int y) head, (int x, int y) tail, string direction)
		{
			(int, int) newHead;
			(int, int) newTail;
			switch (direction)
			{
				case ("R"):
					newHead = (head.x + 1, head.y);
					newTail = Move(head, newHead, tail);
					break;

				case ("U"):
					newHead = (head.x, head.y + 1);
					newTail = Move(head, newHead, tail);
					break;

				case ("L"):
					newHead = (head.x - 1, head.y);
					newTail = Move(head, newHead, tail);
					break;

				case ("D"):
					newHead = (head.x, head.y - 1);
					newTail = Move(head, newHead, tail);
					break;

				default:
					throw new Exception();
			}
			return (newHead, newTail);
		}

		private void SolvePartOne()
		{
			HashSet<(int, int)> tailVisited = new();
			(int tx, int ty) tail = (0, 0);
			(int hx, int hy) head = (0, 0);
			foreach (var instruction in instructions)
			{
				for (int i = 0; i < instruction.times; i++)
				{
					var (newHead, newTail) = MoveHeadTail(head, tail, instruction.direction);
					tailVisited.Add(newTail);
					tail = newTail;
					head = newHead;
				}
			}
			Console.WriteLine($"Tail visited {tailVisited.Count} spots");
		}

		private void SolvePartTwo()
		{
			HashSet<(int, int)> tailVisited = new();
			Dictionary<int, (int, int)> knots = new();
			for (int i = 0; i < 10; i++)
			{
				knots[i] = (0, 0);
			}
			foreach (var instruction in instructions)
			{
				for (int t = 0; t < instruction.times; t++)
				{
					knots = MoveKnots(knots, instruction.direction);
					tailVisited.Add(knots[9]);
				}
			}
			Console.WriteLine($"Tail visited {tailVisited.Count} spots");
		}

		private Dictionary<int, (int, int)> MoveKnots(Dictionary<int, (int x, int y)> knots, string direction)
		{
			Dictionary<int, (int, int)> newKnots = new();

			var zeroMove = MoveHeadTail(knots[0], knots[1], direction);
			newKnots[0] = zeroMove.head;

			var knotIndex = 1;
			while (knotIndex < 10)
			{
				newKnots[knotIndex] = Move(knots[knotIndex - 1], newKnots[knotIndex - 1], knots[knotIndex]);
				knotIndex++;
			}
			return newKnots;
		}
	}
}