namespace Aot.Net.MorphDict.Common
{
	public class ShortStringHolder
	{
		private static readonly Encoding _encoding = Encoding.GetEncoding(1251);
		private readonly IReadOnlyList<string> _strings;

		private ShortStringHolder(IReadOnlyList<string> strings)
		{
			_strings = strings;
		}

		public string this[int i] => _strings[i];

		public int Count => _strings.Count;

		public static ShortStringHolder ReadShortStringHolder(Stream stream)
		{
			Span<byte> buf = stackalloc byte[4];
			stream.Read(buf);
			var count = BitConverter.ToInt32(buf);
			var list = new List<string>(count);

			buf = stackalloc byte[255];
			for (int i = 0; i < count; i++)
			{
				var len = (byte)stream.ReadByte();
				var slice = buf[..len];
				stream.Read(slice);
				var s = _encoding.GetString(slice);
				list.Add(s);
			}
			return new ShortStringHolder(list);
		}
	}
}
