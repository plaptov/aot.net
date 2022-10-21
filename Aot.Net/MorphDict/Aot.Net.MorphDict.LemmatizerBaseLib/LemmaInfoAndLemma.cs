using System.Runtime.InteropServices;

using Aot.Net.MorphDict.Common;
using Aot.Net.MorphDict.MorphWizardLib;

using static Aot.Net.MorphDict.Common.StructSerializer;

namespace Aot.Net.MorphDict.LemmatizerBaseLib
{
	[StructLayout(LayoutKind.Sequential)]
	public struct LemmaInfoAndLemma : ISerializableStruct<LemmaInfoAndLemma>
	{
		public LemmaInfo LemmaInfo;
		public int LemmaStrNo;

		public ReadOnlySpan<byte> RestoreFromBytes(ReadOnlySpan<byte> bytes)
		{
			(LemmaInfo, bytes) = Read<LemmaInfo>(bytes);
			(LemmaStrNo, bytes) = ReadInt32(bytes);
			return bytes;
		}
	}
}
