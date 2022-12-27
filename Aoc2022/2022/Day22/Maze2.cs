using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static _2022.Day22.Day22;

namespace _2022.Day22
{
	public class Maze2
	{
		public List<MazeRow2> Rows { get; set; } = new();
		public int FaceWidth = 50;
		public int FaceHeight = 50;

		public ((int x, int y) pos, Direction direction) GetNext((int x, int y) from, Direction facing)
		{
			#region north

			// walk off 1
			if (facing == Direction.N)
			{
				if (from.y == 0)
				{
					// walk off 1
					if (from.x < 100)
					{
						return ((0, 150 + from.x - 50), Direction.E);
					}
					// walk off 2
					else
					{
						return ((from.x - 100, 199), Direction.N);
					}
				}
				// walk off 5
				if (from.y == 100 && from.x < 50)
				{
					return ((50, from.x + 50), Direction.E);
				}

				return ((from.x, from.y - 1), Direction.N);
			}

			#endregion north

			#region East

			if (facing == Direction.E)
			{
				if ((from.x + 1) % 50 == 0)
				{
					if (from.y < 50)
					{
						// walk off 2
						return ((99, 149 - from.y), Direction.W);
					}
					else if (from.y < 100)
					{
						// walk of 3
						return ((from.y - 50 + 100, 49), Direction.N);
					}
					else if (from.y < 150)
					{
						// walk off 4
						return ((149, 149 - from.y), Direction.W);
					}
					else
					{
						// walk off 6
						return ((from.y - 150 + 50, 149), Direction.N);
					}
				}
				return ((from.x + 1, from.y), Direction.E);
			}

			#endregion East

			#region S

			if (facing == Direction.S)
			{
				if ((from.y + 1) % 50 == 0)
				{
					if (from.x >= 100)
					{
						// fall off 2
						return ((99, from.x - 100 + 50), Direction.W);
					}
					else if (from.y >= 100 && from.x >= 50)
					{
						// fall off 4
						return ((49, from.x - 50 + 150), Direction.W);
					}
					else
					{
						// fall off 6
						return ((from.x + 100, 0), Direction.S);
					}
				}
				return ((from.x, from.y + 1), Direction.S);
			}

			#endregion S

			#region W

			if (facing == Direction.W)
			{
				if (from.x % 50 == 0)
				{
					if (from.y < 50)
					{
						// fall off 1
						return ((0, (49 - from.y) + 100), Direction.E);
					}
					else if (from.y < 100)
					{
						// fall off 3
						return ((from.y - 50, 100), Direction.S);
					}
					else if (from.y < 150)
					{
						// fall off 5
						return ((50, 149 - from.y), Direction.E);
					}
					else if (from.y < 200)
					{
						// fall off 6
						return ((from.y - 150 + 50, 0), Direction.S);
					}
				}
				return ((from.x - 1, from.y), Direction.W);
			}

			#endregion W

			throw new Exception();
		}

		public Maze2(IEnumerable<string> lines)
		{
			foreach (var line in lines)
			{
				this.AddRow(line);
			}
		}

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

			this.Rows.Add(new MazeRow2()
			{
				Start = start.Value,
				End = input.Length - 1,
				Walls = walls.ToHashSet()
			});
		}

		private bool IsRock((int x, int y) pos)
		{
			var row = this.Rows[pos.y];
			return row.Walls.Contains(pos.x);
		}

		internal void Navigate(Player p, int distance)
		{
			var nextSquare = this.GetNext(p.Position, p.Facing);
			int movedSpaces = 0;
			while (movedSpaces < distance)
			{
				if (this.IsRock(nextSquare.pos))
				{
					break;
				}
				movedSpaces++;
				p.Position = nextSquare.pos;
				p.Facing = nextSquare.direction;
				nextSquare = GetNext(p.Position, p.Facing);
			}
		}
	}

	public class MazeRow2
	{
		public int Start { get; set; }
		public int End { get; set; }

		public HashSet<int> Walls { get; set; }
	}
}