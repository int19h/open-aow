using System.Text;
using Xunit;

namespace Aow.Rtl.Tests;

public sealed class BinaryDictionaryTests
{
    [Fact]
    public void ReadsColumnsContextsEmptyFallbackAndAnsiBytes()
    {
        string path = Path.Combine(Path.GetTempPath(), "aow-dictionary-" + Guid.NewGuid().ToString("N"));
        try
        {
            using (var stream = File.Create(path))
            using (var writer = new BinaryWriter(stream))
            {
                WriteHeader(writer, 4);
                void Text(byte[] bytes) { writer.Write((ushort)bytes.Length); writer.Write(bytes); }
                void Entry(string source, string form, string english, string german)
                {
                    Text(Encoding.ASCII.GetBytes(source)); Text(Encoding.ASCII.GetBytes(form)); Text([]);
                    Text(Encoding.ASCII.GetBytes(english)); Text(Encoding.ASCII.GetBytes(german));
                }
                Entry("item(plural)", "", "items", "Dinge");
                Entry("item(plural)", "SomeForm", "context only", "Kontext");
                Entry("unchanged", "", "", "");
                Text([0x63, 0x61, 0x66, 0xe9]); Text([]); Text([]); Text([0x74, 0x65, 0x61]); Text([]);
            }
            var dictionary = new TIvBinaryDictionary { FFileName = path };
            dictionary.SetLanguage(1);
            dictionary.Open();
            Assert.Equal((AnsiString)"items", dictionary.Translate("item(plural)"));
            Assert.Equal((AnsiString)"unchanged", dictionary.Translate("unchanged"));
            Assert.Equal((AnsiString)"missing", dictionary.Translate("missing"));
            Assert.Equal((AnsiString)"tea", dictionary.Translate(AnsiString.FromBytes(0x63, 0x61, 0x66, 0xe9)));
            dictionary.SetLanguage(2);
            Assert.Equal((AnsiString)"Dinge", dictionary.Translate("item(plural)"));
            dictionary.SetLanguage(0);
            Assert.Equal((AnsiString)"item(plural)", dictionary.Translate("item(plural)"));
            dictionary.SetLanguage(1);
            dictionary.Close();
            Assert.Equal((AnsiString)"item(plural)", dictionary.Translate("item(plural)"));
        }
        finally { File.Delete(path); }
    }

    [Fact]
    public void RejectsATruncatedStringTable()
    {
        string path = Path.Combine(Path.GetTempPath(), "aow-dictionary-" + Guid.NewGuid().ToString("N"));
        try
        {
            using (var stream = File.Create(path))
            using (var writer = new BinaryWriter(stream))
            {
                WriteHeader(writer, 1);
                writer.Write((ushort)5);
            }
            var dictionary = new TIvBinaryDictionary { FFileName = path };
            Assert.Throws<InvalidDataException>(dictionary.Open);
        }
        finally { File.Delete(path); }
    }

    private static void WriteHeader(BinaryWriter writer, ushort count)
    {
        var header = new byte[42];
        "MLD\x03"u8.CopyTo(header);
        header[4] = header[5] = header[6] = 1;
        System.Buffers.Binary.BinaryPrimitives.WriteUInt16LittleEndian(header.AsSpan(12), 3);
        System.Buffers.Binary.BinaryPrimitives.WriteUInt32LittleEndian(header.AsSpan(14), 42);
        System.Buffers.Binary.BinaryPrimitives.WriteUInt16LittleEndian(header.AsSpan(18), count);
        System.Buffers.Binary.BinaryPrimitives.WriteUInt32LittleEndian(header.AsSpan(20), 120);
        writer.Write(header);
        for (ushort language = 0; language < 3; language++)
        {
            writer.Write((ushort)26);
            writer.Write(language);
            writer.Write("en"u8);
            writer.Write((ushort)1);
            writer.Write("US"u8);
            writer.Write((ushort)0);
            writer.Write((ushort)0);
            writer.Write((ushort)0);
            writer.Write((ushort)1252);
            writer.Write((byte)0);
            writer.Write((ushort)0);
            writer.Write((ushort)0);
            writer.Write((ushort)0);
            writer.Write((byte)0);
        }
    }
}
