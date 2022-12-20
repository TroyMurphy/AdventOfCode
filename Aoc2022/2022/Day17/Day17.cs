using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace _2022.Day17
{
	public class Day17
	{
		public readonly IEnumerable<string> _lines;
		public HashSet<int>[] columns;

		public long collapsedCount;
		public int emptyRowIndex; // the index of the row with content. Add 3 to drop from
		private IEnumerator<IRock> _rockGenerator;

		public IRock rock;

		public IEnumerator<int> _jetGenerator;
		public int jet;

		public Day17(bool test = false)
		{
			this._lines = GetLines(test);
			this.columns = new HashSet<int>[7];
			for (int i = 0; i < 7; i++)
			{
				columns[i] = new HashSet<int>();
			}
			this.emptyRowIndex = 0;
			this.collapsedCount = 0;

			this._rockGenerator = this.GetRocks().GetEnumerator();
			this._jetGenerator = this.GetJets().GetEnumerator();
		}

		public int NextJet()
		{
			this._jetGenerator.MoveNext();
			this.jet = this._jetGenerator.Current;
			return jet;
		}

		public void Reduce()
		{
			var minY = this.columns.Select(x => x.Count() > 0 ? x.Max() : 0).Min();

			this.collapsedCount += minY;
			for (var i = 0; i < 7; i++)
			{
				columns[i] = columns[i].Where(x => x > minY).Select(x => x - minY).ToHashSet();
			}
			emptyRowIndex -= minY;
		}

		public IRock NextRock()
		{
			this._rockGenerator.MoveNext();
			this.rock = this._rockGenerator.Current;
			return rock;
		}

		private IEnumerable<string> GetLines(bool test)
		{
			return test
				? Utilities.ReadLines(@"./Day17/inputs/demo1.txt")
				: Utilities.ReadLines(@"./Day17/inputs/input.txt");
		}

		public void Solve()
		{
			SolvePartTwo();
		}

		private void SolvePartOne()
		{
			DropRocks(2022);
			Console.WriteLine($"The height is {emptyRowIndex}");
		}

		private void SolvePartTwo()
		{
			DropRocks(1000000000000);
			//DropRocks(1000000);
			Console.WriteLine($"The height is {collapsedCount + emptyRowIndex}");
		}

		public void DropRocks(long count)
		{
			var u = 0;
			var iter = 0;
			for (int i = 0; i < count; i++)
			{
				var rock = NextRock();
				this.DropRock(rock);
				u++;
				if (u == 10000)
				{
					Reduce();
					u = 0;
					iter++;
					Console.WriteLine($"{10000 * iter}");
				}
			}
		}

		public void DropRock(IRock rock)
		{
			var zeroX = 2;
			var zeroY = emptyRowIndex + 3;

			while (true)
			{
				NextJet();
				List<(int x, int y)> checkPoints;
				if (this.jet == 1)
				{
					checkPoints = rock.IfMoveRight().Select(p => (p.x + zeroX, p.y + zeroY)).ToList();
				}
				else
				{
					checkPoints = rock.IfMoveLeft().Select(p => (p.x + zeroX, p.y + zeroY)).ToList();
				}
				var canMove = CanOccupySpace(checkPoints);
				if (canMove)
				{
					zeroX += jet;
				}

				var ifDown = rock.IfMoveDown().Select(p => (p.x + zeroX, p.y + zeroY)).ToList();
				var canMoveDown = CanOccupySpace(ifDown);
				if (canMoveDown)
				{
					zeroY--;
					continue;
				}

				var maxY = emptyRowIndex;
				rock.AllPoints.ForEach(p =>
				{
					columns[p.x + zeroX].Add(p.y + zeroY);
					maxY = int.Max(maxY, p.y + zeroY + 1);
				});
				emptyRowIndex = maxY;

				break;
			}
		}

		public bool CanOccupySpace(List<(int x, int y)> occupiedPoints)
		{
			foreach (var point in occupiedPoints)
			{
				if (point.x < 0 || point.x > 6 || point.y < 0)
				{
					return false;
				}

				if (columns[point.x].Contains(point.y))
				{
					return false;
				}
			}
			return true;
		}

		private IEnumerable<int> GetJets()
		{
			var gusts = this._lines.First().ToCharArray().Select(x => x == '>' ? 1 : -1).ToList();

			var gustsCycleLength = gusts.Count();
			var jetIndex = 0;
			while (true)
			{
				yield return gusts[jetIndex++ % gustsCycleLength];
			}
		}

		private IEnumerable<IRock> GetRocks()
		{
			while (true)
			{
				yield return (new Rock1());
				yield return (new Rock2());
				yield return (new Rock3());
				yield return (new Rock4());
				yield return (new Rock5());
			}
		}

		private void DrawGrid()
		{
			for (int row = emptyRowIndex + 5; row >= 0; row--)
			{
				Console.Write("|");
				for (int col = 0; col < 7; col++)
				{
					if (columns[col].Contains(row))
					{
						Console.Write("#");
						continue;
					}
					Console.Write(".");
				}
				Console.Write("|");
				Console.WriteLine();
			}
			Console.WriteLine("=======");
		}
	}
}