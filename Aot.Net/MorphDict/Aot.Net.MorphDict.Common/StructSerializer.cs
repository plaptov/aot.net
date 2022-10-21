using System.Runtime.InteropServices;

namespace Aot.Net.MorphDict.Common
{
	public static class StructSerializer
	{
		public static T[] ReadVectorInner<T>(Stream stream, int count)
			where T : unmanaged, ISerializableStruct<T>
		{
			var list = new T[count];
			Span<byte> buf = stackalloc byte[Size<T>()];
			for (int i = 0; i < count; i++)
			{
				stream.Read(buf);
				list[i] = Read<T>(buf).Value;
			}
			return list;
		}

		public static ReadResult<T> Read<T>(ReadOnlySpan<byte> bytes)
			where T : unmanaged, ISerializableStruct<T>
		{
			T item = default;
			item.RestoreFromBytes(bytes);
			return new ReadResult<T>(item, bytes);
		}

		public static ReadResult<int> ReadInt32(ReadOnlySpan<byte> bytes)
		{
			var i = BitConverter.ToInt32(bytes);
			return new ReadResult<int>(i, bytes);
		}

		public static ReadResult<ushort> ReadUInt16(ReadOnlySpan<byte> bytes)
		{
			var i = BitConverter.ToUInt16(bytes);
			return new ReadResult<ushort>(i, bytes);
		}

		public static ReadResult<byte> ReadByte(ReadOnlySpan<byte> bytes)
		{
			var i = bytes[0];
			return new ReadResult<byte>(i, bytes);
		}

		public static int Size<T>() where T : unmanaged => Marshal.SizeOf<T>();

		public readonly ref struct ReadResult<T> where T : unmanaged
		{
			internal ReadResult(T value, ReadOnlySpan<byte> bytes)
			{
				Value = value;
				Bytes = bytes[Size<T>()..];
			}

			public T Value { get; }
			public ReadOnlySpan<byte> Bytes { get; }

			public void Deconstruct(out T value, out ReadOnlySpan<byte> bytes)
			{
				value = Value;
				bytes = Bytes;
			}
		}
	}
}
