using System.Diagnostics;
using _2022.Day18;

internal class Program
{
	// TODO: Use benchmark?
	private static void Main(string[] args)
	{
		var watch = new Stopwatch();

		watch.Start();
		var problem = new Day18();
		problem.Solve();
		watch.Stop();

		Console.WriteLine($"Ran in {watch.ElapsedMilliseconds}ms");
	}
}
