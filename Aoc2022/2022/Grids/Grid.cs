namespace Grids;

public class Grid<T>
{
	private bool _keepMatrix { get; set; }
	public T?[,]? Matrix { get; set; }

	public List<Point<T?>> Points { get; set; }

	public int MaxX { get; init; }
	public int MaxY { get; init; }

	public Grid(List<Point<T>> points, bool keepMatrix = false)
	{
		this.Points = points;
		this.MaxX = points.Select(point => point.X).Max();
		this.MaxY = points.Select(point => point.Y).Max();

		_keepMatrix = keepMatrix;
		if (keepMatrix)
		{
			this.Matrix = new T[this.MaxY + 1, this.MaxX + 1];
			this.PopulateMatrix(points);
		}
	}

	public void PopulateMatrix(List<Point<T?>> points)
	{
		foreach (var point in points)
		{
			this.Matrix[point.Y, point.X] = point.Value;
		}
	}

	//public T? GetValueAt(int x, int y) => this.Matrix[y, x];
	public T? GetValueAt(int x, int y)
	{
		var point = this.Points.FirstOrDefault(p => p.X == x && p.Y == y);
		if (point == null)
		{
			return default(T);
		}
		return point.Value;
	}

	public void SetValueAt(int x, int y, T v)
	{
		if (this._keepMatrix)
		{
			this.Matrix[y, x] = v;
		}
		var point = this.Points.FirstOrDefault(p => p.X == x && p.Y == y);
		if (point == null)
		{
			this.Points.Add(new Point<T?>(x, y, v));
			return;
		}
		point.Value = v;
	}

	public void Print()
	{
		for (int i = 0; i < this.GetHeight(); i++)
		{
			for (int j = 0; j < this.GetWidth(); j++)
			{
				Console.Write(this.Matrix[i, j] is null ? "." : $"{this.Matrix[i, j]}");
				Console.Write(" ");
			}
			Console.WriteLine();
		}
	}

	public static void Print2DArray(T[,] input)
	{
		for (int i = 0; i < input.GetLength(0); i++)
		{
			for (int j = 0; j < input.GetLength(1); j++)
			{
				Console.Write($"{input[i, j]}");
				Console.Write(" ");
			}
			Console.WriteLine();
		}
	}

	public static void PrintCharPath(double[,] input, HashSet<(int, int)> pathPoints)
	{
		for (int i = 0; i < input.GetLength(0); i++)
		{
			for (int j = 0; j < input.GetLength(1); j++)
			{
				if (pathPoints.Contains((j, i)))
				{
					Console.BackgroundColor = ConsoleColor.DarkRed;
					Console.ForegroundColor = ConsoleColor.White;
				}
				Console.Write($"{(char)(input[i, j] + 97)}");
				Console.BackgroundColor = ConsoleColor.Black;
				Console.ForegroundColor = ConsoleColor.White;
			}
			Console.WriteLine();
		}
	}

	public static void PrintCharPath(int[,] input, HashSet<(int, int)> pathPoints)
	{
		for (int i = 0; i < input.GetLength(0); i++)
		{
			for (int j = 0; j < input.GetLength(1); j++)
			{
				if (pathPoints.Contains((j, i)))
				{
					Console.BackgroundColor = ConsoleColor.DarkRed;
					Console.ForegroundColor = ConsoleColor.White;
				}
				Console.Write($"{(char)(input[i, j] + 97)}");
				Console.BackgroundColor = ConsoleColor.Black;
				Console.ForegroundColor = ConsoleColor.White;
			}
			Console.WriteLine();
		}
	}

	public static void PrintPath(T[,] input, HashSet<(int, int)> pathPoints)
	{
		for (int i = 0; i < input.GetLength(0); i++)
		{
			for (int j = 0; j < input.GetLength(1); j++)
			{
				if (pathPoints.Contains((j, i)))
				{
					Console.BackgroundColor = ConsoleColor.DarkRed;
					Console.ForegroundColor = ConsoleColor.White;
				}
				Console.Write($"{input[i, j].ToString().PadLeft(3, ' ')}");
				Console.Write(" ");
				Console.BackgroundColor = ConsoleColor.Black;
				Console.ForegroundColor = ConsoleColor.White;
			}
			Console.WriteLine();
		}
	}

	public int GetWidth() => this.Matrix.GetLength(1);

	public int GetHeight() => this.Matrix.GetLength(0);
}