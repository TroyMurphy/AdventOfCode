namespace _2022.Day16
{
	public class Day16
	{
		public readonly IEnumerable<string> _lines;
		// public Dictionary<string, int> flowRates = new();
		// public Dictionary<string, List<string>> tunnels = new();
		public Graph<string> graph;

		public List<Node<string>> importantCaves; // the start cave, and any cave with nonzero value 
		public int[,] importantCaveDistances;
		public int[] bitwiseValveMasks;

		public string start;

		public Day16()
		{

			// this._lines = Utilities.ReadLines(@"./Day16/inputs/input.txt");
			this._lines = Utilities.ReadLines(@"./Day16/inputs/demo1.txt");

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
			var allDists = graph.GetFloydWarshall();

			// keep only the caves that we might actually visit into the minimized distance matrix
			// keep as lookup value from dictionary
			this.importantCaves = graph.Nodes.Values.Where(x => x.Id == 0 || x.Weight!.Value > 0).OrderBy(x => x.Id).ToList();
			var importantCaveIndexes = graph.Nodes.Values.Where(x => x.Id == 0 || x.Weight!.Value > 0).Select(x => x.Id).ToList();

			var importantCavesCount = importantCaves.Count();
			var allCavesSize = graph.Nodes.Keys.Count();
			this.importantCaveDistances = new int[importantCavesCount, importantCavesCount];

			var importantCavesRow = 0;
			var importantCavesCol = 0;
			for (int i = 0; i < allCavesSize; i++)
			{
				if (importantCaveIndexes.Contains(i))
				{
					for (int j = 0; j < allCavesSize; j++)
					{
						if (importantCaveIndexes.Contains(j))
						{
							importantCaveDistances[importantCavesRow, importantCavesCol++] = allDists[i, j];
						}
					}
					importantCavesRow++;
					importantCavesCol = 0;
				}
			}

			int newNodeCount = 0;
			foreach (var n in importantCaves)
			{
				n.Id = newNodeCount++;
			}

			// We only track valves we might turn on. If valve state is all ones, we can terminate
			bitwiseValveMasks = new int[importantCavesCount];
			for (int i = 0; i < importantCavesCount; i++)
			{
				bitwiseValveMasks[i] = 1 << i;
			}
		}

		public void Solve()
		{
			SolvePartOne();
		}


		private void SolvePartOne()
		{
			DefaultDictionary<int, int> bestFlows = new();
			Visit(importantCaves.First(), 30, 0, 0, bestFlows);
			var bestFlow = bestFlows.Values.Max();
			Console.WriteLine($"The best flow is {bestFlow}");
		}

		private void SolvePartTwo()
		{
		}

		// bestFlows maps a valve state (which will intrinsically ordered from bitwise ands so we don't add to a lookup more than once
		private void Visit(Node<string> node, int timeRemaining, int visitedMask, int accFlow, DefaultDictionary<int, int> bestFlows)
		{
			// if we can get to the same set of valves with a better flow, that's the new best state to track.
			// time is implied by a higher flow for the same set of visited valves.
			bestFlows[visitedMask] = Math.Max(bestFlows[visitedMask], accFlow);
			// now we can go to any neighbor at any time 
			for (int i = 0; i < importantCaves.Count(); i++)
			{
				var toVisit = importantCaves[i];
				var timeIfTurnedOn = timeRemaining - importantCaveDistances[node.Id, toVisit.Id];
				var isNeighborVisited = (visitedMask & bitwiseValveMasks[i]) > 0;
				// check if the neighbor is a valid option with time remaining
				if (timeIfTurnedOn <= 0 || isNeighborVisited)
				{
					continue;
				}
				// otherwise we can just zap to the node using our distances and see if better
				Visit(toVisit, timeIfTurnedOn, (visitedMask | bitwiseValveMasks[i]), accFlow + (timeIfTurnedOn * toVisit.Weight!.Value), bestFlows);
			}
		}
	}
}