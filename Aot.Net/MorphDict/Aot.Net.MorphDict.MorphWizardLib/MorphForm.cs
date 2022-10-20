namespace Aot.Net.MorphDict.MorphWizardLib
{
    public readonly struct MorphForm
    {
        public MorphForm(string gramcode, string flexiaStr, string prefixStr)
        {
            Gramcode = gramcode;
            FlexiaStr = flexiaStr;
            PrefixStr = prefixStr;
        }

        public string Gramcode { get; }
        public string FlexiaStr { get; }
        public string PrefixStr { get; }
    }
}
