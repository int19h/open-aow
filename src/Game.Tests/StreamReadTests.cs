using Aow.Engine.Collections;
using Aow.Engine.Storage;
using Aow.Game.Draw.Images;

namespace Aow.Game.Tests;

// A short read in each ReadExactly group raises EndOfStreamException.
public sealed class StreamReadTests
{
    [Fact]
    public void ShortHeaderReadsThrow()
    {
        string text = "old";
        using MemoryStream textStream = new([1, 2]);
        Assert.Throws<EndOfStreamException>(() => GameEngine.StringFromStream(ref text, textStream));
        Assert.Equal("old", text);

        using CompressStream compressed = new();
        using MemoryStream header = new([1, 2, 3]);
        Assert.Throws<EndOfStreamException>(() => compressed.LoadFromStream(header));
    }

    [Fact]
    public void ShortImageDataThrows()
    {
        using LibraryImage image = new(null);
        using MemoryStream stream = new();
        Assert.Throws<EndOfStreamException>(() => image.ReadImageData(stream));
    }

    [Fact]
    public void ShortStorageReadThrows()
    {
        // The integer list's payload, which the token events still store. The byte list's payload
        // methods went with the combat ReadWrite methods (issue 185 PE-3-i part 4).
        Int32List list = new();
        using MemoryStream stream = new();
        Assert.Throws<EndOfStreamException>(() => list.LoadFromStream(stream));
    }
}
