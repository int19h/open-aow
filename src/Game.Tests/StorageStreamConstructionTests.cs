using System.Reflection;
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

        public override void ReadWrite(StorageStream stream)
        {
            stream.String(ref Name, 7);
            stream.String(ref Detail, 9);
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
    public void ConstructionKeepsTypeBorrowedStreamAndPrivateState(int kind)
    {
        using var memory = Memory([0x55, 0]);
        memory.Position = 1;
        GameEngine engine = new GameEngine(null!);
        StorageStream stream;
        stream = kind switch
        {
            0 => new StorageStream(memory, engine),
            1 => new StorageReader(memory, engine, true),
            2 => new StorageWriter(memory, engine, true),
            3 => new TranslateStorageStream(engine),
            _ => new TranslateTextStream(engine)
        };
        Type[] types = [typeof(StorageStream), typeof(StorageReader),
            typeof(StorageWriter), typeof(TranslateStorageStream), typeof(TranslateTextStream)];
        try
        {
            Assert.IsType(types[kind], stream);
            Assert.Same(engine, stream.Engine);
            if (kind >= 3) Assert.Null(stream.Stream);
            else Assert.Same(memory, stream.Stream);
            Assert.False(Field<bool>(stream, "_errorHandled"));
            Assert.Null(stream.OnError);
            Assert.Null(stream.OnProgress);
            Assert.Equal(kind == 1 ? 2L : 1L, memory.Position);
            if (kind == 0) Assert.Null(stream.PropertyTable);
            else Assert.Equal(0, stream.PropertyTable.Count);
            if (stream is StorageReader reader)
            {
                Assert.Equal(2, reader.DataStart);
                Assert.True(Field<bool>(reader, "_partial"));
            }
            if (stream is StorageWriter)
            {
                Assert.True(Field<bool>(stream, "_partial"));
                var buffer = Field<BufferStream>(stream, "_buffer");
                Assert.NotSame(memory, buffer);
                Assert.Equal(0L, buffer.Position);
                Assert.Equal(0L, buffer.Length);
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
        var writer = new StorageWriter(memory, null!, partial);
        Assert.Equal(0L, memory.Length);
        writer.Object(source, 4);
        Assert.Equal(0L, memory.Length);
        writer.Dispose();
        byte[] bytes = memory.ToArray();
        Assert.Equal("0104000207000904034F616B0448696C6C", Convert.ToHexString(bytes));
        memory.Position = 0;
        var reader = new StorageReader(memory, null!, partial);
        var loaded = new StoredObject();
        try
        {
            Assert.Equal(partial, Field<bool>(reader, "_partial"));
            reader.Object(loaded, 4);
            Assert.Equal(source.Name, loaded.Name);
            Assert.Equal(source.Detail, loaded.Detail);
        }
        finally { reader.Dispose(); }
        using var second = new MemoryStream();
        var rewritten = new StorageWriter(second, null!, partial);
        rewritten.Object(loaded, 4);
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
            _ = new StorageReader(memory, null!, false);
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
        var error = Assert.Throws<EndOfStreamException>(() => new StorageReader(memory, null!, false));
        Assert.Equal("Unable to read beyond the end of the stream.", error.Message);
        Assert.Equal(memory.Length, memory.Position);
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
        translator.Object(value, 4);
        Assert.Equal("Oak", value.Name);
        Assert.Equal("Hill", value.Detail);
        Assert.Null(translator.Stream);
        Assert.Equal(0, translator.PropertyTable.Count);
        translator.Dispose();
        Assert.Equal("Oak", dictionary.Translate("Oak"));
        dictionary.Dispose();
    }
}
