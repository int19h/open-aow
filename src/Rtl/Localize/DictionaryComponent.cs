using Aow.Rtl.Localize;

namespace Aow.Rtl;

public partial class TIvDictionary
{
    private readonly MldDictionary dictionary = new();
    public string DictionaryName { get; private set; } = "";
    public MldDictionary Reader => dictionary;

    internal void SelectLanguage(int language)
    {
        if (language == 1 && DictionaryName == "ResourceStringDictionary")
            language = LocalizationRuntime.ReadLanguage(Rtl.Settings);
        dictionary.SetLanguage(language);
        FLanguage = dictionary.Language;
    }

    internal void NameDictionary(string name) => DictionaryName = name;

    internal void OpenDictionary()
    {
        dictionary.Close();
        dictionary.SetLanguage(FLanguage);
        dictionary.Open(FFileName);
        if (DictionaryName == "ResourceStringDictionary")
        {
            string path = Rtl.Files.ExpandPath(FFileName);
            LocalizationRuntime.StartupDirectory = Path.GetDirectoryName(Path.GetDirectoryName(path)) ?? "";
        }
    }

    public string TranslateContext(string source, string form, string component)
        => dictionary.TranslateContext(source, form, component);

    public override void Destroy()
    {
        dictionary.Close();
        base.Destroy();
    }
}
