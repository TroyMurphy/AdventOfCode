using _2022.Day9;
using System.Diagnostics;

class Program
{
	// TODO: Use benchmark?
	static void Main(string[] args)
	{
		var watch = new Stopwatch();

		watch.Start();
		var problem = new Day9();
		problem.Solve();
		watch.Stop();

		Console.WriteLine($"Ran in {watch.ElapsedMilliseconds}ms");
	}
}



