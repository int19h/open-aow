namespace Aow.Rtl;

public partial class TIvDictionary
{
    public void Open() => OpenDictionary();
    public void Close() => dictionary.Close();
    public AnsiString Translate(AnsiString source) => dictionary.Translate(source);
}
