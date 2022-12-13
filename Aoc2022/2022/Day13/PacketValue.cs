using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2022.Day13
{
	public enum PacketType
	{
		ListStart,
		ListEnd,
		Number
	}

	public class PacketValue
	{
		public PacketType Type { get; set; }
		private int? Integer { get; set; }

		public int GetNumber()
		{
			if (this.Type != PacketType.Number || this.Integer == null)
			{
				throw new Exception();
			}
			return this.Integer.Value;
		}

		public PacketValue(int? value, PacketType type)
		{
			this.Integer = value;
			this.Type = type;
		}
	}
}