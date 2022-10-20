using Aot.Net.MorphDict.Common;

namespace Aot.Net.MorphDict.MorphWizardLib
{
    public class MorphWizardBase
    {
        public MorphWizardBase(MorphLanguage language, IReadOnlyList<FlexiaModel> flexiaModels, IReadOnlyList<AccentModel> accentModels)
        {
            Language = language;
            FlexiaModels = flexiaModels;
            AccentModels = accentModels;
        }

        public MorphLanguage Language { get; }

        public IReadOnlyList<FlexiaModel> FlexiaModels { get; }

        public IReadOnlyList<AccentModel> AccentModels { get; }

        protected static int GetCount(TextReader reader) => int.Parse(reader.ReadLine()!);

        protected static IReadOnlyList<FlexiaModel> ReadFlexiaModels(TextReader reader)
        {
            var count = GetCount(reader);
            var result = new List<FlexiaModel>(count);
            for (int i = 0; i < count; i++)
            {
                var line = reader.ReadLine()?.Trim();
                if (line is null)
                    throw new InvalidDataException("Cannot read enough flexia models");
                result.Add(FlexiaModel.ReadFromString(line));
            }
            return result;
        }

        protected static IReadOnlyList<AccentModel> ReadAccentModels(TextReader reader)
        {
            var count = GetCount(reader);
            var result = new List<AccentModel>(count);
            for (int i = 0; i < count; i++)
            {
                var line = reader.ReadLine()?.Trim();
                if (line is null)
                    throw new InvalidDataException("Cannot read enough accent models");
                result.Add(AccentModel.ReadFromString(line));
            }
            return result;
        }
    }
}
