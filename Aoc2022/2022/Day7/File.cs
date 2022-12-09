namespace _2022.Day7
{
	public class File
	{
		public string Name { get; set; }

		public double Size { get; set; }

		public File(double size, string name)
		{
			this.Size = size;
			this.Name = name;
		}
	}
}
