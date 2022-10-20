using Aot.Net.MorphDict.Common;

namespace Aot.Net.MorphDict.MorphWizardLib
{
    public class FlexiaModel
    {
        private const string FlexModelCommDelim = "q//q";
        private const string WiktionaryMorphTemplateDelim = ";";

        private FlexiaModel(string wiktionaryMorphTemplate, string comments, IReadOnlyList<MorphForm> flexia)
        {
            WiktionaryMorphTemplate = wiktionaryMorphTemplate;
            Comments = comments;
            Flexia = flexia;
        }

        public string WiktionaryMorphTemplate { get; }

        public string Comments { get; }

        public IReadOnlyList<MorphForm> Flexia { get; }

        public string GetFirstFlex()
        {
            if (Flexia.Count == 0)
                throw new InvalidOperationException("Flexia is empty");
            return Flexia[0].FlexiaStr;
        }

        public string GetFirstCode()
        {
            if (Flexia.Count == 0)
                throw new InvalidOperationException("Flexia is empty");
            return Flexia[0].Gramcode;
        }

        public static FlexiaModel ReadFromString(string s)
        {
            var semicolon = s.IndexOf(WiktionaryMorphTemplateDelim);
            var wiktionaryMorphTemplate = semicolon > 0 ? s[..semicolon] : "";
            var comm = s.LastIndexOf(FlexModelCommDelim);
            var comments = comm >= 0 ? s[comm..] : "";
            semicolon = Math.Max(semicolon, 0);
            comm = Math.Min(comm, s.Length);
            s = s[semicolon..comm];
            var flexia = new List<MorphForm>();
            foreach (var oneRecord in new StringTokenizer(s, '%'))
            {
                var ast = oneRecord.IndexOf('*');
                if (ast < 0)
                    throw new FormatException($"Cannot parse value: {s}");
                var last_ast = oneRecord.LastIndexOf('*');
                var prefix = last_ast != ast ? oneRecord.Substring(last_ast + 1) : "";
                var g = new MorphForm(
                    oneRecord.Substring(ast + 1, last_ast - ast - 1),
                    oneRecord[..ast],
                    prefix);
                flexia.Add(g);
            }
            return new FlexiaModel(wiktionaryMorphTemplate, comments, flexia);
        }

        public override string? ToString()
        {
            var sb = new StringBuilder();
            if (!string.IsNullOrEmpty(WiktionaryMorphTemplate))
                sb.Append(WiktionaryMorphTemplate);
            foreach (var item in Flexia)
            {
                sb.Append('%').Append(item.FlexiaStr).Append('*').Append(item.Gramcode);
                if (!string.IsNullOrEmpty(item.PrefixStr))
                    sb.Append('*').Append(item.PrefixStr);
            }
            if (!string.IsNullOrEmpty(Comments))
                sb.Append(Comments);
            return sb.ToString();
        }
    }
}
