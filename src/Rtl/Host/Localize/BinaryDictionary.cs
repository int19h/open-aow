namespace Aow.Rtl.Localize;

public partial class IvDictionary
{
    public void Open() => OpenDictionary();
    public void Close() => _dictionary.Close();
    public string Translate(string source) => _dictionary.Translate(source);
}
