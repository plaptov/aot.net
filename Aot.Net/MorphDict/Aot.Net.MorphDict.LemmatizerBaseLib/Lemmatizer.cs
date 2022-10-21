using Aot.Net.MorphDict.Common;

namespace Aot.Net.MorphDict.LemmatizerBaseLib
{
	public class Lemmatizer : MorphDict
	{
		protected readonly PredictBase _predict;
		private bool _enablePrediction = true;
		private HashSet<string> _prefixesSet = new(0);

		public Lemmatizer(MorphLanguage language, MorphAutomat formAutomat) : base(language, formAutomat)
		{
			_predict = new(language);
		}

		public bool Loaded { get; private set; }

		public bool MaximalPrediction { get; private set; }

		public bool UseStatistic { get; private set; }

		public bool AllowRussianJo { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="formsAutomFile"><c>.forms_autom</c> file</param>
		/// <param name="annotFile"><c>.annot</c> file</param>
		/// <param name="basesFile"><c>.bases</c> file</param>
		public void LoadDictionaries(
			Stream formsAutomFile,
			Stream annotFile,
			Stream basesFile,
			Stream optionsFile,
			Stream? predictFile = null)
		{
			Load(formsAutomFile, annotFile, basesFile);

			// TODO Load homonyms statistic for literature

			ReadOptions(optionsFile);
			if (_enablePrediction)
			{
				if (predictFile is null)
					throw new InvalidOperationException("Predict file must be precified if prediction didn't skipped in options");
				_predict.Load(predictFile);
				_predict.FillModelFreq(FlexiaModels.Count, LemmaInfos);
			}
			_prefixesSet = new HashSet<string>(Prefixes);
		}

		private void ReadOptions(Stream optionsFile)
		{
			using var reader = new StreamReader(optionsFile, Encoding.UTF8);
			string? s;
			while ((s = reader.ReadLine()) is not null)
				switch (s.Trim())
				{
					case "AllowRussianJo":
						AllowRussianJo = true;
						break;
					case "SkipPredictBase":
						_enablePrediction = false;
						break;
				}
		}

		protected bool IsPrefix(string prefix) => _prefixesSet.Contains(prefix);

		protected virtual string FilterSrc(string src) => src;

		protected bool LemmatizeWord(string InputWordStr, bool cap, bool predict, bool getLemmaInfos, List<AutomAnnotationInner> results)
		{
			InputWordStr = InputWordStr.Trim().ToUpper();
			int WordOffset = 0;

			results = _formAutomat.GetInnerMorphInfos(InputWordStr, 0);
			var bResult = results.Count > 0;

			if (results.Count == 0 && predict)
			{
				results = PredictBySuffix(InputWordStr, out WordOffset, 4); // the length of the minal suffix is 4 

				if (InputWordStr[WordOffset - 1] != '-') // and there is no hyphen
				{
					var KnownPostfixLen = InputWordStr.Length - WordOffset;
					var UnknownPrefixLen = WordOffset;
					if (KnownPostfixLen < 6)// if  the known part is too short
						//if	(UnknownPrefixLen > 5)// no prediction if unknown prefix is more than 5
					{
						if (!IsPrefix(InputWordStr[..UnknownPrefixLen]))
							results.Clear();
					};
				}

				// отменяем предсказание по местоимениям, например _R("Семыкиным")
				foreach (var item in results)
					if (ProductiveModels[item.ModelNo] == 0)
					{
						results.Clear();
						break;
					};
			}

			if (results.Count > 0)
			{
				if (getLemmaInfos)
					results = GetLemmaInfos(InputWordStr, WordOffset, results.ToArray());
			}
			else if (predict)
			{
				PredictByDataBase
			}
		}

		protected AutomAnnotationInner ConvertPredictTupleToAnnot(PredictTuple input)
		{
			return new AutomAnnotationInner(
				modelNo: LemmaInfos[input.LemmaInfoNo].LemmaInfo.FlexiaModelNo,
				itemNo: input.ItemNo,
				prefixNo: 0)
			{
				LemmaInfoNo = input.LemmaInfoNo,
				Weight = 0,
			};
		}

		protected bool CheckAbbreviation(string InputWordStr, out List<AutomAnnotationInner>? FindResults, bool is_cap)
		{
			FindResults = null;
			if (InputWordStr.Any(c => Utils.IsUpperConsonant(c, Language)))
				return false;

			_predict.Find(_formAutomat.GetCriticalNounLetterPack(), out var res);
			FindResults = new(1)
			{
				ConvertPredictTupleToAnnot(res[0]),
			};
			return true;
		}
	}
}
