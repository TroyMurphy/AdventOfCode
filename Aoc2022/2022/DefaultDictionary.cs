namespace _2022
{
	internal class DefaultDictionary<TKey, TValue> : Dictionary<TKey, TValue>
	where TKey : notnull
	where TValue : struct
	{
		private readonly TValue Default = default;

		public DefaultDictionary()
		{ }

		public DefaultDictionary(TValue defaultValue) => Default = defaultValue;

		public new TValue this[TKey key]
		{
			get
			{
				if (!TryGetValue(key, out TValue value))
				{
					value = Default;
					Add(key, value);
				}
				return value;
			}
			set => base[key] = value;
		}
	}
}