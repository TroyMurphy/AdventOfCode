public class Node<T>
where T : notnull
{
	public T Key { get; set; }

	public int Id { get; set; }

	public int? Weight { get; set; }

	public List<Node<T>> Edges { get; set; }


	public Node(T key, int id)
	{
		this.Id = id;
		this.Key = key;
		this.Edges = new();
	}

	public void SetWeight(int weight)
	{
		this.Weight = weight;
	}

	public void AddEdge(Node<T> neighbor)
	{
		this.Edges.Add(neighbor);
	}

	public void AddEdges(IEnumerable<Node<T>> neighbors)
	{
		foreach (var n in neighbors)
		{
			this.AddEdge(n);
		}
	}
}