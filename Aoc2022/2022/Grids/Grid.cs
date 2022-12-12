namespace Grids;

public class Grid<T>
{
	public T?[,] matrix { get; set; }

	public List<Point<T>> Points { get; set; }

	public int MaxX { get; init; }
	public int MaxY { get; init; }

	public Grid(List<Point<T>> points)
	{
		this.Points = points;
		this.MaxX = points.Select(point => point.X).Max();
		this.MaxY = points.Select(point => point.Y).Max();

		this.matrix = new T[this.MaxY + 1, this.MaxX + 1];
		this.PopulateMatrix(points);
	}

	public void PopulateMatrix(List<Point<T>> points)
	{
		foreach (var point in points)
		{
			this.matrix[point.Y, point.X] = point.Value;
		}
	}

	public T? GetValueAt(int x, int y) => this.matrix[y, x];

	public void SetValueAt(int x, int y, T v)
	{
		var point = this.Points.First(p => p.X == x && p.Y == y);
		point.Value = v;
		this.matrix[y, x] = v;
	}

	public void Print()
	{
		for (int i = 0; i < this.GetHeight(); i++)
		{
			for (int j = 0; j < this.GetWidth(); j++)
			{
				Console.Write(this.matrix[i, j] is null ? "." : $"{this.matrix[i, j]}");
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

	public int GetWidth() => this.matrix.GetLength(1);

	public int GetHeight() => this.matrix.GetLength(0);
}