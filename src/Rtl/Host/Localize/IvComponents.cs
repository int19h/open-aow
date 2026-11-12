namespace Aow.Rtl.Localize;

public class IvTranslator : IvCustomTranslator
{
    public IvTranslator(Component? owner = null) : base(owner) { }
}

public partial class IvDictionary : Component
{
    public string FileName { get; set; } = "";
    public int Language { get; private set; }

    public void SetLanguage(int language) => SelectLanguage(language);
    public void SetDictionaryName(string name) => NameDictionary(name);
}

public class IvBinaryDictionary : IvDictionary { }
