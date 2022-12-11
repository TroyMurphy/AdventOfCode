#nullable disable
using Grids;

namespace _2022.Tests
{
	public class Tests
	{
		private JaggedGrid<int> _sut;

		[SetUp]
		public void Setup()
		{
			string[] lines = new string[] { "123", "456", "789" };
			this._sut = Utilities.ParseJaggedGrid(lines);
		}

		[Test]
		public void Get4NeighborsFromCorner()
		{
			var points = this._sut.Get4Neighbors(0, 0).Select(x => x.Value);
			Assert.Multiple(() =>
			{
				Assert.That(points.Count, Is.EqualTo(2));
				Assert.That(points, Does.Contain(2));
				Assert.That(points, Does.Contain(4));
			});
		}

		[Test]
		public void Get4NeighborsFromEdge()
		{
			var points = this._sut.Get4Neighbors(1, 0).Select(x => x.Value);
			Assert.Multiple(() =>
			{
				Assert.That(points.Count, Is.EqualTo(3));
				Assert.That(points, Does.Contain(1));
				Assert.That(points, Does.Contain(3));
				Assert.That(points, Does.Contain(5));
			});
		}

		[Test]
		public void Get4NeighborsFromCenter()
		{
			var points = this._sut.Get4Neighbors(2, 2).Select(x => x.Value);
			Assert.Multiple(() =>
			{
				Assert.That(points.Count, Is.EqualTo(4));
				Assert.That(points, Does.Contain(2));
				Assert.That(points, Does.Contain(4));
				Assert.That(points, Does.Contain(6));
				Assert.That(points, Does.Contain(8));
			});
		}

		[Test]
		public void Get9NeighborsFromCenter()
		{
			var points = this._sut.Get8Neighbors(1, 1).Select(x => x.Value);
			Assert.Multiple(() =>
			{
				Assert.That(points.Count, Is.EqualTo(8));
				Assert.That(points, Does.Contain(1));
				Assert.That(points, Does.Contain(2));
				Assert.That(points, Does.Contain(3));
				Assert.That(points, Does.Contain(4));
				Assert.That(points, Does.Contain(6));
				Assert.That(points, Does.Contain(7));
				Assert.That(points, Does.Contain(8));
				Assert.That(points, Does.Contain(9));
			});
		}
	}
}