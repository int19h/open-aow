
namespace Aow.Rtl.Localize;

public partial class IvDictionary
{
    private readonly MldDictionary _dictionary = new();

    public string DictionaryName { get; private set; } = "";
    public MldDictionary Reader => _dictionary;

    internal void SelectLanguage(int language)
    {
        if (language == 1 && DictionaryName == "ResourceStringDictionary")
            language = LocalizationRuntime.ReadLanguage(RtlUnit.Settings);
        _dictionary.SetLanguage(language);
        Language = _dictionary.Language;
    }

    internal void NameDictionary(string name) => DictionaryName = name;

    internal void OpenDictionary()
    {
        _dictionary.Close();
        _dictionary.SetLanguage(Language);
        _dictionary.Open(FileName);
        if (DictionaryName == "ResourceStringDictionary")
        {
            string path = RtlUnit.Files.ExpandPath(FileName);
            LocalizationRuntime.StartupDirectory = Path.GetDirectoryName(Path.GetDirectoryName(path)) ?? "";
        }
    }

    public string TranslateContext(string source, string form, string component)
        => _dictionary.TranslateContext(source, form, component);

    public override void Dispose()
    {
        _dictionary.Close();
        base.Dispose();
    }
}
