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
            using var reader = new StreamReader(annotFile, Encoding.UTF8, leaveOpen: true);
            var flexias = ReadFlexiaModels(reader);
            var accents = ReadAccentModels(reader);
            var prefixes = ReadPrefixes(reader);
        }

        private static IReadOnlyList<string> ReadPrefixes(TextReader reader)
        {
            var count = GetCount(reader);
            var list = new List<string>(count + 1);
            list.Add("");
            for (int i = 0; i < count; i++)
            {
                list.Add(reader.ReadLine()!);
            }
            return list;
        }
    }
}
