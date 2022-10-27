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
		public void One_lemma()
		{
			const string word = "спрятавшимся";

			var (found, s) = _lemmatizer.GetAllAncodesAndLemmasQuick(word, false, 1024, true);

			Assert.That(found, Is.True);
			Assert.That(s, Is.EqualTo("спряТАТЬСЯ мдмтмч#"));
		}

		[Test]
		public void Two_lemmas()
		{
			const string word = "стали";

			var (found, s) = _lemmatizer.GetAllAncodesAndLemmasQuick(word, false, 1024, true);

			Assert.That(found, Is.True);
			Assert.That(s, Is.EqualTo("стаТЬ кк#сталЬ гбгвгегжгй#"));
		}
	}
}
