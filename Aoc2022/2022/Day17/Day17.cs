using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

/*
for a given set of heights, and a certain rock, and an index of the jet
 */

namespace _2022.Day17
{
	public class Day17
	{
		public readonly IEnumerable<string> _lines;
		public HashSet<int>[] columns;

		public long totalCyclesHeight;
		public int heightBeforeCycles;
		public int emptyRowIndex; // the index of the row with content. Add 3 to drop from
		private IEnumerator<IRock> _rockGenerator;

		public IRock rock;
		public long piecesDropped;

		public IEnumerator<int> _jetGenerator;
		public int jetDirection;
		public int jetIndex;
		public long targetRockDrop;

		// rock, jet, topologyHash => rockCount, height
		public Dictionary<(int, int, string), (long, int)> seenCache;

		public Day17(bool test = false)
		{
			this.piecesDropped = 0;
			this._lines = GetLines(test);
			this.columns = new HashSet<int>[7];
			for (int i = 0; i < 7; i++)
			{
				columns[i] = new HashSet<int>();
			}
			this.emptyRowIndex = 0;
			this.totalCyclesHeight = 0;

			this._rockGenerator = this.GetRocks().GetEnumerator();
			this._jetGenerator = this.GetJets().GetEnumerator();
			this.seenCache = new();
		}

		public string CreateTopologyHash()
		{
			var minHeight = this.columns.Select(x => x.Any() ? x.Max() : 0).Min();
			var hashStrings = new List<string>();
			foreach (var col in columns)
			{
				var nums = col.Where(x => x > minHeight);
				if (!nums.Any())
				{
					hashStrings.Add("_");
					continue;
				}
				nums = nums.Select(x => x - minHeight - 1);
				hashStrings.Add(string.Join('-', nums));
			}
			return string.Join(".", hashStrings);
		}

		public int NextJet()
		{
			this._jetGenerator.MoveNext();
			this.jetDirection = this._jetGenerator.Current;
			return jetDirection;
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
			this.targetRockDrop = 2022;
			DropRocks();
			Console.WriteLine($"The height is {emptyRowIndex}");
		}

		private void SolvePartTwo()
		{
			this.targetRockDrop = 1000000000000;
			DropRocks();
			//DropRocks(1000000);
			Console.WriteLine($"The height is {totalCyclesHeight + (emptyRowIndex)}");
		}

		public void DropRocks()
		{
			var count = targetRockDrop;
			bool doCache = true;
			while (piecesDropped < count)
			{
				var rock = NextRock();
				this.DropRock(rock);
				piecesDropped++;

				// after having dropped a piece
				var topology = CreateTopologyHash();
				var key = (rock.Id, jetIndex, topology);
				if (doCache && seenCache.ContainsKey(key))
				{
					doCache = false;
					var (numPieces, height) = seenCache[key];
					// the last time I saw this setup, I had dropped
					// numPieces number of pieces
					// that produced a height of height
					// since then I have dropped
					// piecesDropped - newPieces
					// So I am now about to drop the same rocks to the same jets, to the same topology as I did last time.
					var piecesInCycle = piecesDropped - numPieces;
					var addedHeightSinceLastCycle = emptyRowIndex - height;

					// therefore, after dropping
					// numPieces
					// we enter a cycle that adds a height of addedHeightSinceLastDrop every time we drop piecesDroppedSinceLastSeen
					// so how many times can we fit the cycle into what remains after dropping numPieces
					var remainingToDrop = targetRockDrop - numPieces - piecesInCycle;
					long repeatCycleTimes = remainingToDrop / piecesInCycle;
					var heightInCycles = addedHeightSinceLastCycle * repeatCycleTimes;
					var remainingToDropAfterCycles = remainingToDrop - (repeatCycleTimes * piecesInCycle);
					totalCyclesHeight = heightInCycles;
					piecesDropped += (repeatCycleTimes * piecesInCycle);
				}
				if (doCache)
				{
					seenCache[key] = (piecesDropped, emptyRowIndex);
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
				if (this.jetDirection == 1)
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
					zeroX += jetDirection;
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
			jetIndex = 0;
			while (true)
			{
				jetIndex = jetIndex % gustsCycleLength;
				yield return gusts[jetIndex++];
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