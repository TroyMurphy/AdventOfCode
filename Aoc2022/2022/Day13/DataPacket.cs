namespace _2022.Day13
{
	public class DataPacket
	{
		public LinkedList<PacketValue> PacketValues { get; set; }
		public int? DividerIndex { get; set; } = 0;

		public DataPacket(LinkedList<PacketValue> input)
		{
			var newValues = new LinkedList<PacketValue>();
			foreach (var item in input)
			{
				newValues.AddLast(item);
			}
			this.PacketValues = newValues;
		}

		public DataPacket(string input)
		{
			var values = new LinkedList<PacketValue>();
			var cIndex = 0;
			while (cIndex < input.Length)
			{
				var c = input[cIndex++];
				if (c == '[')
				{
					values.AddLast(new PacketValue(null, PacketType.ListStart));
					continue;
				}
				if (c == ']')
				{
					values.AddLast(new PacketValue(null, PacketType.ListEnd));
					continue;
				}
				if (char.IsDigit(c))
				{
					List<char> digits = new() { };

					// we know this isn't the last character
					while (char.IsDigit(c))
					{
						digits.Add(c);
						c = input[cIndex++];
					}
					values.AddLast(new PacketValue(int.Parse(new string(digits.ToArray())), PacketType.Number));
					cIndex--;
				}
			}
			this.PacketValues = values;
		}

		public static string GetString(LinkedList<PacketValue> input)
		{
			return string.Join("", input.Select(x =>
			{
				return x.Type switch
				{
					PacketType.ListStart => "[",
					PacketType.ListEnd => "]",
					_ => $"{x.GetNumber()},"
				};
			}).ToList());
		}

		public static bool? ComparePacket(LinkedList<PacketValue> leftList, LinkedList<PacketValue> rightList)
		{
			if (leftList.First is null || rightList.First is null)
			{
				if (leftList.First is null && rightList.First is null)
				{
					return null;
				}
				return rightList.First is not null;
			}

			var leftType = leftList.First.Value.Type;
			var rightType = rightList.First.Value.Type;

			// both ints
			if (leftType == PacketType.Number && rightType == PacketType.Number)
			{
				if (leftList.First.Value.GetNumber() == rightList.First.Value.GetNumber())
				{
					leftList.RemoveFirst();
					rightList.RemoveFirst();
					return ComparePacket(leftList, rightList);
				}
				return leftList.First.Value.GetNumber() < rightList.First.Value.GetNumber();
			}
			if (leftType == PacketType.Number)
			{
				var firstNode = leftList.First;
				leftList.AddBefore(firstNode, new PacketValue(null, PacketType.ListStart));
				leftList.AddAfter(firstNode, new PacketValue(null, PacketType.ListEnd));
				return ComparePacket(leftList, rightList);
			}
			if (rightType == PacketType.Number)
			{
				var firstNode = rightList.First;
				rightList.AddBefore(firstNode, new PacketValue(null, PacketType.ListStart));
				rightList.AddAfter(firstNode, new PacketValue(null, PacketType.ListEnd));
				return ComparePacket(leftList, rightList);
			}
			// both lists
			var (lCar, lCdr) = GetFirst(leftList);
			var (rCar, rCdr) = GetFirst(rightList);

			// strip first element of each list down
			lCar.RemoveFirst();
			lCar.RemoveLast();
			rCar.RemoveFirst();
			rCar.RemoveLast();

			var firstCompare = ComparePacket(lCar, rCar);
			return firstCompare ?? ComparePacket(lCdr, rCdr);
		}

		public static (LinkedList<PacketValue> car, LinkedList<PacketValue> cdr) GetFirst(LinkedList<PacketValue> input)
		{
			if (input.First is null || input.First.Value.Type != PacketType.ListStart)
			{
				throw new Exception();
			}
			var car = new LinkedList<PacketValue>();
			var depth = 0;
			do
			{
				var element = input.First;
				if (element.Value.Type == PacketType.ListStart)
				{
					depth++;
				}
				if (element.Value.Type == PacketType.ListEnd)
				{
					depth--;
				}
				car.AddLast(element.Value);
				input.RemoveFirst();
				continue;
			} while (depth > 0);
			return (car, input);
		}
	}
}