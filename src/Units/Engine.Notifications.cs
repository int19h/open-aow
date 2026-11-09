using Aow.Rtl;

namespace Aow.Units.Engine;

[LegacyObject]
public sealed class EngineDebugNotification
{
    public TEngineDebugInfo Info;

    public EngineDebugNotification()
    {
        CaptureAddress.Assign(this);
    }
}

public static partial class EngineUnit
{
    internal static int CStringLength(ReadOnlySpan<byte> text)
    {
        int end = text.IndexOf((byte)0);
        return end < 0 ? text.Length : end;
    }
}
