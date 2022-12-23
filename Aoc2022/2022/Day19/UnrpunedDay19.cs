using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace _2022.Day19
{
	public class UnrpunedDay19
	{
		public readonly IEnumerable<string> _lines;
		public List<BluePrint> BluePrints = new();

		public UnrpunedDay19(bool test = true)
		{
			this._lines = GetLines(test);
			foreach (var line in _lines)
			{
				this.BluePrints.Add(new BluePrint(line));
			}
		}

		private IEnumerable<string> GetLines(bool test)
		{
			return test
				? Utilities.ReadLines(@"./Day19/inputs/demo1.txt")
				: Utilities.ReadLines(@"./Day19/inputs/input.txt");
		}

		public void Solve()
		{
			SolvePartOne();
		}

		private void SolvePartOne()
		{
			var emptyResources = new Dictionary<Resource, int>()
			{
				{ Resource.Ore, 0 },
				{ Resource.Clay, 0 },
				{ Resource.Obsidian, 0 },
				{ Resource.Geode, 0 }
			};
			var initialRobots = new Dictionary<Resource, int>() {
				{ Resource.Ore, 1 },
				{ Resource.Clay, 0 },
				{ Resource.Obsidian, 0 },
				{ Resource.Geode, 0 }
			};
			var cache = new BlueprintCache();

			CollectGeodes(24, emptyResources, this.BluePrints[0], initialRobots, cache);
			Console.WriteLine($"Produced Geode");
		}

		private void SolvePartTwo()
		{
		}

		public void CollectGeodes(int timeRemaining, Dictionary<Resource, int> resources, BluePrint blueprint, Dictionary<Resource, int> robots, BlueprintCache cache)
		{
			var key = (timeRemaining,
				(resources[Resource.Ore], resources[Resource.Clay], resources[Resource.Obsidian], resources[Resource.Clay]),
				(robots[Resource.Ore], robots[Resource.Clay], robots[Resource.Obsidian], robots[Resource.Clay]));

			if (cache.cache.ContainsKey(key))
			{
				if (cache.cache[key] > resources[Resource.Geode])
				{
					return;
				}
			}
			cache.cache[key] = resources[Resource.Geode];

			if (timeRemaining == 0)
			{
				return;
			}
			// produce resources
			var newResources = new Dictionary<Resource, int>(resources);
			foreach (var kvp in robots)
			{
				newResources[kvp.Key] += kvp.Value;
			}

			// first option is do nothing
			CollectGeodes(timeRemaining - 1, newResources, blueprint, robots, cache);

			foreach (var resource in new Resource[] { Resource.Ore, Resource.Clay, Resource.Obsidian, Resource.Geode })
			{
				if (CanBuyResource(blueprint, resources, resource))
				{
					// best if do buy
					var paidResources = new Dictionary<Resource, int>(resources);
					foreach (var itemCost in blueprint.Costs[resource])
					{
						paidResources[itemCost.resource] -= itemCost.cost;
					}
					var newRobots = new Dictionary<Resource, int>(robots);
					newRobots[resource] += 1;
					CollectGeodes(timeRemaining - 1, paidResources, blueprint, newRobots, cache);
					// best if don't buy
					CollectGeodes(timeRemaining - 1, newResources, blueprint, robots, cache);
				}
			}
		}

		public static bool CanBuyResource(BluePrint blueprint, Dictionary<Resource, int> resources, Resource resource)
		{
			var required = blueprint.Costs[resource];
			foreach (var itemCost in required)
			{
				if (resources[itemCost.resource] > itemCost.cost)
				{
					continue;
				}
				return false;
			}
			return true;
		}

		public class BluePrint
		{
			private int Id { get; set; }

			public Dictionary<Resource, List<(Resource resource, int cost)>> Costs { get; set; } = new()
			{
				{ Resource.Ore, new() },
				{ Resource.Clay, new() },
				{ Resource.Obsidian, new() },
				{ Resource.Geode, new() }
			};

			//public List<(Resource resource, int cost)> OreCost { get; init; } = new();
			//public List<(Resource resource, int cost)> ClayCost { get; init; } = new();
			//public List<(Resource resource, int cost)> ObsidianCost { get; init; } = new();
			//public List<(Resource resource, int cost)> GeodeCost { get; init; } = new();

			public BluePrint(string bpString)
			{
				//"Blueprint 1: Each ore robot costs 4 ore. Each clay robot costs 4 ore. Each obsidian robot costs 4 ore and 7 clay. Each geode robot costs 2 ore and 19 obsidian.";
				var parts = bpString.Split(":");
				this.Id = int.Parse(parts[0].Substring("Blueprint ".Length));
				var recipes = parts[1].Split(".").Where(x => !string.IsNullOrWhiteSpace(x));

				var itemIndex = 0;
				foreach (var recipe in recipes)
				{
					var resources = recipe.Split("costs")[1].Split("and");
					foreach (var resource in resources)
					{
						var m = Regex.Match(resource, @".*(\d+) (\w+).*");
						var c = int.Parse(m.Groups[1].Captures[0].Value);
						var r = m.Groups[2].Captures[0].Value switch
						{
							"ore" => Resource.Ore,
							"clay" => Resource.Clay,
							"obsidian" => Resource.Obsidian,
							"geode" => Resource.Geode,
							_ => throw new Exception()
						};

						switch (itemIndex)
						{
							case 0:
								Costs[Resource.Ore].Add((r, c)); break;
							case 1:
								Costs[Resource.Clay].Add((r, c)); break;
							case 2:
								Costs[Resource.Obsidian].Add((r, c)); break;
							case 3:
								Costs[Resource.Geode].Add((r, c)); break;
						};
					}
					itemIndex++;
				}
			}
		}

		public enum Resource
		{
			Ore, Clay, Geode, Obsidian
		}

		public record BlueprintCache
		{
			public Dictionary<(int, (int, int, int, int), (int, int, int, int)), int> cache = new();
		}
	}
}