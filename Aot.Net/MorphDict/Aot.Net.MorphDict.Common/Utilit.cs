namespace Aot.Net.MorphDict.Common
{
	public static class Utilit
	{
		public static string ConvertJO2Je(this string s) => s.Replace('Ё', 'Е').Replace('ё', 'е');

		public static void ConvertJO2Je(this Span<char> s)
		{
			for (int i = 0; i < s.Length; i++)
			{
				char c = s[i];
				if (c == 'Ё')
					s[i] = 'Е';
				else if (c == 'ё')
					s[i] = 'е';
			}
		}
	}
}
