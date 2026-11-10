namespace Aow.Rtl.Localize;

public class IvTranslator : IvCustomTranslator
{
    public IvTranslator(global::Aow.Rtl.Component? owner = null) : base(owner) { }
}

public partial class IvDictionary : global::Aow.Rtl.Component
{
    public string FFileName = "";
    public int FLanguage;

    public void SetLanguage(int language) => SelectLanguage(language);
    public void SetDictionaryName(string name) => NameDictionary(name);
}

public class IvBinaryDictionary : IvDictionary { }
