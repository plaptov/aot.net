namespace Aot.Net.MorphDict.Common
{
	internal class StructSerializer
	{
		public List<T> ReadVectorInner<T>(Stream stream, int count)
			where T : struct, ISerializableStruct<T>
		{
			var list = new List<T>(count);
			Span<byte> buf = stackalloc byte[ISerializableStruct<T>.Size()];
			for (int i = 0; i < count; i++)
			{
				stream.Read(buf);
				T item = default;
				item.RestoreFromBytes(buf);
				list.Add(item);
			}
			return list;
		}
	}
}
