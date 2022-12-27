namespace _2022.Tests.Day25
{
	public class Day25Tests
	{
		private _2022.Day25.Day25 day;

		[SetUp]
		public void SetUp()
		{
			this.day = new _2022.Day25.Day25(true);
		}

		[Test]
		public void TestSnafu()
		{
			var input = "2=-01";
			var dec = day.SnafuToDecimal(input);
			Assert.That(dec, Is.EqualTo(976));
		}

		[Test]
		public void DecimalToSnafu1()
		{
			var val = day.DecimalToSnafu(1);
			Assert.That(val, Is.EqualTo("1"));
		}

		[Test]
		public void DecimalToSnafu10()
		{
			var val = day.DecimalToSnafu(5);
			Assert.That(val, Is.EqualTo("10"));
		}

		[Test]
		public void DecimalToSnafu8()
		{
			var val = day.DecimalToSnafu(8);
			Assert.That(val, Is.EqualTo("2="));
		}

		[Test]
		public void DecimalToSnafu2022()
		{
			var val = day.DecimalToSnafu(2022);
			Assert.That(val, Is.EqualTo("1=11-2"));
		}

		[Test]
		public void DecimalToSnafuBigPi()
		{
			var val = day.DecimalToSnafu(314159265);
			Assert.That(val, Is.EqualTo("1121-1110-1=0"));
		}
	}
}