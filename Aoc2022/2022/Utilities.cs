using Grids;

public class Utilities
{
	public static IEnumerable<string> ReadLines(string fileName)
	{
		foreach (var line in System.IO.File.ReadLines(fileName))
		{
			yield return line;
		};
	}

	public static string SwitchNumberBase(string number, int fromBase, int toBase, int paddedWidth = 0)
	{
		return Convert.ToString(Convert.ToInt64(number, fromBase), toBase).PadLeft(paddedWidth, '0');
	}

	public static double BinToDec(string binary)
	{
		return Convert.ToInt64(binary, 2);
	}

	public static Grid<int> ParseGrid(IEnumerable<string> lines)
	{

		var points = new List<Point<int>>();

		int i = 0;
		foreach (var line in lines)
		{
			int j = 0;
			foreach (var x in line.ToCharArray().Select(y => int.Parse(y.ToString())))
			{
				points.Add(new Point<int>(j, i, x));
				j++;
			}
			i++;
		}
		return new Grid<int>(points);
	}
	public static JaggedGrid<int> ParseJaggedGrid(IEnumerable<string> lines)
	{

		var points = new List<Point<int>>();

		int i = 0;
		foreach (var line in lines)
		{
			int j = 0;
			foreach (var x in line.ToCharArray().Select(y => int.Parse(y.ToString())))
			{
				points.Add(new Point<int>(j, i, x));
				j++;
			}
			i++;
		}
		return new JaggedGrid<int>(points);
	}
}