using Aot.Net.MorphDict.Common;

namespace Aot.Net.MorphDict.LemmatizerBaseLib
{
	public class LemmatizerRussian : Lemmatizer
	{
		public LemmatizerRussian(MorphAutomat? formAutomat = null)
			: base(MorphLanguage.Russian, formAutomat ?? new MorphAutomat(MorphLanguage.Russian, CABCEncoder.MorphAnnotChar))
		{
		}

		protected override string FilterSrc(string src)
		{
			if (!AllowRussianJo)
				src = src.ConvertJO2Je();
			src = src.Replace('\'', 'ъ');
			return base.FilterSrc(src);
		}
	}
}
