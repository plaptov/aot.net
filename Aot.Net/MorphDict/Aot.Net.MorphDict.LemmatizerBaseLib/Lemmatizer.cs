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

		public bool IsPrefix(string prefix) => _prefixesSet.Contains(prefix);
	}
}
