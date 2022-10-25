namespace Aot.Net.MorphDict.Common
{
	public class StringTokenizer : IEnumerable<string>
	{
		private readonly IEnumerable<string> _tokens;

		public StringTokenizer(string text, params char[] delims)
		{
			_tokens = text.TrimStart(delims).Split(delims, StringSplitOptions.RemoveEmptyEntries);
		}

		public IEnumerator<string> GetEnumerator()
		{
			return _tokens.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)_tokens).GetEnumerator();
		}
	}
}
