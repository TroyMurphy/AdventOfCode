public class Monkey
{
	public int Id { get; init; }
	public int Divisor { get; set; }

	// a magic product of all the monkey divisors
	// initialize to zero to get divide by error
	public int Reducer { get; set; } = 0;

	public List<long> WorryItems { get; private set; } = new();

	public Func<long, long> Operation { get; private set; }

	public Func<long, bool> DivisibleTest { get; private set; }

	public int TrueMonkey { get; set; }

	public int FalseMonkey { get; set; }

	public Monkey(int id, long[] startingItems)
	{
		this.Id = id;
		this.Operation = (x) => throw new NotImplementedException();
		this.DivisibleTest = (x) => throw new NotImplementedException();
		this.WorryItems.AddRange(startingItems);
	}

	public IEnumerable<(int monkey, long worryLevel)> GetThrowToMonkeys(bool doApplyRelief = true)
	{
		foreach (var item in this.WorryItems)
		{
			var worryLevel = this.Operation(item);
			if (doApplyRelief)
			{
				worryLevel = ApplyRelief(worryLevel);
			}
			else
			{
				worryLevel = worryLevel % this.Reducer;
			}
			yield return DivisibleTest(worryLevel) ? (this.TrueMonkey, worryLevel) : (this.FalseMonkey, worryLevel);
		}
		this.WorryItems = new();
	}

	public void CatchItem(long item)
	{
		this.WorryItems.Add(item);
	}

	public long ApplyRelief(long worryLevel) => worryLevel / 3;

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

	public void SetDivisibleTest(int divisor)
	{
		this.Divisor = divisor;
		this.DivisibleTest = CreateDivisibleBy(divisor);
	}

	private static Func<long, bool> CreateDivisibleBy(int divisor)
	{
		return x => x % divisor == 0;
	}

	private static Func<long, long> CreateAdder(int amountToAdd)
	{
		return x => x + amountToAdd;
	}

	private static Func<long, long> CreateMultiplier(int multiplier)
	{
		return x => x * multiplier;
	}

	private static Func<long, long> CreateSquare()
	{
		return x => x * x;
	}
}