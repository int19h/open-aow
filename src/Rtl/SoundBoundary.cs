using System.Runtime.InteropServices;

namespace Aow.Rtl;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct TWaveFormatEx
{
    public ushort wFormatTag, nChannels;
    public int nSamplesPerSec, nAvgBytesPerSec;
    public ushort nBlockAlign, wBitsPerSample, cbSize;
}
