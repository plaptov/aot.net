namespace Aot.Net
{
	public readonly ref struct IndexBasedReadOnlySpan<T>
	{
		private readonly IReadOnlyList<T> _source;

		public int Start { get; }

		public int Length { get; }

		public bool IsEmpty => Length == 0;

		public IndexBasedReadOnlySpan(IReadOnlyList<T> source, Range range)
			: this(source, range.GetOffsetAndLength(source.Count))
		{
		}

		private IndexBasedReadOnlySpan(IReadOnlyList<T> source, (int Offset, int Length) x)
			: this(source, x.Offset, x.Length)
		{
		}

		public IndexBasedReadOnlySpan(IReadOnlyList<T> source, int start, int length)
		{
			_source = source;
			Start = start;
			Length = length;
		}

		public Enumerator GetEnumerator() => new(this);

		public ref struct Enumerator
		{
			private readonly IndexBasedReadOnlySpan<T> _span;
			private int _index;

			internal Enumerator(IndexBasedReadOnlySpan<T> span)
			{
				_span = span;
				_index = _span.Start - 1;
			}

			public T Current => _span._source[_index];

			public bool MoveNext() => !_span.IsEmpty && ++_index < _span.Start + _span.Length;
		}
	}
}
