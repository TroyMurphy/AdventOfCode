using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
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
			int x;
			int y;
			if (facing == Direction.N)
			{
				if (from.y == 100 && from.x < 50)
				{
					// up from 5
					x = 50;
					y = (from.x % 50) + 50;
					return ((x, y), Direction.E);
				}
				else if (from.y == 0 && from.x >= 50 && from.x < 100)
				{
					// up from 1
					x = 0;
					y = (from.x % 50) + 150;
					return ((x, y), Direction.E);
				}
				else if (from.y == 0 && from.x >= 100)
				{
					// up from 2
					x = (from.x % 50);
					y = 199;
					return ((x, y), Direction.N);
				}
				return ((from.x, from.y - 1), Direction.N);
			}
			else if (facing == Direction.E)
			{
				if (from.x == 149 && from.y < 50)
				{
					// right from 2
					x = 99;
					y = 149 - (from.y % 50);
					return ((x, y), Direction.W);
				}
				else if (from.x == 99 && from.y >= 50 && from.y < 100)
				{
					// right from 3
					x = (from.y % 50) + 100;
					y = 49;
					return ((x, y), Direction.N);
				}
				else if (from.x == 99 && from.y >= 100 && from.y < 150)
				{
					//right from 4
					x = 149;
					y = 49 - (from.y % 50);
					return ((x, y), Direction.W);
				}
				else if (from.x == 49 && from.y >= 150)
				{
					// right from 6
					x = (from.y % 50) + 50;
					y = 149;
					return ((x, y), Direction.N);
				}
				return ((from.x + 1, from.y), Direction.E);
			}
			else if (facing == Direction.S)
			{
				if (from.y == 49 && from.x >= 100)
				{
					// down from 2
					y = (from.x % 50) + 50;
					x = 99;
					return ((x, y), Direction.W);
				}
				else if (from.y == 149 && from.x >= 50 && from.x < 100)
				{
					//down from 4
					x = 49;
					y = (from.x % 50) + 150;
					return ((x, y), Direction.W);
				}
				else if (from.y == 199 && from.x < 50)
				{
					// down from 6
					x = (from.x % 50) + 100;
					y = 0;
					return ((x, y), Direction.S);
				}

				return ((from.x, from.y + 1), Direction.S);
			}
			else if (facing == Direction.W)
			{
				if (from.x == 50 && from.y < 50)
				{
					// left from 1
					x = 0;
					y = 199 - (from.y % 50);
					return ((x, y), Direction.E);
				}
				else if (from.x == 50 && from.y < 100 && from.y >= 50)
				{
					// left from 3
					x = (from.y % 50);
					y = 100;
					return ((x, y), Direction.S);
				}
				else if (from.x == 0 && from.y < 150 && from.y >= 100)
				{
					//left from 5
					x = 50;
					y = 49 - (from.y % 50);
					return ((x, y), Direction.E);
				}
				else if (from.x == 0 && from.y >= 150)
				{
					// left from 6
					x = (from.y % 50) + 50;
					y = 0;
					return ((x, y), Direction.S);
				}
				return ((from.x - 1, from.y), Direction.W);
			}
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

		public void Navigate(Player p, int distance)
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