public class Node<T>
where T : notnull
{
	private int _weight = 1;

	public T Key { get; set; }

	public int Id { get; set; }


	public List<Node<T>> Edges { get; set; }


	public Node(T key, int id)
	{
		this.Id = id;
		this.Key = key;
		this.Edges = new();
	}

	public void SetWeight(int weight)
	{
		this._weight = weight;
	}

	public int Weight { get => this._weight; }

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