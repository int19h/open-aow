using System.Buffers.Binary;
using Aow.Rtl;

namespace Aow.Platform;

public sealed record WaveData(int SampleRate, int Channels, float[] Samples)
{
    public int Frames => Samples.Length / Channels;

    public static WaveData Read(Stream stream)
    {
        using var copy = new MemoryStream();
        stream.CopyTo(copy);
        ReadOnlySpan<byte> bytes = copy.ToArray();
        if (bytes.Length < 12 || !bytes[..4].SequenceEqual("RIFF"u8) || !bytes.Slice(8, 4).SequenceEqual("WAVE"u8))
            throw new InvalidDataException("Expected RIFF WAVE data.");
        long end = 8L + BinaryPrimitives.ReadUInt32LittleEndian(bytes[4..]);
        if (end > bytes.Length || end < 12) throw new InvalidDataException("Truncated RIFF data.");
        TWaveFormatEx format = default;
        bool haveFormat = false, haveData = false;
        ReadOnlySpan<byte> data = default;
        for (long offset = 12; offset + 8 <= end;)
        {
            int at = checked((int)offset);
            uint length = BinaryPrimitives.ReadUInt32LittleEndian(bytes[(at + 4)..]);
            long next = offset + 8 + length;
            if (next > end) throw new InvalidDataException("Truncated WAVE chunk.");
            var chunk = bytes.Slice(at + 8, checked((int)length));
            if (bytes.Slice(at, 4).SequenceEqual("fmt "u8))
            {
                if (chunk.Length < 16) throw new InvalidDataException("Truncated WAVE format.");
                format.wFormatTag = BinaryPrimitives.ReadUInt16LittleEndian(chunk);
                format.nChannels = BinaryPrimitives.ReadUInt16LittleEndian(chunk[2..]);
                format.nSamplesPerSec = BinaryPrimitives.ReadInt32LittleEndian(chunk[4..]);
                format.nBlockAlign = BinaryPrimitives.ReadUInt16LittleEndian(chunk[12..]);
                format.wBitsPerSample = BinaryPrimitives.ReadUInt16LittleEndian(chunk[14..]);
                haveFormat = true;
            }
            else if (bytes.Slice(at, 4).SequenceEqual("data"u8)) { data = chunk; haveData = true; }
            offset = next + (length & 1);
        }
        if (!haveFormat || !haveData) throw new InvalidDataException("WAVE needs fmt and data chunks.");
        return Decode(format, data);
    }

    public static WaveData Decode(in TWaveFormatEx format, ReadOnlySpan<byte> data)
    {
        int channels = format.nChannels, bits = format.wBitsPerSample;
        if (channels is not (1 or 2) || format.nSamplesPerSec <= 0 ||
            !((format.wFormatTag == 1 && bits is 8 or 16) || (format.wFormatTag == 7 && bits == 8)) ||
            format.nBlockAlign != channels * (bits / 8) || data.Length % format.nBlockAlign != 0)
            throw new InvalidDataException("Unsupported or invalid WAVE format.");
        var samples = new float[data.Length / (bits / 8)];
        for (int i = 0; i < samples.Length; i++)
            samples[i] = format.wFormatTag == 7 ? MuLaw(data[i]) / 32768f :
                bits == 8 ? (data[i] - 128) / 128f : BinaryPrimitives.ReadInt16LittleEndian(data[(i * 2)..]) / 32768f;
        return new(format.nSamplesPerSec, channels, samples);
    }

    private static int MuLaw(byte input)
    {
        int value = (~input) & 255;
        int magnitude = (((value & 15) << 3) + 132) << ((value >> 4) & 7);
        return (value & 128) != 0 ? 132 - magnitude : magnitude - 132;
    }
}
