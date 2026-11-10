using System.Reflection;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.Engine.Storage;
using Aow.Rtl.Localize;

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

    private sealed class ExactReadStream : MemoryStream
    {
        public override int Read(Span<byte> buffer)
        {
            int count = base.Read(buffer);
            if (count != buffer.Length) throw new EndOfStreamException("Stream read error");
            return count;
        }
    }

    private static MemoryStream Memory(ReadOnlySpan<byte> bytes)
    {
        var stream = new MemoryStream();
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
            Assert.Equal(kind is >= 1 and <= 4 ? 2L : 1L, memory.Position);
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
                Assert.Equal(0L, buffer.Position);
                Assert.Equal(0L, buffer.Length);
            }
            if (stream is TEWriteTextStorageStream or TEReadTextStorageStream)
                Assert.Equal("", Field<string>(stream, "FPath"));
            if (stream is TEWriteTextStorageStream)
            {
                var buffer = Field<TEBufferStream>(stream, "FBuffer");
                Assert.NotSame(memory, buffer);
                Assert.Equal(0L, buffer.Length);
                Assert.Equal(0L, buffer.Position);
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
            stream.Dispose();
            engine.Dispose();
        }
        Assert.True(memory.Length >= 2);
        memory.Position = 0;
        memory.Write([0x33]);
        Assert.Equal(1L, memory.Position);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void BinaryRoundTripKeepsBytesAndPartialState(bool partial)
    {
        using var memory = new MemoryStream();
        var source = new StoredObject { Name = "Oak", Detail = "Hill" };
        var writer = new TEWriteStorageStream(memory, null!, partial);
        Assert.Equal(0L, memory.Length);
        writer.rwEObject(source, 4);
        Assert.Equal(0L, memory.Length);
        writer.Dispose();
        byte[] bytes = memory.ToArray();
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
        finally { reader.Dispose(); }
        using var second = new MemoryStream();
        var rewritten = new TEWriteStorageStream(second, null!, partial);
        rewritten.rwEObject(loaded, 4);
        rewritten.Dispose();
        Assert.Equal(bytes, second.ToArray());
        loaded.Dispose();
        source.Dispose();
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void TextRoundTripKeepsText(bool export)
    {
        using var memory = new MemoryStream();
        var source = new StoredObject { Name = "Oak", Detail = "Hill" };
        TEWriteTextStorageStream writer = export ? new TExportTextStream(memory, null!)
            : new TEWriteTextStorageStream(memory, null!);
        writer.rwEObject(source, 4);
        writer.Dispose();
        byte[] bytes = memory.ToArray();
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
        finally { reader.Dispose(); }
        using var second = new MemoryStream();
        var rewritten = new TEWriteTextStorageStream(second, null!);
        rewritten.rwEObject(loaded, 4);
        rewritten.Dispose();
        Assert.Equal(bytes, second.ToArray());
        loaded.Dispose();
        source.Dispose();
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
        var error = Assert.Throws<EndOfStreamException>(() =>
        {
            if (kind == 0) _ = new TEReadStorageStream(memory, null!, false);
            else if (kind == 1) _ = new TCompareStorageStream(memory, null!);
            else _ = new TSetupCompareStorageStream(memory, null!, true);
        });
        Assert.Equal("Stream read error", ((System.Exception)error).Message);
        Assert.Equal(2L, memory.Position);
        Assert.Equal(2L, memory.Length);
    }

    [Fact]
    public void OrdinaryMemoryStreamKeepsShortReadBehavior()
    {
        using var memory = Memory([1, 7]);
        using var reader = new TEReadStorageStream(memory, null!, false);
        Assert.Equal(2, reader.DataBase);
        Assert.Equal(1, reader.PropertyTable.Count);
        Assert.Equal(0, reader.PropertyTable.FindOffset(7));
    }

    [Fact]
    public void MalformedTextConstructionKeepsFailure()
    {
        using var memory = Memory("[::]\r\n[]=broken\r\n"u8);
        var error = Assert.Throws<FormatException>(() => new TEReadTextStorageStream(memory, null!, 0));
        Assert.Equal("'' is not a valid integer value", ((System.Exception)error).Message);
        Assert.Equal(memory.Length, memory.Position);
        long end = memory.Position;
        memory.Write([0x33]);
        Assert.Equal(end + 1, memory.Position);
    }

    [Fact]
    public void DebugWriterKeepsBorrowedBufferAndLegacyRoute()
    {
        using var buffer = new TEBufferStream();
        var debug = new TDebugWriteStream(buffer, null!);
        Assert.Same(buffer, debug.Stream);
        Assert.Same(buffer, Field<TEBufferStream>(debug, "FBuffer"));
        Assert.False(Field<bool>(debug, "FPartial"));
        Assert.Equal(0, debug.PropertyTable.Count);
        int value = 42;
        debug.rwInteger(ref value, 7);
        debug.Dispose();
        var bytes = new byte[(int)buffer.Length];
        buffer.Position = 0;
        Assert.Equal(bytes.Length, buffer.Read(bytes));
        Assert.Equal("2A000000010700", Convert.ToHexString(bytes));
        long end = buffer.Position;
        buffer.Write([0x33]);
        Assert.Equal(end + 1, buffer.Position);
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
        using var value = new StoredObject { Name = "Oak", Detail = "Hill" };
        translator.rwEObject(value, 4);
        Assert.Equal("Oak", value.Name);
        Assert.Equal("Hill", value.Detail);
        Assert.Null(translator.Stream);
        Assert.Equal(0, translator.PropertyTable.Count);
        translator.Dispose();
        Assert.Equal("Oak", dictionary.Translate("Oak"));
        dictionary.Dispose();
    }
}
