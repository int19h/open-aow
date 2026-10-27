using Aow.Rtl.Localize;

namespace Aow.Rtl;

public partial class TIvDictionary
{
    private readonly MldDictionary dictionary = new();
    public AnsiString DictionaryName { get; private set; }
    public MldDictionary Reader => dictionary;

    internal void SelectLanguage(int language)
    {
        if (language == 1 && DictionaryName == (AnsiString)"ResourceStringDictionary")
            language = LocalizationRuntime.ReadLanguage(Rtl.Settings);
        dictionary.SetLanguage(language);
        FLanguage = dictionary.Language;
    }

    internal void NameDictionary(AnsiString name) => DictionaryName = name;

    internal void OpenDictionary()
    {
        dictionary.Close();
        dictionary.SetLanguage(FLanguage);
        dictionary.Open(FFileName.ToString());
        if (DictionaryName == (AnsiString)"ResourceStringDictionary")
        {
            string path = Rtl.Files.ExpandPath(FFileName.ToString());
            LocalizationRuntime.StartupDirectory = Path.GetDirectoryName(Path.GetDirectoryName(path)) ?? "";
        }
    }

    public AnsiString TranslateContext(AnsiString source, AnsiString form, AnsiString component)
        => dictionary.TranslateContext(source, form, component);

    public override void Destroy()
    {
        dictionary.Close();
        base.Destroy();
    }
}
