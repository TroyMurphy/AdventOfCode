namespace Grids;
public class Point<T>
{

	public Point(int x, int y, T value)
	{
		this.X = x;
		this.Y = y;
		this.Value = value;
	}
	public int X { get; private set; }
	public int Y { get; private set; }
	public T Value { get; private set; }

	public void setX(int x)
	{
		this.X = x;
	}
	public void setY(int y)
	{
		this.Y = y;
	}
	public void SetValue(T value)
	{
		this.Value = value;
	}

	public (int, int) GetCoord() => (this.X, this.Y);
}
