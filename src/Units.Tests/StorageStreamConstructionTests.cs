using System.Reflection;
using Aow.Units.Engine;
using Aow.Units.AoWE;

namespace Aow.Units.Tests;

[Collection("Engine")]
[EngineState]
public sealed class StorageStreamConstructionTests
{
    private static T Field<T>(object value, string name) => (T)value.GetType()
        .GetField(name, BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(value)!;

    private sealed class StoredObject() : TEObject(null!)
    {
        internal string Name = "";
        internal string Detail = "";
        public override void ReadWrite(TEStorageStream stream)
        {
            stream.rwString(ref Name, 7);
            stream.rwString(ref Detail, 9);
        }
    }

    private sealed class ExactReadStream : TMemoryStream
    {
        public override int Read(Span<byte> buffer)
        {
            int count = base.Read(buffer);
            if (count != buffer.Length) throw new EReadError("Stream read error");
            return count;
        }
    }

    private static TMemoryStream Memory(ReadOnlySpan<byte> bytes)
    {
        var stream = new TMemoryStream();
        stream.Write(bytes);
        stream.Position = 0;
        return stream;
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(8)]
    [InlineData(9)]
    public void ConstructionKeepsTypeBorrowedStreamAndPrivateState(int kind)
    {
        using var memory = Memory([0x55, 0]);
        memory.Position = 1;
        var engine = new TEngine(null!);
        TEStorageStream stream;
        stream = kind switch
        {
            0 => new TEStorageStream(memory, engine),
            1 => new TEReadStorageStream(memory, engine, true),
            2 => new TSetupCompareStorageStream(memory, engine, true),
            3 => new TCompareStorageStream(memory, engine),
            4 => new TEReadTextStorageStream(memory, engine, 1),
            5 => new TEWriteStorageStream(memory, engine, true),
            6 => new TEWriteTextStorageStream(memory, engine),
            7 => new TExportTextStream(memory, engine),
            8 => new TTranslateStorageStream(engine),
            _ => new TTranslateTextStream(engine)
        };
        Type[] types = [typeof(TEStorageStream), typeof(TEReadStorageStream),
            typeof(TSetupCompareStorageStream), typeof(TCompareStorageStream),
            typeof(TEReadTextStorageStream), typeof(TEWriteStorageStream),
            typeof(TEWriteTextStorageStream), typeof(TExportTextStream),
            typeof(TTranslateStorageStream), typeof(TTranslateTextStream)];
        try
        {
            Assert.IsType(types[kind], stream);
            Assert.Same(engine, stream.Engine);
            if (kind >= 8) Assert.Null(stream.Stream);
            else Assert.Same(memory, stream.Stream);
            Assert.False(Field<bool>(stream, "FErrorHandled"));
            Assert.Null(stream.OnError);
            Assert.Null(stream.OnProgress);
            Assert.Equal(kind is >= 1 and <= 4 ? 2 : 1, memory.Position);
            if (kind == 0) Assert.Null(stream.PropertyTable);
            else Assert.Equal(0, stream.PropertyTable.Count);
            if (stream is TEReadStorageStream reader)
            {
                Assert.Equal(2, reader.DataBase);
                Assert.Equal(kind != 3, Field<bool>(reader, "FPartial"));
            }
            if (stream is TEWriteStorageStream)
            {
                Assert.True(Field<bool>(stream, "FPartial"));
                var buffer = Field<TEBufferStream>(stream, "FBuffer");
                Assert.NotSame(memory, buffer);
                Assert.Equal(0, buffer.Position);
                Assert.Equal(0, buffer.Size);
            }
            if (stream is TEWriteTextStorageStream or TEReadTextStorageStream)
                Assert.Equal("", Field<string>(stream, "FPath"));
            if (stream is TEWriteTextStorageStream)
            {
                var buffer = Field<TEBufferStream>(stream, "FBuffer");
                Assert.NotSame(memory, buffer);
                Assert.Equal(0, buffer.Size);
                Assert.Equal(0, buffer.Position);
            }
            if (stream is TEReadTextStorageStream)
            {
                Assert.Equal(1, Field<int>(stream, "FSize"));
                foreach (string field in new[] { "FText", "FSections", "FValues" })
                    Assert.Equal(0, Field<TStringList>(stream, field).Count);
            }
            if (stream is TTranslateTextStream translator) Assert.Null(translator.FDictionary);
        }
        finally
        {
            stream.Destroy();
            engine.Destroy();
        }
        Assert.True(memory.Size >= 2);
        memory.Position = 0;
        Assert.Equal(1, memory.Write([0x33]));
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void BinaryRoundTripKeepsBytesAndPartialState(bool partial)
    {
        using var memory = new TMemoryStream();
        var source = new StoredObject { Name = "Oak", Detail = "Hill" };
        var writer = new TEWriteStorageStream(memory, null!, partial);
        Assert.Equal(0, memory.Size);
        writer.rwEObject(source, 4);
        Assert.Equal(0, memory.Size);
        writer.Destroy();
        byte[] bytes = memory.Memory[..memory.Size];
        Assert.Equal("0104000207000904034F616B0448696C6C", Convert.ToHexString(bytes));
        memory.Position = 0;
        var reader = new TEReadStorageStream(memory, null!, partial);
        var loaded = new StoredObject();
        try
        {
            Assert.Equal(partial, Field<bool>(reader, "FPartial"));
            reader.rwEObject(loaded, 4);
            Assert.Equal(source.Name, loaded.Name);
            Assert.Equal(source.Detail, loaded.Detail);
        }
        finally { reader.Destroy(); }
        using var second = new TMemoryStream();
        var rewritten = new TEWriteStorageStream(second, null!, partial);
        rewritten.rwEObject(loaded, 4);
        rewritten.Destroy();
        Assert.Equal(bytes, second.Memory[..second.Size]);
        loaded.Destroy();
        source.Destroy();
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void TextRoundTripKeepsText(bool export)
    {
        using var memory = new TMemoryStream();
        var source = new StoredObject { Name = "Oak", Detail = "Hill" };
        TEWriteTextStorageStream writer = export ? new TExportTextStream(memory, null!)
            : new TEWriteTextStorageStream(memory, null!);
        writer.rwEObject(source, 4);
        writer.Destroy();
        byte[] bytes = memory.Memory[..memory.Size];
        Assert.Equal(export ? "\r\n[:4:]\r\n[7]=|Oak|\r\n[9]=|Hill|\r\n"
            : "\r\n[:4:]\r\n[7]=Oak\r\n[9]=Hill\r\n", System.Text.Encoding.Latin1.GetString(bytes));
        memory.Position = 0;
        var reader = new TEReadTextStorageStream(memory, null!, 0);
        var loaded = new StoredObject();
        try
        {
            reader.rwEObject(loaded, 4);
            Assert.Equal(export ? "|Oak|" : source.Name, loaded.Name);
            Assert.Equal(export ? "|Hill|" : source.Detail, loaded.Detail);
        }
        finally { reader.Destroy(); }
        using var second = new TMemoryStream();
        var rewritten = new TEWriteTextStorageStream(second, null!);
        rewritten.rwEObject(loaded, 4);
        rewritten.Destroy();
        Assert.Equal(bytes, second.Memory[..second.Size]);
        loaded.Destroy();
        source.Destroy();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void TruncatedBinaryConstructionKeepsFailure(int kind)
    {
        using var memory = new ExactReadStream();
        memory.Write([1, 7]);
        memory.Position = 0;
        var error = Assert.Throws<EReadError>(() =>
        {
            if (kind == 0) _ = new TEReadStorageStream(memory, null!, false);
            else if (kind == 1) _ = new TCompareStorageStream(memory, null!);
            else _ = new TSetupCompareStorageStream(memory, null!, true);
        });
        Assert.Equal("Stream read error", ((System.Exception)error).Message);
        Assert.Equal(2, memory.Position);
        Assert.Equal(2, memory.Size);
    }

    [Fact]
    public void OrdinaryMemoryStreamKeepsShortReadBehavior()
    {
        using var memory = Memory([1, 7]);
        var reader = new TEReadStorageStream(memory, null!, false);
        Assert.Equal(2, reader.DataBase);
        Assert.Equal(1, reader.PropertyTable.Count);
        Assert.Equal(0, reader.PropertyTable.FindOffset(7));
        reader.Destroy();
    }

    [Fact]
    public void MalformedTextConstructionKeepsFailure()
    {
        using var memory = Memory("[::]\r\n[]=broken\r\n"u8);
        var error = Assert.Throws<EConvertError>(() => new TEReadTextStorageStream(memory, null!, 0));
        Assert.Equal("'' is not a valid integer value", ((System.Exception)error).Message);
        Assert.Equal(memory.Size, memory.Position);
        Assert.Equal(1, memory.Write([0x33]));
    }

    [Fact]
    public void DebugWriterKeepsBorrowedBufferAndLegacyRoute()
    {
        var buffer = new TEBufferStream();
        var debug = new TDebugWriteStream(buffer, null!);
        Assert.Same(buffer, debug.Stream);
        Assert.Same(buffer, Field<TEBufferStream>(debug, "FBuffer"));
        Assert.False(Field<bool>(debug, "FPartial"));
        Assert.Equal(0, debug.PropertyTable.Count);
        int value = 42;
        debug.rwInteger(ref value, 7);
        debug.Destroy();
        var bytes = new byte[buffer.Size];
        buffer.Position = 0;
        buffer.Read(bytes);
        Assert.Equal("2A000000010700", Convert.ToHexString(bytes));
        Assert.Equal(1, buffer.Write([0x33]));
        buffer.Destroy();
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void TranslationKeepsObjectAndCreatesNoStream(bool text)
    {
        TTranslateStorageStream translator = text ? new TTranslateTextStream(null!)
            : new TTranslateStorageStream(null!);
        var dictionary = new TIvDictionary();
        if (translator is TTranslateTextStream typed) typed.FDictionary = dictionary;
        var value = new StoredObject { Name = "Oak", Detail = "Hill" };
        translator.rwEObject(value, 4);
        Assert.Equal("Oak", value.Name);
        Assert.Equal("Hill", value.Detail);
        Assert.Null(translator.Stream);
        Assert.Equal(0, translator.PropertyTable.Count);
        translator.Destroy();
        Assert.Equal("Oak", dictionary.Translate("Oak"));
        dictionary.Destroy();
        value.Destroy();
    }
}
