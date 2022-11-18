using Aot.Net.MorphDict.LemmatizerPackage.Russian.Default;

namespace Aot.Net.MorphDict.LemmatizerBaseLib
{
	public static class LemmatizerRussianTests
	{
		public static LemmatizerRussian LoadDefaultPackage(this LemmatizerRussian lemmatizer)
		{
			lemmatizer.LoadDictionaries(
				new MemoryStream(MorphFiles.morph_forms_autom),
				new MemoryStream(MorphFiles.morph_annot),
				new MemoryStream(MorphFiles.morph_bases),
				new MemoryStream(MorphFiles.morph_options),
				new MemoryStream(MorphFiles.npredict_bin)
			);
			return lemmatizer;
		}
	}
}
