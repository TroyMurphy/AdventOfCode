public class Monkey
{
	private static int[] Primes = new int[] { 2, 3, 5, 7, 9, 11, 13, 17 };

	public int Id { get; init; }
	public List<int> WorryItems { get; private set; } = new();

	public Func<int, int> Operation { get; private set; }

	public Func<int, bool> DivisibleTest { get; private set; }

	public int TrueMonkey { get; set; }

	public int FalseMonkey { get; set; }

	public Monkey(int id, int[] startingItems)
	{
		this.Id = id;
		this.WorryItems.AddRange(startingItems);
	}

	public IEnumerable<(int monkey, int worryLevel)> GetThrowToMonkeys(bool applyRelief = true)
	{
		foreach (var item in this.WorryItems)
		{
			var worryLevel = this.Operation(item);
			if (applyRelief)
			{
				worryLevel = ApplyRelief(worryLevel);
			}
			yield return DivisibleTest(worryLevel) ? (this.TrueMonkey, worryLevel) : (this.FalseMonkey, worryLevel);
		}
		this.WorryItems = new();
	}

	public void CatchItem(int item)
	{
		this.WorryItems.Add(item);
	}

	public int ApplyRelief(int worryLevel) => worryLevel / 3;

	public void SetOperation(string symbol, string variable)
	{
		if (symbol == "*")
		{
			this.Operation = variable switch
			{
				"old" => CreateSquare(),
				_ => CreateMultiplier(int.Parse(variable))
			};
			return;
		}
		this.Operation = CreateAdder(int.Parse(variable));
	}

	public void SetDivisbleTest(int divisor)
	{
		this.DivisibleTest = CreateDivisibleBy(divisor);
	}

	private static Func<int, bool> CreateDivisibleBy(int divisor)
	{
		return x => x % divisor == 0;
	}

	private static Func<int, int> CreateAdder(int amountToAdd)
	{
		return x => x + amountToAdd;
	}

	private static Func<int, int> CreateMultiplier(int multiplier)
	{
		return x => x * multiplier;
	}

	private static Func<int, int> CreateSquare()
	{
		return x => x * x;
	}
}