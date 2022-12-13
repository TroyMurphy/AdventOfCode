using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2022.Day13
{
	public class PacketValue
	{
		public int? Number { get; set; }
		public int Depth { get; set; }

		public PacketValue(int? value, int depth)
		{
			this.Number = value;
			this.Depth = depth;
		}
	}
}