namespace Aow.Units.GFXE;

public static partial class GFXE
{
    public static int GetElapsedMilliSeconds() => unchecked((int)Rtl.Rtl.Clock.TickCount);
    public static int GetElapsedMicroSeconds() => unchecked((int)Rtl.Rtl.Clock.TickCount * 1000);
}
