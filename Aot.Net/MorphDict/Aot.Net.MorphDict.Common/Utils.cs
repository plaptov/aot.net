namespace Aot.Net.MorphDict.Common
{
	public class Utils
	{
		private static readonly HashSet<char> _rusUpperChars = new("АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ'"); // твёрдый знак в виде апострофа
		private static readonly HashSet<char> _rusLowerChars = new("абвгдеёжзийклмнопрстуфхцчшщъыьэюя'");
		private const string _diacriticEngUpperChars = "ÂÇÈÉÊÑÔÖÛ";
		private const string _diacriticEngLowerChars = "âçèéêñôöû";
		private static readonly HashSet<char> _engUpperChars = new("ABCDEFGHIJKLMNOPQRSTUVWXYZ" + _diacriticEngUpperChars);
		private static readonly HashSet<char> _engLowerChars = new("abcdefghijklmnopqrstuvwxyz" + _diacriticEngLowerChars);
		private static readonly HashSet<char> _germanUpperChars = new(_engUpperChars.Concat("µÄÜß"));
		private static readonly HashSet<char> _germanLowerChars = new(_engLowerChars.Concat("µäüß"));
		private const string _openingBrackets = "(<[{";
		private const string _closingBrackets = ")>]}";
		private const string _upperRomanDigits = "ILVXØ";
		private const string _lowerRomanDigits = "ilvx";
		private static readonly HashSet<char> _latinVowels = new("AEIOUaeiouÂÄÈÉÊÔÖÛÜâäèéêôöûü");
		private static readonly HashSet<char> _rusVowels = new("АЕЁИОУЫЭЮЯаеёиоуыэюя");
		private static readonly HashSet<char> _urlChars = new("!$%&()*+,-./0123456789:;=?@" + "abcdefghijklmnopqrstuvwxyz");

		public static bool IsUpperAlpha(char x, MorphLanguage language) => language switch
		{
			MorphLanguage.Russian => IsRussianUpper(x),
			MorphLanguage.English => IsEnglishUpper(x),
			MorphLanguage.German => IsGermanUpper(x),
			MorphLanguage.Generic => IsGenericUpper(x),
			_ => throw new ArgumentOutOfRangeException(nameof(language), language, "Unexpected language"),
		};

		public static bool IsRussianLower(char x) => _rusLowerChars.Contains(x);

		public static bool IsRussianUpper(char x) => _rusUpperChars.Contains(x);

		public static bool IsRussianAlpha(char x) => IsRussianLower(x) || IsRussianUpper(x);

		public static bool IsRussianUpperVowel(char c) => IsRussianUpper(c) && _rusVowels.Contains(c);

		public static bool IsEnglishLower(char x) => _engLowerChars.Contains(x);

		public static bool IsEnglishUpper(char x) => _engUpperChars.Contains(x);

		public static bool IsEnglishAlpha(char x) => IsEnglishLower(x) || IsEnglishUpper(x);

		public static bool IsEnglishUpperVowel(char c) => IsEnglishUpper(c) && _latinVowels.Contains(c);

		public static bool IsGermanLower(char x) => _germanLowerChars.Contains(x);

		public static bool IsGermanUpper(char x) => _germanUpperChars.Contains(x);

		public static bool IsGermanAlpha(char x) => IsGermanLower(x) || IsGermanUpper(x);

		public static bool IsGermanUpperVowel(char c) => IsGermanUpper(c) && _latinVowels.Contains(c);

		public static bool IsGenericUpper(char x) => _engUpperChars.Contains(x);

		public static bool IsGenericAlpha(char x) => IsEnglishAlpha(x) || x >= 128;

		public static bool IsURLAlpha(char x) => _urlChars.Contains(x);

		public static bool IsAlpha(char x, MorphLanguage language) => language switch
		{
			MorphLanguage.Russian => IsRussianAlpha(x),
			MorphLanguage.English => IsEnglishAlpha(x),
			MorphLanguage.German => IsGermanAlpha(x),
			MorphLanguage.Generic => IsGenericAlpha(x),
			MorphLanguage.URL => IsURLAlpha(x),
			_ => throw new ArgumentOutOfRangeException(nameof(language), language, "Unexpected language"),
		};

		public static bool IsUpperVowel(char c, MorphLanguage language) => language switch
		{
			MorphLanguage.Russian => IsRussianUpperVowel(c),
			MorphLanguage.English => IsEnglishUpperVowel(c),
			MorphLanguage.German => IsGermanUpperVowel(c),
			_ => throw new ArgumentOutOfRangeException(nameof(language), language, "Unexpected language"),
		};

		public static bool IsUpperConsonant(char c, MorphLanguage language)
		{
			return IsUpperAlpha(c, language) && !IsUpperVowel(c, language);
		}
	}
}
