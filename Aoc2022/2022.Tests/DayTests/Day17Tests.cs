using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2022.Tests.Day17
{
	public class Day17Tests
	{
		private _2022.Day17.Day17 day;

		[SetUp]
		public void SetUp()
		{
			this.day = new _2022.Day17.Day17(true);
		}

		[Test]
		public void GetGusts()
		{
			var gust = this.day.NextJet();
			Assert.That(gust, Is.EqualTo(1));
			gust = this.day.NextJet();
			Assert.That(gust, Is.EqualTo(1));
			gust = this.day.NextJet();
			Assert.That(gust, Is.EqualTo(1));
			gust = this.day.NextJet();
			Assert.That(gust, Is.EqualTo(-1));
		}

		[Test]
		public void GetRocks()
		{
			var rock = this.day.NextRock();
			Assert.That(rock.Id, Is.EqualTo(1));
			rock = this.day.NextRock();
			Assert.That(rock.Id, Is.EqualTo(2));
			rock = this.day.NextRock();
			Assert.That(rock.Id, Is.EqualTo(3));
			rock = this.day.NextRock();
			Assert.That(rock.Id, Is.EqualTo(4));
			rock = this.day.NextRock();
			Assert.That(rock.Id, Is.EqualTo(5));
			rock = this.day.NextRock();
			Assert.That(rock.Id, Is.EqualTo(1));
		}

		[Test]
		public void TestTopologyHash()
		{
			this.day.columns = new HashSet<int>[]
				{
					new HashSet<int> { },
					new HashSet<int> {1, 2 },
					new HashSet<int> { 0},
					new HashSet<int> {1,3 },
					new HashSet<int> {2 },
					new HashSet<int> { },
					new HashSet<int> { },
				};
			var hash = this.day.CreateTopologyHash();
			Assert.That(hash, Is.EqualTo("_.1-2.0.1-3.2._._"));
		}
	}
}