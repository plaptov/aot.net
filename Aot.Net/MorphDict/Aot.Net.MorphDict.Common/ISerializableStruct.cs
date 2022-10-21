namespace Aot.Net.MorphDict.Common
{
	public interface ISerializableStruct<T> where T : unmanaged
	{
		ReadOnlySpan<byte> RestoreFromBytes(ReadOnlySpan<byte> bytes);
	}
}
