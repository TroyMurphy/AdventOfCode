namespace Grids;

public class JaggedGrid<T>
{
	public T?[][] matrix { get; set; }
	public T?[][] invMatrix { get; set; }

	public List<Point<T?>> Points { get; set; }

	public int MaxX { get; init; }
	public int MaxY { get; init; }

	public JaggedGrid(List<Point<T?>> points)
	{
		this.Points = points;
		this.MaxX = points.Select(point => point.X).Max();
		this.MaxY = points.Select(point => point.Y).Max();

		//this.matrix = new T?[this.MaxY + 1][];
		this.matrix = new T[MaxY + 1][];
		for (int i = 0; i <= MaxY; i++)
		{
			this.matrix[i] = new T[MaxX + 1];
		}
		this.invMatrix = new T[MaxX + 1][];
		for (int i = 0; i <= MaxX; i++)
		{
			this.invMatrix[i] = new T[MaxY + 1];
		}
		this.PopulateMatrix(points);
	}

	public T?[] GetRow(int y) => matrix[y];
	public T?[] GetCol(int x) => invMatrix[x];

	public void PopulateMatrix(List<Point<T?>> points)
	{
		foreach (var point in points)
		{
			this.matrix[point.Y][point.X] = point.Value;
			this.invMatrix[point.X][point.Y] = point.Value;
		}
	}

	public T? GetValueAt(int x, int y) => this.matrix[y][x];

	public void Print()
	{
		for (int i = 0; i < this.GetHeight(); i++)
		{
			for (int j = 0; j < this.GetWidth(); j++)
			{
				Console.Write(this.matrix[i][j] is null ? "." : $"{this.matrix[i][j]}");
				Console.Write(" ");
			}
			Console.WriteLine();
		}
	}

	public static void Print2DArray(T[][] input)
	{
		for (int i = 0; i < input.Length; i++)
		{
			for (int j = 0; j < input[i].Length; j++)
			{
				Console.Write($"{input[i][j]}");
				Console.Write(" ");
			}
			Console.WriteLine();
		}
	}

	public static void PrintPath(T[][] input, HashSet<(int, int)> pathPoints)
	{
		for (int i = 0; i < input.Length; i++)
		{
			for (int j = 0; j < input[i].Length; j++)
			{
				if (pathPoints.Contains((j, i)))
				{
					Console.BackgroundColor = ConsoleColor.DarkRed;
					Console.ForegroundColor = ConsoleColor.White;
				}
				Console.Write($"{input[i][j]}");
				Console.BackgroundColor = ConsoleColor.Black;
				Console.ForegroundColor = ConsoleColor.White;
			}
			Console.WriteLine();
		}
	}

	public List<Point<T?>> Get8Neighbors(int x, int y) =>
		this.Points.Where(p => Math.Abs(x - p.X) <= 1 && Math.Abs(y - p.Y) <= 1).ToList();

	public List<Point<T?>> Get4Neighbors(int x, int y) =>
		this.Points.Where(p => (p.X == x && Math.Abs(y - p.Y) == 1) || (p.Y == y && Math.Abs(x - p.X) == 1)).ToList();

	public int GetWidth() => this.matrix[0].Length;

	public int GetHeight() => this.matrix.Length;

}