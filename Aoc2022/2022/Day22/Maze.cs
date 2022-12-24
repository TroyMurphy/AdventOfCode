using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static _2022.Day22.Day22;

namespace _2022.Day22
{
	public class Maze
	{
		public List<MazeRow> Rows { get; set; } = new();

		public void AddRow(string input)
		{
			var cIndex = -1;
			var empty = 0;
			var walls = new List<int>();
			int? start = null;
			while (++cIndex < input.Length)
			{
				if (input[cIndex] == ' ')
				{
					empty++;
					continue;
				}
				if (input[cIndex] == '#')
				{
					walls.Add(cIndex);
				}
				start ??= cIndex;
			}
			this.Rows.Add(new MazeRow()
			{
				Start = start.Value,
				End = input.Length - 1,
				Walls = walls.ToHashSet()
			});
		}

		internal void Navigate(Player p, int distance)
		{
			var position = p.Position;
			if (p.Facing == Day22.Direction.E)
			{
				var row = this.Rows[position.y];
				var next = position.x + 1;
				if (next > row.End)
				{
					next = row.Start;
				}
				var moves = 0;
				while (moves < distance && !row.Walls.Contains(next))
				{
					p.Position = (next, position.y);
					moves++;
					next++;
					if (next > row.End)
					{
						next = row.Start;
					}
				}
			}
			else if (p.Facing == Day22.Direction.W)
			{
				var row = this.Rows[position.y];
				var next = position.x - 1;
				if (next < row.Start)
				{
					next = row.End;
				}
				var moves = 0;
				while (moves < distance && !row.Walls.Contains(next))
				{
					p.Position = (next, position.y);
					moves++;
					next--;
					if (next < row.Start)
					{
						next = row.End;
					}
				}
			}
			else if (p.Facing == Day22.Direction.S)
			{
				var nextY = position.y + 1 >= this.Rows.Count ? 0 : position.y + 1;
				var next = this.Rows[nextY];

				// flip to column
				while (position.x > next.End || position.x < next.Start)
				{
					if (nextY >= this.Rows.Count - 1)
					{
						nextY = -1;
					}
					next = this.Rows[++nextY];
				}

				var moves = 0;
				while (moves < distance && !next.Walls.Contains(position.x))
				{
					p.Position = (position.x, nextY);
					moves++;
					nextY += 1;
					if (nextY >= this.Rows.Count)
					{
						nextY = 0;
					}
					next = this.Rows[nextY];
					while (position.x > next.End || position.x < next.Start)
					{
						if (nextY >= this.Rows.Count - 1)
						{
							nextY = -1;
						}
						next = this.Rows[++nextY];
					}
				}
			}
			else if (p.Facing == Day22.Direction.N)
			{
				var nextY = position.y - 1 < 0 ? this.Rows.Count - 1 : position.y - 1;
				var next = this.Rows[nextY];

				// flip to column
				while (position.x > next.End || position.x < next.Start)
				{
					if (nextY <= 0)
					{
						nextY = this.Rows.Count;
					}
					next = this.Rows[--nextY];
				}

				var moves = 0;
				while (moves < distance && !next.Walls.Contains(position.x))
				{
					p.Position = (position.x, nextY);
					moves++;
					nextY -= 1;
					if (nextY < 0)
					{
						nextY = this.Rows.Count - 1;
					}
					next = this.Rows[nextY];
					while (position.x > next.End || position.x < next.Start)
					{
						if (nextY <= 0)
						{
							nextY = this.Rows.Count;
						}
						next = this.Rows[--nextY];
					}
				}
			}
		}
	}

	public class MazeRow
	{
		public int Start { get; set; }
		public int End { get; set; }

		public HashSet<int> Walls { get; set; }
	}
}