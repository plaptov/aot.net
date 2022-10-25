namespace Aot.Net
{
	public static class Encodings
	{
		static Encodings()
		{
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
			Win1251 = Encoding.GetEncoding(1251);
		}

		public static Encoding Win1251 { get; }
	}
}
