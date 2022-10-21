using System.Runtime.InteropServices;

using Aot.Net.MorphDict.Common;

using static Aot.Net.MorphDict.Common.StructSerializer;

namespace Aot.Net.MorphDict.MorphWizardLib
{
	[StructLayout(LayoutKind.Sequential, Size = 8)]
	public struct LemmaInfo : ISerializableStruct<LemmaInfo>
	{
		public ushort FlexiaModelNo;
		public ushort AccentModelNo;
		public byte CommonAncode1;
		public byte CommonAncode2;

		public readonly string GetCommonAncodeIfCan()
		{
			if (CommonAncode1 == 0)
				return "";
			Span<byte> buf = stackalloc byte[] { CommonAncode1, CommonAncode2 };
			return Encoding.GetEncoding(1251).GetString(buf);
		}

		public ReadOnlySpan<byte> RestoreFromBytes(ReadOnlySpan<byte> bytes)
		{
			(FlexiaModelNo, bytes) = ReadUInt16(bytes);
			(AccentModelNo, bytes) = ReadUInt16(bytes);
			(CommonAncode1, bytes) = ReadByte(bytes);
			(CommonAncode2, bytes) = ReadByte(bytes);
			return bytes;
		}
	}
}
