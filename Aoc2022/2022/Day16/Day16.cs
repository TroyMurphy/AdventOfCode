namespace _2022.Day16
{
	public class Day16
	{
		public readonly IEnumerable<string> _lines;
		// public Dictionary<string, int> flowRates = new();
		// public Dictionary<string, List<string>> tunnels = new();
		public Graph<string> graph;
		readonly int[,] allDistances;

		public List<Node<string>> importantCaves; // the start cave, and any cave with nonzero value 
		public int[,] importantCaveDistances;
		public Dictionary<string, long> bitwiseValveMasks;

		public string start;

		public Day16()
		{

			this._lines = Utilities.ReadLines(@"./Day16/inputs/input.txt");
			// this._lines = Utilities.ReadLines(@"./Day16/inputs/demo1.txt");

			var infoDictionary = new Dictionary<string, (IList<string>, int)>();

			foreach (var line in this._lines)
			{
				var cur = line.Split(" ").Skip(1).First();
				if (start == null)
				{
					start = cur;
				}
				var rate = int.Parse(line.Substring(line.IndexOf("=") + 1).Split(";").First());
				var tunnels = line.Replace("valves", "valve").Substring(line.IndexOf("to valve") + 9).Split(new string[] { ",", " " }, StringSplitOptions.RemoveEmptyEntries).ToList();
				infoDictionary[cur] = (tunnels, rate);
			};
			this.graph = new Graph<string>(infoDictionary);
			this.allDistances = graph.GetFloydWarshall();

			// keep only the caves that we might actually visit into the minimized distance matrix
			// keep as lookup value from dictionary
			var importantCaveIds = graph.Nodes.Values.Where(x => x.Key == "AA" || x.Weight!.Value > 0).Select(x => x.Id).ToList();

			var importantCavesCount = importantCaveIds.Count();

			var allCavesSize = graph.Nodes.Keys.Count();
			this.importantCaveDistances = new int[importantCavesCount, importantCavesCount];

			var deleteIndexes = this.graph.Nodes.Values.Where(x => !importantCaveIds.Any(keep => keep == x.Id));

			for (int i = 0, importantI = 0; i < allCavesSize; i++)
			{
				if (!importantCaveIds.Contains(i))
				{
					continue;
				}
				for (int j = 0, importantJ = 0; j < allCavesSize; j++)
				{
					if (!importantCaveIds.Contains(j))
					{
						continue;
					}
					importantCaveDistances[importantI, importantJ++] = allDistances[i, j];
				}
				importantI++;
			}

			importantCaves = new();
			bitwiseValveMasks = new();
			int newNodeCount = 0;
			foreach (var id in importantCaveIds)
			{
				var cave = graph.Nodes.Values.First(x => x.Id == id);
				cave.Id = newNodeCount++;
				importantCaves.Add(cave);
				bitwiseValveMasks[cave.Key] = 1 << newNodeCount;
			}

			// We only track valves we might turn on. If valve state is all ones, we can terminate
		}

		public void Solve()
		{
			SolvePartTwo();
		}


		private void SolvePartOne()
		{
			DefaultDictionary<long, int> bestFlows = new();
			var home = importantCaves.First(x => x.Key == "AA");
			Visit(home, 30, 0, 0, bestFlows);
			var bestFlow = bestFlows.Values.Max();
			Console.WriteLine($"The best flow is {bestFlow}");
		}

		private void SolvePartTwo()
		{
			DefaultDictionary<long, int> bestFlows = new();
			var home = importantCaves.First(x => x.Key == "AA");
			Visit(home, 26, 0, 0, bestFlows);

			int best = 0;

			foreach (var kvp1 in bestFlows)
			{
				foreach (var kvp2 in bestFlows)
				{
					if ((kvp1.Key & kvp2.Key) != 0) continue; //Only care if valves for disjoint set
					best = int.Max(best, kvp1.Value + kvp2.Value);
				}
			}

			Console.WriteLine($"Hey it is {best}");
		}

		// bestFlows maps a valve state (which will intrinsically ordered from bitwise ands so we don't add to a lookup more than once
		private void Visit(Node<string> node, int timeRemaining, long visitedMask, int accFlow, DefaultDictionary<long, int> bestFlows)
		{
			// if we can get to the same set of valves with a better flow, that's the new best state to track.
			// time is implied by a higher flow for the same set of visited valves.
			bestFlows[visitedMask] = Math.Max(bestFlows[visitedMask], accFlow);
			// now we can go to any neighbor at any time 
			foreach (var toVisit in importantCaves)
			{
				if (toVisit.Id == node.Id)
				{
					continue;
				}
				var timeIfTurnedOn = timeRemaining - importantCaveDistances[node.Id, toVisit.Id] - 1;
				var isCaveVisited = (visitedMask & bitwiseValveMasks[toVisit.Key]) != 0;
				// check if the neighbor is a valid option with time remaining
				if (timeIfTurnedOn <= 0 || isCaveVisited)
				{
					continue;
				}
				// otherwise we can just zap to the node using our distances and see if better
				Visit(toVisit, timeIfTurnedOn, (visitedMask | bitwiseValveMasks[node.Key]), accFlow + (timeIfTurnedOn * toVisit.Weight!.Value), bestFlows);
			}
		}
	}
}