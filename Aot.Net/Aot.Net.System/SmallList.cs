﻿namespace Aot.Net
{
	public ref struct SmallList<T> where T : unmanaged
	{
		private int _count;
		private Span<T> _span;
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
			_span.CopyTo(array);
			if (_count >= _span.Length)
				_list!.CopyTo(array, _span.Length);
			return array;
		}
	}
}