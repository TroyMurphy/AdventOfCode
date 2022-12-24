using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2022.Day22
{
	public class Day22
	{
		public readonly IEnumerable<string> _lines;
		public Maze Maze { get; set; }
		public char[] Instructions { get; set; }
		public IEnumerator<int> _distanceGenerator;
		public Player player { get; set; } = new();

		public Day22(bool test = false)
		{
			this._lines = GetLines(test);
			this.Maze = new();
			this._distanceGenerator = GetDistance().GetEnumerator();
			foreach (var line in _lines.Take(_lines.Count() - 2))
			{
				this.Maze.AddRow(line);
			}
			this.Instructions = _lines.Last().ToCharArray();
		}

		private IEnumerable<string> GetLines(bool test)
		{
			return test
				? Utilities.ReadLines(@"./Day22/inputs/demo1.txt")
				: Utilities.ReadLines(@"./Day22/inputs/input.txt");
		}

		public void Solve()
		{
			SolvePartOne();
		}

		private void SolvePartOne()
		{
			player.Position = GetPlayerStart();
			player.Facing = Direction.E;

			while (true)
			{
				int? distance = NextDistance();
				if (!distance.HasValue)
				{
					break;
				}
				this.Maze.Navigate(player, distance.Value);
			}
			var result = 1000 * (player.Position.y + 1);
			result += 4 * (player.Position.x + 1);
			result += (int)player.Facing;
			Console.WriteLine($"Result is {result}");
		}

		private void SolvePartTwo()
		{
		}

		public int? NextDistance()
		{
			var cont = this._distanceGenerator.MoveNext();
			return cont ? this._distanceGenerator.Current : null;
		}

		public IEnumerable<int> GetDistance()
		{
			int i = 0;
			while (i < this.Instructions.Count())
			{
				var digitString = this.Instructions[i].ToString();
				i++;
				while (i < this.Instructions.Count() && char.IsDigit(this.Instructions[i]))
				{
					digitString += this.Instructions[i++];
				}
				yield return int.Parse(digitString);
				if (i >= this.Instructions.Count())
				{
					break;
				}
				while (!char.IsDigit(this.Instructions[i]))
				{
					switch (this.Instructions[i])
					{
						case 'R':
							player.TurnRight();
							break;

						case 'L':
							player.TurnLeft();
							break;

						default:
							throw new Exception();
					};
					i++;
				}
			}
		}

		public (int, int) GetPlayerStart()
		{
			var topRow = this.Maze.Rows[0];
			var startingX = topRow.Start;
			while (topRow.Walls.Contains(startingX))
			{
				startingX++;
			}
			return (startingX, 0);
		}

		public class Player
		{
			public (int x, int y) Position { get; set; }
			public Direction Facing { get; set; }

			public void TurnRight()
			{
				this.Facing = this.Facing switch
				{
					Direction.N => Direction.E,
					Direction.E => Direction.S,
					Direction.S => Direction.W,
					Direction.W => Direction.N
				};
			}

			public void TurnLeft()
			{
				this.Facing = this.Facing switch
				{
					Direction.N => Direction.W,
					Direction.W => Direction.S,
					Direction.S => Direction.E,
					Direction.E => Direction.N,
				};
			}
		}

		public enum Direction
		{
			E, S, W, N
		}
	}
}