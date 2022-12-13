using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace _2022.Day13
{
	public class Day13
	{
		public readonly IEnumerable<string> _lines;

		public Day13()
		{
			//this._lines = Utilities.ReadLines(@"./Day13/inputs/demo1.txt");

			this._lines = Utilities.ReadLines(@"./Day13/inputs/input.txt");
		}

		public void Solve()
		{
			SolvePartTwo();
		}

		private void SolvePartOne()
		{
			List<int> goodIndecies = new();
			var index = 0;
			foreach (var (left, right) in this.GetPackets())
			{
				var valid = DataPacket.ComparePacket(left.PacketValues, right.PacketValues);
				if (valid is null)
				{
					throw new Exception();
				}
				if (valid.Value)
				{
					goodIndecies.Add(index + 1);
				}
				index++;
			}
			Console.WriteLine($"Good packets are:\n {string.Join("\n", goodIndecies)}");
			Console.WriteLine($"Answer is {goodIndecies.Sum()}");
		}

		private void SolvePartTwo()
		{
			var packets = GetPart2Packets().ToList();
			packets.Sort(CompareByDataPacket);
			foreach (var p in packets)
			{
				Console.WriteLine(DataPacket.GetString(p.PacketValues));
			}
			var p1 = packets.FindIndex(x => x.DividerIndex == 1);
			var p2 = packets.FindIndex(x => x.DividerIndex == 2);
			Console.WriteLine($"Indexes are: {p1 + 1}, {p2 + 1}");
			Console.WriteLine($"Product is: {((p1 + 1) * (p2 + 1))}");
		}

		public static int CompareByDataPacket(DataPacket left, DataPacket right)
		{
			var cloneLeft = new DataPacket(left.PacketValues);
			var cloneRight = new DataPacket(right.PacketValues);

			var test = DataPacket.ComparePacket(cloneLeft.PacketValues, cloneRight.PacketValues);
			if (test == null)
			{
				return 0;
			}
			if (test == true)
			{
				return -1;
			}
			return 1;
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

		public IEnumerable<DataPacket> GetPart2Packets()
		{
			var lineIndex = 0;
			while (lineIndex < _lines.Count())
			{
				var left = new DataPacket(_lines.Skip(lineIndex++).First());
				var right = new DataPacket(_lines.Skip(lineIndex++).First());
				yield return left;
				yield return right;
				lineIndex++;
			}
			yield return new DataPacket("[[2]]")
			{
				DividerIndex = 1
			};
			yield return new DataPacket("[[6]]")
			{
				DividerIndex = 2
			};
		}
	}
}