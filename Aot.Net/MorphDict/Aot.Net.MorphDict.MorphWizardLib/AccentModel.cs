using Aot.Net.MorphDict.Common;

namespace Aot.Net.MorphDict.MorphWizardLib
{
    public readonly struct AccentModel
    {
        public IReadOnlyList<byte> Accents { get; }

        public AccentModel(IReadOnlyList<byte> accents)
        {
            Accents = accents;
        }

        public static AccentModel ReadFromString(string s)
        {
            var accents = new List<byte>();
            foreach (var item in new StringTokenizer(s, ';', ' ', '\r', '\n'))
            {
                if (string.IsNullOrEmpty(item) || !byte.TryParse(item, out var b))
                    throw new FormatException($"Invalid string: {item}");
                accents.Add(b);
            }
            return new AccentModel(accents);
        }

        public override string ToString() => string.Join(';', Accents);
    }
}
