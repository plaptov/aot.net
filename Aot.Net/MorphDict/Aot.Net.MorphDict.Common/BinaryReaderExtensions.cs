using System.Text;

namespace Aot.Net
{
	public static class BinaryReaderExtensions
	{
		public static string ReadTerminatedShortString(this BinaryReader reader)
		{
			Span<char> buf = stackalloc char[16];
			int i = 0;
			char c;
			while ((c = reader.ReadChar()) != '\n')
				buf[i++] = c;
			return new string(buf[..i]);
		}

		public static string ReadTerminatedLongString(this BinaryReader reader)
		{
			var sb = new StringBuilder();
			char c;
			while ((c = reader.ReadChar()) != '\n')
				sb.Append(c);
			return sb.ToString();
		}

		public static string ReadTerminatedString1251(this BinaryReader reader)
		{
			Span<byte> buf = stackalloc byte[128];
			int i = 0;
			byte b;
			while ((b = reader.ReadByte()) != '\n')
				buf[i++] = b;
			return Encodings.Win1251.GetString(buf[..i]);
		}
	}
}
