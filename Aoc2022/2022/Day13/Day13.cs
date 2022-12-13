using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2022.Day13
{
	public class Day13
	{
		public readonly IEnumerable<string> _lines;

		public Day13()
		{
			this._lines = Utilities.ReadLines(@"./Day13/inputs/demo2.txt");
		}

		public void Solve()
		{
			SolvePartOne();
		}

		private void SolvePartOne()
		{
			List<int> goodIndecies = new();
			var index = 0;
			foreach (var (left, right) in this.GetPackets())
			{
				var valid = DataPacket.ComparePacket(left.PacketValues, right.PacketValues);
				if (valid)
				{
					goodIndecies.Add(index + 1);
				}
				index++;
			}
			Console.WriteLine($"Good packets are indexes {string.Join(",", goodIndecies)}");
		}

		private void SolvePartTwo()
		{
		}

		public IEnumerable<(DataPacket left, DataPacket right)> GetPackets()
		{
			var lineIndex = 0;
			while (lineIndex < _lines.Count())
			{
				var left = new DataPacket(_lines.Skip(lineIndex++).First());
				var right = new DataPacket(_lines.Skip(lineIndex++).First());
				// skip empty line
				lineIndex++;
				yield return (left, right);
			}
		}
	}
}