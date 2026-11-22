namespace Aow.Rtl.Tests;

public sealed class IntegerTruncationTests
{
    // Each truncation keeps the value a store into the narrow Delphi type kept.
    [Theory]
    [InlineData(0, 0, 0, 0, 0)]
    [InlineData(127, 127, 127, 127, 127)]
    [InlineData(128, -128, 128, 128, 128)]
    [InlineData(255, -1, 255, 255, 255)]
    [InlineData(-1, -1, 255, -1, 65535)]
    [InlineData(0x1FF, -1, 255, 511, 511)]
    [InlineData(0x18000, 0, 0, -32768, 32768)]
    public void AnIntKeepsTheNarrowValue(int value, int signedByte, int unsignedByte, int signedWord, int unsignedWord)
    {
        Assert.Equal((sbyte)signedByte, value.TruncateToSByte());
        Assert.Equal((byte)unsignedByte, value.TruncateToByte());
        Assert.Equal((short)signedWord, value.TruncateToInt16());
        Assert.Equal((ushort)unsignedWord, value.TruncateToUInt16());
    }

    [Fact]
    public void OtherWidthsGiveTheSameBits()
    {
        Assert.Equal((sbyte)-1, ((byte)255).TruncateToSByte());
        Assert.Equal((byte)255, ((sbyte)-1).TruncateToByte());
        Assert.Equal((ushort)65535, ((sbyte)-1).TruncateToUInt16());
        Assert.Equal((short)-1, ((ushort)65535).TruncateToInt16());
        Assert.Equal((byte)255, ((short)-1).TruncateToByte());
        Assert.Equal((sbyte)-2, 0xFFFF_FFFEu.TruncateToSByte());
        Assert.Equal((byte)0xFE, (-2L).TruncateToByte());
        Assert.Equal((short)-32768, 0x8000UL.TruncateToInt16());
    }

    [Fact]
    public void NarrowSlotsTakeTheResultWithNoCast()
    {
        int value = 0x1FF;
        sbyte signedByte = value.TruncateToSByte();
        byte unsignedByte = value.TruncateToByte();
        short signedWord = value.TruncateToInt16();
        ushort unsignedWord = value.TruncateToUInt16();
        Assert.Equal((sbyte)-1, signedByte);
        Assert.Equal((byte)255, unsignedByte);
        Assert.Equal((short)511, signedWord);
        Assert.Equal((ushort)511, unsignedWord);
    }
}
