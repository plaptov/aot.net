namespace Aot.Net.MorphDict.Common
{
	public static class Utilit
	{
		public static string ConvertJO2Je(this string s) => s.Replace('Ё', 'Е').Replace('ё', 'е');
	}
}
