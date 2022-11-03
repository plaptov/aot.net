using Aot.Net.MorphDict.LemmatizerBaseLib;

namespace Aot.Net.Tests
{
	[TestFixture]
	public class LemmatizerRussianTests
	{
		protected readonly LemmatizerRussian _lemmatizer = new() { TreatApostropheAsHardSign = true };

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
			Assert.That(s, Is.EqualTo("СПРЯТАТЬСЯ мдмтмч#"));
		}

		[Test]
		public void Two_lemmas()
		{
			const string word = "стали";

			var (found, s) = _lemmatizer.GetAllAncodesAndLemmasQuick(word, false, 1024, true);

			Assert.That(found, Is.True);
			Assert.That(s, Is.EqualTo("СТАТЬ кк#СТАЛЬ гбгвгегжгй#"));
		}

		[Test]
		public void Should_lemmatize_span_with_modification()
		{
			Span<char> span = stackalloc char[] { ' ', 'п', 'о', 'д', '\'', 'е', 'з', 'д', 'о', 'в', '\t' };

			var lemma = _lemmatizer.GetBestLemma(span);

			Assert.That(lemma, Is.Not.Null);
			Assert.That(lemma, Is.EqualTo("ПОДЪЕЗД"));
			Assert.That(span.ToString(), Is.EqualTo(" ПОДЪЕЗДОВ\t"));
		}

		[Test]
		public void Should_lemmatize_readonlyspan_when_filtered()
		{
			var nonValidString = " под'ездов\t";

			var validString = _lemmatizer.FilterSrc(nonValidString);
			var lemma = _lemmatizer.GetBestLemma(validString.AsSpan());

			Assert.That(lemma, Is.Not.Null);
			Assert.That(lemma, Is.EqualTo("ПОДЪЕЗД"));
		}

		[Test]
		public void Should_not_lemmatize_readonlyspan_when_non_filtered()
		{
			var nonValidString = " под'ездов\t";

			Assert.Throws<InvalidOperationException>(() =>
				_lemmatizer.GetBestLemma(nonValidString.AsSpan()));
		}


		[Test]
		[TestCase("123456")]
		[TestCase("www.mos.ru")]
		[TestCase("ВТБ")]
		[TestCase("abc@example.com")]
		public void Input_as_result_when_not_a_word(string word)
		{
			var lemmas = _lemmatizer.GetBestLemma(word);

			Assert.That(lemmas, Is.EqualTo(word.ToUpperInvariant()));
		}

	}
}
