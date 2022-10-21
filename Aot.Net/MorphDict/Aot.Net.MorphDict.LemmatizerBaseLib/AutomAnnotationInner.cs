namespace Aot.Net.MorphDict.LemmatizerBaseLib
{
	public struct AutomAnnotationInner
	{
		public AutomAnnotationInner(int modelNo, ushort itemNo, ushort prefixNo)
		{
			ModelNo = modelNo;
			ItemNo = itemNo;
			PrefixNo = prefixNo;
			LemmaInfoNo = 0;
			Weight = 0;
		}

		// these members are read from the automat
		public int ModelNo { get; }
		public ushort ItemNo { get; }
		public ushort PrefixNo { get; }

		// these members are calculated later
		public int LemmaInfoNo { get; set; }
		public int Weight { get; set; }

		public readonly int GetParadigmId()
		{
			return (PrefixNo << 23) | LemmaInfoNo;
		}

		//public void SplitParadigmId(uint value)
		//{
		//	m_PrefixNo = (ushort)(value >> 23);
		//	m_LemmaInfoNo = (int)(value & 0x7fffff);
		//}
}
}
