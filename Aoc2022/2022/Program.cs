using System.Diagnostics;
using _2022.Day24;

internal class Program
{
	// TODO: Use benchmark?
	private static void Main(string[] args)
	{
		var watch = new Stopwatch();

		watch.Start();
		var problem = new Day24();
		problem.Solve();
		watch.Stop();

		Console.WriteLine($"Ran in {watch.ElapsedMilliseconds}ms");
	}
}




