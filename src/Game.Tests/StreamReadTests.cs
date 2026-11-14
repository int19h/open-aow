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
        using var textStream = new MemoryStream([1, 2]);
        Assert.Throws<EndOfStreamException>(() => GameEngine.StringFromStream(ref text, textStream));
        Assert.Equal("old", text);

        using var compressed = new CompressStream();
        using var header = new MemoryStream([1, 2, 3]);
        Assert.Throws<EndOfStreamException>(() => compressed.LoadFromStream(header));
    }

    [Fact]
    public void ShortImageDataThrows()
    {
        using var image = new LibraryImage(null!);
        using var stream = new MemoryStream();
        Assert.Throws<EndOfStreamException>(() => image.ReadImageData(stream));
    }

    [Fact]
    public void ShortStorageReadThrows()
    {
        // The integer list's payload, which the token events still store. The byte list's payload
        // methods went with the combat ReadWrite methods (issue 185 PE-3-i part 4).
        var list = new Int32List();
        using var stream = new MemoryStream();
        Assert.Throws<EndOfStreamException>(() => list.LoadFromStream(stream));
    }
}
