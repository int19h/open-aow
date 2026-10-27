namespace Aow.Units.ImageLib;

public partial class TBitMask
{
    public void AllocateMaskData(int AWidth, int AHeight)
    {
        FPitch = checked(((AWidth + 31) >> 5) * 4);
        FDataSize = checked(FPitch * FMaskHeight);
        // GrabImage flushes one extra zero dword after a full final word.
        FMaskData = FMaskData.Realloc(checked(FDataSize + 4));
    }
}
