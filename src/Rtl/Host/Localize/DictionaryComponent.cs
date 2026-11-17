
namespace Aow.Rtl.Localize;

public partial class IvDictionary
{
    public string DictionaryName { get; private set; } = "";
    public MldDictionary Reader { get; } = new();

    internal void SelectLanguage(int language)
    {
        if (language == 1 && DictionaryName == "ResourceStringDictionary")
        {
            language = LocalizationRuntime.ReadLanguage(RtlUnit.Settings);
        }

        Reader.SetLanguage(language);
        Language = Reader.Language;
    }

    internal void NameDictionary(string name) => DictionaryName = name;

    internal void OpenDictionary()
    {
        Reader.Close();
        Reader.SetLanguage(Language);
        Reader.Open(FileName);
        if (DictionaryName == "ResourceStringDictionary")
        {
            LocalizationRuntime.StartupDirectory = Path.GetDirectoryName(Path.GetDirectoryName(RtlUnit.Files.ExpandPath(FileName))) ?? "";
        }
    }

    public string TranslateContext(string source, string form, string component)
        => Reader.TranslateContext(source, form, component);

    public override void Dispose()
    {
        Reader.Close();
        base.Dispose();
    }
}
