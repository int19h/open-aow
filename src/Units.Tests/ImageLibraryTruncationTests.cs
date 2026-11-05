using Aow.Units.ImageLib;
using Images = Aow.Units.ImageLib.ImageLib;

namespace Aow.Units.Tests;

// Issue #83: a truncated ILB file raises EReadError in place of an endless index loop. The image
// classes are registered, so the reader reaches the index loop instead of failing on an empty registry.
public sealed class ImageLibraryTruncationTests
{
    private static readonly TimeSpan HangTimeout = TimeSpan.FromSeconds(30);

    private static byte[] Header(float version)
    {
        var bytes = new byte[24];
        BitConverter.TryWriteBytes(bytes.AsSpan(8), version);
        BitConverter.TryWriteBytes(bytes.AsSpan(12), 24);
        BitConverter.TryWriteBytes(bytes.AsSpan(16), 24);
        BitConverter.TryWriteBytes(bytes.AsSpan(20), 24);
        return bytes;
    }

    public static IEnumerable<object[]> TruncatedFiles()
    {
        yield return new object[] { "three bytes", new byte[] { 3, (byte)'I', (byte)'L' } };
        yield return new object[] { "version 2 header only", Header(2) };
        yield return new object[] { "version 3 header only", Header(3) };
        yield return new object[] { "version 3 index cut short", Header(3).Concat(new byte[] { 0, 0 }).ToArray() };
    }

    [Theory]
    [MemberData(nameof(TruncatedFiles))]
    public void TruncatedLibraryRaisesReadError(string name, byte[] contents)
    {
        var stream = new TMemoryStream();
        var library = new TImageLibrary(null!);
        var previousClasses = Images.LibraryImageClasses;
        Images.LibraryImageClasses = new TList();
        Images.RegisterLibraryImageClasses([LibraryImageClass.Of<TPicture16>()]);
        try
        {
            stream.WriteBuffer(contents);
            stream.Position = 0;
            var load = Task.Run(() => library.LoadFromStream(stream));
            Assert.True(((IAsyncResult)load).AsyncWaitHandle.WaitOne(HangTimeout), $"{name}: the load did not end within {HangTimeout}");
            Assert.IsType<EReadError>(load.Exception?.InnerException);
            Assert.Equal(0, library.Count);
        }
        finally
        {
            library.Destroy();
            stream.Destroy();
            Images.LibraryImageClasses.Destroy();
            Images.LibraryImageClasses = previousClasses;
        }
    }
}
