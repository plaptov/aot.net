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

		protected bool LemmatizeWord(string InputWordStr, bool cap, bool predict, bool getLemmaInfos, out List<AutomAnnotationInner> results)
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
				results = PredictByDataBase(InputWordStr, cap);
				for (int i = results.Count - 1; i >= 0; i--)
				{
					var A = results[i];
					var M = FlexiaModels[A.ModelNo];
					var F = M.Flexia[A.ItemNo];
					if (F.FlexiaStr.Length >= InputWordStr.Length)
					{
						results.RemoveAt(i);
					}
				}
			}

			return bResult;
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

		public List<AutomAnnotationInner> PredictByDataBase(string InputWordString, bool is_cap)
		{
			if (CheckAbbreviation(InputWordString, out var FindResults, is_cap))
				return FindResults!;

			List<PredictTuple> res = new(0);
			if (CheckABC(InputWordString)) // if the ABC is wrong this prediction yuilds to many variants
			{
				InputWordString = new string(InputWordString.Reverse().ToArray());
				_predict.Find(_formAutomat.GetCriticalNounLetterPack(), out res);
			}

			List<int> has_nps = new(32); // assume not more than 32 different pos
			FindResults = new();
			foreach (var item in res)
			{
				var PartOfSpeechNo = item.PartOfSpeechNo;
				if (!MaximalPrediction && has_nps[PartOfSpeechNo] != -1)
				{
					var old_freq = _predict.ModelFreq[FindResults[has_nps[PartOfSpeechNo]].ModelNo];
					var new_freq = _predict.ModelFreq[LemmaInfos[item.LemmaInfoNo].LemmaInfo.FlexiaModelNo];
					if (old_freq < new_freq)
						FindResults[has_nps[PartOfSpeechNo]] = ConvertPredictTupleToAnnot(item);

					continue;
				}

				has_nps[PartOfSpeechNo] = FindResults.Count;

				FindResults.Add(ConvertPredictTupleToAnnot(item));
			}

			if ((has_nps[0] == -1) // no noun
				|| (is_cap && (Language != MorphLanguage.German)) // or can be a proper noun (except German, where all nouns are written uppercase)
				)
			{
				_predict.Find(_formAutomat.GetCriticalNounLetterPack(), out res);
				FindResults.Add(ConvertPredictTupleToAnnot(res.Last()));
			};
			return FindResults;
		}

		public bool CheckABC(string wordForm) => _formAutomat.CheckABCWithoutAnnotator(wordForm);

		public (bool, string) GetAllAncodesAndLemmasQuick(string InputWordString, bool capital, int MaxBufferSize, bool usePrediction)
		{
			InputWordString = FilterSrc(InputWordString);

			bool found = LemmatizeWord(InputWordString, capital, usePrediction, false, out var FindResults);

			var sb = new StringBuilder();
			foreach (var A in FindResults)
			{
				var M = FlexiaModels[A.ModelNo];
				var F = M.Flexia[A.ItemNo];
				var PrefixLen = F.PrefixStr.Length;
				var BaseStart = 0;
				if (found || InputWordString[..PrefixLen] != F.PrefixStr)
					BaseStart = PrefixLen;
				var BaseLen = InputWordString.Length - F.FlexiaStr.Length - BaseStart;
				if (BaseLen < 0)
					BaseLen = InputWordString.Length;
				var GramCodeLen = M.Flexia[A.ItemNo].Gramcode.Length;
				var FlexiaLength = M.Flexia[0].FlexiaStr.Length;
				if (BaseLen + FlexiaLength + 3 + GramCodeLen > MaxBufferSize - sb.Length)
					return (false, sb.ToString());

				sb.Append(InputWordString.AsSpan(BaseStart, BaseLen));
				sb.Append(M.Flexia[0].FlexiaStr);
				sb.Append(' ');
				sb.Append(M.Flexia[A.ItemNo].Gramcode);
				sb.Append('#');
			}
			return (true, sb.ToString());
		}
	}
}
