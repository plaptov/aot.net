using System.Runtime.InteropServices;
using Aot.Net.MorphDict.MorphWizardLib;

namespace Aot.Net.MorphDict.LemmatizerBaseLib
{
	[StructLayout(LayoutKind.Sequential)]
	public struct LemmaInfoAndLemma
	{
		public LemmaInfo LemmaInfo;
		public int LemmaStrNo;
	}
}
