namespace _2022.Tests.Day23
{
	public class Day23Tests
	{
		private _2022.Day23.Day23 day;

		[SetUp]
		public void SetUp()
		{
			this.day = new _2022.Day23.Day23(true);
		}

		[Test]
		public void TestOneRound()
		{
			//Assert.That(day.ElfPositions.ToList(), Does.Contain((2, 1)));
			//Assert.That(day.ElfPositions.ToList(), Does.Contain((3, 1)));
			//Assert.That(day.ElfPositions.ToList(), Does.Contain((2, 2)));
			//Assert.That(day.ElfPositions.ToList(), Does.Contain((2, 4)));
			//Assert.That(day.ElfPositions.ToList(), Does.Contain((3, 4)));
			day.DoRound();
			var elves = day.ElfSet.Select(x => x.Position).ToList();
			Assert.That(elves, Does.Contain((2, 0)));
			Assert.That(elves, Does.Contain((3, 0)));
			Assert.That(elves, Does.Contain((2, 2)));
			Assert.That(elves, Does.Contain((2, 4)));
			Assert.That(elves, Does.Contain((3, 3)));
		}
	}
}