using System.Diagnostics;
using _2022.Day15;

class Program
{
	// TODO: Use benchmark?
	static void Main(string[] args)
	{
		var watch = new Stopwatch();

		watch.Start();
		var problem = new Day15();
		problem.Solve();
		watch.Stop();

		Console.WriteLine($"Ran in {watch.ElapsedMilliseconds}ms");
	}
}








