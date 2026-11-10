namespace Aow.Rtl.Localize;

public partial class TIvDictionary
{
    public void Open() => OpenDictionary();
    public void Close() => dictionary.Close();
    public string Translate(string source) => dictionary.Translate(source);
}
