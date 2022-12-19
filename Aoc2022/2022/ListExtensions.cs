public static class LispExtensions
{
	public static List<T> Cons<T>(this List<T> list, T item)
	{
		return list.Concat(new List<T> { item }).ToList();
	}

	public static T Car<T>(this List<T> list)
	{
		return list.First();
	}

	public static List<T> Cdr<T>(this List<T> list)
	{
		return list.Skip(1).ToList();
	}
}
