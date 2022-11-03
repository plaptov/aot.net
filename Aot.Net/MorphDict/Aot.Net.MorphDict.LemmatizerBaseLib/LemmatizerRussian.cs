using Aot.Net.MorphDict.Common;

namespace Aot.Net.MorphDict.LemmatizerBaseLib
{
	public class LemmatizerRussian : Lemmatizer
	{
		public LemmatizerRussian(MorphAutomat? formAutomat = null)
			: base(MorphLanguage.Russian, formAutomat ?? new MorphAutomat(MorphLanguage.Russian, CABCEncoder.MorphAnnotChar))
		{
		}

		/// <summary>
		/// Обрабатывать апостроф (') как твёрдый знак (поведение нативного AOT)
		/// </summary>
		public bool TreatApostropheAsHardSign { get; init; }

		public override string FilterSrc(string src)
		{
			src = base.FilterSrc(src);
			if (!AllowRussianJo)
				src = src.ConvertJO2Je();
			if (TreatApostropheAsHardSign)
				src = src.Replace('\'', 'Ъ');
			return src;
		}

		public override Span<char> FilterSrc(Span<char> src)
		{
			src = base.FilterSrc(src);
			if (!AllowRussianJo)
				src.ConvertJO2Je();
			if (TreatApostropheAsHardSign)
				for (int i = 0; i < src.Length; i++)
				{
					if (src[i] == '\'')
						src[i] = 'Ъ';
				}
			return src;
		}

		protected override bool NeedFilter(char c)
		{
			if (base.NeedFilter(c))
				return true;
			if (!AllowRussianJo && (c == 'Ё' || c == 'ё'))
				return true;
			if (TreatApostropheAsHardSign && c == '\'')
				return true;
			return false;
		}
	}
}
