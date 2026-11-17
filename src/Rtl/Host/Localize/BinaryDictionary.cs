namespace Aow.Rtl.Localize;

public partial class IvDictionary
{
    public void Open() => OpenDictionary();
    public void Close() => Reader.Close();
    public string Translate(string source) => Reader.Translate(source);
}
