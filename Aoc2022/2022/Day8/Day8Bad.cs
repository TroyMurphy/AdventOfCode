using Grids;
using System.Xml.Schema;

namespace _2022.Day8
{
	public class Day8Bad
	{
		public readonly IEnumerable<string> _lines;
		public JaggedGrid<int> grid;


		public Day8Bad()
		{
			//this._lines = Utilities.ReadLines(@"./Day8/inputs/input.txt");
			this._lines = Utilities.ReadLines(@"./Day8/inputs/demo1.txt");
			//this.grid = Utilities.ParseJaggedGrid(this._lines);
			List<Point<int>> test = new()
			{
				new Point<int>(0,0,4),
				new Point<int>(0,1,5),
			};
			this.grid = new JaggedGrid<int>(test);
		}


		public void Solve()
		{
			//var row = this.grid.GetRow(0);
			//var col = this.grid.GetCol(0);

			//Console.WriteLine("Row:");
			//Console.WriteLine(string.Join(",", row));
			//Console.WriteLine("Col:");
			//Console.WriteLine(string.Join(",", col));

			Console.WriteLine("Matrix:");
			JaggedGrid<int>.Print2DArray(this.grid.matrix);
			Console.WriteLine("");
			Console.WriteLine("Inverse:");
			JaggedGrid<int>.Print2DArray(this.grid.invMatrix);

		}
	}
}

