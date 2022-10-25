using Aot.Net.MorphDict.Common;
using Aot.Net.MorphDict.MorphWizardLib;

namespace Aot.Net.MorphDict.LemmatizerBaseLib
{
    public class MorphDict : MorphWizardBase
    {
        private int[] _modelsIndex;
        protected LemmaInfoAndLemma[] _lemmaInfos;
        protected readonly MorphAutomat _formAutomat;

        public MorphDict(MorphLanguage language, MorphAutomat formAutomat)
            : base(language)
        {
            _formAutomat = formAutomat;
        }

        public ShortStringHolder Bases { get; protected set; }

        public IReadOnlyList<string> Prefixes { get; protected set; }

        public IReadOnlyList<LemmaInfoAndLemma> LemmaInfos { get => _lemmaInfos; }

        public IReadOnlyList<byte> ProductiveModels { get; protected set; }

        public List<AutomAnnotationInner> PredictBySuffix(string text, out int textPos, int minimalPredictSuffixlen)
        {
            for (textPos = 1; textPos + minimalPredictSuffixlen < text.Length; textPos++)
            {
                var result = _formAutomat.GetInnerMorphInfos(text, textPos);
                if (result.Count > 0)
                    return result;
            }
            return new List<AutomAnnotationInner>(0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="formsAutomFile"><c>.forms_autom</c> file</param>
        /// <param name="annotFile"><c>.annot</c> file</param>
        /// <param name="basesFile"><c>.bases</c> file</param>
        public void Load(
            Stream formsAutomFile,
            Stream annotFile,
            Stream basesFile)
        {
            _formAutomat.Load(formsAutomFile);
            ReadAnnotFile(annotFile);
            Bases = ShortStringHolder.ReadShortStringHolder(basesFile);
            CreateModelsIndex();
        }

        protected void ReadAnnotFile(Stream annotFile)
        {
			using var reader = new BinaryReader(annotFile, Encoding.UTF8);
			FlexiaModels = ReadFlexiaModels(reader);
			AccentModels = ReadAccentModels(reader);
			Prefixes = ReadPrefixes(reader);
			var count = GetCount(reader);
			_lemmaInfos = StructSerializer.ReadVectorInner<LemmaInfoAndLemma>(reader, count);
			count = GetCount(reader);
			var productiveModels = new byte[count];
			reader.Read(productiveModels);
			ProductiveModels = productiveModels;
		}

		private static IReadOnlyList<string> ReadPrefixes(BinaryReader reader)
        {
            var count = GetCount(reader);
            var list = new List<string>(count + 1) { "" };
            for (int i = 0; i < count; i++)
            {
                list.Add(reader.ReadTerminatedString1251().Trim());
            }
            return list;
        }

        /// <summary>
        /// This procedure builds CMorphDict::m_ModelsIndex, which is an index to CMorphDict::m_LemmaInfos
        /// *  m_LemmaInfos is sorted by m_LemmaInfo.m_FlexiaModelNo
        /// * CMorphDict::m_ModelsIndex stores all periods of  CMorphDict::m_ModelsIndex with equal m_LemmaInfo.m_FlexiaModelNo
        /// *  if a= m_ModelsIndex[i] and b = m_ModelsIndex[i + 1], then for each j (a &lt;= j &lt; b)
        /// 
        ///    LemmaInfos[j].m_LemmaInfo.m_FlexiaModelNo == i
        /// *  for some i m_ModelsIndex[i] can be equal to m_ModelsIndex[i + 1], it means
        ///    that flexia model i is not used.To delete unused models the dictionary should be packed.
        /// </summary>
        private void CreateModelsIndex()
        {
            if (LemmaInfos is null || LemmaInfos.Count == 0)
            {
                _modelsIndex = Array.Empty<int>();
                return;
            }

            _modelsIndex = new int[FlexiaModels.Count + 1];

            int CurrentModel = LemmaInfos[0].LemmaInfo.FlexiaModelNo;
            _modelsIndex[CurrentModel] = 0;

            for (var i = 0; i < LemmaInfos.Count; i++)
                for (; CurrentModel < LemmaInfos[i].LemmaInfo.FlexiaModelNo; CurrentModel++)
                {
                    _modelsIndex[CurrentModel + 1] = i;
                };

            for (; CurrentModel < FlexiaModels.Count; CurrentModel++)
                _modelsIndex[CurrentModel + 1] = LemmaInfos.Count;
        }

        public List<AutomAnnotationInner> GetLemmaInfos(string text, int textPos, AutomAnnotationInner[] infos)
        {
            var textLength = text.Length;
            var additInfos = new List<AutomAnnotationInner>();
            for (int i = 0; i < infos.Length; i++)
            {
                ref AutomAnnotationInner annot = ref infos[i];
                var F = FlexiaModels[annot.ModelNo];
                var M = F.Flexia[annot.ItemNo];
                int textStartPos = textPos + Prefixes[annot.PrefixNo].Length + M.PrefixStr.Length;
                var Base = string.Concat(Prefixes[annot.PrefixNo], text.AsSpan(textStartPos, textLength - textStartPos - M.FlexiaStr.Length));

                var start = _modelsIndex[annot.ModelNo];
                var end = _modelsIndex[annot.ModelNo + 1];

                var pair_it = _lemmaInfos.EqualRange(start, end, Base, l => Bases[l.LemmaStrNo]);
                if (pair_it.IsEmpty)
                    throw new Exception("Cannot find lemma info");
                annot.LemmaInfoNo = pair_it.Start;
                foreach (var j in Enumerable.Range(pair_it.Start + 1, pair_it.Length - 1))
                {
                    var newAnnot = annot;
                    newAnnot.LemmaInfoNo = j;
                    additInfos.Add(newAnnot);
                }
            }
            var list = new List<AutomAnnotationInner>(infos.Length + additInfos.Count);
            list.AddRange(infos);
            list.AddRange(additInfos);
            return list;
        }
    }
}
