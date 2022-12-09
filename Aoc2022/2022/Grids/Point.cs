namespace Grids;
public class Point<T>
{

	public Point(int x, int y, T value)
	{
		this.X = x;
		this.Y = y;
		this.Value = value;
	}
	public int X { get; init; }
	public int Y { get; init; }
	public T Value { get; set; }

	public (int, int) GetCoord() => (this.X, this.Y);
}
