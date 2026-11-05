using Aow.Rtl;

namespace Aow.Units.Engine;

public sealed class EngineDebugNotification : TObject
{
    public TEngineDebugInfo Info;
}

public static partial class Engine
{
    internal static int CStringLength(ReadOnlySpan<byte> text)
    {
        int end = text.IndexOf((byte)0);
        return end < 0 ? text.Length : end;
    }
}
