using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2022.Day22;
using static _2022.Day22.Day22;

namespace _2022.Tests.Day22
{
	public class Day22Tests
	{
		private _2022.Day22.Day22 day;

		[SetUp]
		public void SetUp()
		{
			this.day = new _2022.Day22.Day22(false);
		}

		[Test]
		public void TestNorth()
		{
			day.player.Position = (52, 0);
			day.player.Facing = Direction.N;

			day.Maze.Navigate(day.player, 1);

			Assert.That(day.player.Position.x, Is.EqualTo(0));
			Assert.That(day.player.Position.y, Is.EqualTo(152));
		}
	}
}