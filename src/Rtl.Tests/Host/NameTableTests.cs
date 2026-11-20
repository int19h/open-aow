using System.Text;
using Aow.Rtl.Localize;

namespace Aow.Rtl.Tests;

/// <summary>
/// An original dictionary keeps the Delphi class name when the current C# name differs.
/// The product reads MLD files and does not write them, so the test writes the file.
/// </summary>
public sealed class NameTableTests
{
    public NameTableTests() => TranslatorProperties.Register<RenamedForm>("Caption", read: form => form.Caption, (form, value) => form.Caption = value);

    [Fact]
    public void OriginalFilesKeepTheDelphiName()
    {
        string table = $"kind,delphi,current\ntype,TDelphiForm,{NameTable.QualifiedName(typeof(RenamedForm))}\n";
        string path = Path.Combine(Path.GetTempPath(), $"aow-names-{Guid.NewGuid():N}");
        try
        {
            using (FileStream stream = File.Create(path))
            using (BinaryWriter writer = new(stream))
            {
                WriteHeader(writer, count: 2);
                void Text(string value)
                {
                    byte[] bytes = Encoding.ASCII.GetBytes(value);
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
                Entry("Hello", form: "TDelphiForm", english: "from delphi");
                Entry("Hello", form: "RenamedForm", english: "from current");
            }
            MldDictionary dictionary = new();
            dictionary.Open(path);
            using (NameTable.Install(table))
            {
                Assert.Equal("TDelphiForm", NameTable.Delphi(typeof(RenamedForm)));
                Assert.Equal(nameof(RenamedForm), NameTable.Stored(typeof(RenamedForm)));
                Assert.Equal("from delphi", dictionary.TranslateContext("Hello", form: NameTable.Delphi(typeof(RenamedForm)), component: ""));
                Assert.Equal("from current", dictionary.TranslateContext("Hello", form: NameTable.Stored(typeof(RenamedForm)), component: ""));

                RenamedForm form = new() { Caption = "Hello" };
                IvCustomTranslator translator = new(form) { Dictionary = dictionary, Language = 1 };
                translator.TargetsData.Add(new TranslationTarget(NameTable.Delphi(typeof(RenamedForm)), PropertyName: "Caption"));
                translator.Translate(form);
                Assert.Equal("from delphi", form.Caption);

                form.Caption = "Hello";
                translator.TargetsData.Clear();
                translator.TargetsData.Add(new TranslationTarget(NameTable.Stored(typeof(RenamedForm)), PropertyName: "Caption"));
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

    [Fact]
    public void AStoredNameStaysWhenTheCSharpNameChanges()
    {
        string qualified = NameTable.QualifiedName(typeof(RenamedForm));
        string table = $"kind,delphi,current,stored\ntype,TOld,{qualified},EOld\n";
        using (NameTable.Install(table))
        {
            Assert.Equal("EOld", NameTable.Stored(typeof(RenamedForm)));
            Assert.Equal("TOld", NameTable.Delphi(typeof(RenamedForm)));
            Assert.Equal("EOld", NameTable.ResolveStored("TOld"));
            Assert.Equal("EOld", NameTable.ResolveStored("EOld"));
            Assert.Equal(typeof(RenamedForm), NameTable.Find("EOld"));
            Assert.NotEqual(nameof(RenamedForm), NameTable.Stored(typeof(RenamedForm)));
        }
    }

    private static void WriteHeader(BinaryWriter writer, ushort count)
    {
        byte[] header = new byte[42];
        "MLD\x03"u8.CopyTo(header);
        header[4] = header[5] = header[6] = 1;
        System.Buffers.Binary.BinaryPrimitives.WriteUInt16LittleEndian(header.AsSpan(12), value: 3);
        System.Buffers.Binary.BinaryPrimitives.WriteUInt32LittleEndian(header.AsSpan(14), value: 42);
        System.Buffers.Binary.BinaryPrimitives.WriteUInt16LittleEndian(header.AsSpan(18), value: count);
        System.Buffers.Binary.BinaryPrimitives.WriteUInt32LittleEndian(header.AsSpan(20), value: 120);
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

    private sealed class RenamedForm : Component
    {
        public string Caption { get; set; } = "";
    }
}
