#nullable disable
namespace _2022.Day7
{
	public class Day7
	{
		public readonly IEnumerable<string> _lines;

		public readonly Folder Root;

		public Day7()
		{
			this._lines = Utilities.ReadLines(@"./Day7/inputs/input.txt");
			this.Root = new("/", null);
		}


		public void Solve()
		{
			SolvePartTwo();
		}

		public void Build()
		{
			Folder currentFolder = Root;
			var splitChars = new string[] { "$", " " };
			var lines = this._lines.ToArray();
			var lineIndex = 0;
			while (lineIndex < lines.Length)
			{
				var line = lines[lineIndex++];
				var cmd = line.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);
				switch (cmd.First())
				{
					case ("cd"):
						currentFolder = cmd.Last() switch
						{
							"/" => Root,
							".." => currentFolder.Parent,
							_ => currentFolder.AddFolder(cmd.Last())
						};
						break;

					case ("ls"):
						var nestedLine = lines[lineIndex];
						while (!nestedLine.StartsWith("$"))
						{
							var toCreate = nestedLine.Split(" ");
							var nestedType = toCreate.First();
							var nestedName = toCreate.Last();

							switch (nestedType) {
								case ("dir"):
									_ = currentFolder.AddFolder(nestedName);
									break;
								default:
									_ = currentFolder.AddFile(double.Parse(nestedType), nestedName);
									break;
							}
							if (lineIndex + 1 >= lines.Length)
							{
								return;
							}
							nestedLine = lines[++lineIndex];
						}
						break;
				}
			}

		}

		private void SolvePartOne()
		{
			Build();
			//Root.Write();
			var result = Root.SolutionWeight();
			Root.WriteOut();

			Console.WriteLine($"Sum is {result}");
		}

		private void SolvePartTwo()
		{
			Build();
			//Root.Write();
			_ = Root.TotalWeight();
			var systemSize = 70000000;
			var required = 30000000;
			var available = systemSize - Root.CachedWeight.Value;
			var minFolderSize = required - available;
			var smallest = Root.SmallestWeight(minFolderSize);

			Console.WriteLine($"Directory to delte is of size: {smallest}");

		}
	}
}

