using static _2022.Day20.Day20;

namespace _2022
{
	public class CycledLinkedList : LinkedList<GroveNum>
	{
		public LinkedListNode<GroveNum> GetNext(LinkedListNode<GroveNum> node)
		{
			var baseNext = node.Next;
			return baseNext ?? this.First ?? throw new Exception();
		}

		public LinkedListNode<GroveNum> GetPrevious(LinkedListNode<GroveNum> node)
		{
			var basePrevious = node.Previous;
			return basePrevious ?? this.Last ?? throw new Exception();
		}

		public void MoveNode(LinkedListNode<GroveNum> node, int distance)
		{
			GroveNum cacheValue = node.Value ?? throw new Exception();
			cacheValue.MarkMoved();

			if (int.Abs(distance) == this.Count - 1)
			{
				return;
			}
			if (distance > 0)
			{
				var tracer = GetPrevious(node);
				this.Remove(node);
				for (int i = 0; i < distance; i++)
				{
					tracer = this.GetNext(tracer);
				}
				this.AddAfter(tracer, cacheValue);
			}
			else
			{
				var tracer = GetNext(node);
				this.Remove(node);
				for (int i = 0; i < Math.Abs(distance); i++)
				{
					tracer = this.GetPrevious(tracer);
				}
				if (tracer == this.First)
				{
					this.AddAfter(this.Last, cacheValue);
					return;
				}
				this.AddBefore(tracer, cacheValue);
			}
		}

		public GroveNum GetAtIndex(int index)
		{
			var realIndex = index % this.Count;
			return this.ElementAt<GroveNum>(realIndex);
		}

		public int IndexOfZero()
		{
			var i = 0;
			var item = this.First;
			while (item.Value.Value != 0)
			{
				item = item.Next;
				i++;
			}
			return i;
		}

		public LinkedListNode<GroveNum> GetAtMoveNum(int moveNum)
		{
			var item = this.First;
			while (item.Value.MoveOrder != moveNum)
			{
				item = item.Next;
			}
			return item;
		}
	}
}