using Aow.Rtl.Host;

namespace Aow.Rtl;

public class TRegistry : TObject
{
    private readonly ISettingsStore values = RtlUnit.Settings ?? new MemorySettingsStore();
    private string key = "";

    public bool OpenKey(string name, bool create) { key = name; return true; }

    public void CloseKey() => key = "";
    public bool ValueExists(string name) => values.Read(key + "\\" + name) is not null;
    public int ReadInteger(string name) => (int)values.Read(key + "\\" + name)!;
    public bool ReadBool(string name) => (bool)values.Read(key + "\\" + name)!;
    // REGISTRY.PAS TRegistry.ReadString returns '' for a missing value (strings.md 2.2).
    public string ReadString(string name) => (string?)values.Read(key + "\\" + name) ?? "";
    public void WriteInteger(string name, int value) => values.Write(key + "\\" + name, value);
    public void WriteBool(string name, bool value) => values.Write(key + "\\" + name, value);
    public void WriteString(string name, string value) => values.Write(key + "\\" + name, value);

    public int ReadBinaryData(string name, Span<byte> buffer, int size)
    {
        var bytes = (byte[])values.Read(key + "\\" + name)!;
        int count = Math.Min(size, bytes.Length);
        bytes.AsSpan(0, count).CopyTo(buffer);
        return count;
    }

    public void WriteBinaryData(string name, ReadOnlySpan<byte> buffer, int size) => values.Write(key + "\\" + name, buffer[..size].ToArray());
}
