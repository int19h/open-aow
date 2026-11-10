using System.Text;
using Aow.Rtl.Localize;

namespace Aow.Rtl.Tests;

/// <summary>
/// An original dictionary keeps the Delphi class name when the current C# name differs.
/// The product reads MLD files and does not write them, so the test writes the file.
/// </summary>
public sealed class NameTableTests
{
    public NameTableTests()
    {
        TranslatorProperties.Register<TRenamedForm>("Caption", form => form.Caption, (form, value) => form.Caption = value);
    }

    [Fact]
    public void OriginalFilesKeepTheDelphiName()
    {
        string table = "kind,delphi,current\n"
            + "type,TDelphiForm," + NameTable.QualifiedName(typeof(TRenamedForm)) + "\n";
        string path = Path.Combine(Path.GetTempPath(), "aow-names-" + Guid.NewGuid().ToString("N"));
        try
        {
            using (var stream = File.Create(path))
            using (var writer = new BinaryWriter(stream))
            {
                WriteHeader(writer, 2);
                void Text(string value)
                {
                    var bytes = Encoding.ASCII.GetBytes(value);
                    writer.Write((ushort)bytes.Length);
                    writer.Write(bytes);
                }
                void Entry(string source, string form, string english)
                {
                    Text(source);
                    Text(form);
                    Text("");
                    Text(english);
                    Text("");
                }
                Entry("Hello", "TDelphiForm", "from delphi");
                Entry("Hello", "TRenamedForm", "from current");
            }
            var dictionary = new MldDictionary();
            dictionary.Open(path);
            using (NameTable.Install(table))
            {
                Assert.Equal("TDelphiForm", NameTable.Delphi(typeof(TRenamedForm)));
                Assert.Equal(nameof(TRenamedForm), NameTable.Stored(typeof(TRenamedForm)));
                Assert.Equal("from delphi", dictionary.TranslateContext("Hello", NameTable.Delphi(typeof(TRenamedForm)), ""));
                Assert.Equal("from current", dictionary.TranslateContext("Hello", NameTable.Stored(typeof(TRenamedForm)), ""));

                var form = new TRenamedForm { Caption = "Hello" };
                var translator = new IvCustomTranslator(form) { Dictionary = dictionary, Language = 1 };
                translator.TargetsData.Add(new TranslationTarget(NameTable.Delphi(typeof(TRenamedForm)), "Caption"));
                translator.Translate(form);
                Assert.Equal("from delphi", form.Caption);

                form.Caption = "Hello";
                translator.TargetsData.Clear();
                translator.TargetsData.Add(new TranslationTarget(NameTable.Stored(typeof(TRenamedForm)), "Caption"));
                translator.Translate(form);
                Assert.Equal("Hello", form.Caption);
                form.Dispose();
            }
        }
        finally
        {
            File.Delete(path);
        }
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

    private sealed class TRenamedForm : global::Aow.Rtl.Component
    {
        public string Caption { get; set; } = "";
    }
}
