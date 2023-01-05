public class Graph<T>
where T : notnull
{

	public Dictionary<T, Node<T>> Nodes { get; set; }

	// public Dictionary<(T startKey, T endKey), int> Distances;
	private Lazy<HashSet<(Node<T> from, Node<T> to)>> Edges;

	public Graph(Dictionary<T, (IList<T> edges, int weight)> adjacency)
	{
		this.Nodes = new();
		var nodeCounter = 0;
		foreach (var nodeKey in adjacency.Keys)
		{
			this.Nodes[nodeKey] = new Node<T>(nodeKey, nodeCounter++);
		}
		foreach (var nodeKey in adjacency.Keys)
		{
			var (edges, weight) = adjacency[nodeKey];
			var node = this.GetNode(nodeKey);
			if (node is null || edges is null)
			{
				throw new Exception();
			}
			node.AddEdges(edges.Select(e => GetNode(e)));
			node.SetWeight(weight);
		}
		this.Edges = new Lazy<HashSet<(Node<T>, Node<T>)>>(InitEdges);
	}

	public HashSet<(Node<T>, Node<T>)> InitEdges()
	{
		var edges = new HashSet<(Node<T>, Node<T>)>();
		foreach (Node<T> node in this.Nodes.Values)
		{
			foreach (Node<T> neighbor in node.Edges)
			{
				edges.Add((node, neighbor));
			}
		}
		return edges;
	}

	public Node<T> GetNode(T key) => this.Nodes[key];

	public List<T> GetDijkstra(T startKey, T endKey)
	{
		var start = GetNode(startKey);
		var bests = new Dictionary<T, double>();
		foreach (var node in this.Nodes)
		{
			bests[node.Key] = double.MaxValue;
		}
		var toVisit = new PriorityQueue<(T, double), double>(this.Nodes.Count);
		toVisit.Enqueue((startKey, 0), 0);
		// priority queue with the node name, its best weight, and priority
		var visited = new Dictionary<T, T?>();
		visited[start.Key] = default(T?);

		while (toVisit.Count > 0)
		{
			var (vertex, w) = toVisit.Dequeue();
			var neighbors = GetNode(vertex).Edges;

			foreach (var n in neighbors)
			{
				if (visited.ContainsKey(n.Key))
				{
					continue;
				}

				var weightToNeighbor = w + n.Weight;
				var currentNeighborWeight = bests[n.Key];

				if (weightToNeighbor < currentNeighborWeight)
				{
					bests[n.Key] = weightToNeighbor;
					visited[n.Key] = vertex;
					toVisit.Enqueue((n.Key, weightToNeighbor), weightToNeighbor);
				}
			}
		}

		var solution = new List<T>();
		var solutionNode = endKey;
		while (solutionNode is not null)
		{
			solution.Add(solutionNode);
			solutionNode = visited[solutionNode];
		}
		solution.Reverse();
		return solution;
	}

	public int[,] GetFloydWarshall()
	{
		int V = this.Nodes.Keys.Count();
		var dist = new int[V, V];
		for (int i = 0; i < V; i++)
		{
			for (int j = 0; j < V; j++)
			{
				// don't set to max because it's going to be increased
				dist[i, j] = int.MaxValue >> 2;
			}
		}

		for (int i = 0; i < V; i++)
		{
			dist[i, i] = 0;
		}

		foreach (var edge in this.Edges.Value.AsEnumerable())
		{
			dist[edge.from.Id, edge.to.Id] = 1;
		}

		for (int k = 0; k < V; k++)
		{
			for (int i = 0; i < V; i++)
			{
				for (int j = 0; j < V; j++)
				{
					// prevent overflow
					if (dist[i, j] > dist[i, k] + dist[k, j])
					{
						dist[i, j] = dist[i, k] + dist[k, j];
						dist[j, i] = dist[i, j];
					}
				}
			}
		}
		return dist;
	}
}