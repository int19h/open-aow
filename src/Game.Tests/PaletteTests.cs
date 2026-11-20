using Aow.Game.Draw;

namespace Aow.Game.Tests;

// The palette as 32-bit pixel values (#192 LY-6). The ILB palette image is the pixel format
// identifier and 256 little-endian entries of 4 bytes, and it round trips byte for byte, the
// fourth byte included. The channel members name the bytes of an entry, and Remap and Compare
// keep the byte semantics of the Delphi record (GFXE.pas TPaletteEntry and TGFXEPalette).
public sealed class PaletteTests
{
    [Fact]
    public void TheStreamImageRoundTripsAndNamesEveryByte()
    {
        byte[] image = new byte[4 + 1024];
        new Random(5).NextBytes(image);
        Palette palette = new();
        palette.LoadFromStream(new MemoryStream(image));
        Assert.Equal((PixelFormat)BitConverter.ToInt32(image, startIndex: 0), palette.PixelFormatId);
        for (int i = 0; i < 256; i++)
        {
            Palette.Entry entry = palette[i];
            Assert.Equal(image[4 + (4 * i)], entry.Red);
            Assert.Equal(image[5 + (4 * i)], entry.Green);
            Assert.Equal(image[6 + (4 * i)], entry.Blue);
            Assert.Equal(image[7 + (4 * i)], entry.Flags);
            Assert.Equal(BitConverter.ToUInt16(image, startIndex: 4 + (4 * i)), entry.PackedColor);
            Assert.Equal(BitConverter.ToUInt32(image, startIndex: 4 + (4 * i)), entry.Value);
            Assert.Equal(entry.Value, palette.Pixels[i]);
        }

        MemoryStream saved = new();
        palette.SaveToStream(saved);
        Assert.Equal(image, saved.ToArray());
    }

    [Fact]
    public void EachChannelWritesOnlyItsBytes()
    {
        Palette.Entry entry = new(0x4433_2211u);
        Assert.Equal(0x4433_22AAu, (entry with { Red = 0xAA }).Value);
        Assert.Equal(0x4433_AA11u, (entry with { Green = 0xAA }).Value);
        Assert.Equal(0x44AA_2211u, (entry with { Blue = 0xAA }).Value);
        Assert.Equal(0xAA33_2211u, (entry with { Flags = 0xAA }).Value);
        Assert.Equal(0x4433_BEEFu, (entry with { PackedColor = 0xBEEF }).Value);
        Assert.Equal(0x4433_2211u, new Palette.Entry(red: 0x11, green: 0x22, blue: 0x33, flags: 0x44).Value);
    }

    [Theory]
    [InlineData(PixelFormat.Bgr24, PixelFormat.Rgb565)]
    [InlineData(PixelFormat.Bgr24, PixelFormat.Rgb555)]
    [InlineData(PixelFormat.Rgb24, PixelFormat.Rgb565)]
    [InlineData(PixelFormat.Rgb24, PixelFormat.Rgb555)]
    public void RemapWritesTheLowWordAndKeepsTheHighBytes(PixelFormat from, PixelFormat to)
    {
        Palette palette = new() { PixelFormatId = from };
        Random random = new(11);
        for (int i = 0; i < 256; i++)
        {
            palette[i] = new((uint)random.Next() ^ ((uint)random.Next() << 16));
        }

        uint[] before = palette.Pixels.ToArray();

        Assert.Equal(GraphicsResult.Ok, palette.Remap(to));

        Assert.Equal(to, palette.PixelFormatId);
        for (int i = 0; i < 256; i++)
        {
            Palette.Entry old = new(before[i]);
            (byte first, byte last) = from == PixelFormat.Bgr24 ? (old.Red, old.Blue) : (old.Blue, old.Red);
            int expected = to == PixelFormat.Rgb565
                ? ((first & 248) << 8) + ((old.Green & 252) << 3) + (last >> 3)
                : ((first & 248) << 7) + ((old.Green & 248) << 2) + (last >> 3);
            Assert.Equal((ushort)expected, palette[i].PackedColor);
            Assert.Equal(before[i] & 0xFFFF_0000u, palette.Pixels[i] & 0xFFFF_0000u);
        }
    }

    [Theory]
    [InlineData(PixelFormat.Rgb565, 0x0000_FFFFu)]
    [InlineData(PixelFormat.Bgr24, 0x00FF_FFFFu)]
    public void CompareReadsTheBytesOfTheFormat(PixelFormat format, uint compared)
    {
        Palette first = new() { PixelFormatId = format };
        Palette second = new() { PixelFormatId = format };
        for (int i = 0; i < 256; i++)
        {
            first[i] = second[i] = new((uint)(i * 0x0101_0101));
        }

        Assert.True(first.Compare(second));
        for (int shift = 0; shift < 32; shift += 8)
        {
            second[200] = new(first[200].Value ^ (0x80u << shift));
            Assert.Equal((compared & (0x80u << shift)) == 0, actual: first.Compare(second));
        }
    }

    // Compare switches on PixelFormatId and $1F. A 32-bit format leaves 0 there, so its arm, a
    // comparison of all 1,024 bytes, is never reached and two 32-bit palettes compare unequal, as
    // in the original.
    [Fact]
    public void ThirtyTwoBitPalettesNeverCompareEqual()
    {
        Palette first = new() { PixelFormatId = PixelFormat.Xrgb32 };
        Palette second = new() { PixelFormatId = PixelFormat.Xrgb32 };
        Assert.Equal(0, (int)PixelFormat.Xrgb32 & 31);
        Assert.False(first.Compare(second));
    }
}
