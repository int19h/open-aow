namespace Aow.Rtl.Localize;

public class TIvTranslator : TIvCustomTranslator
{
    public TIvTranslator(TComponent? owner = null) : base(owner) { }
}

public partial class TIvDictionary : TComponent
{
    public string FFileName = "";
    public int FLanguage;

    public void SetLanguage(int language) => SelectLanguage(language);
    public void SetDictionaryName(string name) => NameDictionary(name);
}

public class TIvBinaryDictionary : TIvDictionary { }
