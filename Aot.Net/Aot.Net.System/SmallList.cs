namespace Aot.Net
{
	/// <summary>
	/// List-like structure which uses <see cref="Span{T}"/> as storage
	/// while possible and allocates <see cref="List{T}"/> when needed
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public ref struct SmallList<T>
	{
		private int _count;
		private readonly Span<T> _span;
		private List<T>? _list;

		public SmallList(Span<T> span)
		{
			_count = 0;
			_span = span;
			_list = null;
		}

		public void Add(T item)
		{
			if (_count <_span.Length - 1)
				_span[_count++] = item;
			else
			{
				_list ??= new List<T>();
				_list.Add(item);
			}
		}

		public T Last()
		{
			if (_count == 0)
				throw new InvalidOperationException("List is empty");
			if (_count < _span.Length)
				return _span[_count - 1];
			return _list![_count - _span.Length - 1];
		}

		public T[] ToArray()
		{
			if (_count == 0)
				return Array.Empty<T>();
			var array = new T[_count];
			_span[.._count].CopyTo(array);
			if (_count >= _span.Length)
				_list!.CopyTo(array, _span.Length);
			return array;
		}

		public List<T> ToList()
		{
			var list = new List<T>(_count);
			foreach (var item in _span[.._count])
				list.Add(item);
			if (_count >= _span.Length)
				list.AddRange(_list!);
			return list;
		}
	}
}
