using System.Diagnostics;
using _2022.Day12;

class Program
{
	// TODO: Use benchmark?
	static void Main(string[] args)
	{
		var watch = new Stopwatch();

		watch.Start();
		var problem = new Day12();
		problem.Solve();
		watch.Stop();

		Console.WriteLine($"Ran in {watch.ElapsedMilliseconds}ms");
	}
}





