namespace Aow.Rtl.Localize;

public static class LocalizationRuntime
{
    public const string LanguageKey = "\\Software\\Triumph Studios\\Age of Wonders\\General\\Language";
    private static readonly Dictionary<string, MldDictionary> dictionaries = new(StringComparer.OrdinalIgnoreCase);
    public static string StartupDirectory { get; set; } = "";

    public static int ReadLanguage(ISettingsStore? settings) => settings?.Read(LanguageKey) is int value && value is >= 2 and <= 5 ? value : 1;

    public static MldDictionary Dictionary(string name, int? language = null)
    {
        string root = StartupDirectory.Length == 0 ? RtlUnit.Files.CurrentDirectory : StartupDirectory;
        string path = Path.Combine(root, "Dict", name + ".mld");
        string key = RtlUnit.Files.ExpandPath(path);
        if (!dictionaries.TryGetValue(key, out var dictionary))
        {
            dictionary = new();
            dictionary.SetLanguage(language ?? ReadLanguage(RtlUnit.Settings));
            dictionary.Open(path);
            dictionaries.Add(key, dictionary);
        }
        else if (dictionary.Language != (language ?? ReadLanguage(RtlUnit.Settings)))
            dictionary.SetLanguage(language ?? ReadLanguage(RtlUnit.Settings));
        return dictionary;
    }

    public static void ClearDictionaries()
    {
        foreach (var dictionary in dictionaries.Values) dictionary.Close();
        dictionaries.Clear();
    }
}
