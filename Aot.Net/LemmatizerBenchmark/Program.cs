using Aot.Net.MorphDict.LemmatizerBaseLib;
using Aot.Net.Tests;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace LemmatizerBenchmark
{
	internal class Program
	{
		static void Main(string[] args)
		{
			BenchmarkRunner.Run<Bench>();

			//var bench = new Bench();
			//for (int i = 0; i < 10_000; i++)
			//{
			//	bench.A();
			//}
		}
	}

	[SimpleJob]
	[MemoryDiagnoser]
	public class Bench
	{
		public const string InputString =
			"❗️Глава дипломатии ЕС Жозеп Боррель заявил, что Евросоюз возместит ущерб третьим странам, если такой возникнет в результате антироссийских санкций\r\n\r\n\"Последствия санкций должны быть тщательно измерены. Они не должны влиять на логистические факторы. Вчера мы провели очень продуктивное обсуждение о специфичных проблемах, которые мы хотели бы решить очень аккуратно, но они не направлены на экономику третьих стран. Если есть какие-то последовательные сопутствующие ущербы, мы будем их восстанавливать\", сказал он на брифинге в Астане.";

		private readonly LemmatizerRussian _lemmatizer;
		private readonly string[] _items;

		public Bench()
		{
			_lemmatizer = new();
			_lemmatizer.LoadDictionaries(
				new MemoryStream(MorphFiles.morph_forms_autom),
				new MemoryStream(MorphFiles.morph_annot),
				new MemoryStream(MorphFiles.morph_bases),
				new MemoryStream(MorphFiles.morph_options),
				new MemoryStream(MorphFiles.npredict_bin)
			);
			_items = InputString
				.Split()
				.Where(s => !string.IsNullOrWhiteSpace(s))
				.Select(_lemmatizer.FilterSrc)
				.ToArray();
		}

		[Benchmark]
		public void A()
		{
			foreach (var item in _items)
			{
				_lemmatizer.GetBestLemma(item.AsSpan());
			}
		}
	}
}