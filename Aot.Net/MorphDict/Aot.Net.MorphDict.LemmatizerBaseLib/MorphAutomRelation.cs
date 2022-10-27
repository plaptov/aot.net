namespace Aot.Net.MorphDict.LemmatizerBaseLib
{
	public struct MorphAutomRelation
	{
		//  the highest byte of CMorphAutomRelation::m_Data contains relational char;
		//  the rest is an index to CMorphAutomat::m_Nodes 
		private uint _data;

		public MorphAutomRelation(uint data)
		{
			_data = data;
		}

		public readonly int GetChildNo()
		{
			return (int)(_data & 0xffffff);
		}

		/// <summary>
		/// Returns char in Win1251 or Win1252 codepage
		/// </summary>
		public readonly byte GetRelationalChar()
		{
			return (byte)(_data >> 24);
		}

		public void SetChildNo(uint v)
		{
			_data = (0xff000000 & _data) | v;
		}

		public void SetRelationalChar(byte v)
		{
			_data = (0xffffffu & _data) | ((uint)v << 24);
		}
	}
}
