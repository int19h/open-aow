using Aow.Rtl.Localize;

namespace Aow.Rtl.Localize;

public partial class TIvDictionary
{
    private readonly MldDictionary dictionary = new();

    public string DictionaryName { get; private set; } = "";
    public MldDictionary Reader => dictionary;

    internal void SelectLanguage(int language)
    {
        if (language == 1 && DictionaryName == "ResourceStringDictionary")
            language = LocalizationRuntime.ReadLanguage(RtlUnit.Settings);
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
            string path = RtlUnit.Files.ExpandPath(FFileName);
            LocalizationRuntime.StartupDirectory = Path.GetDirectoryName(Path.GetDirectoryName(path)) ?? "";
        }
    }

    public string TranslateContext(string source, string form, string component)
        => dictionary.TranslateContext(source, form, component);

    public override void Dispose()
    {
        dictionary.Close();
        base.Dispose();
    }
}
