using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2022.Day17
{
	public interface IRock
	{
		int Id { get; }
		List<(int x, int y)> AllPoints { get; }

		public List<(int x, int y)> IfMoveLeft();

		public List<(int x, int y)> IfMoveDown();

		public List<(int x, int y)> IfMoveRight();
	}

	// ####
	public class Rock1 : IRock
	{
		public int Id { get; }
		public List<(int x, int y)> AllPoints { get; init; }

		public Rock1()
		{
			this.Id = 1;
			this.AllPoints = new List<(int x, int y)>
			{
				(0,0),(1,0),(2,0),(3,0)
			};
		}

		public List<(int x, int y)> IfMoveLeft() => new List<(int x, int y)> { (-1, 0) };

		public List<(int x, int y)> IfMoveDown() => new List<(int x, int y)> { (0, -1), (1, -1), (2, -1), (3, -1) };

		public List<(int x, int y)> IfMoveRight() => new List<(int x, int y)> { (4, 0) };
	}

	// .#.
	// ###
	// .#.
	public class Rock2 : IRock
	{
		public int Id { get; }
		public List<(int x, int y)> AllPoints { get; init; }

		public Rock2()
		{
			this.Id = 2;
			this.AllPoints = new List<(int x, int y)>
			{
				(1,0), (0,1),(1,1),(2, 1), (1, 2)
			};
		}

		public List<(int x, int y)> IfMoveLeft() => new List<(int x, int y)> { (0, 0), (-1, 1), (0, 2) };

		public List<(int x, int y)> IfMoveDown() => new List<(int x, int y)> { (0, 0), (1, -1), (2, 0) };

		public List<(int x, int y)> IfMoveRight() => new List<(int x, int y)> { (2, 0), (3, 1), (2, 2) };
	}

	//
	// ..#
	// ..#
	// ###
	public class Rock3 : IRock
	{
		public int Id { get; }
		public List<(int x, int y)> AllPoints { get; init; }

		public Rock3()
		{
			this.Id = 3;
			this.AllPoints = new List<(int x, int y)>
			{
				(0,0),(1, 0), (2, 0), (2, 1), (2, 2)
			};
		}

		public List<(int x, int y)> IfMoveLeft() => new List<(int x, int y)> { (-1, 0), (1, 1), (1, 2) };

		public List<(int x, int y)> IfMoveDown() => new List<(int x, int y)> { (0, -1), (1, -1), (2, -1) };

		public List<(int x, int y)> IfMoveRight() => new List<(int x, int y)> { (3, 0), (3, 1), (3, 2) };
	}

	//
	// #
	// #
	// #
	// #
	public class Rock4 : IRock
	{
		public int Id { get; }
		public List<(int x, int y)> AllPoints { get; init; }

		public Rock4()
		{
			this.Id = 4;
			this.AllPoints = new List<(int x, int y)>
			{
				(0,0),(0,1),(0,2),(0,3)
			};
		}

		public List<(int x, int y)> IfMoveLeft() => new List<(int x, int y)> { (-1, 0), (-1, 1), (-1, 2), (-1, 3) };

		public List<(int x, int y)> IfMoveDown() => new List<(int x, int y)> { (0, -1) };

		public List<(int x, int y)> IfMoveRight() => new List<(int x, int y)> { (1, 0), (1, 1), (1, 2), (1, 3) };
	}

	// ##
	// ##

	public class Rock5 : IRock
	{
		public int Id { get; }
		public List<(int x, int y)> AllPoints { get; init; }

		public Rock5()
		{
			this.Id = 5;
			this.AllPoints = new List<(int x, int y)>
			{
				(0,0), (0,1),(1,0),(1,1)
			};
		}

		public List<(int x, int y)> IfMoveLeft() => new List<(int x, int y)> { (-1, 0), (-1, 1) };

		public List<(int x, int y)> IfMoveDown() => new List<(int x, int y)> { (0, -1), (1, -1) };

		public List<(int x, int y)> IfMoveRight() => new List<(int x, int y)> { (2, 0), (2, 1) };
	}
}