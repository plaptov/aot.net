using Aot.Net.MorphDict.Common;
using static Aot.Net.MorphDict.Common.Utils;

namespace Aot.Net.MorphDict.LemmatizerBaseLib
{
	public class CABCEncoder
	{
		public const int MaxAlphabetSize = 54;
		public const int ChildrenCacheSize = 1000;
		public const char MorphAnnotChar = '+';
		public const int MinimalPredictionSuffix = 3;

		private readonly string _criticalNounLetterPack;

		public MorphLanguage Language { get; }

		public char AnnotChar { get; }

		public int AlphabetSize { get; }

		public IReadOnlyDictionary<char, int> Alphabet2Code { get; }

		public IReadOnlyList<char> Code2Alphabet { get; }

		public int AlphabetSizeWithoutAnnotator { get; }

		public IReadOnlyDictionary<char, int> Alphabet2CodeWithoutAnnotator { get; }

		public IReadOnlyList<char> Code2AlphabetWithoutAnnotator { get; }

		public CABCEncoder(MorphLanguage language, char annotChar)
		{
			Language = language;
			AnnotChar = annotChar;
			_criticalNounLetterPack = new(AnnotChar, MinimalPredictionSuffix);

			var code2Alphabet = new char[MaxAlphabetSize];
			AlphabetSize = InitAlphabet(language, code2Alphabet, out var alphabet2Code, annotChar);
			Code2Alphabet = code2Alphabet;
			Alphabet2Code = alphabet2Code;

			var code2AlphabetWithoutAnnotator = new char[MaxAlphabetSize];
			AlphabetSizeWithoutAnnotator = InitAlphabet(language, code2AlphabetWithoutAnnotator, out var alphabet2CodeWithoutAnnotator, (char)257/* non-exeting symbol */);
			Code2AlphabetWithoutAnnotator = code2AlphabetWithoutAnnotator;
			Alphabet2CodeWithoutAnnotator = alphabet2CodeWithoutAnnotator;

			if (AlphabetSizeWithoutAnnotator + 1 != AlphabetSize)
				throw new Exception("Invalid alphabets sizes");
		}

		public string GetCriticalNounLetterPack() => _criticalNounLetterPack;

		public void CheckABCWithAnnotator(string WordForm)
		{
			for (var i = 0; i < WordForm.Length; i++)
				if (Alphabet2Code[WordForm[i]] == -1)
					throw new Exception($"Bad ABC Word=\"{WordForm}\", char='{WordForm[i]}', index={i}");
		}

		public bool CheckABCWithoutAnnotator(ReadOnlySpan<char> WordForm)
		{
			for (var i = 0; i < WordForm.Length; i++)
				if (Alphabet2CodeWithoutAnnotator[WordForm[i]] == -1)
					return false;
			return true;
		}

		public string EncodeIntToAlphabet(int v)
		{
			if (v == 0)
				return Code2AlphabetWithoutAnnotator[0].ToString();

			var Result = new StringBuilder();
			while (v > 0)
			{
				Result.Append(Code2AlphabetWithoutAnnotator[v % AlphabetSizeWithoutAnnotator]);
				v /= AlphabetSizeWithoutAnnotator;
			}
			return Result.ToString();
		}

		public int DecodeFromAlphabet(IEnumerable<char> v)
		{
			int c = 1;
			int Result = 0;
			foreach (var ch in v)
			{
				Result += Alphabet2CodeWithoutAnnotator[ch] * c;
				c *= AlphabetSizeWithoutAnnotator;
			};
			return Result;
		}

		private static int InitAlphabet(
			MorphLanguage Language,
			char[] pCode2Alphabet,
			out IReadOnlyDictionary<char, int> pAlphabet2Code,
			char AnnotChar)
		{
			if (IsUpperAlpha(AnnotChar, Language))
				throw new ArgumentException($"AnnotChar must not be uppercase. Value: {AnnotChar}");
			const string additionalEnglishChars = "'1234567890";
			const string additionalGermanChars = "";
			const string additionalRussianChars = "&_";
			var allChars = Encodings.GetNonUnicodeChars(Language);
			int AlphabetSize = 0;
			var dict = new DefaultDictionary<char, int>(-1);
			for (int i = 0; i < 256; i++)
			{
				char c = allChars[i];
				if (IsUpperAlpha(c, Language)
					|| (c == '-')
					|| (c == AnnotChar)
					|| (Language == MorphLanguage.English && additionalEnglishChars.Contains(c))
					|| (Language == MorphLanguage.German && additionalGermanChars.Contains(c))
					|| (Language == MorphLanguage.Russian && additionalRussianChars.Contains(c))
					|| (Language == MorphLanguage.URL && IsAlpha(c, MorphLanguage.URL))
				)
				{
					pCode2Alphabet[AlphabetSize] = c;
					dict[c] = AlphabetSize;
					AlphabetSize++;
				}
			}

			if (AlphabetSize > MaxAlphabetSize)
				throw new Exception("Error! The ABC is too large");

			pAlphabet2Code = dict;
			return AlphabetSize;
		}
	}
}
