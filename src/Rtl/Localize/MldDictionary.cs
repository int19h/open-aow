using System.Buffers.Binary;
using System.Collections.ObjectModel;
using System.Text;

namespace Aow.Rtl.Localize;

public sealed record MldLanguage(int Offset, int? BlockSize, int PrimaryLanguageId,
    string IsoLanguage, int SubLanguageId, string IsoCountry, string AllSubs,
    string AllCountries, int CharacterSetType, int CodePage, byte Options,
    string EnglishName, string NativeName, string ExtraString, byte ExtraByte, int Padding);

public sealed record MldEntry(int Index, int Offset, string Native, string Form,
    string Component, ReadOnlyCollection<string> Translations);

public sealed record MldInfo(int Offset, string Description, string Owner,
    ReadOnlyCollection<string> Users, string? Timestamp);

public sealed class MldDictionary
{
    private MldEntry[] sorted = [];
    private MldEntry[] contextual = [];
    private string? fileName;
    private int language = 1;

    public int Version { get; private set; }
    public int ByteOrder { get; private set; }
    public int CharacterSet { get; private set; }
    public int ContextCode { get; private set; }
    public int FileSize { get; private set; }
    public int ConsumedBytes { get; private set; }
    public ReadOnlyCollection<MldLanguage> Languages { get; private set; } = Array.AsReadOnly<MldLanguage>([]);
    public ReadOnlyCollection<MldEntry> Entries { get; private set; } = Array.AsReadOnly<MldEntry>([]);
    public MldInfo? Info { get; private set; }
    public bool IsOpen { get; private set; }
    public int Language => language;

    public void Open(string path)
    {
        Close();
        if (!RtlUnit.Files.FileExists(path)) return;
        using var stream = RtlUnit.Files.Open(path, RtlUnit.fmOpenRead);
        Read(stream);
        fileName = path;
    }

    public void Read(Stream stream)
    {
        Close();
        using var copy = new MemoryStream();
        stream.CopyTo(copy);
        var reader = new Reader(copy.ToArray());
        var bytes = reader.Data;
        if (bytes.Length < 42 || !bytes.AsSpan(0, 3).SequenceEqual("MLD"u8))
            throw new InvalidDataException("Invalid MLD header");
        Version = bytes[3];
        ByteOrder = bytes[4];
        CharacterSet = Version == 1 ? bytes[7] : bytes[5];
        ContextCode = bytes[6];
        if (Version is < 1 or > 3 || ByteOrder > 1 || CharacterSet > 1 || ContextCode > 3)
            throw new InvalidDataException("Unsupported MLD format");
        reader.LittleEndian = ByteOrder != 0;
        reader.Ansi = CharacterSet != 0;
        reader.Position = 12;
        int languageCount = reader.Word(), languageOffset = reader.Offset();
        int entryCount = reader.Word(), entryOffset = reader.Offset();
        int localeCount = reader.Word();
        _ = reader.Offset();
        int infoSize = reader.Word(), infoOffset = reader.Offset();
        if (Version == 1) { infoSize = 0; infoOffset = 0; }
        if (languageCount == 0) throw new InvalidDataException("MLD has no native language");
        if (localeCount != 0) throw new InvalidDataException("MLD locale records are unsupported");
        var coverage = new List<(int Start, int End)> { (0, 42) };
        var languages = new MldLanguage[languageCount];
        reader.Seek(languageOffset);
        for (int i = 0; i < languageCount; i++)
        {
            int start = reader.Position;
            int? block = Version >= 3 ? reader.Word() : null;
            int primary = reader.Word();
            var isoLanguage = Version >= 2 ? reader.Chars(2) : "";
            int sub = reader.Word();
            var country = Version >= 2 ? reader.Chars(2) : "";
            var allSubs = reader.Text();
            var countries = Version >= 2 ? reader.Text() : "";
            int charset = Version >= 3 ? reader.Word() : 0;
            int codePage = reader.Word();
            if (reader.Ansi && codePage is not (0 or 1252))
                throw new InvalidDataException($"Unsupported MLD code page {codePage}");
            byte options = reader.Byte();
            var english = reader.Text();
            var native = reader.Text();
            var extra = reader.Text();
            byte extraByte = reader.Byte();
            int padding = 0;
            if (block is int size)
            {
                int end = checked(start + size);
                if (reader.Position > end) throw new InvalidDataException("MLD language overruns its block");
                padding = end - reader.Position;
                reader.Seek(end);
            }
            languages[i] = new(start, block, primary, isoLanguage, sub, country, allSubs,
                countries, charset, codePage, options, english, native, extra, extraByte, padding);
            coverage.Add((start, reader.Position));
        }
        reader.Seek(entryOffset);
        var entries = new MldEntry[entryCount];
        for (int i = 0; i < entryCount; i++)
        {
            int start = reader.Position;
            var native = reader.Text();
            var form = ContextCode is 1 or 3 ? reader.Text() : "";
            var component = ContextCode is 1 or 2 ? reader.Text() : "";
            var translations = RtlUnit.Strings(languageCount - 1);
            for (int j = 0; j < translations.Length; j++) translations[j] = reader.Text();
            entries[i] = new(i, start, native, form, component, Array.AsReadOnly(translations));
        }
        coverage.Add((entryOffset, reader.Position));
        MldInfo? info = null;
        if (infoOffset != 0)
        {
            reader.Seek(infoOffset);
            var description = reader.Text();
            var owner = reader.Text();
            var users = RtlUnit.Strings(reader.Word());
            for (int i = 0; i < users.Length; i++) users[i] = reader.Text();
            string? timestamp = null;
            if (Version >= 3)
            {
                int day = reader.Byte(), month = reader.Byte(), year = reader.Word();
                int hour = reader.Byte(), minute = reader.Byte(), second = reader.Byte();
                timestamp = FormattableString.Invariant($"{year:D4}-{month:D2}-{day:D2}T{hour:D2}:{minute:D2}:{second:D2}");
            }
            if (infoSize != 0 && reader.Position - infoOffset != infoSize)
                throw new InvalidDataException("MLD info size differs from header");
            info = new(infoOffset, description, owner, Array.AsReadOnly(users), timestamp);
            coverage.Add((infoOffset, reader.Position));
        }
        int consumed = 0;
        foreach (var range in coverage.OrderBy(r => r.Start).ThenBy(r => r.End))
        {
            if (range.Start != consumed) throw new InvalidDataException("MLD has gaps or overlapping sections");
            consumed = range.End;
        }
        if (consumed != bytes.Length) throw new InvalidDataException("MLD has unexplained trailing bytes");
        Languages = Array.AsReadOnly(languages);
        Entries = Array.AsReadOnly(entries);
        Info = info;
        FileSize = bytes.Length;
        ConsumedBytes = consumed;
        // OrderBy preserves on-disk order for equal keys.
        sorted = entries.OrderBy(SortKey, Cp1252.Comparer).ToArray();
        contextual = sorted;
        IsOpen = true;
    }

    private static string Key(MldEntry entry) => entry.Native + "\u007F" + entry.Form + entry.Component;

    /// <summary>The key the lookup array is sorted by: the source text, with its context when the file has one.</summary>
    public string SortKey(MldEntry entry) => ContextCode == 0 ? entry.Native : Key(entry);

    /// <summary>The entries in the order the binary search reads them.</summary>
    public ReadOnlyCollection<MldEntry> SortedEntries => Array.AsReadOnly(sorted);

    public void SetLanguage(int value)
    {
        if (value is < -1 or > 5) throw new ArgumentOutOfRangeException(nameof(value));
        language = value == -1 ? LocalizationLanguage.Automatic() : value;
        if (fileName is string path) Open(path);
    }

    public void Close()
    {
        fileName = null;
        IsOpen = false;
        sorted = [];
        contextual = [];
        Languages = Array.AsReadOnly<MldLanguage>([]);
        Entries = Array.AsReadOnly<MldEntry>([]);
        Info = null;
        Version = ByteOrder = CharacterSet = ContextCode = FileSize = ConsumedBytes = 0;
    }

    private MldEntry? Find(string source)
    {
        var key = ContextCode == 0 ? source : source + "\u007F";
        return Search(sorted, key, entry => ContextCode == 0 ? entry.Native : entry.Native + "\u007F");
    }

    private static MldEntry? Search(MldEntry[] entries, string key, Func<MldEntry, string> selector)
    {
        int low = 0, high = entries.Length - 1;
        while (low <= high)
        {
            int middle = low + ((high - low) >> 1);
            int comparison = Cp1252.Compare(selector(entries[middle]), key);
            if (comparison == 0) return entries[middle];
            if (comparison < 0) low = middle + 1;
            else high = middle - 1;
        }
        return null;
    }

    private string Translation(string source, MldEntry? entry)
    {
        if (entry is null || language == 0 || language > entry.Translations.Count) return source;
        var translated = entry.Translations[language - 1];
        return translated.Length == 0 ? source : translated;
    }

    public string Translate(string source) => source.Length == 0 ? source : Translation(source, Find(source));

    public string TranslateContext(string source, string form, string component)
    {
        if (source.Length == 0 || ContextCode == 0) return Translate(source);
        var key = source + "\u007F" + (ContextCode is 1 or 3 ? form : "")
            + (ContextCode is 1 or 2 ? component : "");
        return Translation(source, Search(contextual, key, Key) ?? Find(source));
    }

    private sealed class Reader(byte[] data)
    {
        public byte[] Data { get; } = data;
        public int Position { get; set; }
        public bool LittleEndian { get; set; }
        public bool Ansi { get; set; }

        public void Seek(int position)
        {
            if ((uint)position > (uint)Data.Length) throw new InvalidDataException("MLD offset outside file");
            Position = position;
        }

        private ReadOnlySpan<byte> Take(int count)
        {
            if (count < 0 || Position < 0 || Position > Data.Length - count)
                throw new InvalidDataException("Truncated MLD data");
            var span = Data.AsSpan(Position, count);
            Position += count;
            return span;
        }

        public byte Byte() => Take(1)[0];
        public int Word() => LittleEndian ? BinaryPrimitives.ReadUInt16LittleEndian(Take(2)) : BinaryPrimitives.ReadUInt16BigEndian(Take(2));

        public int Offset()
        {
            uint offset = LittleEndian ? BinaryPrimitives.ReadUInt32LittleEndian(Take(4)) : BinaryPrimitives.ReadUInt32BigEndian(Take(4));
            if (offset > int.MaxValue) throw new InvalidDataException("MLD offset outside file");
            return (int)offset;
        }

        public string Text() => Chars(Word());

        public string Chars(int count)
        {
            var raw = Take(checked(count * (Ansi ? 1 : 2)));
            if (Ansi) return Cp1252.Decode(raw.ToArray());
            var encoding = new UnicodeEncoding(!LittleEndian, false, true);
            return Cp1252.Normalize(encoding.GetString(raw));
        }
    }
}
