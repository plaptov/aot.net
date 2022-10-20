namespace Aot.Net.MorphDict.LemmatizerBaseLib
{
	public struct MorphAutomNode
	{
		//  the highest bit of CMorphAutomNode::m_Data contains final/not final flag;
		//  the rest is an index to CMorphAutomat::m_Relations (an index 
		//   to the place where the first child is)
		private uint _data;

		public MorphAutomNode(uint data)
		{
			_data = data;
		}

		public readonly int GetChildrenStart()
		{
			return (int)(_data & (0x80000000 - 1));
		}

		public readonly bool IsFinal()
		{
			return (_data & 0x80000000) > 0;
		}

		public void SetChildrenStart(int v)
		{
			_data = (0x80000000 & _data) | (uint)v;
		}

		public void SetFinal(bool v)
		{
			if (v)
				_data |= 0x80000000;
			else
				_data &= 0x80000000 - 1;
		}
	}
}
