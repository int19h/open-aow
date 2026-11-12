using System.Reflection;
using System.Text;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Rtl.Localize;

namespace Aow.Game.Tests;

[Collection("Engine")]
[EngineState]
public sealed class StorageStreamConstructionTests
{
    private static T Field<T>(object value, string name) => (T)MemberNames.Field(value.GetType(), name)!.GetValue(value)!;

    private sealed class StoredObject() : EngineObject(null!)
    {
        internal string Name = "";
        internal string Detail = "";

        public override void ReadWrite(EStorageStream stream)
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
    public void ConstructionKeepsTypeBorrowedStreamAndPrivateState(int kind)
    {
        using var memory = Memory([0x55, 0]);
        memory.Position = 1;
        GameEngine engine = new GameEngine(null!);
        EStorageStream stream;
        stream = kind switch
        {
            0 => new EStorageStream(memory, engine),
            1 => new EReadStorageStream(memory, engine, true),
            2 => new EReadTextStorageStream(memory, engine, 1),
            3 => new EWriteStorageStream(memory, engine, true),
            4 => new EWriteTextStorageStream(memory, engine),
            5 => new ExportTextStream(memory, engine),
            6 => new TranslateStorageStream(engine),
            _ => new TranslateTextStream(engine)
        };
        Type[] types = [typeof(EStorageStream), typeof(EReadStorageStream),
            typeof(EReadTextStorageStream), typeof(EWriteStorageStream),
            typeof(EWriteTextStorageStream), typeof(ExportTextStream),
            typeof(TranslateStorageStream), typeof(TranslateTextStream)];
        try
        {
            Assert.IsType(types[kind], stream);
            Assert.Same(engine, stream.Engine);
            if (kind >= 6) Assert.Null(stream.Stream);
            else Assert.Same(memory, stream.Stream);
            Assert.False(Field<bool>(stream, "FErrorHandled"));
            Assert.Null(stream.OnError);
            Assert.Null(stream.OnProgress);
            Assert.Equal(kind is >= 1 and <= 2 ? 2L : 1L, memory.Position);
            if (kind == 0) Assert.Null(stream.PropertyTable);
            else Assert.Equal(0, stream.PropertyTable.Count);
            if (stream is EReadStorageStream reader)
            {
                Assert.Equal(2, reader.DataBase);
                Assert.True(Field<bool>(reader, "FPartial"));
            }
            if (stream is EWriteStorageStream)
            {
                Assert.True(Field<bool>(stream, "FPartial"));
                var buffer = Field<BufferStream>(stream, "FBuffer");
                Assert.NotSame(memory, buffer);
                Assert.Equal(0L, buffer.Position);
                Assert.Equal(0L, buffer.Length);
            }
            if (stream is EWriteTextStorageStream or EReadTextStorageStream)
                Assert.Equal("", Field<string>(stream, "FPath"));
            if (stream is EWriteTextStorageStream)
            {
                var buffer = Field<BufferStream>(stream, "FBuffer");
                Assert.NotSame(memory, buffer);
                Assert.Equal(0L, buffer.Length);
                Assert.Equal(0L, buffer.Position);
            }
            if (stream is EReadTextStorageStream)
            {
                Assert.Equal(1, Field<int>(stream, "FSize"));
                foreach (string field in new[] { "FText", "FSections", "FValues" })
                    Assert.Equal(0, Field<StringList>(stream, field).Count);
            }
            if (stream is TranslateTextStream translator) Assert.Null(translator.FDictionary);
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
        var writer = new EWriteStorageStream(memory, null!, partial);
        Assert.Equal(0L, memory.Length);
        writer.rwEObject(source, 4);
        Assert.Equal(0L, memory.Length);
        writer.Dispose();
        byte[] bytes = memory.ToArray();
        Assert.Equal("0104000207000904034F616B0448696C6C", Convert.ToHexString(bytes));
        memory.Position = 0;
        var reader = new EReadStorageStream(memory, null!, partial);
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
        var rewritten = new EWriteStorageStream(second, null!, partial);
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
        EWriteTextStorageStream writer = export ? new ExportTextStream(memory, null!)
            : new EWriteTextStorageStream(memory, null!);
        writer.rwEObject(source, 4);
        writer.Dispose();
        byte[] bytes = memory.ToArray();
        Assert.Equal(export ? "\r\n[:4:]\r\n[7]=|Oak|\r\n[9]=|Hill|\r\n"
            : "\r\n[:4:]\r\n[7]=Oak\r\n[9]=Hill\r\n", Encoding.Latin1.GetString(bytes));
        memory.Position = 0;
        var reader = new EReadTextStorageStream(memory, null!, 0);
        var loaded = new StoredObject();
        try
        {
            reader.rwEObject(loaded, 4);
            Assert.Equal(export ? "|Oak|" : source.Name, loaded.Name);
            Assert.Equal(export ? "|Hill|" : source.Detail, loaded.Detail);
        }
        finally { reader.Dispose(); }
        using var second = new MemoryStream();
        var rewritten = new EWriteTextStorageStream(second, null!);
        rewritten.rwEObject(loaded, 4);
        rewritten.Dispose();
        Assert.Equal(bytes, second.ToArray());
        loaded.Dispose();
        source.Dispose();
    }

    [Fact]
    public void TruncatedBinaryConstructionKeepsFailure()
    {
        using var memory = new ExactReadStream();
        memory.Write([1, 7]);
        memory.Position = 0;
        var error = Assert.Throws<EndOfStreamException>(() =>
        {
            _ = new EReadStorageStream(memory, null!, false);
        });
        Assert.Equal("Stream read error", ((Exception)error).Message);
        Assert.Equal(2L, memory.Position);
        Assert.Equal(2L, memory.Length);
    }

    // A property table that ends inside a pair raises EndOfStreamException.
    [Fact]
    public void OrdinaryMemoryStreamShortReadThrows()
    {
        using var memory = Memory([1, 7]);
        var error = Assert.Throws<EndOfStreamException>(() => new EReadStorageStream(memory, null!, false));
        Assert.Equal("Unable to read beyond the end of the stream.", error.Message);
        Assert.Equal(memory.Length, memory.Position);
    }

    [Fact]
    public void MalformedTextConstructionKeepsFailure()
    {
        using var memory = Memory("[::]\r\n[]=broken\r\n"u8);
        var error = Assert.Throws<FormatException>(() => new EReadTextStorageStream(memory, null!, 0));
        Assert.Equal("'' is not a valid integer value", ((Exception)error).Message);
        Assert.Equal(memory.Length, memory.Position);
        long end = memory.Position;
        memory.Write([0x33]);
        Assert.Equal(end + 1, memory.Position);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void TranslationKeepsObjectAndCreatesNoStream(bool text)
    {
        TranslateStorageStream translator = text ? new TranslateTextStream(null!)
            : new TranslateStorageStream(null!);
        var dictionary = new IvDictionary();
        if (translator is TranslateTextStream typed) typed.FDictionary = dictionary;
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
