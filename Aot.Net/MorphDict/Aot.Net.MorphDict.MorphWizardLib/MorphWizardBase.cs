using Aot.Net.MorphDict.Common;

namespace Aot.Net.MorphDict.MorphWizardLib
{
    public class MorphWizardBase
    {
        public MorphWizardBase(MorphLanguage language)
        {
            Language = language;
        }

        public MorphLanguage Language { get; }

        public IReadOnlyList<FlexiaModel> FlexiaModels { get; protected set; }

        public IReadOnlyList<AccentModel> AccentModels { get; protected set; }

        protected static int GetCount(TextReader reader) => int.Parse(reader.ReadLine()!);

        protected static int GetCount(BinaryReader reader) => int.Parse(reader.ReadTerminatedShortString());

        protected static IReadOnlyList<FlexiaModel> ReadFlexiaModels(BinaryReader reader)
        {
            var count = GetCount(reader);
            var result = new List<FlexiaModel>(count);
            for (int i = 0; i < count; i++)
            {
                var line = reader.ReadTerminatedLongString()?.Trim();
                if (line is null)
                    throw new InvalidDataException("Cannot read enough flexia models");
                result.Add(FlexiaModel.ReadFromString(line));
            }
            return result;
        }

        protected static IReadOnlyList<AccentModel> ReadAccentModels(BinaryReader reader)
        {
            var count = GetCount(reader);
            var result = new List<AccentModel>(count);
            for (int i = 0; i < count; i++)
            {
                var line = reader.ReadTerminatedLongString()?.Trim();
                if (line is null)
                    throw new InvalidDataException("Cannot read enough accent models");
                result.Add(AccentModel.ReadFromString(line));
            }
            return result;
        }
    }
}
