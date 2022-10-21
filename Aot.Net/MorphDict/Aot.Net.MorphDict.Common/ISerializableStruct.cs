using System.Runtime.InteropServices;

namespace Aot.Net.MorphDict.Common
{
	internal interface ISerializableStruct<T> where T : struct
	{
		static int Size() => Marshal.SizeOf(typeof(T));
		void RestoreFromBytes(ReadOnlySpan<byte> bytes);
	}
}
