using Aow.Rtl;

namespace Aow.Game.Draw;

public static partial class GFXEUnit
{
    public static int GetElapsedMilliSeconds() => (int)RtlUnit.Clock.TickCount;
    public static int GetElapsedMicroSeconds() => (int)RtlUnit.Clock.TickCount * 1000;
}
