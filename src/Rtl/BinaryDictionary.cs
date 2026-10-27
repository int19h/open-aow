namespace Aow.Rtl;

public partial class TIvDictionary
{
    private readonly Dictionary<AnsiString, AnsiString[]> translations = [];

    public void Open()
    {
        Close();
        if (!Rtl.FileExists(FFileName)) return;
        using var stream = Rtl.Files.Open(FFileName.ToString(), Rtl.fmOpenRead);
        using var reader = new BinaryReader(stream);
        if (reader.ReadUInt32() != 0x03444c4d) throw new InvalidDataException("Unsupported MLD dictionary");
        stream.Position = 12;
        int languages = reader.ReadUInt16();
        stream.Position = 18;
        int entries = reader.ReadUInt16();
        uint dataOffset = reader.ReadUInt32();
        if (languages < 1 || dataOffset > stream.Length) throw new InvalidDataException("Invalid MLD directory");
        stream.Position = dataOffset;
        AnsiString ReadText()
        {
            int size = reader.ReadUInt16();
            byte[] bytes = reader.ReadBytes(size);
            if (bytes.Length != size) throw new EndOfStreamException("Truncated MLD string");
            return AnsiString.FromBytes(bytes);
        }
        for (int i = 0; i < entries; i++)
        {
            var source = ReadText();
            var form = ReadText();
            var control = ReadText();
            var values = new AnsiString[languages - 1];
            for (int language = 0; language < values.Length; language++) values[language] = ReadText();
            // Translate has no form or control context. Native column zero is the source.
            if (form.Length == 0 && control.Length == 0) translations[source] = values;
        }
    }

    public void Close() => translations.Clear();

    public AnsiString Translate(AnsiString text) => FLanguage > 0 &&
        translations.TryGetValue(text, out var values) && FLanguage <= values.Length &&
        values[FLanguage - 1].Length != 0 ? values[FLanguage - 1] : text;
}
