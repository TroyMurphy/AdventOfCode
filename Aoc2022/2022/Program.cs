using System.Diagnostics;
using _2022.Day11;

class Program
{
	// TODO: Use benchmark?
	static void Main(string[] args)
	{
		var watch = new Stopwatch();

		watch.Start();
		var problem = new Day11();
		problem.Solve();
		watch.Stop();

		Console.WriteLine($"Ran in {watch.ElapsedMilliseconds}ms");
	}
}




