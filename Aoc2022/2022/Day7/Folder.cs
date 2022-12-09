#nullable disable

namespace _2022.Day7
{
	public class Folder
	{
		public Folder? Parent { get; set; }

		public string Name { get; set; }

		public List<File> Files { get; set; }

		public List<Folder> SubFolders { get; set; }

		public double? CachedWeight { get; set; }

		public Folder(string name, Folder? parent)
		{
			this.Name = name;
			this.Files = new List<File>();
			this.SubFolders = new List<Folder>();
			this.Parent = parent;
		}

		public File AddFile(double size, string name)
		{
			var f = this.Files.FirstOrDefault(x => x.Name == name);
			if (f == null)
			{
				f = new File(size, name);
				this.Files.Add(f);
			}
			return f;
		}

		public Folder AddFolder(string name)
		{
			var f = this.SubFolders.FirstOrDefault(x => x.Name == name);
			if (f == null)
			{
				f = new Folder(name, this);
				this.SubFolders.Add(f);
			}
			return f;
		}

		public void Write(int depth = 0)
		{
			Console.Write(String.Concat(Enumerable.Repeat("  ", depth)));
			Console.Write($"- {this.Name} (dir, size={this.CachedWeight})");
			Console.WriteLine();
			foreach(var f in this.Files)
			{
				Console.Write(String.Concat(Enumerable.Repeat("  ", depth + 1)));
				Console.Write("- ");
				Console.Write(f.Name);
				Console.Write($" (file, size={f.Size})");
				Console.WriteLine();
			}
			foreach(var f in this.SubFolders)
			{
				f.Write(depth + 1);
			}
		}
		public void WriteOut(int depth = 0)
		{
			if (this.CachedWeight < 100000)
			{
				Console.Write($"{this.CachedWeight}");
				Console.WriteLine();
			}
			foreach(var f in this.SubFolders)
			{
				f.WriteOut(depth + 1);
			}
		}

		public double SolutionWeight()
		{
			_ = this.TotalWeight();
			return SolutionWeight(100000);
		}

		// this didn't work. I did it manually.
		private double SolutionWeight(double sum = 0)
		{
			double direct = 0;
			double indirect = 0;
			if (this.CachedWeight <= 100000)
			{
				direct = this.CachedWeight.Value;
			}
			foreach(var f in this.SubFolders)
			{
				indirect += f.SolutionWeight(sum);
			}

			return sum + direct + indirect;
		}

		public double SmallestWeight(double min, double acc = double.MaxValue)
		{
			if (this.CachedWeight.Value < acc && this.CachedWeight.Value >= min)
			{
				acc = this.CachedWeight.Value;
			}

			if (this.SubFolders.Count == 0)
			{
				return acc;
			}

			return this.SubFolders.Select(x => x.SmallestWeight(min, acc)).Min();

		}

		public double TotalWeight()
		{
			var direct = this.Files.Sum(x => x.Size);
			if (this.SubFolders.Count == 0)
			{
				this.CachedWeight = direct;
				return direct;
			}
			if (this.CachedWeight is not null)
			{
				return this.CachedWeight.Value;
			}

			var indirect = this.SubFolders.Sum(x => x.TotalWeight());
			var weight = direct + indirect;
			this.CachedWeight = weight;
			return weight;
		}
	}
}
