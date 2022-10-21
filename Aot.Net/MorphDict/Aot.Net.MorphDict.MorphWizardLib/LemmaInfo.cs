using System.Runtime.InteropServices;

namespace Aot.Net.MorphDict.MorphWizardLib
{
	[StructLayout(LayoutKind.Sequential, Size = 8)]
	public struct LemmaInfo
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
	}
}
