using Grids;

namespace _2022.Day8
{
	public class Day8
	{
		public readonly IEnumerable<string> _lines;
		public Grid<int> grid;


		public Day8()
		{
			//this._lines = Utilities.ReadLines(@"./Day8/inputs/input.txt");
			this._lines = Utilities.ReadLines(@"./Day8/inputs/input.txt");
			this.grid = Utilities.ParseGrid(this._lines);
		}


		public void Solve()
		{
			SolvePartTwo();
		}

		public void SolvePartOne()
		{
			var r = this.grid.Points.Where(x => TreeVisible(x)).Count();

			Console.WriteLine($"There are {r} visble trees");
		}

		public void SolvePartTwo()
		{
			var r = this.grid.Points.Select(x => ScenicScore(x)).Max();

			Console.WriteLine($"Max scenic score is {r}");

		}

		public bool TreeVisible(Point<int> tree)
		{
			//visible left
			var visible = !grid.Points.Where(x => x.X < tree.X && x.Y == tree.Y).Any(x => x.Value >= tree.Value);
			//visible right
			visible |= !grid.Points.Where(x => x.X > tree.X && x.Y == tree.Y).Any(x => x.Value >= tree.Value); 
			//visible down
			visible |= !grid.Points.Where(x => x.X == tree.X && x.Y > tree.Y).Any(x => x.Value >= tree.Value); 
			//visible up
			visible |= !grid.Points.Where(x => x.X == tree.X && x.Y < tree.Y).Any(x => x.Value >= tree.Value);

			return visible;
		}

		public static int CountTrees(int val, IEnumerable<int> trees)
		{
			var r = 0;
			foreach(var t in trees)
			{
				r++;
				if (t >= val)
				{
					return r;
				}
			}
			return r;
		}

		public int ScenicScore(Point<int> start)
		{
			var score = CountTrees(start.Value, LookLeft(start));
			score *= CountTrees(start.Value, LookRight(start));
			score *= CountTrees(start.Value, LookUp(start));
			score *= CountTrees(start.Value, LookDown(start));
			return score;
		}


		public IEnumerable<int> LookLeft(Point<int> start)
		{
			var (x, y) = start.GetCoord();
			x--;
			while (x >= 0)
			{
				yield return grid.GetValueAt(x--, y);
			}
		}

		public IEnumerable<int> LookRight(Point<int> start)
		{
			var (x, y) = start.GetCoord();
			x++;
			while (x <= grid.MaxX)
			{
				yield return grid.GetValueAt(x++, y);
			}
		}
		public IEnumerable<int> LookUp(Point<int> start)
		{
			var (x, y) = start.GetCoord();
			y--;
			while (y >= 0)
			{
				yield return grid.GetValueAt(x, y--);
			}
		}
		public IEnumerable<int> LookDown(Point<int> start)
		{
			var (x, y) = start.GetCoord();
			y++;
			while (y <= grid.MaxY)
			{
				yield return grid.GetValueAt(x, y++);
			}
		}
	}
}
