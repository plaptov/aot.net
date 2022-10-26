using Aot.Net.MorphDict.LemmatizerBaseLib;

namespace Aot.Net.Tests
{
	[TestFixture]
	public class LemmatizerRussianTests
	{
		protected readonly LemmatizerRussian _lemmatizer = new();

		[OneTimeSetUp]
		public void Setup()
		{
			_lemmatizer.LoadDictionaries(
				new MemoryStream(MorphFiles.morph_forms_autom),
				new MemoryStream(MorphFiles.morph_annot),
				new MemoryStream(MorphFiles.morph_bases),
				new MemoryStream(MorphFiles.morph_options),
				new MemoryStream(MorphFiles.npredict_bin)
			);
		}

		[Test]
		public void LoadTest()
		{
			const string word = "спрятавшимся";

			var (found, s) = _lemmatizer.GetAllAncodesAndLemmasQuick(word, false, 1024, true);

			Assert.IsTrue(found);

		}

	}
}
