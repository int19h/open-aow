namespace Aow.Units.GFXE;

public static partial class GFXE
{
    public static int GetElapsedMilliSeconds() => (int)Rtl.Rtl.Clock.TickCount;
    public static int GetElapsedMicroSeconds() => (int)Rtl.Rtl.Clock.TickCount * 1000;
}
