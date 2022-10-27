using Aot.Net.MorphDict.Common;

namespace Aot.Net
{
	public static class Encodings
	{
		static Encodings()
		{
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
			Win1251 = Encoding.GetEncoding(1251);
			Win1252 = Encoding.GetEncoding(1252);
			Span<byte> bytes = stackalloc byte[256];
			for (int i = 0; i < 256; i++)
				bytes[i] = (byte)i;
			Win1251Chars = Win1251.GetString(bytes).ToCharArray();
			Win1252Chars = Win1252.GetString(bytes).ToCharArray();
		}

		public static Encoding Win1251 { get; }

		public static char[] Win1251Chars { get; }

		public static Encoding Win1252 { get; }

		public static char[] Win1252Chars { get; }

		public static Encoding GetNonUnicodeEncoding(MorphLanguage language) => language switch
		{
			MorphLanguage.Russian => Win1251,
			_ => Win1252,
		};

		public static char[] GetNonUnicodeChars(MorphLanguage language) => language switch
		{
			MorphLanguage.Russian => Win1251Chars,
			_ => Win1252Chars,
		};
	}
}
