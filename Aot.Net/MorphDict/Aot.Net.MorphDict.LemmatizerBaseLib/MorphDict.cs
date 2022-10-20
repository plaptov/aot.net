using Aot.Net.MorphDict.Common;
using Aot.Net.MorphDict.MorphWizardLib;

namespace Aot.Net.MorphDict.LemmatizerBaseLib
{
    public class MorphDict : MorphWizardBase
    {
        protected readonly MorphAutomat _formAutomat;

        public MorphDict(
            MorphLanguage language,
            IReadOnlyList<FlexiaModel> flexiaModels,
            IReadOnlyList<AccentModel> accentModels,
            ShortStringHolder bases,
            MorphAutomat formAutomat)
            : base(language, flexiaModels, accentModels)
        {
            Bases = bases;
            _formAutomat = formAutomat;
        }

        public ShortStringHolder Bases { get; }

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

        void Load(
            Stream formsAutomFile,
            Stream annotFile,
            Stream basesFile)
        {
            _formAutomat.Load(formsAutomFile);

        }

        public static List<FlexiaModel> ReadFlexiaModels(Stream annotFile)
        {
            using var reader = new StreamReader(annotFile, Encoding.UTF8, leaveOpen: true);
            var count = GetCount(reader);
            var models = new List<FlexiaModel>(count);
            for (int i = 0; i < count; i++)
            {
                string s = reader.ReadLine()?.Trim() ?? throw new Exception("Cannot read flexia models");
                models.Add(FlexiaModel.ReadFromString(s));
            }
            return models;
        }
	}
}
