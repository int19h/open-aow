using Aow.Rtl;

namespace Aow.Game.Engine;

[LegacyObject]
public sealed class EngineDebugNotification
{
    public TEngineDebugInfo Info;

    public EngineDebugNotification()
    {
        CaptureAddress.Assign(this);
    }
}
