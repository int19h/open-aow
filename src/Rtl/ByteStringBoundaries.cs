namespace Aow.Rtl;

public static partial class Rtl
{
    public static AnsiString NullTerminatedText(AnsiString value)
    {
        var text = value.Span;
        int end = text.IndexOf((byte)0);
        return end < 0 ? value : AnsiString.FromBytes(text[..end].ToArray());
    }
}
