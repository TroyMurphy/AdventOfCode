namespace _2022.Day13
{
	public class DataPacket
	{
		public LinkedList<PacketValue> PacketValues { get; set; }

		public DataPacket(string input)
		{
			var values = new LinkedList<PacketValue>();
			var depth = 0;
			foreach (var c in input)
			{
				if (c == ']')
				{
					values.AddLast(new PacketValue(null, depth));
				}

				depth += c switch
				{
					'[' => 1,
					']' => -1,
					_ => 0
				};

				if (char.IsDigit(c))
				{
					values.AddLast(new PacketValue(int.Parse(c.ToString()), depth));
				}
			}
			this.PacketValues = values;
		}

		public static bool ComparePacket(LinkedList<PacketValue> leftList, LinkedList<PacketValue> rightList)
		{
			if (leftList.First is null || rightList.First is null)
			{
				return rightList.First is null;
			}

			var leftItems = PullItems(leftList);
			var rightItems = PullItems(rightList);

			var leftCdr = DropItems(leftList, leftItems);
			var rightCdr = DropItems(rightList, rightItems);

			var result = CompareLists(leftItems, rightItems);
			if (result is null)
			{
				return ComparePacket(leftCdr, rightCdr);
			}
			return result.Value;
		}

		public static bool? CompareLists(List<int> left, List<int> right)
		{
			var index = 0;
			int itemLeft;
			int itemRight;
			while (index < left.Count)
			{
				try
				{
					itemLeft = left[index];
				}
				catch
				{
					return true;
				}
				try
				{
					itemRight = right[index];
				}
				catch
				{
					return false;
				}

				if (itemLeft == itemRight)
				{
					index++;
					continue;
				}
				return itemLeft < itemRight;
			}
			return null;
		}

		public static List<int> PullItems(LinkedList<PacketValue> leftList)
		{
			if (leftList.First is null)
			{
				throw new Exception("Check first");
			}

			List<int> items = new() { };

			LinkedListNode<PacketValue> firstLeft = leftList.First;

			if (firstLeft.Value.Number is null)
			{
				return items;
			}
			items.Add(firstLeft.Value.Number.Value);

			LinkedListNode<PacketValue>? nextLeft = firstLeft.Next;
			while (nextLeft is not null && nextLeft.Value.Depth == firstLeft.Value.Depth)
			{
				// remove the empty null at the end of every list
				if (nextLeft.Value.Number is not null)
				{
					items.Add(nextLeft.Value.Number.Value);
				}
				else
				{
					break;
				}
				nextLeft = nextLeft.Next;
			}
			return items;
		}

		public static LinkedList<PacketValue> DropItems(LinkedList<PacketValue> items, List<int> toDelete)
		{
			foreach (var _ in toDelete)
			{
				items.RemoveFirst();
			}
			while (items.First is not null && items.First.Value.Number is null)
			{
				items.RemoveFirst();
			}
			return items;
		}
	}
}